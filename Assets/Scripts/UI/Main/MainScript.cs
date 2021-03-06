﻿using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TLJCommon;
using System.Threading;

public class MainScript : MonoBehaviour
{
    public string m_hotfix_class = "MainScript_hotfix";
    public string m_hotfix_path = "HotFix_Project.MainScript_hotfix";

    public Image m_notice_redPoint;
    public Image m_task_redPoint;
    public Image m_sign_redPoint;
    public Image m_mail_redPoint;

    public Image VipImage;

    public Button m_button_xiuxianchang;
    public Button m_button_jingjichang;
    public Button m_button_DDZ_Back;
    public GameObject m_xiuxianchang;
    public Image m_nickName_bg;
    public Text UserAccount;
    public Text UserGold;
    public Text MyGold;
    public Text MyMedal;
    public Text UserYuanBao;
    public Text UserMedal;

    public GameObject m_laba;
    public GameObject m_headIcon;

    public GameObject m_waitMatchPanel = null;
    public GameObject exitGameObject;
    public LaBaScript m_laBaScript;

    //发送验证码的倒计时
    public float nextTime = 1; //一秒之后执行

    public static GameObject logicEnginer;
    public static GameObject playEnginer;

    public string m_curInGameRoomType;
    public string m_wantInGameRoomType;

    private void Awake()
    {
        OtherData.s_mainScript = this;
    }

    // Use this for initialization
    void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "Start", null, null);
            return;
        }

        initUI_Image();

        // 禁止多点触摸
        Input.multiTouchEnabled = false;

        ToastScript.clear();

        // 安卓回调
        AndroidCallBack.s_onPauseCallBack = onPauseCallBack;
        AndroidCallBack.s_onResumeCallBack = onResumeCallBack;

        AudioScript.getAudioScript().stopMusic();

        startBgm();

        // 逻辑服务器
        {
            if (logicEnginer == null)
            {
                logicEnginer = LogicEnginerScript.create();
                LogicEnginerScript.Instance.setOnLogicService_Connect(onSocketConnect_Logic);
                LogicEnginerScript.Instance.setOnLogicService_Close(onSocketClose_Logic);
                LogicEnginerScript.Instance.GetComponent<MainRequest>().CallBack = onReceive_Main;
            }
            else
            {
                LogicEnginerScript.Instance.setOnLogicService_Connect(onSocketConnect_Logic);
                LogicEnginerScript.Instance.setOnLogicService_Close(onSocketClose_Logic);
                print(LogicEnginerScript.Instance);
                print(LogicEnginerScript.Instance.GetComponent<MainRequest>());

                LogicEnginerScript.Instance.GetComponent<MainRequest>().CallBack = onReceive_Main;

                if (LogicEnginerScript.Instance.isConnecion())
                {
                    NetLoading.getInstance().Show();

                    LogicEnginerScript.Instance.GetComponent<GetUserInfoRequest>().OnRequest();
                    LogicEnginerScript.Instance.GetComponent<GetRankRequest>().OnRequest();
                    LogicEnginerScript.Instance.GetComponent<GetSignRecordRequest>().OnRequest();
                    LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().CallBack = onReceive_GetUserBag;
                    LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().OnRequest();
                    LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().OnRequest();
                    LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().OnRequest();
                    LogicEnginerScript.Instance.GetComponent<GetSign30RewardRequest>().OnRequest();
                }
                else
                {
                    NetErrorPanelScript.getInstance().Show();
                    NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
                    NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");
                }
            }
        }

        // 游戏打牌服务器
        {
            if (playEnginer == null)
            {
                playEnginer = PlayServiceSocket.create();

                PlayServiceSocket.s_instance.setOnPlayService_Connect(onSocketConnect_Play);
                PlayServiceSocket.s_instance.setOnPlayService_Receive(onSocketReceive_Play);
                PlayServiceSocket.s_instance.setOnPlayService_Close(onSocketClose_Play);

                PlayServiceSocket.s_instance.startConnect();
            }
            else
            {
                PlayServiceSocket.s_instance.setOnPlayService_Connect(onSocketConnect_Play);
                PlayServiceSocket.s_instance.setOnPlayService_Receive(onSocketReceive_Play);
                PlayServiceSocket.s_instance.setOnPlayService_Close(onSocketClose_Play);
            }
        }

        m_laBaScript = m_laba.GetComponent<LaBaScript>();
    }

    public void initUI_Image()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "initUI_Image"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "initUI_Image", null, null);
            return;
        }

        gameObject.transform.Find("Room/Button_EnterXiuXianChang").GetComponent<PlayAnimation>().start("animations.unity3d", "putongchang",true);
        gameObject.transform.Find("Room/Button_EnterJingJiChang").GetComponent<PlayAnimation>().start("animations.unity3d", "bishaichang", true);
        
        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Bg").GetComponent<Image>(), "main.unity3d", "beijing");
        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("RankingList").GetComponent<Image>(), "main.unity3d", "di01");
        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Room/xiuxianchang/Button_jingdianchang").GetComponent<Image>(), "main.unity3d", "main_jingdian");
        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Room/xiuxianchang/Button_chaodichang").GetComponent<Image>(), "main.unity3d", "main_chaodi");
        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Room/Button_EnterDDZ").GetComponent<Image>(), "main.unity3d", "ddz");
        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Room/Button_EnterShengji").GetComponent<Image>(), "main.unity3d", "shengji");

        gameObject.transform.Find("Room/Button_EnterXiuXianChang").gameObject.SetActive(false);
        gameObject.transform.Find("Room/Button_EnterJingJiChang").gameObject.SetActive(false);

        m_button_DDZ_Back.gameObject.SetActive(false);
    }

    // 显示新人推广礼
    public bool checkShowNewPlayerTuiGuang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "checkShowNewPlayerTuiGuang"))
        {
            return (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "checkShowNewPlayerTuiGuang", null, null);
        }

        if (PlayerPrefs.GetInt("isShowNewPlayerTuiGuang_" + UserData.uid, 0) == 0)
        {
            NewPlayerShowTuiGuangPanelScript.create();
            PlayerPrefs.SetInt("isShowNewPlayerTuiGuang_" + UserData.uid, 1);
            return true;
        }

        return false;
    }

    public void startBgm()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "startBgm"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "startBgm", null, null);
            return;
        }

        // 3秒后播放背景音乐,每隔55秒重复播放背景音乐
        InvokeRepeating("onInvokeStartMusic", 3, 55);
    }

    public void onInvokeStartMusic()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onInvokeStartMusic"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onInvokeStartMusic", null, null);
            return;
        }

        AudioScript.getAudioScript().playMusic_MainBg();
    }

    public GameObject getLogicEnginerObj()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "getLogicEnginerObj"))
        {
            return (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "getLogicEnginerObj", null, null);
        }

        return logicEnginer;
    }

    public void setLogicEnginerObj(GameObject obj)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "setLogicEnginerObj"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "setLogicEnginerObj", null, obj);
            return;
        }

        logicEnginer = obj;
    }

    public GameObject getPlayEnginerObj()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "getPlayEnginerObj"))
        {
            return (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "getPlayEnginerObj", null, null);
        }

        return playEnginer;
    }

    public void setPlayEnginerObj(GameObject obj)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "setPlayEnginerObj"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "setPlayEnginerObj", null, obj);
            return;
        }

        playEnginer = obj;
    }

    void Update()
    {
        if (BindPhoneScript.totalTime > 0)
        {
            if (nextTime <= Time.time)
            {
                BindPhoneScript.totalTime--;
                nextTime = Time.time + 1; //到达一秒后加1
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlatformHelper.isThirdSDKQuit())
            {
                PlatformHelper.thirdSDKQuit("AnroidCallBack", "", "");
            }
            else
            {
                if (exitGameObject == null)
                {
                    exitGameObject = ExitGamePanelScript.create();
                }
            }
           
        }
    }

    void OnDestroy()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnDestroy"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnDestroy", null, null);
            return;
        }

        OtherData.s_mainScript = null;
        //LogicEnginerScript.Instance.m_socketUtil.stop();
        //PlayServiceSocket.getInstance().Stop();
    }

    // 检测服务器是否连接
    public void checkNet()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "checkNet"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "checkNet", null, null);
            return;
        }

        if (!LogicEnginerScript.Instance.isConnecion())
        {
            //ToastScript.createToast("与Logic服务器断开连接");
            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
            NetErrorPanelScript.getInstance().setContentText("与逻辑服务器断开连接，请重新连接");
        }
        else if (!PlayServiceSocket.s_instance.isConnecion())
        {
            //ToastScript.createToast("与Play服务器断开连接");
            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Play);
            NetErrorPanelScript.getInstance().setContentText("与游戏服务器断开连接，请重新连接");
        }
        else
        {
            //ToastScript.createToast("两个服务器都成功连接");

            NetLoading.getInstance().Show();
            reqPVPRoom();
        }
    }

    public void refreshUI()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "refreshUI"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "refreshUI", null, null);
            return;
        }

        // 头像
        m_headIcon.GetComponent<HeadIconScript>().setIcon(UserData.head);

        // 昵称
        UserAccount.text = UserData.name;

        // 金币
        UserGold.text = UserData.gold + "";
        MyGold.text = "我的金币:" + UserData.gold;
        MyMedal.text = "我的徽章:" + UserData.medal;

        // 元宝
        UserYuanBao.text = UserData.yuanbao + "";

        // 徽章
        UserMedal.text = UserData.medal + "";

        // 贵族
        GameUtil.setNickNameFontColor(UserAccount, UserData.vipLevel);
        CommonUtil.setImageSpriteByAssetBundle(VipImage, "vip.unity3d", "user_vip_" + UserData.vipLevel);
        if (UserData.vipLevel > 0)
        {
            CommonUtil.setImageSprite(m_nickName_bg, "Sprites/Common/vipname_bg");
        }

        // 首冲
        if (OtherData.s_hasShouChong)
        {
            GameObject.Find("Canvas").transform.Find("ButtonList").Find("shouchong").localScale = new Vector3(0, 0, 0);
        }

        checkRedPoint();
        
        NetLoading.getInstance().Close();
    }
    
    public void showWaitMatchPanel(float time, string gameroomtype)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "showWaitMatchPanel"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "showWaitMatchPanel", null, time, gameroomtype);
            return;
        }

        if (m_waitMatchPanel != null)
        {
            Destroy(m_waitMatchPanel);
            m_waitMatchPanel = null;
        }

        m_waitMatchPanel = WaitMatchPanelScript.create(gameroomtype);
        WaitMatchPanelScript script = m_waitMatchPanel.GetComponent<WaitMatchPanelScript>();
        script.setOnTimerEvent_TimeEnd(onTimerEvent_TimeEnd);
        script.start(GameData.getInstance().m_gameRoomType,time, false);
    }

    public void onTimerEvent_TimeEnd(bool isContinueGame)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onTimerEvent_TimeEnd"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onTimerEvent_TimeEnd", null, isContinueGame);
            return;
        }

        LogUtil.Log("暂时没有匹配到玩家,请求匹配机器人");

        //// 让服务端匹配机器人
        //reqWaitMatchTimeOut();
    }

    public void onClickEnterXiuXianChang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickEnterXiuXianChang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickEnterXiuXianChang", null, null);
            return;
        }

        AudioScript.getAudioScript().playSound_ButtonClick();
        //SceneManager.LoadScene("GameScene");

        m_button_xiuxianchang.transform.localScale = new Vector3(0, 0, 0);
        m_button_jingjichang.transform.localScale = new Vector3(0, 0, 0);
        m_xiuxianchang.transform.localScale = new Vector3(1, 1, 1);
        m_button_DDZ_Back.gameObject.SetActive(false);


        m_xiuxianchang.GetComponent<Animation>().Play("xiuxianchang_show");
    }

    public void onClickEnterJingJiChang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickEnterJingJiChang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickEnterJingJiChang", null, null);
            return;
        }

        AudioScript.getAudioScript().playSound_ButtonClick();
        PVPChoiceScript.create();
    }

    public void onClickEnterDDZ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickEnterDDZ"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickEnterDDZ", null, null);
            return;
        }

        if (UserData.gold < 1000)
        {
            ToastScript.createToast("金币不足1000，无法进入游戏");
            return;
        }

        m_wantInGameRoomType = TLJCommon.Consts.GameRoomType_DDZ_Normal;
        reqIsJoinRoom();
    }

    //升级按钮
    public void onClickEnterShengji()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickEnterShengji"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickEnterShengji", null, null);
            return;
        }

        gameObject.transform.Find("Room/Button_EnterXiuXianChang").gameObject.SetActive(true);
        gameObject.transform.Find("Room/Button_EnterJingJiChang").gameObject.SetActive(true);
        m_button_DDZ_Back.gameObject.SetActive(true);


        gameObject.transform.Find("Room/Button_EnterDDZ").gameObject.SetActive(false);
        gameObject.transform.Find("Room/Button_EnterShengji").gameObject.SetActive(false);
    }

    // 升级-经典场
    public void onClickJingDianChang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickJingDianChang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickJingDianChang", null, null);
            return;
        }

        m_wantInGameRoomType = TLJCommon.Consts.GameRoomType_XiuXian_JingDian_Common;
        GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_Common);
        reqIsJoinRoom();
        //GameLevelChoiceScript.create(GameLevelChoiceScript.GameChangCiType.GameChangCiType_jingdian);
    }

    // 升级-炒底场
    public void onClickChaoDiChang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickChaoDiChang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickChaoDiChang", null, null);
            return;
        }

        m_wantInGameRoomType = TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_Common;
        GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_Common);
        reqIsJoinRoom();
        //GameLevelChoiceScript.create(GameLevelChoiceScript.GameChangCiType.GameChangCiType_chaodi);
    }

    public void onClickXiuXianChang_back()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickXiuXianChang_back"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickXiuXianChang_back", null, null);
            return;
        }

        m_button_xiuxianchang.transform.localScale = new Vector3(1, 1, 1);
        m_button_jingjichang.transform.localScale = new Vector3(1, 1, 1);
        m_xiuxianchang.transform.localScale = new Vector3(0, 0, 0);
        m_button_DDZ_Back.gameObject.SetActive(true);
    }

    public void onClickDDZ_back()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickDDZ_back"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickDDZ_back", null, null);
            return;
        }

        gameObject.transform.Find("Room/Button_EnterXiuXianChang").gameObject.SetActive(false);
        gameObject.transform.Find("Room/Button_EnterJingJiChang").gameObject.SetActive(false);
        m_button_DDZ_Back.gameObject.SetActive(false);

        gameObject.transform.Find("Room/Button_EnterDDZ").gameObject.SetActive(true);
        gameObject.transform.Find("Room/Button_EnterShengji").gameObject.SetActive(true);
    }

    public void OnClickHead()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickHead"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickHead", null, null);
            return;
        }

        UserInfoScript.create();
        //TuiGuangYouLiPanelScript.create();
        //MedalDuiHuanPanelScript.create();
    }

    public void OnClickNotice()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickNotice"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickNotice", null, null);
            return;
        }

        Activity.create();
//        NoticePanelScript.create();
    }

    public void OnClickSign()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickSign"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickSign", null, null);
            return;
        }

        Sign30PanelScript.create();
        //WeeklySignScript.create();
    }

    public void OnClickInventory()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickInventory"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickInventory", null, null);
            return;
        }

        BagPanelScript.create(false);
    }

    public void OnClickYuanBaoShop()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickYuanBaoShop"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickYuanBaoShop", null, null);
            return;
        }

        ShopPanelScript.create(2);
    }

    public void OnClickGoldShop()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickGoldShop"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickGoldShop", null, null);
            return;
        }

        ShopPanelScript.create(1);
    }

    public void OnClickEmail()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickEmail"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickEmail", null, null);
            return;
        }

        EmailPanelScript.create();
    }

    public void OnClickSetting()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickSetting"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickSetting", null, null);
            return;
        }

        SetScript.create(false);
    }

    public void OnClickKeFu()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickKeFu"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickKeFu", null, null);
            return;
        }

        KeFuPanelScript.create();
    }

    public void OnClickTask()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickTask"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickTask", null, null);
            return;
        }

        TaskPanelScript.create();
    }

    public void OnClickShare()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickShare"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickShare", null, null);
            return;
        }

        ChoiceShareScript.Create("疯狂升级天天玩，玩就有话费奖品抱回家！", "");
    }

    public void onClickLaBa()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickLaBa"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickLaBa", null, null);
            return;
        }

        LaBaPanelScript.create();
    }

    public void OnClickFirstRecharge()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickFirstRecharge"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickFirstRecharge", null, null);
            return;
        }

        ShouChongPanelScript.create();
    }

    public void OnClickTuiGuangYouLi()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickTuiGuangYouLi"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickTuiGuangYouLi", null, null);
            return;
        }

        TuiGuangYouLiPanelScript.create();
    }

    public void OnClickMedalHelp()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickMedalHelp"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickMedalHelp", null, null);
            return;
        }

        GameObject prefab = Resources.Load("Prefabs/UI/Panel/MedalExplainPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
    }

    public void OnClickMedalDuiHuan()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnClickMedalDuiHuan"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnClickMedalDuiHuan", null, null);
            return;
        }

        MedalDuiHuanPanelScript.create();
    }

    public void onClickZhuanPan()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickZhuanPan"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickZhuanPan", null, null);
            return;
        }

        TurntablePanelScript.create();
    }

    public void onClickRetryJoinGame()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickRetryJoinGame"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickRetryJoinGame", null, null);
            return;
        }

        if (m_curInGameRoomType.CompareTo(TLJCommon.Consts.GameRoomType_DDZ_Normal) == 0)
        {
            SceneManager.LoadScene("GameScene_doudizhu");
        }
        else
        {
            GameData.getInstance().m_tag = TLJCommon.Consts.Tag_XiuXianChang;
            GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_Common);
            SceneManager.LoadScene("GameScene");
        }
    }

    //-----------------------------------------------------------------------------

    public void reqWaitMatchTimeOut()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "reqWaitMatchTimeOut"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "reqWaitMatchTimeOut", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = TLJCommon.Consts.Tag_JingJiChang;
        jsonData["uid"] = UserData.uid;
        jsonData["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_WaitMatchTimeOut;

        PlayServiceSocket.s_instance.sendMessage(jsonData.ToJson());
    }

    public void reqPVPRoom()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "reqPVPRoom"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "reqPVPRoom", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = TLJCommon.Consts.Tag_GetPVPGameRoom;
        jsonData["uid"] = UserData.uid;
        string requestData = jsonData.ToJson();

        PlayServiceSocket.s_instance.sendMessage(requestData);
    }

    // 是否已经加入房间
    public void reqIsJoinRoom()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "reqIsJoinRoom"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "reqIsJoinRoom", null, null);
            return;
        }

        NetLoading.getInstance().Show();

        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_IsJoinGame;
        data["uid"] = UserData.uid;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    //-----------------------------------------------------------------------------
    public void onReceive_Main(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onReceive_Main"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onReceive_Main", null, data);
            return;
        }

        JsonData jd = JsonMapper.ToObject(data);
        string tag = (string) jd["tag"];

        // 有人使用喇叭
        if (tag.CompareTo(TLJCommon.Consts.Tag_Broadcast_LaBa) == 0)
        {
            string text = (string) jd["text"];

            m_laBaScript.addText(text);
        }
        // 强制离线
        else if (tag.CompareTo(TLJCommon.Consts.Tag_ForceOffline) == 0)
        {
            Destroy(LogicEnginerScript.Instance.gameObject);
            Destroy(PlayServiceSocket.s_instance.gameObject);

            GameObject obj = CommonExitPanelScript.create();
            obj.GetComponent<CommonExitPanelScript>().ButtonConfirm.onClick.RemoveAllListeners();
            obj.GetComponent<CommonExitPanelScript>().ButtonConfirm.onClick.AddListener(delegate()
            {
                OtherData.s_isFromSetToLogin = true;
                SceneManager.LoadScene("LoginScene");
            });
        }
        // 购买元宝结果通知
        else if (tag.CompareTo(TLJCommon.Consts.Tag_BuyYuanBao) == 0)
        {
            GameObject go = GameObject.Find("PayTypePanel(Clone)");
            if (go != null)
            {
                Destroy(go);
            }
            var code = (int) jd["code"];
            if (code == (int) Consts.Code.Code_OK)
            {
                LogicEnginerScript.Instance.GetComponent<GetUserInfoRequest>().OnRequest();
                ToastScript.createToast("支付成功");
            }
            else
            {
                ToastScript.createToast("支付失败");
            }
        }
        // 有人使用转盘
        else if (tag.CompareTo(TLJCommon.Consts.Tag_TurntableBroadcast) == 0)
        {
            if (TurntablePanelScript.s_instance != null)
            {
                TurntablePanelScript.s_instance.GetComponent<TurntablePanelScript>().onReceive_TurntableBroadcast(data);
            }
        }
        // 救济金
        else if (tag.CompareTo(TLJCommon.Consts.Tag_SupplyGold) == 0)
        {
            int todayCount = (int)jd["todayCount"];
            int goldNum = (int)jd["goldNum"];

            GameUtil.changeDataWithStr("1:" + goldNum);

            if (todayCount == 1)
            {
                ToastScript.createToast("金币低于1500，今日第一次赠送金币" + goldNum);
            }
            else if (todayCount == 2)
            {
                ToastScript.createToast("金币低于1500，今日第二次赠送金币" + goldNum);
            }
            else if (todayCount == 3)
            {
                ToastScript.createToast("金币低于1500，今日最后一次赠送金币" + goldNum);
            }
        }
        else
        {
            LogUtil.Log("onReceive_Main：未知tag");
        }
    }

    public void onReceive_GetUserBag(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onReceive_GetUserBag"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onReceive_GetUserBag", null, data);
            return;
        }

        {
            LogUtil.Log("处理背包回调");
            JsonData jsonData = JsonMapper.ToObject(data);
            var code = (int)jsonData["code"];
            if (code == (int)Consts.Code.Code_OK)
            {
                UserData.propData = JsonMapper.ToObject<List<UserPropData>>(jsonData["prop_list"].ToString());
                for (int i = 0; i < PropData.getInstance().getPropInfoList().Count; i++)
                {
                    PropInfo propInfo = PropData.getInstance().getPropInfoList()[i];
                    for (int j = 0; j < UserData.propData.Count; j++)
                    {
                        UserPropData userPropData = UserData.propData[j];
                        if (propInfo.m_id == userPropData.prop_id)
                        {
                            userPropData.prop_icon = propInfo.m_icon;
                            userPropData.prop_name = propInfo.m_name;
                        }
                    }
                }
            }
            else
            {
                ToastScript.createToast("用户背包数据错误");
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------

    public void onSocketReceive_Play(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onSocketReceive_Play"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onSocketReceive_Play", null, data);
            return;
        }

        LogUtil.Log("Play:收到服务器消息:" + data);

        JsonData jd = JsonMapper.ToObject(data);
        string tag = (string) jd["tag"];

        if (tag.CompareTo(TLJCommon.Consts.Tag_JingJiChang) == 0)
        {
            int playAction = (int)jd["playAction"];

            switch (playAction)
            {
                // 加入游戏
                case (int)TLJCommon.Consts.PlayAction.PlayAction_JoinGame:
                    {
                        doTask_PlayAction_JoinGame(data);
                    }
                    break;

                // 退出游戏
                case (int)TLJCommon.Consts.PlayAction.PlayAction_ExitPVP:
                    {
                        doTask_PlayAction_ExitPVP(data);
                    }
                    break;

                // 开始游戏
                case (int)TLJCommon.Consts.PlayAction.PlayAction_StartGame:
                    {
                        doTask_PlayAction_StartGame(data);
                    }
                    break;

                // 匹配失败
                case (int)TLJCommon.Consts.PlayAction.PlayAction_MatchFail:
                    {
                        if (m_waitMatchPanel != null)
                        {
                            Destroy(m_waitMatchPanel);
                            m_waitMatchPanel = null;
                        }

                        NetErrorPanelScript.getInstance().Show();
                        NetErrorPanelScript.getInstance().setOnClickButton(null);
                        NetErrorPanelScript.getInstance().setContentText("匹配队友失败，请稍后再试。");
                    }
                    break;
            }
        }
        // 获取pvp场次信息
        else if (tag.CompareTo(TLJCommon.Consts.Tag_GetPVPGameRoom) == 0)
        {
            NetLoading.getInstance().Close();

            PVPGameRoomDataScript.getInstance().initJson(data);
        }
        // 是否已经加入房间
        else if (tag.CompareTo(TLJCommon.Consts.Tag_IsJoinGame) == 0)
        {
            doTask_PlayAction_IsJoinGame(data);
        }
    }

    public void doTask_PlayAction_IsJoinGame(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "doTask_PlayAction_IsJoinGame"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "doTask_PlayAction_IsJoinGame", null, data);
            return;
        }

        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(data);
        int isJoinGame = (int)jd["isJoinGame"];

        if (isJoinGame == 1)
        {
            m_curInGameRoomType = jd["gameRoomType"].ToString();
            string roonName = GameUtil.getRoomName(m_curInGameRoomType);
            HasInRoomPanelScript.create("您当前正在：\n" +  roonName +"\n进行游戏，点击确认回到该房间。", onClickRetryJoinGame);
        }
        else
        {
            // 斗地主
            if (m_wantInGameRoomType.CompareTo(TLJCommon.Consts.GameRoomType_DDZ_Normal) == 0)
            {
                SceneManager.LoadScene("GameScene_doudizhu");
            }
            // 升级
            else
            {
                GameData.getInstance().m_tag = TLJCommon.Consts.Tag_XiuXianChang;
                SceneManager.LoadScene("GameScene");
            }
        }
    }

    public void doTask_PlayAction_JoinGame(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "doTask_PlayAction_JoinGame"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "doTask_PlayAction_JoinGame", null, data);
            return;
        }

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int) jd["code"];

        switch (code)
        {
            case (int) TLJCommon.Consts.Code.Code_OK:
            {
                int roomId = (int) jd["roomId"];
                string gameroomtype = (string) jd["gameRoomType"].ToString();

                ToastScript.createToast("报名成功");

                showWaitMatchPanel(GameData.getInstance().m_waitMatchTime, gameroomtype);

                // 扣除报名费
                {
                    string baomingfei = PVPGameRoomDataScript.getInstance().getDataByRoomType(gameroomtype).baomingfei;
                    if (baomingfei.CompareTo("0") != 0)
                    {
                        List<string> tempList = new List<string>();
                        CommonUtil.splitStr(baomingfei, tempList, ':');
                        GameUtil.changeData(int.Parse(tempList[0]), -int.Parse(tempList[1]));

                        if (int.Parse(tempList[0]) == 1)
                        {
                            OtherData.s_pvpChoiceScript.showMyBaoMingFei(true);
                        }
                        else
                        {
                            OtherData.s_pvpChoiceScript.showMyBaoMingFei(false);
                        }
                    }
                }
            }
            break;

            case (int) TLJCommon.Consts.Code.Code_CommonFail:
            {
                string gameRoomType = jd["gameRoomType"].ToString();
                string roonName = GameUtil.getRoomName(gameRoomType);

                HasInRoomPanelScript.create("您当前正在：\n" + roonName + "\n进行游戏，点击确认回到该房间。", onClickRetryJoinGame);
            }
            break;
        }
    }

    public void doTask_PlayAction_ExitPVP(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "doTask_PlayAction_ExitPVP"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "doTask_PlayAction_ExitPVP", null, data);
            return;
        }

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];

        switch (code)
        {
            case (int)TLJCommon.Consts.Code.Code_OK:
                {
                    int roomId = (int)jd["roomId"];
                    string gameroomtype = jd["gameroomtype"].ToString();

                    PVPGameRoomData pvpGameRoomData = PVPGameRoomDataScript.getInstance().getDataByRoomType(gameroomtype);
                    if (pvpGameRoomData != null)
                    {
                        if (pvpGameRoomData.baomingfei.CompareTo("0") == 0)
                        {
                            ToastScript.createToast("退赛成功");
                        }
                        else
                        {
                            ToastScript.createToast("退赛成功,请到邮箱领取报名费");

                            // 报名费返还通过邮件
                            LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().OnRequest();
                        }
                    }
                }
                break;

            case (int)TLJCommon.Consts.Code.Code_CommonFail:
                {
                    ToastScript.createToast("退赛失败，当前并没有加入房间");
                }
                break;
        }
    }

    public void doTask_PlayAction_StartGame(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "doTask_PlayAction_StartGame"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "doTask_PlayAction_StartGame", null, data);
            return;
        }

        GameData.getInstance().m_startGameJsonData = data;
        SceneManager.LoadScene("GameScene");
    }

    public void checkRedPoint()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "checkRedPoint"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "checkRedPoint", null, null);
            return;
        }

        // 活动
        {
            bool isShowRedPoint = false;

            for (int i = 0; i < Activity.activityDatas.Count; i++)
            {
                Activity.ActivityData activityData = Activity.activityDatas[i];
                string data = UserData.uid + "activity" + activityData.ActivityId;
                string s = PlayerPrefs.GetString(data);
                if (string.IsNullOrEmpty(s))
                {
                    isShowRedPoint = true;
                    break;
                }
            }

            for (int i = 0; i < Activity.noticeDatas.Count; i++)
            {
                NoticeData noticeData = Activity.noticeDatas[i];
                string data = UserData.uid + "notice" + noticeData.notice_id;
                string s = PlayerPrefs.GetString(data);
                if (string.IsNullOrEmpty(s))
                {
                    isShowRedPoint = true;
                    break;
                }
            }

            if (isShowRedPoint)
            {
                m_notice_redPoint.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                m_notice_redPoint.transform.localScale = new Vector3(0, 0, 0);
            }
        }

        // 任务
        {
            bool isShowRedPoint = false;
            for (int i = 0; i < TaskDataScript.getInstance().getTaskDataList().Count; i++)
            {
                if ((TaskDataScript.getInstance().getTaskDataList()[i].progress ==
                     TaskDataScript.getInstance().getTaskDataList()[i].target) &&
                    (TaskDataScript.getInstance().getTaskDataList()[i].isover == 0))
                {
                    isShowRedPoint = true;
                    break;
                }
            }

            if (isShowRedPoint)
            {
                m_task_redPoint.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                m_task_redPoint.transform.localScale = new Vector3(0, 0, 0);
            }
        }

        // 签到
        {
            if (!Sign30RecordData.getInstance().todayIsSign())
            {
                m_sign_redPoint.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                m_sign_redPoint.transform.localScale = new Vector3(0, 0, 0);
            }
        }

        // 邮件
        {
            bool isShowRedPoint = false;
            for (int i = 0; i < UserMailData.getInstance().getUserMailDataList().Count; i++)
            {
                if (UserMailData.getInstance().getUserMailDataList()[i].m_state == 0)
                {
                    isShowRedPoint = true;
                    break;
                }
            }

            if (isShowRedPoint)
            {
                m_mail_redPoint.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                m_mail_redPoint.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

    //-------------------------------------Logic服务器相关----------------------------------------

    public void onSocketConnect_Logic(bool result)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onSocketConnect_Logic"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onSocketConnect_Logic", null, result);
            return;
        }

        NetLoading.getInstance().Close();

        if (result)
        {
            //LogUtil.Log("连接服务器成功");

            //ToastScript.createToast("连接Logic服务器成功");
            
            NetLoading.getInstance().Close();
            NetErrorPanelScript.getInstance().Close();

            {
                NetLoading.getInstance().Show();

                LogicEnginerScript.Instance.GetComponent<GetUserInfoRequest>().OnRequest();
                LogicEnginerScript.Instance.GetComponent<GetRankRequest>().OnRequest();
                LogicEnginerScript.Instance.GetComponent<GetSignRecordRequest>().OnRequest();
                LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().CallBack = onReceive_GetUserBag;
                LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().OnRequest();
                LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().OnRequest();
                LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().OnRequest();
                LogicEnginerScript.Instance.GetComponent<GetSign30RewardRequest>().OnRequest();
            }

            // 检测服务器是否连接
            checkNet();
        }
        else
        {
            //LogUtil.Log("连接服务器失败，尝试重新连接");

            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
            NetErrorPanelScript.getInstance().setContentText("连接服务器失败，请重新连接");
        }
    }

    public void onSocketClose_Logic()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onSocketClose_Logic"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onSocketClose_Logic", null, null);
            return;
        }

        //LogUtil.Log("被动与服务器断开连接,尝试重新连接");

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
        NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");
    }

    public void onSocketStop_Logic()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onSocketStop_Logic"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onSocketStop_Logic", null, null);
            return;
        }

        //LogUtil.Log("主动与服务器断开连接");

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
        NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");
    }

    // 点击网络断开弹框中的重连按钮
    public void onClickChongLian_Logic()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickChongLian_Logic"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickChongLian_Logic", null, null);
            return;
        }

        NetLoading.getInstance().Show();
        NetErrorPanelScript.getInstance().Close();
        LogicEnginerScript.Instance.startConnect();
    }


    //-------------------------------------Play服务器相关----------------------------------------

    public void onSocketConnect_Play(bool result)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onSocketConnect_Play"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onSocketConnect_Play", null, result);
            return;
        }

        NetLoading.getInstance().Close();

        if (result)
        {
            //LogUtil.Log("连接服务器成功");

            //ToastScript.createToast("连接Play服务器成功");
            
            NetLoading.getInstance().Close();
            NetErrorPanelScript.getInstance().Close();

            // 检测服务器是否连接
            checkNet();
        }
        else
        {
            //LogUtil.Log("连接服务器失败，尝试重新连接");

            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Play);
            NetErrorPanelScript.getInstance().setContentText("连接逻辑服务器失败，请重新连接");
        }
    }

    public void onSocketClose_Play()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onSocketClose_Play"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onSocketClose_Play", null, null);
            return;
        }

        //LogUtil.Log("被动与服务器断开连接,尝试重新连接");

        if (m_waitMatchPanel != null)
        {
            Destroy(m_waitMatchPanel);
            m_waitMatchPanel = null;
        }

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Play);
        NetErrorPanelScript.getInstance().setContentText("与游戏服务器断开连接，请重新连接");
    }

    public void onSocketStop_Play()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onSocketStop_Play"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onSocketStop_Play", null, null);
            return;
        }

        //LogUtil.Log("主动与服务器断开连接");

        if (m_waitMatchPanel != null)
        {
            Destroy(m_waitMatchPanel);
            m_waitMatchPanel = null;
        }

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Play);
        NetErrorPanelScript.getInstance().setContentText("与游戏服务器断开连接，请重新连接");
    }

    // 点击网络断开弹框中的重连按钮
    public void onClickChongLian_Play()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickChongLian_Play"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickChongLian_Play", null, null);
            return;
        }

        NetLoading.getInstance().Show();
        NetErrorPanelScript.getInstance().Close();
        PlayServiceSocket.s_instance.startConnect();
    }

    //--------------------------------------------------------------------------------------------------
    public void onPauseCallBack()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onPauseCallBack"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onPauseCallBack", null, null);
            return;
        }

        //LogicEnginerScript.Instance.Stop();
        //PlayServiceSocket.s_instance.Stop();
    }

    public void onResumeCallBack()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onResumeCallBack"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onResumeCallBack", null, null);
            return;
        }

        //if (m_waitMatchPanel != null)
        //{
        //    Destroy(m_waitMatchPanel);
        //    m_waitMatchPanel = null;
        //}

        //NetErrorPanelScript.getInstance().Show();
        //NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
        //NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");

        checkNet();
    }
}