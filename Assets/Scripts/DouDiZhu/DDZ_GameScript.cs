﻿using CrazyLandlords.Helper;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class DDZ_GameScript : MonoBehaviour {

    public string m_hotfix_class = "GameScript_doudizhu_hotfix";
    public string m_hotfix_path = "HotFix_Project.GameScript_doudizhu_hotfix";

    public Button m_buttonStartGame;

    public GameObject m_startChuPai;
    public Button m_buttonBuChu;
    public Button m_buttonTiShi;
    public Button m_buttonChuPai;
    public Button m_buttonTuoGuan;

    public Text m_text_beishu;

    public GameObject m_waitMatchPanel = null;
    public GameObject m_qiangDiZhu = null;
    public GameObject m_jiabang = null;
    public GameObject m_waitjiabang = null;
    public GameObject m_dipaiObj = null;

    public GameObject m_playerHead_up = null;
    public GameObject m_playerHead_down = null;
    public GameObject m_playerHead_left = null;
    public GameObject m_playerHead_right = null;

    public DDZ_NetReqLogic m_DDZ_NetReqLogic = null;

    // 倒计时
    public GameObject m_timer;

    // 托管
    public GameObject m_tuoguanObj = null;

    public TimerScript m_timerScript;

    public int m_fuwufei = 500;

    // Use this for initialization
    void Start ()
    {
        OtherData.s_ddz_gameScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "Start", null, null);
            return;
        }

        // 禁止多点触摸
        Input.multiTouchEnabled = false;

        ToastScript.clear();

        // 安卓回调
        AndroidCallBack.s_onPauseCallBack = onPauseCallBack;
        AndroidCallBack.s_onResumeCallBack = onResumeCallBack;

        AudioScript.getAudioScript().stopMusic();

        DDZ_GameData.getInstance().m_tag = TLJCommon.Consts.Tag_DouDiZhu_Game;
        DDZ_GameData.getInstance().m_gameRoomType = TLJCommon.Consts.GameRoomType_DDZ_Normal;

        m_DDZ_NetReqLogic = gameObject.GetComponent<DDZ_NetReqLogic>();

        initUI();
        initData();
        initUI_Image();

        m_DDZ_NetReqLogic.reqIsJoinRoom();

        AudioScript.getAudioScript().stopMusic();
        AudioScript.getAudioScript().playMusic_DDZ();
    }

    public void initUI()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "initUI"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "initUI", null, null);
            return;
        }

        // 初始化定时器
        {
            m_timer = TimerScript.createTimer();
            m_timerScript = m_timer.GetComponent<TimerScript>();
            m_timerScript.setOnTimerEvent_TimeEnd(onTimerEventTimeEnd);
        }

        GameUtil.hideGameObject(m_qiangDiZhu);
        GameUtil.hideGameObject(m_jiabang);
        GameUtil.hideGameObject(m_waitjiabang);

        GameUtil.hideGameObject(m_playerHead_up);
        GameUtil.hideGameObject(m_playerHead_left);
        GameUtil.hideGameObject(m_playerHead_right);

        GameUtil.showGameObject(m_buttonStartGame.gameObject);
        GameUtil.hideGameObject(m_startChuPai);

        m_playerHead_down.transform.Find("Text_name").GetComponent<Text>().text = UserData.name;
        m_playerHead_down.transform.Find("Text_gold").GetComponent<Text>().text = UserData.gold.ToString();
    }

    public void initData()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "initData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "initData", null, null);
            return;
        }

        m_playerHead_down.name = UserData.uid;

        // 游戏服务器
        PlayServiceSocket.s_instance.setOnPlayService_Connect(onSocketConnect_Play);
        PlayServiceSocket.s_instance.setOnPlayService_Receive(onSocketReceive_Play);
        PlayServiceSocket.s_instance.setOnPlayService_Close(onSocketClose_Play);

        // 逻辑服务器
        LogicEnginerScript.Instance.GetComponent<MainRequest>().CallBack = onReceive_Main;
        LogicEnginerScript.Instance.setOnLogicService_Connect(onSocketConnect_Logic);
        LogicEnginerScript.Instance.setOnLogicService_Close(onSocketClose_Logic);

        if (PlayServiceSocket.s_instance != null)
        {
            HeartBeat_Play.getInstance().startHeartBeat();
        }
        else
        {
            LogUtil.Log("PlayServiceSocket.s_instance == null");
        }
    }

    public void initUI_Image()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "initUI_Image"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "initUI_Image", null, null);
            return;
        }

        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_bg").GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_bg");
    }

    public void startGame_InitUI(string jsonData)
    {
        try
        {
            // 优先使用热更新的代码
            if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "startGame_InitUI"))
            {
                ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "startGame_InitUI", null, jsonData);
                return;
            }

            DDZ_GameData.getInstance().m_isStartGame = true;

            {
                //禁用开始游戏按钮
                m_buttonStartGame.transform.localScale = new Vector3(0, 0, 0);

                m_playerHead_left.transform.localScale = new Vector3(1,1,1);
                m_playerHead_right.transform.localScale = new Vector3(1,1,1);
            }

            JsonData jd = JsonMapper.ToObject(jsonData);

            // 显示所有玩家的头像、昵称、金币
            {
                int myIndex = 0;
                for (int i = 0; i < jd["userList"].Count; i++)
                {
                    if (jd["userList"][i]["uid"].ToString().CompareTo(UserData.uid) == 0)
                    {
                        myIndex = i;

                        break;
                    }
                }

                switch (myIndex)
                {
                    case 0:
                        {
                            m_playerHead_down.name = jd["userList"][0]["uid"].ToString();
                            m_playerHead_right.name = jd["userList"][1]["uid"].ToString();
                            m_playerHead_left.name = jd["userList"][2]["uid"].ToString();
                        }
                        break;

                    case 1:
                        {
                            m_playerHead_down.name = jd["userList"][1]["uid"].ToString();
                            m_playerHead_right.name = jd["userList"][2]["uid"].ToString();
                            m_playerHead_left.name = jd["userList"][0]["uid"].ToString();
                        }
                        break;

                    case 2:
                        {
                            m_playerHead_down.name = jd["userList"][2]["uid"].ToString();
                            m_playerHead_right.name = jd["userList"][0]["uid"].ToString();
                            m_playerHead_left.name = jd["userList"][1]["uid"].ToString();
                        }
                        break;
                }
            }

            // 本桌所有人信息
            for (int i = 0; i < jd["userList"].Count; i++)
            {
                string uid = jd["userList"][i]["uid"].ToString();

                DDZ_GameData.getInstance().m_playerDataList.Add(new PlayerData(uid));
            }
        }
        catch (Exception ex)
        {
            LogUtil.Log("startGame_InitUI()报错：" + ex.Message);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnDestroy()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "OnDestroy"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "OnDestroy", null, null);
            return;
        }

        OtherData.s_ddz_gameScript = null;

        if (HeartBeat_Play.getInstance() != null)
        {
            HeartBeat_Play.getInstance().stopHeartBeat();
        }

    }

    public GameObject getPlayerHeadByUid(string uid)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "getPlayerHeadByUid"))
        {
            GameObject obj = (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "getPlayerHeadByUid", null, uid);
            return obj;
        }

        if (uid.CompareTo(m_playerHead_down.transform.name) == 0)
        {
            return m_playerHead_down;
        }
        else if (uid.CompareTo(m_playerHead_up.transform.name) == 0)
        {
            return m_playerHead_up;
        }
        else if (uid.CompareTo(m_playerHead_left.transform.name) == 0)
        {
            return m_playerHead_left;
        }
        else if (uid.CompareTo(m_playerHead_right.transform.name) == 0)
        {
            return m_playerHead_right;
        }

        return null;
    }

    public void onTimerEventTimeEnd()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onTimerEventTimeEnd"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onTimerEventTimeEnd", null, null);
            return;
        }

        // 全部设为未选中状态
        PokerScript.setAllPokerWeiXuanZe();

        switch (m_timerScript.getTimerType())
        {
            // 抢地主结束
            case TimerScript.TimerType.TimerType_QiangDiZhu:
                {
                    GameUtil.hideGameObject(m_qiangDiZhu);
                }
                break;

            // 加棒结束
            case TimerScript.TimerType.TimerType_JiaBang:
                {
                    GameUtil.hideGameObject(m_jiabang);
                }
                break;
        }
    }

    public void onClickPlayerHead_down()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickPlayerHead_down"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickPlayerHead_down", null, null);
            return;
        }

        GameUserInfoPanelScript.create_ddz(m_playerHead_down.transform.name);
    }

    public void onClickPlayerHead_up()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickPlayerHead_up"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickPlayerHead_up", null, null);
            return;
        }

        GameUserInfoPanelScript.create_ddz(m_playerHead_up.transform.name);
    }

    public void onClickPlayerHead_left()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickPlayerHead_left"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickPlayerHead_left", null, null);
            return;
        }

        GameUserInfoPanelScript.create_ddz(m_playerHead_left.transform.name);
    }

    public void onClickPlayerHead_right()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickPlayerHead_right"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickPlayerHead_right", null, null);
            return;
        }

        GameUserInfoPanelScript.create_ddz(m_playerHead_right.transform.name);
    }

    public void onClickSet()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickSet"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickSet", null, null);
            return;
        }

        SetScript.create(true);
    }

    public void onClickJoinRoom()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickJoinRoom"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickJoinRoom", null, null);
            return;
        }

        AudioScript.getAudioScript().playSound_ButtonClick();

        m_DDZ_NetReqLogic.reqJoinRoom(TLJCommon.Consts.GameRoomType_DDZ_Normal);
    }

    public void onClickExitRoom()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickExitRoom"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickExitRoom", null, null);
            return;
        }

        QueRenExitPanelScript.create(this, "是否确定退出？");
    }

    public void onClickBuOutPoker()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickBuOutPoker"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickBuOutPoker", null, null);
            return;
        }

        // 所有牌设为未选中
        PokerScript.setAllPokerWeiXuanZe();

        m_DDZ_NetReqLogic.reqOutPoker();
    }

    public void onClickOutPoker()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickOutPoker"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickOutPoker", null, null);
            return;
        }
        
        bool hasOutPoker = false;
        for (int i = 0; i < DDZ_GameData.getInstance().m_myPokerObjList.Count; i++)
        {
            PokerScript pokerScript = DDZ_GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>();
            if (pokerScript.getIsJump())
            {
                hasOutPoker = true;
                break;
            }
        }

        if (hasOutPoker)
        {
            m_DDZ_NetReqLogic.reqOutPoker();
        }
        else
        {
            ToastScript.createToast("请选择您出的牌");
        }
    }

    public void onClickCustomPoker()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickCustomPoker"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickCustomPoker", null, null);
            return;
        }

        TestPoker.create();
    }

    public List<PokerInfo[]> promptPokers;
    public void onClickTiShi()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickTiShi"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickTiShi", null, null);
            return;
        }

        // 所有牌设为未选中
        PokerScript.setAllPokerWeiXuanZe();

        AudioScript.getAudioScript().playSound_XuanPai();


        if (promptPokers == null || promptPokers.Count == 0)
        {
            LandlordsCardsHelper.SetWeight(DDZ_GameData.getInstance().m_myPokerList);
            LandlordsCardsHelper.SetWeight(DDZ_GameData.getInstance().m_maxPlayerOutPokerList);
            CardsType lastType;
            LandlordsCardsHelper.GetCardsType(DDZ_GameData.getInstance().m_maxPlayerOutPokerList.ToArray(), out lastType);
            promptPokers = LandlordsCardsHelper.GetPrompt(DDZ_GameData.getInstance().m_myPokerList, DDZ_GameData.getInstance().m_maxPlayerOutPokerList, lastType);
            if (promptPokers.Count > 0)
            {
//                List<PokerInfo> listPoker = promptPokers[RandomHelper.RandomNumber(0, promptPokers.Count)].ToList();
                List<PokerInfo> listPoker = promptPokers[0].ToList();
                promptPokers.RemoveAt(0);

                for (int i = 0; i < listPoker.Count; i++)
                {
                    for (int j = DDZ_GameData.getInstance().m_myPokerObjList.Count - 1; j >= 0; j--)
                    {
                        PokerScript pokerScript = DDZ_GameData.getInstance().m_myPokerObjList[j].GetComponent<PokerScript>();

                        if ((pokerScript.getPokerNum() == listPoker[i].m_num) &&
                            (pokerScript.getPokerType() == (int)listPoker[i].m_pokerType))
                        {
                            pokerScript.setIsSelect(true);
                            pokerScript.setIsJump(true);
                            break;
                        }
                    }
                }
            }
            else
            {
                ToastScript.createToast("没有牌可以出");
                onClickBuOutPoker();
            }
        }
        else
        {
            List<PokerInfo> listPoker = promptPokers[0].ToList();
            promptPokers.RemoveAt(0);

            for (int i = 0; i < listPoker.Count; i++)
            {
                for (int j = DDZ_GameData.getInstance().m_myPokerObjList.Count - 1; j >= 0; j--)
                {
                    PokerScript pokerScript = DDZ_GameData.getInstance().m_myPokerObjList[j].GetComponent<PokerScript>();

                    if ((pokerScript.getPokerNum() == listPoker[i].m_num) &&
                        (pokerScript.getPokerType() == (int)listPoker[i].m_pokerType))
                    {
                        pokerScript.setIsSelect(true);
                        pokerScript.setIsJump(true);
                        break;
                    }
                }
            }
        }
    }
    public void onClickChat()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickChat"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickChat", null, null);
            return;
        }

        ChatPanelScript.create(this);
    }

    public void onClickTuoGuan()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickTuoGuan"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickTuoGuan", null, null);
            return;
        }

        AudioScript.getAudioScript().playSound_ButtonClick();

        m_DDZ_NetReqLogic.reqSetTuoGuanState(true);
    }

    public void onClickCancelTuoGuan()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickCancelTuoGuan"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickCancelTuoGuan", null, null);
            return;
        }

        AudioScript.getAudioScript().playSound_ButtonClick();

        m_DDZ_NetReqLogic.reqSetTuoGuanState(false);
    }

    public void onClickQiangDiZhu_0fen()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickQiangDiZhu_0fen"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickQiangDiZhu_0fen", null, null);
            return;
        }

        m_DDZ_NetReqLogic.reqQiangDiZhu(0);
    }

    public void onClickQiangDiZhu_1fen()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickQiangDiZhu_1fen"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickQiangDiZhu_1fen", null, null);
            return;
        }

        m_DDZ_NetReqLogic.reqQiangDiZhu(1);
    }

    public void onClickQiangDiZhu_2fen()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickQiangDiZhu_2fen"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickQiangDiZhu_2fen", null, null);
            return;
        }

        m_DDZ_NetReqLogic.reqQiangDiZhu(2);
    }

    public void onClickQiangDiZhu_3fen()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickQiangDiZhu_3fen"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickQiangDiZhu_3fen", null, null);
            return;
        }

        m_DDZ_NetReqLogic.reqQiangDiZhu(3);
    }

    public void onClickJiaBang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickJiaBang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickJiaBang", null, null);
            return;
        }

        m_DDZ_NetReqLogic.reqJiaBang(1);
    }

    public void onClickBuJiaBang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClickBuJiaBang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClickBuJiaBang", null, null);
            return;
        }

        m_DDZ_NetReqLogic.reqJiaBang(0);
    }

    public void showWaitMatchPanel(float time, bool isContinueGame)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "showWaitMatchPanel"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "showWaitMatchPanel", null, time, isContinueGame);
            return;
        }

        m_waitMatchPanel = WaitMatchPanelScript.create(DDZ_GameData.getInstance().getGameRoomType());
        WaitMatchPanelScript script = m_waitMatchPanel.GetComponent<WaitMatchPanelScript>();
        script.start(DDZ_GameData.getInstance().m_gameRoomType, time, false);
    }

    public void createMyPokerObj()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "createMyPokerObj"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "createMyPokerObj", null, null);
            return;
        }

        // 先删掉旧的
        for (int i = DDZ_GameData.getInstance().m_myPokerObjList.Count - 1; i >= 0; i--)
        {
            Destroy(DDZ_GameData.getInstance().m_myPokerObjList[i]);
            DDZ_GameData.getInstance().m_myPokerObjList.RemoveAt(i);
        }

        for (int i = 0; i < DDZ_GameData.getInstance().m_myPokerList.Count; i++)
        {
            GameObject poker = PokerScript.createPoker();
            poker.transform.SetParent(GameObject.Find("Canvas/MyHandPoker").transform);
            poker.transform.localScale = new Vector3(1, 1, 1);

            poker.GetComponent<PokerScript>().initPoker(DDZ_GameData.getInstance().m_myPokerList[i].m_num,
                (int)DDZ_GameData.getInstance().m_myPokerList[i].m_pokerType);
            poker.GetComponent<PokerScript>().m_canTouch = true;

            DDZ_GameData.getInstance().m_myPokerObjList.Add(poker);
        }

        initMyPokerPos(DDZ_GameData.getInstance().m_myPokerObjList);
    }

    public void createDiPokerObj()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "createDiPokerObj"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "createDiPokerObj", null, null);
            return;
        }

        for (int i = 0; i < DDZ_GameData.getInstance().m_dipaiList.Count; i++)
        {
            GameObject poker = PokerScript.createPoker();
            poker.transform.SetParent(GameObject.Find("Canvas/DiPai").transform);
            poker.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            poker.transform.localPosition = new Vector3(-76.5f + i * 76.5f, 0, 0);

            poker.GetComponent<PokerScript>().initPoker(DDZ_GameData.getInstance().m_dipaiList[i].m_num,(int)DDZ_GameData.getInstance().m_dipaiList[i].m_pokerType);
            poker.GetComponent<PokerScript>().m_canTouch = false;
        }
    }

    public void showOtherOutPoker(List<TLJCommon.PokerInfo> pokerList, string uid)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "showOtherOutPoker"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "showOtherOutPoker", null, pokerList, uid);
            return;
        }

        if (pokerList.Count == 0)
        {
            return;
        }

        // 创建现在出的牌
        List<GameObject> tempList = new List<GameObject>();
        for (int i = 0; i < pokerList.Count; i++)
        {
            GameObject poker = PokerScript.createPoker();
            poker.transform.SetParent(GameObject.Find("Canvas/ChuDePai").transform);
            poker.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            poker.GetComponent<PokerScript>().initPoker(pokerList[i].m_num, (int)pokerList[i].m_pokerType);

            tempList.Add(poker);
        }

        DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_outPokerObjList = tempList;

        initOutPokerPos(tempList, uid);
    }

    public void initOutPokerPos(List<GameObject> objList, string uid)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "initOutPokerPos"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "initOutPokerPos", null, objList, uid);
            return;
        }

        int jiange = 30;

        if (uid.CompareTo(m_playerHead_down.transform.name) == 0)
        {
            for (int i = 0; i < objList.Count; i++)
            {
                int x = CommonUtil.getPosX(objList.Count, jiange, i, 0);
                objList[i].transform.localPosition = new Vector3(x, -85, 0);
            }
        }
        else if (uid.CompareTo(m_playerHead_up.transform.name) == 0)
        {
            for (int i = 0; i < objList.Count; i++)
            {
                int x = CommonUtil.getPosX(objList.Count, jiange, i, 0);
                objList[i].transform.localPosition = new Vector3(x, 130, 0);
            }
        }
        else if (uid.CompareTo(m_playerHead_left.transform.name) == 0)
        {
            for (int i = 0; i < objList.Count; i++)
            {
                float startX = -440;
                objList[i].transform.localPosition = new Vector3(startX + (i * jiange), 0, 0);
            }
        }
        else if (uid.CompareTo(m_playerHead_right.transform.name) == 0)
        {
            for (int i = 0; i < objList.Count; i++)
            {
                float endX = 480;
                objList[i].transform.localPosition = new Vector3(endX - ((objList.Count - i) * jiange), 0, 0);
            }
        }
    }

    public void initMyPokerPos(List<GameObject> objList)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "initMyPokerPos"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "initMyPokerPos", null, objList);
            return;
        }

        int jiange = 35;

        int minJiange = 35;
        int maxJiange = 50;

        jiange = (int)(840.0f / (float)objList.Count);

        if (jiange > maxJiange)
        {
            jiange = maxJiange;
        }

        if (jiange < minJiange)
        {
            jiange = minJiange;
        }

        for (int i = 0; i < objList.Count; i++)
        {
            int x = CommonUtil.getPosX(objList.Count, jiange, i, 0);
            objList[i].transform.localPosition = new Vector3(x, -225, 0);
            objList[i].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            // 设置最后渲染
            objList[i].transform.SetAsLastSibling();
        }
    }

    public void setTimerPos(string uid)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "setTimerPos"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "setTimerPos", null, uid);
            return;
        }

        if (uid.CompareTo(m_playerHead_down.transform.name) == 0)
        {
            m_timer.transform.localPosition = new Vector3(-513, -170, 0);
        }
        else if (uid.CompareTo(m_playerHead_up.transform.name) == 0)
        {
            //m_timer.transform.localPosition = new Vector3(0, 0, 0);
        }
        else if (uid.CompareTo(m_playerHead_left.transform.name) == 0)
        {
            m_timer.transform.localPosition = new Vector3(-513, 177, 0);
        }
        else if (uid.CompareTo(m_playerHead_right.transform.name) == 0)
        {
            m_timer.transform.localPosition = new Vector3(513, 177, 0);
        }
    }

    //-------------------------------------------------------------------------------------------------------

    public void onReceive(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onReceive"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onReceive", null, data);
            return;
        }

        JsonData jd = JsonMapper.ToObject(data);
        string tag = (string)jd["tag"];

        if (tag.CompareTo(TLJCommon.Consts.Tag_DouDiZhu_Game) == 0)
        {
            onReceive_PlayGame(data);
        }
        else if (tag.CompareTo(TLJCommon.Consts.Tag_UserInfo_Game) == 0)
        {
            onReceive_UserInfo_Game(data);
        }
        else if (tag.CompareTo(TLJCommon.Consts.Tag_IsJoinGame) == 0)
        {
            onReceive_IsJoinGame(data);
        }
        else if (tag.CompareTo(TLJCommon.Consts.Tag_ResumeGame) == 0)
        {
            onReceive_ResumeGame(data);
        }
        else if (tag.CompareTo(TLJCommon.Consts.Tag_HeartBeat_Play) == 0)
        {
            HeartBeat_Play.getInstance().onRespond();
        }
        else if (tag.CompareTo(TLJCommon.Consts.Tag_BaoXiangDrop) == 0)
        {
            onReceive_BaoXiangDrop(data);
        }
    }

    public void onReceive_PlayGame(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onReceive_PlayGame"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onReceive_PlayGame", null, data);
            return;
        }

        JsonData jd = JsonMapper.ToObject(data);
        int playAction = (int)jd["playAction"];

        switch (playAction)
        {
            // 加入游戏
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_JoinGame:
                {
                    int code = (int)jd["code"];

                    switch (code)
                    {
                        case (int)TLJCommon.Consts.Code.Code_OK:
                            {
                                int roomId = (int)jd["roomId"];

                                //禁用开始游戏按钮
                                m_buttonStartGame.transform.localScale = new Vector3(0, 0, 0);

                                showWaitMatchPanel(DDZ_GameData.getInstance().m_waitMatchTime, false);
                            }
                            break;

                        case (int)TLJCommon.Consts.Code.Code_CommonFail:
                            {
                                ToastScript.createToast("您已加入其它房间，无法开始新游戏");
                            }
                            break;
                    }
                }
                break;

            // 退出游戏
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_ExitGame:
                {
                    int code = (int)jd["code"];

                    switch (code)
                    {
                        case (int)TLJCommon.Consts.Code.Code_OK:
                            {
                                int roomId = (int)jd["roomId"];
                                //ToastScript.createToast("退出房间成功：" + roomId);
                            }
                            break;

                        case (int)TLJCommon.Consts.Code.Code_CommonFail:
                            {
                                //ToastScript.createToast("退出房间失败，当前并没有加入房间");
                            }
                            break;
                    }
                }
                break;

            // 开始游戏
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_StartGame:
                {
                    Destroy(m_waitMatchPanel);

                    // 扣除报名费
                    {
                        if (DDZ_GameData.getInstance().getGameRoomType().CompareTo(TLJCommon.Consts.GameRoomType_DDZ_Normal) == 0)
                        {
                            GameUtil.changeData(1, -m_fuwufei);
                            m_playerHead_down.transform.Find("Text_gold").GetComponent<Text>().text = UserData.gold.ToString();
                        }
                    }

                    startGame_InitUI(data);
                }
                break;

            // 发牌
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_FaPai:
                {
                    int num = (int)jd["num"];
                    int pokerType = (int)jd["pokerType"];

                    int isEnd = (int)jd["isEnd"];

                    DDZ_GameData.getInstance().m_myPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));

                    // 对我的牌进行排序
                    CrazyLandlords.Helper.LandlordsCardsHelper.SetWeight(DDZ_GameData.getInstance().m_myPokerList);

                    createMyPokerObj(); // 创建我的牌对象
                }
                break;

            // 通知玩家抢地主
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_CallPlayerQiangDiZhu:
                {
                    int curMaxFen = (int)jd["curMaxFen"];
                    string uid = (string)jd["curJiaoDiZhuUid"];

                    if (UserData.uid.CompareTo(uid) == 0)
                    {
                        m_qiangDiZhu.transform.localScale = new Vector3(1.2f, 1.2f,1.2f);
                    }

                    {
                        // 开始倒计时
                        setTimerPos(uid);
                        m_timerScript.start(DDZ_GameData.getInstance().m_qiangDiZhuTime, TimerScript.TimerType.TimerType_QiangDiZhu, true);
                    }
                }
                break;

            // 抢地主
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_QiangDiZhu:
                {
                    int code = (int)jd["code"];
                    string uid = (string)jd["uid"];

                    if (code != (int)TLJCommon.Consts.Code.Code_OK)
                    {
                        if (UserData.uid.CompareTo(uid) == 0)
                        {
                            string msg = (string)jd["msg"];
                            ToastScript.createToast(msg);

                            return;
                        }
                    }
                    
                    int fen = (int)jd["fen"];

                    // 图片提示
                    {
                        string imgName = "doudizhu_" + fen + "fen";

                        if (uid.CompareTo(m_playerHead_down.transform.name) == 0)
                        {
                            ShowImageScript.create(CommonUtil.getImageSpriteByAssetBundle("doudizhu.unity3d", imgName), new Vector3(-490, -188, 0));
                        }
                        else if (uid.CompareTo(m_playerHead_left.transform.name) == 0)
                        {
                            ShowImageScript.create(CommonUtil.getImageSpriteByAssetBundle("doudizhu.unity3d", imgName), new Vector3(-490, 160, 0));
                        }
                        if (uid.CompareTo(m_playerHead_right.transform.name) == 0)
                        {
                            ShowImageScript.create(CommonUtil.getImageSpriteByAssetBundle("doudizhu.unity3d", imgName), new Vector3(490, 160, 0));
                        }
                    }

                    if (UserData.uid.CompareTo(uid) == 0)
                    {
                        GameUtil.hideGameObject(m_qiangDiZhu);
                    }

                    AudioScript.getAudioScript().playSound_QiangDiZhu(fen);

                    // 停止倒计时
                    m_timerScript.stop();
                }
                break;

            // 通知玩家谁是地主
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_WhoIsDiZhu:
                {
                    string uid = (string)jd["uid"];

                    GameObject playerHead = getPlayerHeadByUid(uid);
                    CommonUtil.setImageSpriteByAssetBundle(playerHead.GetComponent<Image>(),"doudizhu.unity3d", "doudizhu_dizhu");

                    // 倍数
                    {
                        int my_beishu = (int)jd["beishu_" + UserData.uid];
                        m_text_beishu.text = ("X" + my_beishu);
                    }

                    // 把底牌加上去
                    {
                        DDZ_GameData.getInstance().m_dipaiList.Clear();

                        for (int i = 0; i < jd["diPokerList"].Count; i++)
                        {
                            int num = (int)jd["diPokerList"][i]["num"];
                            int pokerType = (int)jd["diPokerList"][i]["pokerType"];

                            if (UserData.uid.CompareTo(uid) == 0)
                            {
                                DDZ_GameData.getInstance().m_myPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                            }

                            DDZ_GameData.getInstance().m_dipaiList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                        }

                        createDiPokerObj();
                    }

                    if (UserData.uid.CompareTo(uid) == 0)
                    {
                        DDZ_GameData.getInstance().m_isDiZhu = 1;

                        // 对我的牌进行排序
                        CrazyLandlords.Helper.LandlordsCardsHelper.SetWeight(DDZ_GameData.getInstance().m_myPokerList);

                        createMyPokerObj(); // 创建我的牌对象

                        m_waitjiabang.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    }

                    // 剩余牌数
                    {
                        GameUtil.showGameObject(m_playerHead_left.transform.Find("Image_shengyupaishu").gameObject);
                        GameUtil.showGameObject(m_playerHead_right.transform.Find("Image_shengyupaishu").gameObject);
                        
                        GameObject obj = getPlayerHeadByUid(uid);
                        obj.transform.Find("Image_shengyupaishu/Text_num").GetComponent<Text>().text = "20";
                    }
                }
                break;

            // 没人抢地主
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_NoOneQiangDiZhu:
                {
                    ToastScript.createToast("没有人抢地主，重新发牌");

                    DDZ_GameData.getInstance().m_myPokerList.Clear();
                }
                break;

            // 通知玩家加棒
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_CallPlayerJiaBang:
                {
                    m_jiabang.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

                    {
                        // 开始倒计时
                        m_timer.transform.localPosition = new Vector3(0, 0, 0);
                        m_timerScript.start(DDZ_GameData.getInstance().m_jiabangTime, TimerScript.TimerType.TimerType_JiaBang, true);
                    }
                }
                break;

            // 加棒
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_JiaBang:
                {
                    // 倍数
                    {
                        int my_beishu = (int)jd["beishu_" + UserData.uid];
                        m_text_beishu.text = ("X" + my_beishu);
                    }

                    string uid = (string)jd["uid"];
                    int isJiaBang = (int)jd["isJiaBang"];

                    if (isJiaBang == 1)
                    {
                        AudioScript.getAudioScript().playSound_DouDiZhu_jiabang();

                        GameObject playerHead = getPlayerHeadByUid(uid);
                        if (playerHead != null)
                        {
                            GameUtil.showGameObject(playerHead.transform.Find("Image_bangzi").gameObject);
                        }
                    }

                    if (UserData.uid.CompareTo(uid) == 0)
                    {
                        GameUtil.hideGameObject(m_jiabang);

                        // 停止倒计时
                        m_timerScript.stop();
                    }
                }
                break;

            // 通知某人出牌
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_CallPlayerOutPoker:
                {
                    GameUtil.hideGameObject(m_waitjiabang);

                    try
                    {
                        string uid = (string)jd["uid"];
                        bool isFreeOutPoker = (bool)jd["isFreeOutPoker"];

                        // 任意出
                        if (isFreeOutPoker)
                        {
                            DDZ_GameData.getInstance().m_maxPlayerOutPokerUID = uid;
                            // 清空每个人座位上的牌
                            {
                                for (int i = 0; i < DDZ_GameData.getInstance().m_playerDataList.Count; i++)
                                {
                                    for (int j = DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Count - 1;j >= 0;j--)
                                    {
                                        Destroy(DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList[j]);
                                    }
                                    DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Clear();
                                }
                            }
                        }

                        // 检测是否轮到自己出牌
                        {
                            if (uid.CompareTo(UserData.uid) == 0)
                            {
                                if (isFreeOutPoker)
                                {
                                    GameUtil.hideGameObject(m_buttonBuChu.gameObject);
                                    GameUtil.hideGameObject(m_buttonTiShi.gameObject);
                                }
                                else
                                {
                                    GameUtil.showGameObject(m_buttonBuChu.gameObject);
                                    GameUtil.showGameObject(m_buttonTiShi.gameObject);
                                }

                                // 清空此人之前出的牌
                                {
                                    for (int i = 0; i < DDZ_GameData.getInstance().m_playerDataList.Count; i++)
                                    {
                                        if (DDZ_GameData.getInstance().m_playerDataList[i].m_uid.CompareTo(uid) == 0)
                                        {
                                            for (int j = DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Count - 1; j >= 0; j--)
                                            {
                                                Destroy(DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList[j]);
                                            }
                                            DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Clear();

                                            break;
                                        }
                                    }
                                }

                                DDZ_GameData.getInstance().m_isFreeOutPoker = isFreeOutPoker;

                                m_startChuPai.transform.localScale = new Vector3(1.2f, 1.2f,1.2f);

                                // 开始出牌倒计时
                                m_timerScript.start(DDZ_GameData.getInstance().m_outPokerTime, TimerScript.TimerType.TimerType_OutPoker, true);
                                setTimerPos(uid);
                            }
                            else
                            {
                                GameUtil.hideGameObject(m_startChuPai);

                                // 开始出牌倒计时
                                m_timerScript.start(DDZ_GameData.getInstance().m_outPokerTime, TimerScript.TimerType.TimerType_OutPoker, false);
                                setTimerPos(uid);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //ToastScript.createToast("异常：" + ex.Message);
                    }
                }
                break;

            // 玩家出牌
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_PlayerOutPoker:
                {
                    try
                    {
                        // 倍数
                        {
                            int my_beishu = (int)jd["beishu_" + UserData.uid];
                            m_text_beishu.text = ("X" + my_beishu);
                        }

                        m_timerScript.stop();

                        string uid = (string)jd["uid"];
                        int restPokerCount = (int)jd["restPokerCount"];

                        // 清空此人之前出的牌
                        {
                            for (int i = 0; i < DDZ_GameData.getInstance().m_playerDataList.Count; i++)
                            {
                                if (DDZ_GameData.getInstance().m_playerDataList[i].m_uid.CompareTo(uid) == 0)
                                {
                                    for (int j = DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Count - 1; j >= 0; j--)
                                    {
                                        Destroy(DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList[j]);
                                    }
                                    DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Clear();

                                    break;
                                }
                            }
                        }

                        // 出牌列表
                        List<TLJCommon.PokerInfo> outPokerList = new List<TLJCommon.PokerInfo>();
                        {
                            for (int i = 0; i < jd["pokerList"].Count; i++)
                            {
                                int num = (int)jd["pokerList"][i]["num"];
                                int pokerType = (int)jd["pokerList"][i]["pokerType"];

                                outPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                            }
                        }

                        // 不出
                        if (outPokerList.Count == 0)
                        {
                            if (uid.CompareTo(m_playerHead_down.transform.name) == 0)
                            {
                                ShowImageScript.create(CommonUtil.getImageSpriteByAssetBundle("doudizhu.unity3d", "doudizhu_buchu_text"), new Vector3(-490, -188, 0));
                            }
                            else if (uid.CompareTo(m_playerHead_left.transform.name) == 0)
                            {
                                ShowImageScript.create(CommonUtil.getImageSpriteByAssetBundle("doudizhu.unity3d", "doudizhu_buchu_text"), new Vector3(-490, 160, 0));
                            }
                            if (uid.CompareTo(m_playerHead_right.transform.name) == 0)
                            {
                                ShowImageScript.create(CommonUtil.getImageSpriteByAssetBundle("doudizhu.unity3d", "doudizhu_buchu_text"), new Vector3(490, 160, 0));
                            }
                        }
                        else
                        {
                            DDZ_GameData.getInstance().m_maxPlayerOutPokerList = outPokerList;
                        }                        

                        //// 出牌类型
                        //{
                        //    CheckOutPoker.OutPokerType outPokerType = CheckOutPoker.checkOutPokerType(outPokerList, GameData.getInstance().m_levelPokerNum, GameData.getInstance().m_masterPokerType);
                        //    switch (outPokerType)
                        //    {
                        //        case CheckOutPoker.OutPokerType.OutPokerType_TuoLaJi:
                        //            {
                        //                AudioScript.getAudioScript().playSound_TuoLaJi();
                        //            }
                        //            break;
                        //    }
                        //}

                        // 显示出的牌
                        showOtherOutPoker(outPokerList, uid);

                        AudioScript.getAudioScript().playSound_DouDiZhu_ChuPai(outPokerList, uid);

                        // 如果是自己出的牌，那么就得删掉这些牌
                        if (uid.CompareTo(UserData.uid) == 0)
                        {
                            AudioScript.getAudioScript().playSound_ChuPai();

                            GameUtil.hideGameObject(m_startChuPai);

                            for (int i = 0; i < outPokerList.Count; i++)
                            {
                                int num = outPokerList[i].m_num;
                                int pokerType = (int)outPokerList[i].m_pokerType;

                                for (int j = DDZ_GameData.getInstance().m_myPokerObjList.Count - 1; j >= 0; j--)
                                {
                                    PokerScript pokerScript = DDZ_GameData.getInstance().m_myPokerObjList[j].GetComponent<PokerScript>();
                                    if ((pokerScript.getPokerNum() == num) && (pokerScript.getPokerType() == pokerType))
                                    {
                                        // 出的牌从自己的牌堆实体对象里删除
                                        {
                                            Destroy(DDZ_GameData.getInstance().m_myPokerObjList[j]);
                                            DDZ_GameData.getInstance().m_myPokerObjList.RemoveAt(j);
                                        }

                                        break;
                                    }
                                }

                                for (int j = DDZ_GameData.getInstance().m_myPokerList.Count - 1; j >= 0; j--)
                                {
                                    if ((DDZ_GameData.getInstance().m_myPokerList[j].m_num == num) &&
                                        ((int)DDZ_GameData.getInstance().m_myPokerList[j].m_pokerType == pokerType))
                                    {
                                        // 出的牌从自己的牌堆内存里删除
                                        {
                                            DDZ_GameData.getInstance().m_myPokerList.RemoveAt(j);
                                        }

                                        break;
                                    }
                                }
                            }

                            PokerScript.setAllPokerWeiXuanZe();
                            initMyPokerPos(DDZ_GameData.getInstance().m_myPokerObjList);
                        }
                        else
                        {
                            GameObject playerHead = getPlayerHeadByUid(uid);

                            // 剩余牌数
                            playerHead.transform.Find("Image_shengyupaishu/Text_num").GetComponent<Text>().text = restPokerCount.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("异常："+ ex);
                        //ToastScript.createToast("异常：" + ex.Message);
                    }
                }
                //清除出牌提示
                promptPokers = null;
                break;

            // 改变托管状态
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_SetTuoGuanState:
                {
                    try
                    {
                        NetLoading.getInstance().Close();

                        int code = (int)jd["code"];

                        if (code == (int)TLJCommon.Consts.Code.Code_OK)
                        {
                            string uid = (string)jd["uid"];
                            bool isTuoGuan = (bool)jd["isTuoGuan"];
                            
                            // 托管
                            if (isTuoGuan)
                            {
                                m_tuoguanObj = TuoGuanPanelScript.create(this);
                                CommonUtil.setImageSpriteByAssetBundle(m_buttonTuoGuan.transform.Find("Image").GetComponent<Image>(), "game.unity3d", "game_yituoguan");

                                DDZ_GameData.getInstance().m_isTuoGuan = true;
                            }
                            // 取消托管
                            else
                            {
                                CommonUtil.setImageSpriteByAssetBundle(m_buttonTuoGuan.transform.Find("Image").GetComponent<Image>(), "game.unity3d", "game_tuoguan");

                                DDZ_GameData.getInstance().m_isTuoGuan = false;
                            }
                        }
                        else
                        {
                            ToastScript.createToast("改变托管状态失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        //ToastScript.createToast("异常：" + ex.Message);
                    }
                }
                break;

            // 游戏结束
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_GameOver:
                {
                    try
                    {
                        // 处理
                        {
                            Destroy(m_tuoguanObj);
                            CommonUtil.setImageSpriteByAssetBundle(m_buttonTuoGuan.transform.Find("Image").GetComponent<Image>(), "game.unity3d", "game_tuoguan");
                        }

                        DDZ_GameResult.create(this, data);
                    }
                    catch (Exception ex)
                    {
                        //ToastScript.createToast("异常：" + ex.Message);
                    }
                }
                break;
                
            // 强制解散房间
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_BreakRoom:
                {
                    NetErrorPanelScript.getInstance().Show();
                    NetErrorPanelScript.getInstance().setOnClickButton(exitRoom);
                    NetErrorPanelScript.getInstance().setContentText("该房间超时，强制解散房间");
                }
                break;

            // 匹配失败
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_MatchFail:
                {
                    NetErrorPanelScript.getInstance().Show();
                    NetErrorPanelScript.getInstance().setOnClickButton(exitRoom);
                    NetErrorPanelScript.getInstance().setContentText("匹配队友失败，请稍后再试。");
                }
                break;

            // 聊天
            case (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_Chat:
                {
                    try
                    {
                        string uid = (string)jd["uid"];
                        int type = (int)jd["type"];
                        int content_id = (int)jd["content_id"];

                        if (uid.CompareTo(UserData.uid) == 0)
                        {
                            if (type == 1)
                            {
                                string content_text = content_text = ChatData.getInstance().getChatTextById((int)jd["content_id"]).m_text;
                                ChatContentScript.createChatContent(content_text, new Vector2(-530, -190), TextAnchor.MiddleLeft);
                            }
                            else
                            {
                                EmojiScript.create(content_id, new Vector2(-470, -190));
                            }
                        }
                        else
                        {
                            if (uid.CompareTo(m_playerHead_left.transform.name) == 0)
                            {
                                if (type == 1)
                                {
                                    string content_text = content_text = ChatData.getInstance().getChatTextById((int)jd["content_id"]).m_text;
                                    ChatContentScript.createChatContent(content_text, new Vector2(-530, 130), TextAnchor.MiddleLeft);
                                }
                                else
                                {
                                    EmojiScript.create(content_id, new Vector2(-460, 130));
                                }
                            }
                            else if (uid.CompareTo(m_playerHead_right.transform.name) == 0)
                            {
                                if (type == 1)
                                {
                                    string content_text = content_text = ChatData.getInstance().getChatTextById((int)jd["content_id"]).m_text;
                                    GameObject obj = ChatContentScript.createChatContent(content_text, new Vector2(530, 130), TextAnchor.MiddleRight);
                                    obj.transform.localScale = new Vector3(-1, 1, 1);
                                    obj.transform.Find("Text").localScale = new Vector3(-1, 1, 1);
                                }
                                else
                                {
                                    EmojiScript.create(content_id, new Vector2(450, 130));
                                }
                            }
                            else if (uid.CompareTo(m_playerHead_up.transform.name) == 0)
                            {
                                //if (type == 1)
                                //{
                                //    string content_text = content_text = ChatData.getInstance().getChatTextById((int)jd["content_id"]).m_text;
                                //    GameObject obj = ChatContentScript.createChatContent(content_text, new Vector2(-80, 285), TextAnchor.MiddleRight);
                                //    obj.transform.localScale = new Vector3(-1, 1, 1);
                                //    obj.transform.Find("Text").localScale = new Vector3(-1, 1, 1);
                                //}
                                //else
                                //{
                                //    EmojiScript.create(content_id, new Vector2(0, 200));
                                //}
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Log("onReceive_PlayGame.PlayAction_Chat异常：" + ex.Message);
                    }
                }
                break;
        }
    }

    public void onReceive_UserInfo_Game(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onReceive_UserInfo_Game"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onReceive_UserInfo_Game", null, data);
            return;
        }

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            string uid = (string)jd["uid"];

            if (UserData.uid.CompareTo(uid) != 0)
            {
                PlayerData playerData = DDZ_GameData.getInstance().getPlayerDataByUid(uid);
                if (playerData != null)
                {
                    DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_name = (string)jd["name"];
                    DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_head = "head_" + (int)jd["head"];
                    DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_gold = (int)jd["gold"];
                    DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_vipLevel = (int)jd["vipLevel"];
                    DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_allGameCount = (int)jd["gameData"]["allGameCount"];
                    DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_winCount = (int)jd["gameData"]["winCount"];
                    DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_runCount = (int)jd["gameData"]["runCount"];
                    DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_meiliZhi = (int)jd["gameData"]["meiliZhi"];

                    {
                        GameObject playerHead = getPlayerHeadByUid(uid);

                        if (playerHead != null)
                        {
                            playerHead.transform.Find("Text_name").GetComponent<Text>().text = (string)jd["name"];
                            playerHead.transform.Find("Text_gold").GetComponent<Text>().text = ((int)jd["gold"]).ToString();
                        }
                    }
                }
                else
                {
                    ToastScript.createToast("没有此人信息：" + (string)jd["name"]);
                }
            }
        }
    }

    public void onReceive_IsJoinGame(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onReceive_IsJoinGame"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onReceive_IsJoinGame", null, data);
            return;
        }

        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(data);

        int isJoinGame = (int)jd["isJoinGame"];

        if (isJoinGame == 1)
        {
            ToastScript.createToast("当前已经加入房间");

            string gameRoomType = jd["gameRoomType"].ToString();
            m_DDZ_NetReqLogic.reqRetryJoinGame();
        }
        else
        {
            GameUtil.showGameObject(m_buttonStartGame.gameObject);
                
            ToastScript.createToast("每局服务费" + m_fuwufei + "金币");
        }
    }

    public void onReceive_ResumeGame(string data)
    {
        try
        {
            // 优先使用热更新的代码
            if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onReceive_ResumeGame"))
            {
                ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onReceive_ResumeGame", null, data);
                return;
            }

            NetLoading.getInstance().Close();

            clearData();

            JsonData jd = JsonMapper.ToObject(data);

            int roomState = (int)jd["roomState"];
            DDZ_GameData.getInstance().m_gameRoomType = jd["gameroomtype"].ToString();

            startGame_InitUI(data);

            // 底牌
            {
                DDZ_GameData.getInstance().m_dipaiList.Clear();

                for (int i = 0; i < jd["diPokerList"].Count; i++)
                {
                    int num = (int)jd["diPokerList"][i]["num"];
                    int pokerType = (int)jd["diPokerList"][i]["pokerType"];

                    DDZ_GameData.getInstance().m_dipaiList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                }
            }

            // 我的手牌
            if (roomState != (int)TLJCommon.Consts.DDZ_RoomState.RoomState_fapai)
            {
                {
                    for (int i = 0; i < jd["myPokerList"].Count; i++)
                    {
                        int num = (int)jd["myPokerList"][i]["num"];
                        int pokerType = (int)jd["myPokerList"][i]["pokerType"];

                        DDZ_GameData.getInstance().m_myPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                    }

                    // 对我的牌进行排序
                    CrazyLandlords.Helper.LandlordsCardsHelper.SetWeight(DDZ_GameData.getInstance().m_myPokerList);

                    // 创建我的牌对象
                    createMyPokerObj();
                }
            }

            // 谁是地主
            {
                string dizhuUID = (string)jd["dizhuUID"];
                if (dizhuUID.CompareTo("") != 0)
                {
                    GameObject playerHead = getPlayerHeadByUid(dizhuUID);
                    CommonUtil.setImageSpriteByAssetBundle(playerHead.GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_dizhu");

                    // 把底牌加上去
                    {
                        DDZ_GameData.getInstance().m_dipaiList.Clear();

                        for (int i = 0; i < jd["diPokerList"].Count; i++)
                        {
                            int num = (int)jd["diPokerList"][i]["num"];
                            int pokerType = (int)jd["diPokerList"][i]["pokerType"];

                            //if (UserData.uid.CompareTo(dizhuUID) == 0)
                            //{
                            //    DDZ_GameData.getInstance().m_myPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                            //}

                            DDZ_GameData.getInstance().m_dipaiList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                        }

                        createDiPokerObj();
                    }

                    if (UserData.uid.CompareTo(dizhuUID) == 0)
                    {
                        DDZ_GameData.getInstance().m_isDiZhu = 1;

                        // 对我的牌进行排序
                        CrazyLandlords.Helper.LandlordsCardsHelper.SetWeight(DDZ_GameData.getInstance().m_myPokerList);

                        createMyPokerObj(); // 创建我的牌对象
                    }

                    // 剩余牌数
                    {
                        GameUtil.showGameObject(m_playerHead_left.transform.Find("Image_shengyupaishu").gameObject);
                        GameUtil.showGameObject(m_playerHead_right.transform.Find("Image_shengyupaishu").gameObject);

                        GameObject obj = getPlayerHeadByUid(dizhuUID);
                        obj.transform.Find("Image_shengyupaishu/Text_num").GetComponent<Text>().text = "20";
                    }
                }
            }

            // 加棒状态
            {
                for (int i = 0; i < jd["jiabangState"].Count; i++)
                {
                    string uid = jd["jiabangState"][i]["uid"].ToString();
                    int isJiaBang = (int)jd["jiabangState"][i]["isJiaBang"];

                    if (isJiaBang == 1)
                    {
                        GameObject playerHead = getPlayerHeadByUid(uid);
                        GameUtil.showGameObject(playerHead.transform.Find("Image_bangzi").gameObject);
                    }

                    if (UserData.uid.CompareTo(uid) == 0)
                    {
                        GameUtil.hideGameObject(m_jiabang);
                    }
                }
            }
            
            // 倍数
            {
                int beishu = (int)jd["beishu"];
                m_text_beishu.text = ("X" + beishu);
            }

            //----------------------------------------------------------------------------------------------------------------
            switch (roomState)
            {
                case (int)TLJCommon.Consts.DDZ_RoomState.RoomState_waiting:
                    {
                        // 不需要处理
                    }
                    break;

                case (int)TLJCommon.Consts.DDZ_RoomState.RoomState_fapai:
                    {
                        // 已经发的牌
                        {
                            for (int i = 0; i < jd["allotPokerList"].Count; i++)
                            {
                                int num = (int)jd["allotPokerList"][i]["num"];
                                int pokerType = (int)jd["allotPokerList"][i]["pokerType"];

                                DDZ_GameData.getInstance().m_myPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                            }

                            // 对我的牌进行排序
                            CrazyLandlords.Helper.LandlordsCardsHelper.SetWeight(DDZ_GameData.getInstance().m_myPokerList);

                            // 创建我的牌对象
                            createMyPokerObj();
                        }
                    }
                    break;

                case (int)TLJCommon.Consts.DDZ_RoomState.RoomState_qiangdizhu:
                    {
                        int curMaxFen = (int)jd["curMaxFen"];
                        string uid = (string)jd["curJiaoDiZhuUid"];

                        if (UserData.uid.CompareTo(uid) == 0)
                        {
                            m_qiangDiZhu.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                        }

                        {
                            // 开始倒计时
                            setTimerPos(uid);
                            m_timerScript.start(DDZ_GameData.getInstance().m_qiangDiZhuTime, TimerScript.TimerType.TimerType_QiangDiZhu, true);
                        }
                    }
                    break;

                case (int)TLJCommon.Consts.DDZ_RoomState.RoomState_jiabang:
                    {
                        if (DDZ_GameData.getInstance().m_isDiZhu == 1)
                        {
                            m_waitjiabang.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                            return;
                        }

                        int isNeedChoiceJiaBang = (int)jd["isNeedChoiceJiaBang"];

                        // 通知玩家加棒
                        if (isNeedChoiceJiaBang == 1)
                        {
                            m_jiabang.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

                            {
                                // 开始倒计时
                                m_timer.transform.localPosition = new Vector3(0, 0, 0);
                                m_timerScript.start(DDZ_GameData.getInstance().m_jiabangTime, TimerScript.TimerType.TimerType_JiaBang, true);
                            }
                        }
                    }
                    break;

                case (int)TLJCommon.Consts.DDZ_RoomState.RoomState_gaming:
                    {
                        // 当前谁出牌
                        {
                            string curOutPokerPlayer = jd["curOutPokerPlayer"].ToString();
                            bool isFreeOutPoker = (bool)jd["isFreeOutPoker"];

                            // 任意出
                            if (isFreeOutPoker)
                            {
                                DDZ_GameData.getInstance().m_maxPlayerOutPokerUID = curOutPokerPlayer;
                                // 清空每个人座位上的牌
                                {
                                    for (int i = 0; i < DDZ_GameData.getInstance().m_playerDataList.Count; i++)
                                    {
                                        for (int j = DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Count - 1; j >= 0; j--)
                                        {
                                            Destroy(DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList[j]);
                                        }
                                        DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Clear();
                                    }
                                }
                            }

                            // 检测是否轮到自己出牌
                            {
                                if (curOutPokerPlayer.CompareTo(UserData.uid) == 0)
                                {
                                    if (isFreeOutPoker)
                                    {
                                        GameUtil.hideGameObject(m_buttonBuChu.gameObject);
                                        GameUtil.hideGameObject(m_buttonTiShi.gameObject);
                                    }
                                    else
                                    {
                                        GameUtil.showGameObject(m_buttonBuChu.gameObject);
                                        GameUtil.showGameObject(m_buttonTiShi.gameObject);
                                    }

                                    // 清空此人之前出的牌
                                    {
                                        for (int i = 0; i < DDZ_GameData.getInstance().m_playerDataList.Count; i++)
                                        {
                                            if (DDZ_GameData.getInstance().m_playerDataList[i].m_uid.CompareTo(curOutPokerPlayer) == 0)
                                            {
                                                for (int j = DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Count - 1; j >= 0; j--)
                                                {
                                                    Destroy(DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList[j]);
                                                }
                                                DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Clear();

                                                break;
                                            }
                                        }
                                    }

                                    DDZ_GameData.getInstance().m_isFreeOutPoker = isFreeOutPoker;

                                    m_startChuPai.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

                                    // 开始出牌倒计时
                                    m_timerScript.start(DDZ_GameData.getInstance().m_outPokerTime, TimerScript.TimerType.TimerType_OutPoker, true);
                                    setTimerPos(curOutPokerPlayer);
                                }
                                else
                                {
                                    GameUtil.hideGameObject(m_startChuPai);

                                    // 开始出牌倒计时
                                    m_timerScript.start(DDZ_GameData.getInstance().m_outPokerTime, TimerScript.TimerType.TimerType_OutPoker, false);
                                    setTimerPos(curOutPokerPlayer);
                                }
                            }
                        }

                        // 剩余牌数
                        for (int i = 0; i < jd["playerRestPokerCount"].Count; i++)
                        {
                            string uid = jd["playerRestPokerCount"][i]["uid"].ToString();
                            int restPokerCount = (int)jd["playerRestPokerCount"][i]["restPokerCount"];

                            GameObject playerHead = getPlayerHeadByUid(uid);

                            // 剩余牌数
                            playerHead.transform.Find("Image_shengyupaishu/Text_num").GetComponent<Text>().text = restPokerCount.ToString();
                        }

                        string biggestPlayerUID = jd["biggestPlayerUID"].ToString();
                        if (biggestPlayerUID.CompareTo("") != 0)
                        {
                            DDZ_GameData.getInstance().m_maxPlayerOutPokerUID = biggestPlayerUID;
                        }

                        // 显示当前回合出的牌
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                PlayerData playerData = DDZ_GameData.getInstance().m_playerDataList[i];

                                List<TLJCommon.PokerInfo> outPokerList = new List<TLJCommon.PokerInfo>();
                                for (int j = 0; j < jd["player" + i + "OutPokerList"].Count; j++)
                                {
                                    int num = (int)jd["player" + i + "OutPokerList"][j]["num"];
                                    int pokerType = (int)jd["player" + i + "OutPokerList"][j]["pokerType"];

                                    outPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));

                                    if (biggestPlayerUID.CompareTo(playerData.m_uid) == 0)
                                    {
                                        DDZ_GameData.getInstance().m_maxPlayerOutPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                                    }
                                }

                                showOtherOutPoker(outPokerList, playerData.m_uid);
                            }
                        }
                    }
                    break;

                case (int)TLJCommon.Consts.DDZ_RoomState.RoomState_end:
                    {
                        GameUtil.showGameObject(m_buttonStartGame.gameObject);
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            LogUtil.Log(ex.Message);
        }
    }

    public void onReceive_BaoXiangDrop(string data)
    {
        try
        {
            // 优先使用热更新的代码
            if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onReceive_BaoXiangDrop"))
            {
                ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onReceive_BaoXiangDrop", null, data);
                return;
            }

            JsonData jd = JsonMapper.ToObject(data);

            string reward = (string)jd["reward"];

            for (int i = 0; i < 50; i++)
            {
                BaoXiangScript.create();
            }

            GameUtil.changeDataWithStr(reward);
            ShowRewardPanelScript.Show(reward, false);
        }
        catch (Exception ex)
        {
            LogUtil.Log(ex.Message);
        }
    }

    public void exitRoom()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "exitRoom"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "exitRoom", null, null);
            return;
        }

        // 清空本局数据
        {
            clearData();
        }

        m_DDZ_NetReqLogic.reqExitRoom();

        SceneManager.LoadScene("MainScene");
    }

    public void cleanRoom()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "cleanRoom"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "cleanRoom", null, null);
            return;
        }

        {
            m_playerHead_left.transform.Find("Text_name").GetComponent<Text>().text = UserData.name;
            m_playerHead_left.transform.Find("Text_gold").GetComponent<Text>().text = UserData.gold.ToString();

            m_playerHead_right.transform.Find("Text_name").GetComponent<Text>().text = UserData.name;
            m_playerHead_right.transform.Find("Text_gold").GetComponent<Text>().text = UserData.gold.ToString();

            m_playerHead_up.transform.Find("Text_name").GetComponent<Text>().text = UserData.name;
            m_playerHead_up.transform.Find("Text_gold").GetComponent<Text>().text = UserData.gold.ToString();

            GameUtil.hideGameObject(m_playerHead_left);
            GameUtil.hideGameObject(m_playerHead_right);
            GameUtil.hideGameObject(m_playerHead_up);

            CommonUtil.setImageSpriteByAssetBundle(m_playerHead_down.GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_nongmin");
            CommonUtil.setImageSpriteByAssetBundle(m_playerHead_up.GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_nongmin");
            CommonUtil.setImageSpriteByAssetBundle(m_playerHead_left.GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_nongmin");
            CommonUtil.setImageSpriteByAssetBundle(m_playerHead_right.GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_nongmin");

            m_text_beishu.text = "X1";
        }

        {
            // 剩余牌数
            GameUtil.hideGameObject(m_playerHead_down.transform.Find("Image_shengyupaishu").gameObject);
            GameUtil.hideGameObject(m_playerHead_up.transform.Find("Image_shengyupaishu").gameObject);
            GameUtil.hideGameObject(m_playerHead_left.transform.Find("Image_shengyupaishu").gameObject);
            GameUtil.hideGameObject(m_playerHead_right.transform.Find("Image_shengyupaishu").gameObject);

            m_playerHead_down.transform.Find("Image_shengyupaishu/Text_num").GetComponent<Text>().text = "17";
            m_playerHead_up.transform.Find("Image_shengyupaishu/Text_num").GetComponent<Text>().text = "17";
            m_playerHead_left.transform.Find("Image_shengyupaishu/Text_num").GetComponent<Text>().text = "17";
            m_playerHead_right.transform.Find("Image_shengyupaishu/Text_num").GetComponent<Text>().text = "17";

            // 棒子
            GameUtil.hideGameObject(m_playerHead_down.transform.Find("Image_bangzi").gameObject);
            GameUtil.hideGameObject(m_playerHead_up.transform.Find("Image_bangzi").gameObject);
            GameUtil.hideGameObject(m_playerHead_left.transform.Find("Image_bangzi").gameObject);
            GameUtil.hideGameObject(m_playerHead_right.transform.Find("Image_bangzi").gameObject);
        }

        // 清空每个人座位上的牌
        {
            for (int i = 0; i < DDZ_GameData.getInstance().m_playerDataList.Count; i++)
            {
                for (int j = DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Count - 1; j >= 0; j--)
                {
                    Destroy(DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList[j]);
                }
                DDZ_GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Clear();
            }
        }

        // 删除我的手牌对象
        {
            for (int i = DDZ_GameData.getInstance().m_myPokerObjList.Count - 1; i >= 0; i--)
            {
                Destroy(DDZ_GameData.getInstance().m_myPokerObjList[i]);
            }
        }

        // 删除3张底牌对象
        {
            for (int i = 0; i < m_dipaiObj.transform.childCount; i++)
            {
                Destroy(m_dipaiObj.transform.GetChild(i).gameObject);
            }
        }

        {
            DDZ_GameData.getInstance().m_myPokerList.Clear();
            DDZ_GameData.getInstance().m_dipaiList.Clear();
            DDZ_GameData.getInstance().m_maxPlayerOutPokerList.Clear();
            DDZ_GameData.getInstance().m_playerDataList.Clear();

            DDZ_GameData.getInstance().m_isDiZhu = 0;
            DDZ_GameData.getInstance().m_maxPlayerOutPokerUID = "";
        }
    }

    public void clearData()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "clearData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "clearData", null, null);
            return;
        }

        DDZ_GameData.getInstance().clear();
    }

    //--------------------------------------------------------------------------------------------------------------

    public void onSocketReceive_Play(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onSocketReceive_Play"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onSocketReceive_Play", null, data);
            return;
        }

        onReceive(data);
    }

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
            NetLoading.getInstance().Close();
            NetErrorPanelScript.getInstance().Close();

            // 检查是否已经加入房间，已经加入的话则恢复房间
            //reqIsJoinRoom();

            // 检测服务器是否连接
            checkNet();
        }
        else
        {
            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(exitRoom);
            NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，点击确认回到主界面");
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

        checkNet();
    }

    public void onReceive_Main(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onReceive_Main"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onReceive_Main", null, data);
            return;
        }

        LogUtil.Log("DDZ_GameScript.onReceive_Main----" + data);
        JsonData jd = JsonMapper.ToObject(data);
        string tag = (string)jd["tag"];

        // 强制离线
        if (tag.CompareTo(TLJCommon.Consts.Tag_ForceOffline) == 0)
        {
            Destroy(LogicEnginerScript.Instance.gameObject);
            Destroy(PlayServiceSocket.s_instance.gameObject);

            GameObject obj = CommonExitPanelScript.create();
            obj.GetComponent<CommonExitPanelScript>().ButtonConfirm.onClick.RemoveAllListeners();
            obj.GetComponent<CommonExitPanelScript>().ButtonConfirm.onClick.AddListener(delegate ()
            {
                OtherData.s_isFromSetToLogin = true;
                SceneManager.LoadScene("LoginScene");
            });
        }
        // 救济金
        else if (tag.CompareTo(TLJCommon.Consts.Tag_SupplyGold) == 0)
        {
            int todayCount = (int)jd["todayCount"];
            int goldNum = (int)jd["goldNum"];

            GameUtil.changeData(1,goldNum);
            m_playerHead_down.transform.Find("Text_gold").GetComponent<Text>().text = UserData.gold.ToString();

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
            NetLoading.getInstance().Close();
            NetErrorPanelScript.getInstance().Close();
            // 检测服务器是否连接
            checkNet();
        }
        else
        {
            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(exitRoom);
            NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，点击确认回到主界面");
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

        checkNet();
    }

    // 点击网络断开弹框中的重连按钮:logic
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

    // 点击网络断开弹框中的重连按钮:play
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
            Destroy(LogicEnginerScript.Instance.gameObject);

            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(exitRoom);
            NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，点击确认回到主界面");
        }
        else if (!PlayServiceSocket.s_instance.isConnecion())
        {
            Destroy(PlayServiceSocket.s_instance.gameObject);

            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(exitRoom);
            NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，点击确认回到主界面");
        }
        else
        {
            //ToastScript.createToast("两个服务器都成功连接");
        }
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

        //NetErrorPanelScript.getInstance().Show();
        //NetErrorPanelScript.getInstance().setOnClickButton(onClickBack);
        //NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，点击确定回到主界面");

        checkNet();
    }
}
