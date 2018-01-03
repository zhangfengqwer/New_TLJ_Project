using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    public GameObject m_gameInfoSign;

    public Image m_gameRoomLogo;
    public Button m_button_bag;
    public Button m_button_set;
    public Button m_button_exit;

    public Button m_buttonStartGame;
    public Button m_buttonOutPoker;
    public Button m_buttonTiShi;
    public Button m_buttonMaiDi;
    public Button m_buttonChat;
    public Button m_buttonTuoGuan;
    public Button m_buttonDiPai;
    public Button m_buttonJiPaiQi;
    public Button m_buttonCustomPoker;
    public Text m_textScore;
    public Image m_imageMasterPokerType;
    public Text m_text_myLevelPoker;
    public Text m_text_otherLevelPoker;

    // 倒计时
    GameObject m_timer;

    TimerScript m_timerScript;

    // 记牌器
    GameObject m_jiPaiGameObject = null;

    // 托管
    GameObject m_tuoguanObj = null;

    public GameObject m_myUserInfoUI;

    //GameObject m_waitOtherPlayer;
    GameObject m_waitMatchPanel = null;

    GameObject m_liangzhuObj = null;
    GameObject m_pvpGameResultPanel = null;

    Vector3 m_screenPos;

    //bool m_isStartGame = false;
    bool m_hasJiPaiQiUse = false;

    void Start()
    {
        OtherData.s_gameScript = this;

        // 禁止多点触摸
        Input.multiTouchEnabled = false;

        ToastScript.clear();

        // 安卓回调
        AndroidCallBack.s_onPauseCallBack = onPauseCallBack;
        AndroidCallBack.s_onResumeCallBack = onResumeCallBack;

        AudioScript.getAudioScript().stopMusic();

        // 3秒后播放背景音乐,每隔75秒重复播放背景音乐
        InvokeRepeating("onInvokeStartMusic", 3, 75);

        initData();

        initUI();

        checkGameRoomType();

        m_screenPos = Camera.main.WorldToScreenPoint(transform.position);
    }

    void onInvokeStartMusic()
    {
        AudioScript.getAudioScript().playMusic_GameBg_PVP();
    }

    void initData()
    {
        // 游戏服务器
        PlayServiceSocket.s_instance.setOnPlayService_Connect(onSocketConnect_Play);
        PlayServiceSocket.s_instance.setOnPlayService_Receive(onSocketReceive_Play);
        PlayServiceSocket.s_instance.setOnPlayService_Close(onSocketClose_Play);

        // 逻辑服务器
        LogicEnginerScript.Instance.GetComponent<MainRequest>().CallBack = onReceive_Main;
        LogicEnginerScript.Instance.setOnLogicService_Connect(onSocketConnect_Logic);
        LogicEnginerScript.Instance.setOnLogicService_Close(onSocketClose_Logic);
    }

    void initUI()
    {
        m_imageMasterPokerType.transform.localScale = new Vector3(0, 0, 0);
        m_gameInfoSign.transform.localScale = new Vector3(0, 0, 0);
        m_buttonJiPaiQi.transform.localScale = new Vector3(0, 0, 0);

        // 设置房间类型logo
        GameUtil.setGameRoomTypeLogoPath(GameData.getInstance().getGameRoomType(), m_gameRoomLogo);

        // 初始化定时器
        {
            m_timer = TimerScript.createTimer();
            m_timerScript = m_timer.GetComponent<TimerScript>();
            m_timerScript.setOnTimerEvent_TimeEnd(onTimerEventTimeEnd);
        }

        // 初始化亮主
        {
            if (m_liangzhuObj != null)
            {
                Destroy(m_liangzhuObj);
                m_liangzhuObj = null;
            }

            m_liangzhuObj = LiangZhu.create(this);
            m_liangzhuObj.GetComponent<LiangZhu>().setUseType(LiangZhu.UseType.UseType_liangzhu);
            m_liangzhuObj.GetComponent<LiangZhu>().UpdateUi(GameData.getInstance().m_myPokerList,null);
            m_liangzhuObj.transform.localScale = new Vector3(0, 0, 0);
        }

        m_buttonOutPoker.transform.localScale = new Vector3(0, 0, 0);
        m_buttonTiShi.transform.localScale = new Vector3(0, 0, 0);
        m_buttonMaiDi.transform.localScale = new Vector3(0, 0, 0);
        m_buttonChat.transform.localScale = new Vector3(0, 0, 0);
        m_buttonTuoGuan.transform.localScale = new Vector3(0, 0, 0);
        m_buttonDiPai.transform.localScale = new Vector3(0, 0, 0);

        // 我的信息
        {
            m_myUserInfoUI.GetComponent<MyUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon(UserData.head);
            m_myUserInfoUI.GetComponent<MyUIScript>().setName(UserData.name);
            m_myUserInfoUI.GetComponent<MyUIScript>().m_uid = UserData.uid;
        }
    }

    bool isPVP()
    {
        bool b = false;
        List<string> list = new List<string>();
        CommonUtil.splitStr(GameData.getInstance().getGameRoomType(), list, '_');
        if (list[0].CompareTo("PVP") == 0)
        {
            b = true;
        }

        return b;
    }

    void checkGameRoomType()
    {
        // 休闲场
        if (!isPVP())
        {
            setCommonUI(GameData.getInstance().getGameRoomType());

            reqIsJoinRoom();
        }
        // 比赛场
        else
        {
            setCommonUI(GameData.getInstance().getGameRoomType());

            // 从比赛场过来，直接开始游戏
            startGame_InitUI(GameData.getInstance().m_startGameJsonData);
        }
    }

    public void setCommonUI(string gameRoomType)
    {
        List<string> list = new List<string>();
        CommonUtil.splitStr(gameRoomType, list, '_');

        // 休闲场
        if (list[0].CompareTo("XiuXian") == 0)
        {
            m_myUserInfoUI.GetComponent<MyUIScript>().setGoldNum(UserData.gold);
        }
        // PVP
        else
        {
            m_button_bag.transform.localScale = new Vector3(0, 0, 0);

            m_buttonTuoGuan.interactable = false;
            CommonUtil.setImageSprite(m_buttonTuoGuan.GetComponent<Image>(), "Sprites/Game/game_btn_gray");

            m_buttonJiPaiQi.transform.localScale = new Vector3(0, 0, 0);

            {
                m_button_set.transform.localPosition = new Vector3(-70, -8, 0);
                m_button_exit.transform.localPosition = new Vector3(90, -8, 0);
            }

            // 比赛场把金币图标替换为积分图标
            CommonUtil.setImageSprite(m_myUserInfoUI.transform.Find("Image_goldIcon").GetComponent<Image>(), "Sprites/Icon/icon_jifen");
        }
    }

    void startGame_InitUI(string jsonData)
    {
        try
        {
            //m_isStartGame = true;
            GameData.getInstance().m_isStartGame = true;

            // 托管状态
            {
                if (!isPVP())
                {
                    m_buttonTuoGuan.transform.localScale = new Vector3(1, 1, 1);
                    m_buttonTuoGuan.interactable = true;
                    CommonUtil.setImageSprite(m_buttonTuoGuan.GetComponent<Image>(), "Sprites/Game/game_btn_green");
                }
                else
                {
                    m_buttonTuoGuan.transform.localScale = new Vector3(1, 1, 1);
                    m_buttonTuoGuan.interactable = false;
                    CommonUtil.setImageSprite(m_buttonTuoGuan.GetComponent<Image>(), "Sprites/Game/game_btn_gray");
                }

                CommonUtil.setImageSprite(m_buttonTuoGuan.transform.Find("Image").GetComponent<Image>(), "Sprites/Game/game_tuoguan");
                GameData.getInstance().m_isTuoGuan = false;
            }

            // 底牌按钮状态
            {
                m_buttonDiPai.transform.localScale = new Vector3(1, 1, 1);
                m_buttonDiPai.interactable = false;
                CommonUtil.setImageSprite(m_buttonDiPai.GetComponent<Image>(),"Sprites/Game/game_btn_gray");
            }

            // 记牌器按钮
            {
                bool canUse = false;
                for (int i = 0; i < UserData.buffData.Count; i++)
                {
                    if ((UserData.buffData[i].prop_id == (int) TLJCommon.Consts.Prop.Prop_jipaiqi) &&
                        (UserData.buffData[i].buff_num > 0))
                    {
                        canUse = true;
                        break;
                    }
                }

                if (!isPVP() && canUse)
                {
                    m_buttonJiPaiQi.transform.localScale = new Vector3(1, 1, 1);
                    m_hasJiPaiQiUse = true;
                }
                else
                {
                    m_buttonJiPaiQi.transform.localScale = new Vector3(0, 0, 0);
                    m_hasJiPaiQiUse = false;
                }
            }

            // 初始化记牌器
            {
                if (m_jiPaiGameObject == null)
                {
                    m_jiPaiGameObject = RememberPokerHelper.create();
                    m_jiPaiGameObject.GetComponentInChildren<RememberPokerHelper>().OnClickClose();
                }
            }

            JsonData jd = JsonMapper.ToObject(jsonData);
            {
                if (!isPVP())
                {
                    m_buttonTuoGuan.transform.localScale = new Vector3(1, 1, 1);
                    m_buttonTuoGuan.interactable = true;
                    CommonUtil.setImageSprite(m_buttonTuoGuan.GetComponent<Image>(), "Sprites/Game/game_btn_green");
                }
                else
                {
                    m_buttonTuoGuan.transform.localScale = new Vector3(1, 1, 1);
                    m_buttonTuoGuan.interactable = false;
                    CommonUtil.setImageSprite(m_buttonTuoGuan.GetComponent<Image>(), "Sprites/Game/game_btn_gray");
                }

                m_buttonStartGame.transform.localScale = new Vector3(0, 0, 0); // 禁用开始游戏按钮
                m_buttonChat.transform.localScale = new Vector3(1, 1, 1);
                m_buttonDiPai.transform.localScale = new Vector3(1, 1, 1);
                m_liangzhuObj.transform.localScale = new Vector3(1, 1, 1);

                // 显示左上角提示牌信息
                m_gameInfoSign.transform.localScale = new Vector3(1, 1, 1);

                {
                    // 上边的玩家
                    {
                        GameObject obj = OtherPlayerUIScript.create();
                        obj.transform.localPosition = new Vector3(0, 297.27f, 0);
                        obj.GetComponent<OtherPlayerUIScript>().m_direction =
                            OtherPlayerUIScript.Direction.Direction_Up;

                        if (isPVP())
                        {
                            CommonUtil.setImageSprite(obj.transform.Find("Image_icon_gold").GetComponent<Image>(),
                                "Sprites/Icon/icon_jifen");
                        }


                        GameData.getInstance().m_otherPlayerUIObjList.Add(obj);
                    }

                    // 左边的玩家
                    {
                        GameObject obj = OtherPlayerUIScript.create();
                        obj.transform.localPosition = new Vector3(-550, 0, 0);
                        obj.GetComponent<OtherPlayerUIScript>().m_direction =
                            OtherPlayerUIScript.Direction.Direction_Left;

                        if (isPVP())
                        {
                            CommonUtil.setImageSprite(obj.transform.Find("Image_icon_gold").GetComponent<Image>(),
                                "Sprites/Icon/icon_jifen");
                        }

                        GameData.getInstance().m_otherPlayerUIObjList.Add(obj);
                    }

                    // 右边的玩家
                    {
                        GameObject obj = OtherPlayerUIScript.create();
                        obj.transform.localPosition = new Vector3(550, 0, 0);
                        obj.GetComponent<OtherPlayerUIScript>().m_direction =
                            OtherPlayerUIScript.Direction.Direction_Right;

                        if (isPVP())
                        {
                            CommonUtil.setImageSprite(obj.transform.Find("Image_icon_gold").GetComponent<Image>(),
                                "Sprites/Icon/icon_jifen");
                        }

                        GameData.getInstance().m_otherPlayerUIObjList.Add(obj);
                    }
                }
            }

            // 分数
            m_textScore.text = "0";

            // 级牌
            {
                GameData.getInstance().m_levelPokerNum = (int) jd["levelPokerNum"];
                ToastScript.createToast("本局主牌为" + GameUtil.getPokerNumWithStr(GameData.getInstance().m_levelPokerNum));
            }

            // 我方级数
            {
                GameData.getInstance().m_myLevelPoker = (int) jd["myLevelPoker"];
                m_text_myLevelPoker.text = GameUtil.getPokerNumWithStr(GameData.getInstance().m_myLevelPoker);
            }

            // 对方级数
            {
                GameData.getInstance().m_otherLevelPoker = (int) jd["otherLevelPoker"];
                m_text_otherLevelPoker.text = GameUtil.getPokerNumWithStr(GameData.getInstance().m_otherLevelPoker);
            }

            // 我的队友uid
            GameData.getInstance().m_teammateUID = jd["teammateUID"].ToString();

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
                        GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid =
                            jd["userList"][2]["uid"].ToString();
                        GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid =
                            jd["userList"][3]["uid"].ToString();
                        GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid =
                            jd["userList"][1]["uid"].ToString();
                    }
                        break;

                    case 1:
                    {
                        GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid =
                            jd["userList"][3]["uid"].ToString();
                        GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid =
                            jd["userList"][0]["uid"].ToString();
                        GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid =
                            jd["userList"][2]["uid"].ToString();
                    }
                        break;

                    case 2:
                    {
                        GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid =
                            jd["userList"][0]["uid"].ToString();
                        GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid =
                            jd["userList"][1]["uid"].ToString();
                        GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid =
                            jd["userList"][3]["uid"].ToString();
                    }
                        break;

                    case 3:
                    {
                        GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid =
                            jd["userList"][1]["uid"].ToString();
                        GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid =
                            jd["userList"][2]["uid"].ToString();
                        GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid =
                            jd["userList"][0]["uid"].ToString();
                    }
                        break;
                }
            }

            // 开始获取玩家信息倒计时
            startDaoJiShi_GetUserInfo_Game();

            // 本桌所有人信息
            for (int i = 0; i < jd["userList"].Count; i++)
            {
                string uid = jd["userList"][i]["uid"].ToString();

                GameData.getInstance().m_playerDataList.Add(new PlayerData(uid));

                GameData.getInstance().getPlayerDataByUid(uid).m_score = (int) jd["userList"][i]["score"];

                if (uid.CompareTo(UserData.uid) == 0)
                {
                    GameData.getInstance().getPlayerDataByUid(uid).m_name = UserData.name;
                    GameData.getInstance().getPlayerDataByUid(uid).m_head = UserData.head;
                    GameData.getInstance().getPlayerDataByUid(uid).m_gold = UserData.gold;
                    GameData.getInstance().getPlayerDataByUid(uid).m_allGameCount = UserData.gameData.allGameCount;
                    GameData.getInstance().getPlayerDataByUid(uid).m_winCount = UserData.gameData.winCount;
                    GameData.getInstance().getPlayerDataByUid(uid).m_runCount = UserData.gameData.runCount;
                    GameData.getInstance().getPlayerDataByUid(uid).m_meiliZhi = UserData.gameData.meiliZhi;

                    if (isPVP())
                    {
                        m_myUserInfoUI.GetComponent<MyUIScript>().m_textGoldNum.text =
                            jd["userList"][i]["score"].ToString();
                    }
                    else
                    {
                        m_myUserInfoUI.GetComponent<MyUIScript>().m_textGoldNum.text = UserData.gold.ToString();
                    }
                }
                else
                {
                    reqUserInfo_Game(uid);
                }
            }
        }
        catch (Exception ex)
        {
            LogUtil.Log("startGame_InitUI()报错：" + ex.Message);
        }
    }

    public void startDaoJiShi_GetUserInfo_Game()
    {
        Invoke("onInvoke_GetUserInfo_Game", 2);
    }

    void onInvoke_GetUserInfo_Game()
    {
        bool isOK = true;

        for (int i = 0; i < GameData.getInstance().m_playerDataList.Count; i++)
        {
            if (string.IsNullOrEmpty(GameData.getInstance().m_playerDataList[i].m_name))
            {
                isOK = false;

                //ToastScript.createToast("没有获取到玩家信息：" + GameData.getInstance().m_playerDataList[i].m_uid);
                reqUserInfo_Game(GameData.getInstance().m_playerDataList[i].m_uid);
            }
        }

        if (!isOK)
        {
            startDaoJiShi_GetUserInfo_Game();
        }
        else
        {
            //ToastScript.createToast("所有玩家信息都已经获取到");
        }
    }

    public void showWaitMatchPanel(float time, bool isContinueGame)
    {
        m_waitMatchPanel = WaitMatchPanelScript.create(GameData.getInstance().getGameRoomType());
        WaitMatchPanelScript script = m_waitMatchPanel.GetComponent<WaitMatchPanelScript>();
        script.setOnTimerEvent_TimeEnd(onTimerEvent_TimeEnd);
        script.start(time, isContinueGame);
    }

    public void onTimerEvent_TimeEnd(bool isContinueGame)
    {
        if (isContinueGame)
        {
            ToastScript.createToast("同桌玩家没有继续游戏");

            //GameUtil.showGameObject(m_buttonStartGame.gameObject);
            exitRoom();
        }
        else
        {
            LogUtil.Log("暂时没有匹配到玩家,请求匹配机器人");

            // 让服务端匹配机器人
            //reqWaitMatchTimeOut();
        }
    }

    void clearData()
    {
        {
            m_myUserInfoUI.GetComponent<MyUIScript>().m_imageZhuangJiaIcon.transform.localScale = new Vector3(0, 0, 0);
        }

        {
            Destroy(m_timer);
            Destroy(m_jiPaiGameObject);
            Destroy(m_liangzhuObj);
            Destroy(m_waitMatchPanel);

            m_timer = null;
            m_jiPaiGameObject = null;
            m_liangzhuObj = null;
            m_waitMatchPanel = null;

            if (m_tuoguanObj != null)
            {
                Destroy(m_tuoguanObj);
            }
        }

        // 删除另外3个人的头像昵称UI
        {
            for (int i = GameData.getInstance().m_otherPlayerUIObjList.Count - 1; i >= 0; i--)
            {
                Destroy(GameData.getInstance().m_otherPlayerUIObjList[i]);
            }
            GameData.getInstance().m_otherPlayerUIObjList.Clear();
        }

        // 删除我的手牌对象
        {
            for (int i = GameData.getInstance().m_myPokerObjList.Count - 1; i >= 0; i--)
            {
                Destroy(GameData.getInstance().m_myPokerObjList[i]);
            }
            GameData.getInstance().m_myPokerObjList.Clear();
        }

        // 删除当前已出的牌对象
        {
            for (int i = 0; i < GameData.getInstance().m_playerDataList.Count; i++)
            {
                for (int j = GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Count - 1; j >= 0; j--)
                {
                    Destroy(GameData.getInstance().m_playerDataList[i].m_outPokerObjList[j]);
                }
                GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Clear();
            }
        }

        GameData.getInstance().clear();
    }

    // Update is called once per frame
    void Update()
    {
        // 点击返回键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (QueRenExitPanelScript.s_gameobject == null)
            {
                onClickExitRoom();
            }
        }
    }

    void OnDestroy()
    {
        OtherData.s_gameScript = null;
    }

    public void onClickBag()
    {
        BagPanelScript.create(false);
    }

    public void onClickSet()
    {
        SetScript.create(true);
    }

    public void onClickJoinRoom()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();
        reqJoinRoom();
    }

    public void onClickExitRoom()
    {
        if (isPVP())
        {
            QueRenExitPanelScript.create(this, "是否确定退出？报名费不可退还。");
        }
        else
        {
            QueRenExitPanelScript.create(this, "是否确定退出？");
        }
    }

    public void onClickOutPoker()
    {
        //AudioScript.getAudioScript().playSound_ButtonClick();
        reqOutPoker();
    }

    public void onClickCustomPoker()
    {
        TestPoker.create();
    }

    // 提示
    public void onClickTiShi()
    {
        AudioScript.getAudioScript().playSound_XuanPai();
        tishi();
    }

    public void onClickQiangZhu(List<TLJCommon.PokerInfo> pokerList)
    {
        AudioScript.getAudioScript().playSound_ButtonClick();
        //m_liangzhuObj.transform.localScale = new Vector3(0, 0, 0);
        reqQiangZhu(pokerList);
    }

    public void onClickChaoDi(List<TLJCommon.PokerInfo> pokerList)
    {
        AudioScript.getAudioScript().playSound_ButtonClick();
        m_liangzhuObj.transform.localScale = new Vector3(0, 0, 0);
        reqChaoDi(pokerList);
    }

    public void onClickMaiDi()
    {
        reqMaiDi();
    }

    public void onClickChat()
    {
        ChatPanelScript.create(this);
    }

    public void onClickTuoGuan()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();

        reqSetTuoGuanState(true);
    }

    public void onClickDiPai()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();

        if (GameData.getInstance().m_dipaiList.Count == 8)
        {
            ShowDiPokerScript.create(GameData.getInstance().m_dipaiList);
        }
        else
        {
            ToastScript.createToast("底牌数据错误");
        }
    }

    public void onClickCancelTuoGuan()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();

        reqSetTuoGuanState(false);
    }

    public void OnClickJiPaiQi()
    {
        // 显示记牌器
        if (m_jiPaiGameObject != null)
        {
            m_jiPaiGameObject.GetComponentInChildren<RememberPokerHelper>().OnClickShow();
        }
    }

    public void exitRoom()
    {
        // 清空本局数据
        {
            clearData();
            //initUI();
        }

        SceneManager.LoadScene("MainScene");

        reqExitRoom();
    }

    void tishi()
    {
        // 所有牌设为未选中
        PokerScript.setAllPokerWeiXuanZe();

        // 自由出牌
        if (GameData.getInstance().m_isFreeOutPoker)
        {
            GameData.getInstance().m_myPokerObjList[GameData.getInstance().m_myPokerObjList.Count - 1].GetComponent<PokerScript>().setIsSelect(true);
            GameData.getInstance().m_myPokerObjList[GameData.getInstance().m_myPokerObjList.Count - 1].GetComponent<PokerScript>().setIsJump(true);
        }
        // 跟牌
        else
        {
            List<TLJCommon.PokerInfo> listPoker = PlayRuleUtil.GetPokerWhenTuoGuan(
                GameData.getInstance().m_curRoundFirstOutPokerList, GameData.getInstance().m_myPokerList,
                GameData.getInstance().m_levelPokerNum, GameData.getInstance().m_masterPokerType);
            if (listPoker.Count == GameData.getInstance().m_curRoundFirstOutPokerList.Count)
            {
                for (int i = 0; i < listPoker.Count; i++)
                {
                    for (int j = GameData.getInstance().m_myPokerObjList.Count - 1; j >= 0; j--)
                    {
                        PokerScript pokerScript =GameData.getInstance().m_myPokerObjList[j].GetComponent<PokerScript>();

                        if ((pokerScript.getPokerNum() == listPoker[i].m_num) &&
                            (pokerScript.getPokerType() == (int) listPoker[i].m_pokerType))
                        {
                            pokerScript.setIsSelect(true);
                            pokerScript.setIsJump(true);
                            break;
                        }
                    }
                }
            }
        }
    }

    //----------------------------------------------------------发送数据 start--------------------------------------------------

    // 是否已经加入房间
    public void reqIsJoinRoom()
    {
        NetLoading.getInstance().Show();

        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_IsJoinGame;
        data["uid"] = UserData.uid;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求恢复房间
    public void reqRetryJoinGame()
    {
        NetLoading.getInstance().Close();

        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_RetryJoinGame;
        data["uid"] = UserData.uid;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求加入房间
    public void reqJoinRoom()
    {
        JsonData data = new JsonData();

        data["tag"] = GameData.getInstance().m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_JoinGame;
        data["gameroomtype"] = GameData.getInstance().getGameRoomType();

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    public void reqWaitMatchTimeOut()
    {
        //JsonData jsonData = new JsonData();
        //jsonData["tag"] = GameData.getInstance().m_tag;
        //jsonData["uid"] = UserData.uid;
        //jsonData["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_WaitMatchTimeOut;

        //PlayServiceSocket.s_instance.sendMessage(jsonData.ToJson());
    }

    public void reqUseBuff(int prop_id)
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = TLJCommon.Consts.Tag_UseBuff;
        jsonData["uid"] = UserData.uid;
        jsonData["prop_id"] = prop_id;

        PlayServiceSocket.s_instance.sendMessage(jsonData.ToJson());
    }

    // 请求退出房间
    public void reqExitRoom()
    {
        JsonData data = new JsonData();

        data["tag"] = GameData.getInstance().m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_ExitGame;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求出牌
    public void reqOutPoker()
    {
        JsonData data = new JsonData();

        data["tag"] = GameData.getInstance().m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_PlayerOutPoker;

        bool hasOutPoker = false;
        List<TLJCommon.PokerInfo> myOutPokerList = new List<TLJCommon.PokerInfo>();

        // 自己出的牌
        {
            JsonData jarray = new JsonData();
            for (int i = 0; i < GameData.getInstance().m_myPokerObjList.Count; i++)
            {
                PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>();
                if (pokerScript.getIsJump())
                {
                    hasOutPoker = true;

                    JsonData jd = new JsonData();
                    jd["num"] = pokerScript.getPokerNum();
                    jd["pokerType"] = pokerScript.getPokerType();
                    jarray.Add(jd);

                    myOutPokerList.Add(new TLJCommon.PokerInfo(pokerScript.getPokerNum(),
                        (TLJCommon.Consts.PokerType) pokerScript.getPokerType()));
                }
            }
            data["pokerList"] = jarray;
        }

        if (hasOutPoker)
        {
            // 检测出牌合理性
            {
                if (!CheckOutPoker.checkOutPoker(GameData.getInstance().m_isFreeOutPoker, myOutPokerList,
                    GameData.getInstance().m_curRoundFirstOutPokerList,
                    GameData.getInstance().m_myPokerList, GameData.getInstance().m_levelPokerNum,
                    GameData.getInstance().m_masterPokerType))
                {
                    string str = "出的牌不合规则:";
                    for (int i = 0; i < myOutPokerList.Count; i++)
                    {
                        str += myOutPokerList[i].m_num;
                        str += "  ";
                    }
                    //ToastScript.createToast(str);
                    ToastScript.createToast("出的牌不合规则");
                    LogUtil.Log(str);

                    return;
                }
            }

            PlayServiceSocket.s_instance.sendMessage(data.ToJson());

            m_buttonOutPoker.transform.localScale = new Vector3(0, 0, 0);
            m_buttonTiShi.transform.localScale = new Vector3(0, 0, 0);

            // 所有牌设为未选中
            PokerScript.setAllPokerWeiXuanZe();
        }
        else
        {
            ToastScript.createToast("请选择你要出的牌");
        }
    }

    public void reqSetTuoGuanState(bool isTuoGuan)
    {
        NetLoading.getInstance().Show();

        JsonData data = new JsonData();

        data["tag"] = GameData.getInstance().m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_SetTuoGuanState;
        data["isTuoGuan"] = isTuoGuan;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 获取游戏内玩家信息
    public void reqUserInfo_Game(string uid)
    {
        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_UserInfo_Game;
        data["uid"] = uid;
        data["isClientReq"] = 1;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求换桌
    public void reqChangeRoom()
    {
        JsonData data = new JsonData();

        data["tag"] = GameData.getInstance().m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_ChangeRoom;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求继续游戏
    public void reqContinueGame()
    {
        JsonData data = new JsonData();

        data["tag"] = GameData.getInstance().m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_ContinueGame;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求抢主
    public void reqQiangZhu(List<TLJCommon.PokerInfo> pokerList)
    {
        JsonData data = new JsonData();

        data["tag"] = GameData.getInstance().m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_QiangZhu;

        JsonData jarray = new JsonData();
        for (int i = 0; i < pokerList.Count; i++)
        {
            JsonData jd = new JsonData();
            jd["num"] = pokerList[i].m_num;
            jd["pokerType"] = (int) pokerList[i].m_pokerType;
            jarray.Add(jd);
        }

        data["pokerList"] = jarray;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求抄底
    public void reqChaoDi(List<TLJCommon.PokerInfo> pokerList)
    {
        JsonData data = new JsonData();

        data["tag"] = GameData.getInstance().m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_PlayerChaoDi;

        if (pokerList.Count > 0)
        {
            JsonData jarray = new JsonData();
            for (int i = 0; i < pokerList.Count; i++)
            {
                data["hasPoker"] = 1;

                JsonData jd = new JsonData();
                jd["num"] = pokerList[i].m_num;
                jd["pokerType"] = (int) pokerList[i].m_pokerType;
                jarray.Add(jd);
            }

            data["pokerList"] = jarray;
        }
        else
        {
            data["hasPoker"] = 0;
        }

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求埋底
    public void reqMaiDi()
    {
        JsonData data = new JsonData();

        data["tag"] = GameData.getInstance().m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_MaiDi;

        int selectNum = 0;
        List<TLJCommon.PokerInfo> myOutPokerList = new List<TLJCommon.PokerInfo>();

        // 自己出的牌
        {
            JsonData jarray = new JsonData();
            for (int i = 0; i < GameData.getInstance().m_myPokerObjList.Count; i++)
            {
                PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>();
                if (pokerScript.getIsJump())
                {
                    ++selectNum;

                    JsonData jd = new JsonData();
                    jd["num"] = pokerScript.getPokerNum();
                    jd["pokerType"] = pokerScript.getPokerType();
                    jarray.Add(jd);

                    myOutPokerList.Add(new TLJCommon.PokerInfo(pokerScript.getPokerNum(),
                        (TLJCommon.Consts.PokerType) pokerScript.getPokerType()));
                }
            }
            data["diPokerList"] = jarray;
        }

        if (selectNum == 8)
        {
            PlayServiceSocket.s_instance.sendMessage(data.ToJson());

            m_buttonMaiDi.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            ToastScript.createToast("请选择8张牌");
        }
    }

    // 放弃抢主
    public void reqGiveUpQiangZhu()
    {
        JsonData data = new JsonData();

        data["tag"] = GameData.getInstance().m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_QiangZhu;
        data["pokerType"] = -1;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 抢主结束
    public void reqQiangZhuEnd()
    {
        JsonData data = new JsonData();

        data["tag"] = GameData.getInstance().m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_QiangZhuEnd;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 发送聊天信息
    public void reqChat(int type, int content_id)
    {
        JsonData data = new JsonData();

        data["tag"] = GameData.getInstance().m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int) TLJCommon.Consts.PlayAction.PlayAction_Chat;
        data["type"] = type;
        data["content_id"] = content_id;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }
    //----------------------------------------------------------发送数据 end--------------------------------------------------

    //----------------------------------------------------------接收数据 start--------------------------------------------------
    void onReceive(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        string tag = (string) jd["tag"];

        if (tag.CompareTo(GameData.getInstance().m_tag) == 0)
        {
            onReceive_PlayGame(data);
        }
        else if (tag.CompareTo(TLJCommon.Consts.Tag_UserInfo_Game) == 0)
        {
            onReceive_UserInfo_Game(data);
        }
        else if (tag.CompareTo(TLJCommon.Consts.Tag_UseBuff) == 0)
        {
            onReceive_UseBuff(data);
        }
        else if (tag.CompareTo(TLJCommon.Consts.Tag_IsJoinGame) == 0)
        {
            onReceive_IsJoinGame(data);
        }
        else if (tag.CompareTo(TLJCommon.Consts.Tag_ResumeGame) == 0)
        {
            onReceive_ResumeGame(data);
        }
    }

    void onReceive_PlayGame(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int playAction = (int) jd["playAction"];

        switch (playAction)
        {
            // 加入游戏
            case (int) TLJCommon.Consts.PlayAction.PlayAction_JoinGame:
            {
                int code = (int) jd["code"];

                switch (code)
                {
                    case (int) TLJCommon.Consts.Code.Code_OK:
                    {
                        int roomId = (int) jd["roomId"];
                        //ToastScript.createToast("加入房间成功");

                        // 禁用开始游戏按钮
                        m_buttonStartGame.transform.localScale = new Vector3(0, 0, 0);

                        //m_waitOtherPlayer = WaitOtherPlayerScript.create();

                        showWaitMatchPanel(GameData.getInstance().m_waitMatchTime, false);
                    }
                        break;

                    case (int) TLJCommon.Consts.Code.Code_CommonFail:
                    {
                        ToastScript.createToast("您已加入其它房间，无法开始新游戏");
                    }
                        break;
                }
            }
                break;

            // 退出游戏
            case (int) TLJCommon.Consts.PlayAction.PlayAction_ExitGame:
            {
                int code = (int) jd["code"];

                switch (code)
                {
                    case (int) TLJCommon.Consts.Code.Code_OK:
                    {
                        int roomId = (int) jd["roomId"];
                        //ToastScript.createToast("退出房间成功：" + roomId);
                    }
                        break;

                    case (int) TLJCommon.Consts.Code.Code_CommonFail:
                    {
                        //ToastScript.createToast("退出房间失败，当前并没有加入房间");
                    }
                        break;
                }
            }
                break;

            // 开始游戏
            case (int) TLJCommon.Consts.PlayAction.PlayAction_StartGame:
            {
                //Destroy(m_waitOtherPlayer);
                Destroy(m_waitMatchPanel);
                Destroy(m_pvpGameResultPanel);
                JueShengJuTiShiPanelScript.checkClose();

                startGame_InitUI(data);

                // 休闲场有记牌器的情况下自动使用
                if (m_hasJiPaiQiUse)
                {
                    reqUseBuff((int) TLJCommon.Consts.Prop.Prop_jipaiqi);
                }
            }
                break;

            // 发牌
            case (int) TLJCommon.Consts.PlayAction.PlayAction_FaPai:
            {
                int num = (int) jd["num"];
                int pokerType = (int) jd["pokerType"];

                int isEnd = (int) jd["isEnd"];

                GameData.getInstance().m_myPokerList
                    .Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));

                sortMyPokerList(-1); // 对我的牌进行排序
                createMyPokerObj(); // 创建我的牌对象

                if (isEnd == 1)
                {
                    {
                        // 开始倒计时
                        m_timerScript.start(GameData.getInstance().m_qiangZhuTime,
                            TimerScript.TimerType.TimerType_QiangZhu, true);
                    }
                }

                m_liangzhuObj.GetComponent<LiangZhu>().UpdateUi(GameData.getInstance().m_myPokerList,GameData.getInstance().m_beforeQiangzhuPokerList);
            }
                break;

            // 抢主
            case (int) TLJCommon.Consts.PlayAction.PlayAction_QiangZhu:
            {
                ToastScript.createToast("玩家抢主");

                string uid = (string)jd["uid"];

                GameData.getInstance().m_beforeQiangzhuPokerList.Clear();
                    
                {
                    PlayerData playerData = GameData.getInstance().getPlayerDataByUid(uid);
                    if (playerData != null)
                    {
                        for (int i = playerData.m_outPokerObjList.Count - 1; i >= 0 ; i--)
                        {
                            Destroy(playerData.m_outPokerObjList[i]);
                        }

                        playerData.m_outPokerObjList.Clear();
                    }
                }

                List<TLJCommon.PokerInfo> outPokerList = new List<TLJCommon.PokerInfo>();

                // 抢主所用的牌
                {
                    {
                        for (int i = 0; i < jd["pokerList"].Count; i++)
                        {
                            int num = (int) jd["pokerList"][i]["num"];
                            int pokerType = (int) jd["pokerList"][i]["pokerType"];

                            GameData.getInstance().m_masterPokerType = pokerType;

                            outPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));
                            GameData.getInstance().m_beforeQiangzhuPokerList
                                .Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));
                        }
                    }

                    // 更新抢主对象数据
                    m_liangzhuObj.GetComponent<LiangZhu>().UpdateUi(GameData.getInstance().m_myPokerList,GameData.getInstance().m_beforeQiangzhuPokerList);
                }

                // 显示出的牌
                showOtherOutPoker(outPokerList, uid);

                initMyPokerPos(GameData.getInstance().m_myPokerObjList);
            }
                break;

            // 抢主结束
            case (int) TLJCommon.Consts.PlayAction.PlayAction_QiangZhuEnd:
            {
                // 删除当前抢主的牌对象
                {
                    for (int i = 0; i < GameData.getInstance().m_playerDataList.Count; i++)
                    {
                        for (int j = GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Count - 1;
                            j >= 0;
                            j--)
                        {
                            Destroy(GameData.getInstance().m_playerDataList[i].m_outPokerObjList[j]);
                        }
                        GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Clear();
                    }
                }

                m_timerScript.stop();
                m_liangzhuObj.transform.localScale = new Vector3(0, 0, 0);

                // 主牌花色
                {
                    GameUtil.showGameObject(m_imageMasterPokerType.gameObject);
                    GameData.getInstance().m_masterPokerType = (int) jd["masterPokerType"];

                    if (GameData.getInstance().m_masterPokerType != -1)
                    {
                        CommonUtil.setImageSprite(m_imageMasterPokerType,
                            GameUtil.getMasterPokerIconPath(GameData.getInstance().m_masterPokerType));
                    }
                    else
                    {
                        ToastScript.createToast("本局打无主牌");

                        // 抢主所用的牌
                        {
                            GameData.getInstance().m_beforeQiangzhuPokerList
                                .Add(new TLJCommon.PokerInfo(16, TLJCommon.Consts.PokerType.PokerType_Wang));
                            GameData.getInstance().m_beforeQiangzhuPokerList
                                .Add(new TLJCommon.PokerInfo(16, TLJCommon.Consts.PokerType.PokerType_Wang));
                        }

                        CommonUtil.setImageSprite(m_imageMasterPokerType,
                            GameUtil.getMasterPokerIconPath(GameData.getInstance().m_masterPokerType));
                    }
                }

                // 对我的牌重新排序
                {
                    if (GameData.getInstance().m_masterPokerType != -1)
                    {
                        sortMyPokerList(GameData.getInstance().m_masterPokerType);
                        createMyPokerObj();
                    }
                }

                // 判断谁是庄家
                {
                    string uid = jd["zhuangjiaUid"].ToString();
                    if (uid.CompareTo(UserData.uid) == 0)
                    {
                        m_myUserInfoUI.GetComponent<MyUIScript>().m_imageZhuangJiaIcon.transform.localScale =
                            new Vector3(1, 1, 1);
                    }
                    else
                    {
                        for (int i = 0; i < GameData.getInstance().m_otherPlayerUIObjList.Count; i++)
                        {
                            if (GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>()
                                    .m_uid.CompareTo(uid) == 0)
                            {
                                GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>()
                                    .m_imageZhuangJiaIcon.transform.localScale = new Vector3(1, 1, 1);
                            }
                        }
                    }
                }

                // 判断身份：庄家一方、普通人一方
                {
                    GameData.getInstance().m_isBanker = (int) jd["isBanker"];
                }

                // 所有牌设为未选中状态
                PokerScript.setAllPokerWeiXuanZe();

                checkShowZhuPaiLogo();
            }
                break;

            // 埋底
            case (int) TLJCommon.Consts.PlayAction.PlayAction_MaiDi:
            {
                m_timerScript.stop();

                // 判断谁是庄家
                {
                    string uid = jd["uid"].ToString();
                    // 庄家开始埋底
                    if (uid.CompareTo(UserData.uid) == 0)
                    {
                        ToastScript.createToast("开始埋底");

                        // 把底牌加上去
                        {
                            for (int i = 0; i < jd["diPokerList"].Count; i++)
                            {
                                int num = (int) jd["diPokerList"][i]["num"];
                                int pokerType = (int) jd["diPokerList"][i]["pokerType"];

                                GameData.getInstance().m_myPokerList
                                    .Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));
                            }

                            sortMyPokerList(GameData.getInstance().m_masterPokerType);
                            createMyPokerObj();
                        }

                        // 开始埋底倒计时
                        m_timerScript.start(GameData.getInstance().m_maiDiTime, TimerScript.TimerType.TimerType_MaiDi,
                            true);
                        setTimerPos(uid);

                        // 启用埋底按钮
                        m_buttonMaiDi.transform.localScale = new Vector3(1, 1, 1);

                        checkShowZhuPaiLogo();
                    }
                    else
                    {
                        ToastScript.createToast("等待庄家埋底");

                        // 开始埋底倒计时
                        m_timerScript.start(GameData.getInstance().m_maiDiTime, TimerScript.TimerType.TimerType_MaiDi,
                            false);
                        setTimerPos(uid);
                    }
                }
            }
                break;

            // 埋底结果返回
            case (int) TLJCommon.Consts.PlayAction.PlayAction_MaiDiBack:
            {
                {
                    string uid = jd["uid"].ToString();

                    // 最后埋底的人
                    {
                        GameData.getInstance().m_lastMaiDiPlayer = uid;
                        if (GameData.getInstance().m_lastMaiDiPlayer.CompareTo(UserData.uid) == 0)
                        {
                            m_buttonDiPai.transform.localScale = new Vector3(1, 1, 1);
                            m_buttonDiPai.interactable = true;
                            CommonUtil.setImageSprite(m_buttonDiPai.GetComponent<Image>(),
                                "Sprites/Game/game_btn_green");
                        }
                        else
                        {
                            m_buttonDiPai.transform.localScale = new Vector3(1, 1, 1);
                            m_buttonDiPai.interactable = false;
                            CommonUtil.setImageSprite(m_buttonDiPai.GetComponent<Image>(),
                                "Sprites/Game/game_btn_gray");
                        }
                    }

                    // 保存底牌
                    {
                        GameData.getInstance().m_dipaiList.Clear();

                        for (int i = 0; i < jd["diPokerList"].Count; i++)
                        {
                            int num = (int) jd["diPokerList"][i]["num"];
                            int pokerType = (int) jd["diPokerList"][i]["pokerType"];

                            GameData.getInstance().m_dipaiList
                                .Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));
                        }
                    }

                    if (uid.CompareTo(UserData.uid) == 0)
                    {
                        {
                            m_timerScript.stop();
                            m_buttonMaiDi.transform.localScale = new Vector3(0, 0, 0);
                        }

                        for (int i = 0; i < jd["diPokerList"].Count; i++)
                        {
                            int num = (int) jd["diPokerList"][i]["num"];
                            int pokerType = (int) jd["diPokerList"][i]["pokerType"];

                            for (int j = GameData.getInstance().m_myPokerObjList.Count - 1; j >= 0; j--)
                            {
                                PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[j]
                                    .GetComponent<PokerScript>();
                                if ((pokerScript.getPokerNum() == num) && (pokerScript.getPokerType() == pokerType))
                                {
                                    // 出的牌从自己的牌堆里删除
                                    {
                                        Destroy(GameData.getInstance().m_myPokerObjList[j]);
                                        GameData.getInstance().m_myPokerObjList.RemoveAt(j);
                                    }

                                    break;
                                }
                            }

                            for (int j = GameData.getInstance().m_myPokerList.Count - 1; j >= 0; j--)
                            {
                                if ((GameData.getInstance().m_myPokerList[j].m_num == num) &&
                                    ((int) GameData.getInstance().m_myPokerList[j].m_pokerType == pokerType))
                                {
                                    // 出的牌从自己的牌堆里删除
                                    {
                                        GameData.getInstance().m_myPokerList.RemoveAt(j);
                                    }

                                    break;
                                }
                            }
                        }

                        initMyPokerPos(GameData.getInstance().m_myPokerObjList);

                        {
                            // 全部设为未选中状态
                            PokerScript.setAllPokerWeiXuanZe();
                        }
                    }
                }
            }
                break;

            // 通知某人抄底
            case (int) TLJCommon.Consts.PlayAction.PlayAction_CallPlayerChaoDi:
            {
                try
                {
                    m_timerScript.stop();

                    string uid = (string) jd["uid"];

                    // 检测是否轮到自己抄底
                    {
                        if (uid.CompareTo(UserData.uid) == 0)
                        {
                            if (m_liangzhuObj != null)
                            {
                                Destroy(m_liangzhuObj);
                                m_liangzhuObj = null;
                            }

                            m_liangzhuObj = LiangZhu.create(this);
                            m_liangzhuObj.GetComponent<LiangZhu>().setUseType(LiangZhu.UseType.UseType_chaodi);
                            m_liangzhuObj.GetComponent<LiangZhu>().UpdateUi(GameData.getInstance().m_myPokerList,
                            GameData.getInstance().m_beforeQiangzhuPokerList);

                            // 开始抄底倒计时
                            m_timerScript.start(GameData.getInstance().m_chaodiTime,
                            TimerScript.TimerType.TimerType_ChaoDi, true);
                            setTimerPos(uid);
                        }
                        else
                        {
                            ToastScript.createToast("等待玩家抄底");

                            // 开始抄底倒计时
                            m_timerScript.start(GameData.getInstance().m_chaodiTime,
                            TimerScript.TimerType.TimerType_ChaoDi, false);
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

            // 玩家抄底
            case (int) TLJCommon.Consts.PlayAction.PlayAction_PlayerChaoDi:
            {
                m_timerScript.stop();

                string uid = jd["uid"].ToString();

                {
                    m_liangzhuObj.transform.localScale = new Vector3(0, 0, 0);
                }

                // 抄底用的牌
                {
                    if ((int) jd["hasPoker"] == 1)
                    {
                        GameData.getInstance().m_beforeQiangzhuPokerList.Clear();
                        showIsChaoDi(uid,true);

                        for (int i = 0; i < jd["pokerList"].Count; i++)
                        {
                            int num = (int) jd["pokerList"][i]["num"];
                            int pokerType = (int) jd["pokerList"][i]["pokerType"];

                            GameData.getInstance().m_beforeQiangzhuPokerList
                                .Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));
                        }

                        AudioScript.getAudioScript().playSound_ChaoDi();
                        //ToastScript.createToast("玩家抄底：" + GameData.getInstance().getPlayerDataByUid(uid).m_name);

                        {
                            // 庄家开始埋底
                            if (uid.CompareTo(UserData.uid) == 0)
                            {
                                ToastScript.createToast("开始埋底");

                                // 把底牌加上去
                                {
                                    for (int i = 0; i < jd["diPokerList"].Count; i++)
                                    {
                                        int num = (int) jd["diPokerList"][i]["num"];
                                        int pokerType = (int) jd["diPokerList"][i]["pokerType"];

                                        GameData.getInstance().m_myPokerList.Add(
                                            new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));
                                    }

                                    sortMyPokerList(GameData.getInstance().m_masterPokerType);
                                    createMyPokerObj();

                                    checkShowZhuPaiLogo();
                                }

                                // 开始埋底倒计时
                                m_timerScript.start(GameData.getInstance().m_maiDiTime,
                                    TimerScript.TimerType.TimerType_OtherMaiDi, true);
                                setTimerPos(uid);

                                // 启用埋底按钮
                                m_buttonMaiDi.transform.localScale = new Vector3(1, 1, 1);
                            }
                            else
                            {
                                ToastScript.createToast("等待庄家埋底");

                                // 开始埋底倒计时
                                m_timerScript.start(GameData.getInstance().m_maiDiTime,
                                    TimerScript.TimerType.TimerType_OtherMaiDi, false);
                                setTimerPos(uid);
                            }
                        }
                    }
                    else
                    {
                        AudioScript.getAudioScript().playSound_BuChaoDi();

                        showIsChaoDi(uid, false);
                        //ToastScript.createToast("不抄底");
                    }
                }
            }
                break;

            // 通知某人出牌
            case (int) TLJCommon.Consts.PlayAction.PlayAction_CallPlayerOutPoker:
            {
                // 禁用埋底按钮
                m_buttonMaiDi.transform.localScale = new Vector3(0, 0, 0);

                try
                {
                    string uid = (string) jd["cur_uid"];
                    bool isFreeOutPoker = (bool) jd["isFreeOutPoker"];

                    // 收到此回合第一个人出的牌
                    if (isFreeOutPoker)
                    {
                        GameData.getInstance().m_curRoundFirstOutPokerList.Clear();

                        // 清空每个人座位上的牌
                        {
                            for (int i = 0; i < GameData.getInstance().m_playerDataList.Count; i++)
                            {
                                for (int j = GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Count - 1;
                                    j >= 0;
                                    j--)
                                {
                                    Destroy(GameData.getInstance().m_playerDataList[i].m_outPokerObjList[j]);
                                }
                                GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Clear();
                            }
                        }
                    }

                    // 检测是否轮到自己出牌
                    {
                        GameData.getInstance().m_curOutPokerPlayerUid = uid;
                        if (uid.CompareTo(UserData.uid) == 0)
                        {
                            {
                                // 全部设为未选中状态
                                //PokerScript.setAllPokerWeiXuanZe();
                            }

                            GameData.getInstance().m_isFreeOutPoker = isFreeOutPoker;

                            m_buttonOutPoker.transform.localScale = new Vector3(1, 1, 1);

                            // 开始出牌倒计时
                            m_timerScript.start(GameData.getInstance().m_outPokerTime,TimerScript.TimerType.TimerType_OutPoker, true);
                            setTimerPos(uid);

                            if ((GameData.getInstance().getGameRoomType().CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ChuJi) == 0) ||
                                (GameData.getInstance().getGameRoomType().CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_ChuJi) == 0))
                            {
                                m_buttonTiShi.transform.localScale = new Vector3(1, 1, 1);
                            }
                        }
                        else
                        {
                            // 开始出牌倒计时
                            m_timerScript.start(GameData.getInstance().m_outPokerTime,TimerScript.TimerType.TimerType_OutPoker, false);
                            setTimerPos(uid);

                            m_buttonTiShi.transform.localScale = new Vector3(0, 0, 0);
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
            case (int) TLJCommon.Consts.PlayAction.PlayAction_PlayerOutPoker:
            {
                try
                {
                    m_timerScript.stop();

                    // 闲家抓到的分数
                    {
                        int beforeScore = GameData.getInstance().m_getAllScore;
                        int getScore = (int) jd["getScore"];
                        GameData.getInstance().m_getAllScore += getScore;
                        m_textScore.text = GameData.getInstance().m_getAllScore.ToString();

                        // 检查有没有达到80分
                        if ((beforeScore < 80) && (GameData.getInstance().m_getAllScore >= 80))
                        {
                            AudioScript.getAudioScript().playSound_Po();

                            if (GameData.getInstance().m_isBanker == 1)
                            {
                                ShowImageScript.create("Sprites/Game/img_game_po_fail", new Vector3(0, 0, 0));
                            }
                            else
                            {
                                ShowImageScript.create("Sprites/Game/img_game_po_success", new Vector3(0, 0, 0));
                            }
                        }
                    }

                    bool isFreeOutPoker = (bool) jd["isFreeOutPoker"];
                    bool isOutPokerOK = (bool) jd["isOutPokerOK"];

                    string uid = (string) jd["uid"];

                    // 清空此人之前出的牌
                    {
                        for (int i = 0; i < GameData.getInstance().m_playerDataList.Count; i++)
                        {
                            if (GameData.getInstance().m_playerDataList[i].m_uid.CompareTo(uid) == 0)
                            {
                                for (int j = GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Count - 1;j >= 0;j--)
                                {
                                    Destroy(GameData.getInstance().m_playerDataList[i].m_outPokerObjList[j]);
                                }
                                GameData.getInstance().m_playerDataList[i].m_outPokerObjList.Clear();

                                break;
                            }
                        }
                    }

                    // 出牌列表
                    List<TLJCommon.PokerInfo> outPokerList = new List<TLJCommon.PokerInfo>();
                    {
                        for (int i = 0; i < jd["pokerList"].Count; i++)
                        {
                            int num = (int) jd["pokerList"][i]["num"];
                            int pokerType = (int) jd["pokerList"][i]["pokerType"];

                            outPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));

                            // 如果是此回合第一个人出的牌
                            if (isFreeOutPoker && isOutPokerOK)
                            {
                                GameData.getInstance().m_curRoundFirstOutPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));
                            }
                        }
                    }

                    // 出牌类型
                    {
                        CheckOutPoker.OutPokerType outPokerType = CheckOutPoker.checkOutPokerType(outPokerList, GameData.getInstance().m_levelPokerNum, GameData.getInstance().m_masterPokerType);
                        switch (outPokerType)
                        {
                            case CheckOutPoker.OutPokerType.OutPokerType_TuoLaJi:
                                {
                                    AudioScript.getAudioScript().playSound_TuoLaJi();
                                }
                                break;
                        }
                    }

                    // 显示出的牌
                    showOtherOutPoker(outPokerList, uid);

                    // 提示和出牌按钮层级高一点
                    {
                        m_buttonTiShi.transform.SetAsLastSibling();
                        m_buttonOutPoker.transform.SetAsLastSibling();
                    }

                    if (isOutPokerOK)
                    {
                        // 刷新记牌器
                        if (m_jiPaiGameObject != null)
                        {
                            m_jiPaiGameObject.GetComponent<RememberPokerHelper>().UpdateUi(outPokerList);
                        }

                        // 如果是自己出的牌，那么就得删掉这些牌
                        if (uid.CompareTo(UserData.uid) == 0)
                        {
                            AudioScript.getAudioScript().playSound_ChuPai();

                            m_buttonOutPoker.transform.localScale = new Vector3(0, 0, 0);
                            m_buttonTiShi.transform.localScale = new Vector3(0, 0, 0);

                            for (int i = 0; i < outPokerList.Count; i++)
                            {
                                int num = outPokerList[i].m_num;
                                int pokerType = (int) outPokerList[i].m_pokerType;

                                for (int j = GameData.getInstance().m_myPokerObjList.Count - 1; j >= 0; j--)
                                {
                                    PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[j]
                                        .GetComponent<PokerScript>();
                                    if ((pokerScript.getPokerNum() == num) && (pokerScript.getPokerType() == pokerType))
                                    {
                                        // 出的牌从自己的牌堆实体对象里删除
                                        {
                                            Destroy(GameData.getInstance().m_myPokerObjList[j]);
                                            GameData.getInstance().m_myPokerObjList.RemoveAt(j);
                                        }

                                        break;
                                    }
                                }

                                for (int j = GameData.getInstance().m_myPokerList.Count - 1; j >= 0; j--)
                                {
                                    if ((GameData.getInstance().m_myPokerList[j].m_num == num) &&
                                        ((int) GameData.getInstance().m_myPokerList[j].m_pokerType == pokerType))
                                    {
                                        // 出的牌从自己的牌堆内存里删除
                                        {
                                            GameData.getInstance().m_myPokerList.RemoveAt(j);
                                        }

                                        break;
                                    }
                                }
                            }

                            initMyPokerPos(GameData.getInstance().m_myPokerObjList);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ToastScript.createToast("异常：" + ex.Message);
                }
            }
                break;

            // 改变托管状态
            case (int) TLJCommon.Consts.PlayAction.PlayAction_SetTuoGuanState:
            {
                try
                {
                    NetLoading.getInstance().Close();

                    int code = (int) jd["code"];

                    if (code == (int) TLJCommon.Consts.Code.Code_OK)
                    {
                        string uid = (string) jd["uid"];
                        bool isTuoGuan = (bool) jd["isTuoGuan"];

                        // 托管
                        if (isTuoGuan)
                        {
                            m_tuoguanObj = TuoGuanPanelScript.create(this);
                            CommonUtil.setImageSprite(m_buttonTuoGuan.transform.Find("Image").GetComponent<Image>(),"Sprites/Game/game_yituoguan");

                            GameData.getInstance().m_isTuoGuan = true;
                        }
                        // 取消托管
                        else
                        {
                            CommonUtil.setImageSprite(m_buttonTuoGuan.transform.Find("Image").GetComponent<Image>(),"Sprites/Game/game_tuoguan");

                            GameData.getInstance().m_isTuoGuan = false;
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
            case (int) TLJCommon.Consts.PlayAction.PlayAction_GameOver:
            {
                try
                {
                    //ToastScript.createToast("游戏结束");

                    bool isContiune = (bool)jd["isContiune"];

                    // 闲家抓到的分数
                    {
                        GameData.getInstance().m_getAllScore = (int) jd["getAllScore"];
                        m_textScore.text = GameData.getInstance().m_getAllScore.ToString();
                    }

                    // 判断输赢
                    {
                        int isBankerWin = (int) jd["isBankerWin"];
                        if (GameData.getInstance().m_isBanker == isBankerWin)
                        {
                            // 显示pvp结算界面
                            if (GameData.getInstance().m_isPVP)
                            {
                                if (isContiune)
                                {
                                    m_pvpGameResultPanel = PVPGameResultPanelScript.create(this);
                                    PVPGameResultPanelScript script = m_pvpGameResultPanel.GetComponent<PVPGameResultPanelScript>();
                                    script.setData(true);
                                }

                                // 更新积分
                                {
                                    int score = (int) jd["score"];
                                    m_myUserInfoUI.GetComponent<MyUIScript>().setGoldNum(score);
                                }
                            }
                            // 显示休闲场结算界面
                            else
                            {
                                GameObject obj = GameResultPanelScript.create(this);
                                GameResultPanelScript script = obj.GetComponent<GameResultPanelScript>();
                                script.setData(true, GameData.getInstance().m_getAllScore, (int) jd["score"],GameData.getInstance().getGameRoomType());

                                // 更新金币数量
                                {
                                    GameUtil.changeData(1, (int) jd["score"]);
                                    m_myUserInfoUI.GetComponent<MyUIScript>().setGoldNum(UserData.gold);
                                }
                            }
                        }
                        else
                        {
                            // 显示pvp结算界面
                            if (GameData.getInstance().m_isPVP)
                            {
                                if (isContiune)
                                {
                                    m_pvpGameResultPanel = PVPGameResultPanelScript.create(this);
                                    PVPGameResultPanelScript script = m_pvpGameResultPanel.GetComponent<PVPGameResultPanelScript>();
                                    script.setData(false);
                                }

                                // 更新积分
                                {
                                    int score = (int) jd["score"];
                                    m_myUserInfoUI.GetComponent<MyUIScript>().setGoldNum(score);
                                }
                            }
                            // 显示休闲场结算界面
                            else
                            {
                                GameObject obj = GameResultPanelScript.create(this);
                                GameResultPanelScript script = obj.GetComponent<GameResultPanelScript>();
                                script.setData(false, GameData.getInstance().m_getAllScore, (int) jd["score"],GameData.getInstance().getGameRoomType());

                                // 更新金币数量
                                {
                                    GameUtil.changeData(1, (int) jd["score"]);
                                    m_myUserInfoUI.GetComponent<MyUIScript>().setGoldNum(UserData.gold);
                                }
                            }
                        }

                        // 清空本局数据
                        {
                            clearData();
                            initUI();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ToastScript.createToast("异常：" + ex.Message);
                }
            }
                break;

            // PVP决胜局通知
            case (int) TLJCommon.Consts.PlayAction.PlayAction_JueShengJuTongZhi:
            {
                try
                {
                    Destroy(m_pvpGameResultPanel);

                    // 显示pvp结束界面
                    {
                        JueShengJuTiShiPanelScript.show();
                    }
                }
                catch (Exception ex)
                {
                    //ToastScript.createToast("异常：" + ex.Message);
                }
            }
                break;

            // PVP游戏结束
            case (int) TLJCommon.Consts.PlayAction.PlayAction_PVPGameOver:
            {
                try
                {
                    Destroy(m_pvpGameResultPanel);

                    int mingci = (int) jd["mingci"];
                    string pvpreward = jd["pvpreward"].ToString();

                    // 没有名次
                    if (pvpreward.CompareTo("") == 0)
                    {
                        m_pvpGameResultPanel = PVPGameResultPanelScript.create(this);
                        PVPGameResultPanelScript script = m_pvpGameResultPanel.GetComponent<PVPGameResultPanelScript>();
                        script.setData(false);
                    }
                    // 显示pvp结束界面
                    else
                    {
                        GameObject obj = PVPEndPanelScript.create(this);
                        PVPEndPanelScript script = obj.GetComponent<PVPEndPanelScript>();
                        script.setData(mingci, pvpreward);

                        ToastScript.createToast("游戏结束，稍后请在邮箱查看奖励");
                    }
                }
                catch (Exception ex)
                {
                    //ToastScript.createToast("异常：" + ex.Message);
                }
            }
                break;

            // 换桌
            case (int) TLJCommon.Consts.PlayAction.PlayAction_ChangeRoom:
            {
                int code = (int) jd["code"];

                switch (code)
                {
                    case (int) TLJCommon.Consts.Code.Code_OK:
                    {
                        int roomId = (int) jd["roomId"];
                        //ToastScript.createToast("加入房间成功");

                        // 禁用开始游戏按钮
                        m_buttonStartGame.transform.localScale = new Vector3(0, 0, 0);

                        //m_waitOtherPlayer = WaitOtherPlayerScript.create();
                        showWaitMatchPanel(GameData.getInstance().m_waitMatchTime, false);
                    }
                        break;

                    case (int) TLJCommon.Consts.Code.Code_CommonFail:
                    {
                        ToastScript.createToast("您已加入其它房间，无法开始新游戏");
                    }
                        break;
                }
            }
                break;

            // 继续游戏
            case (int) TLJCommon.Consts.PlayAction.PlayAction_ContinueGame:
            {
                int code = (int) jd["code"];

                switch (code)
                {
                    case (int) TLJCommon.Consts.Code.Code_OK:
                    {
                        ToastScript.createToast("继续游戏成功，等待同桌玩家");

                        // 禁用开始游戏按钮
                        m_buttonStartGame.transform.localScale = new Vector3(0, 0, 0);

                        //m_waitOtherPlayer = WaitOtherPlayerScript.create();
                        showWaitMatchPanel(GameData.getInstance().m_waitMatchTime, true);
                    }
                        break;

                    default:
                    {
                        ToastScript.createToast("有玩家退出，无法继续游戏");

                        exitRoom();
                    }
                    break;
                }
            }
                break;

            // 继续游戏失败
            case (int) TLJCommon.Consts.PlayAction.PlayAction_ContinueGameFail:
            {
                ToastScript.createToast("同桌玩家退出，无法继续游戏");

                exitRoom();
            }
            break;

            // 强制解散房间
            case (int)TLJCommon.Consts.PlayAction.PlayAction_BreakRoom:
            {
                NetErrorPanelScript.getInstance().Show();
                NetErrorPanelScript.getInstance().setOnClickButton(exitRoom);
                NetErrorPanelScript.getInstance().setContentText("该房间超时，强制解散房间");
            }
            break;

            // 匹配失败
            case (int)TLJCommon.Consts.PlayAction.PlayAction_MatchFail:
            {
                NetErrorPanelScript.getInstance().Show();
                NetErrorPanelScript.getInstance().setOnClickButton(exitRoom);
                NetErrorPanelScript.getInstance().setContentText("匹配队友失败，请稍后再试。");
            }
            break;

            // 聊天
            case (int) TLJCommon.Consts.PlayAction.PlayAction_Chat:
            {
                try
                {
                    string uid = (string) jd["uid"];
                    int type = (int) jd["type"];
                    int content_id = (int) jd["content_id"];

                    if (uid.CompareTo(UserData.uid) == 0)
                    {
                        if (type == 1)
                        {
                            string content_text = content_text = ChatData.getInstance().getChatTextById((int) jd["content_id"]).m_text;
                            ChatContentScript.createChatContent(content_text, new Vector2(-485, -280),TextAnchor.MiddleLeft);
                        }
                        else
                        {
                            EmojiScript.create(content_id, new Vector2(-470, -280));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < GameData.getInstance().m_otherPlayerUIObjList.Count; i++)
                        {
                            if (GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_uid.CompareTo(uid) == 0)
                            {
                                switch (GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_direction)
                                {
                                    case OtherPlayerUIScript.Direction.Direction_Up:
                                    {
                                        if (type == 1)
                                        {
                                            string content_text = content_text = ChatData.getInstance().getChatTextById((int) jd["content_id"]).m_text;
                                            GameObject obj = ChatContentScript.createChatContent(content_text, new Vector2(-80, 285),TextAnchor.MiddleRight);
                                            obj.transform.localScale = new Vector3(-1, 1, 1);
                                            obj.transform.Find("Text").localScale = new Vector3(-1, 1, 1);
                                        }
                                        else
                                        {
                                            EmojiScript.create(content_id, new Vector2(0, 200));
                                        }
                                    }
                                        break;

                                    case OtherPlayerUIScript.Direction.Direction_Left:
                                    {
                                        if (type == 1)
                                        {
                                            string content_text = content_text = ChatData.getInstance().getChatTextById((int) jd["content_id"]).m_text;
                                            ChatContentScript.createChatContent(content_text, new Vector2(-470, 0),TextAnchor.MiddleLeft);
                                        }
                                        else
                                        {
                                            EmojiScript.create(content_id, new Vector2(-460, 5));
                                        }
                                    }
                                        break;

                                    case OtherPlayerUIScript.Direction.Direction_Right:
                                    {
                                        if (type == 1)
                                        {
                                            string content_text = content_text = ChatData.getInstance().getChatTextById((int) jd["content_id"]).m_text;
                                            GameObject obj = ChatContentScript.createChatContent(content_text, new Vector2(480, 0),TextAnchor.MiddleRight);
                                            obj.transform.localScale = new Vector3(-1,1,1);
                                            obj.transform.Find("Text").localScale = new Vector3(-1, 1, 1);
                                        }
                                        else
                                        {
                                            EmojiScript.create(content_id, new Vector2(450, 5));
                                        }
                                    }
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ToastScript.createToast("异常：" + ex.Message);

                    LogUtil.Log("onReceive_PlayGame.PlayAction_Chat异常：" + ex.Message);
                }
            }
                break;
        }
    }

    void onReceive_UserInfo_Game(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int) jd["code"];

        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            string uid = (string) jd["uid"];

            GameData.getInstance().getPlayerDataByUid(uid).m_name = (string) jd["name"];
            GameData.getInstance().getPlayerDataByUid(uid).m_head = "Sprites/Head/head_" + (int) jd["head"];
            GameData.getInstance().getPlayerDataByUid(uid).m_gold = (int) jd["gold"];
            GameData.getInstance().getPlayerDataByUid(uid).m_vipLevel = (int) jd["vipLevel"];
            GameData.getInstance().getPlayerDataByUid(uid).m_allGameCount = (int) jd["gameData"]["allGameCount"];
            GameData.getInstance().getPlayerDataByUid(uid).m_winCount = (int) jd["gameData"]["winCount"];
            GameData.getInstance().getPlayerDataByUid(uid).m_runCount = (int) jd["gameData"]["runCount"];
            GameData.getInstance().getPlayerDataByUid(uid).m_meiliZhi = (int) jd["gameData"]["meiliZhi"];

            GameData.getInstance().setOtherPlayerUI(uid, isPVP());
        }
    }

    void onReceive_UseBuff(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int) jd["code"];

        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            string uid = (string) jd["uid"];
            int prop_id = (int) jd["prop_id"];

            // 记牌器
            if (prop_id == (int) TLJCommon.Consts.Prop.Prop_jipaiqi)
            {
                // 剩余数量-1
                for (int i = 0; i < UserData.buffData.Count; i++)
                {
                    if (UserData.buffData[i].prop_id == (int) TLJCommon.Consts.Prop.Prop_jipaiqi)
                    {
                        --UserData.buffData[i].buff_num;
                        break;
                    }
                }
            }
        }
    }

    void onReceive_IsJoinGame(string data)
    {
        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(data);

        int isJoinGame = (int) jd["isJoinGame"];

        if (isJoinGame == 1)
        {
            ToastScript.createToast("当前已经加入房间");

            string gameRoomType = jd["gameRoomType"].ToString();
            List<string> list = new List<string>();
            CommonUtil.splitStr(gameRoomType, list, '_');
            if (list[0].CompareTo("PVP") == 0)
            {
                GameData.getInstance().m_tag = TLJCommon.Consts.Tag_JingJiChang;
                GameData.getInstance().setGameRoomType(gameRoomType);
                
                reqRetryJoinGame();
            }
            else
            {
                GameData.getInstance().m_tag = TLJCommon.Consts.Tag_XiuXianChang;
                GameData.getInstance().setGameRoomType(gameRoomType);

                reqRetryJoinGame();
            }
        }
        else
        {
            if (GameData.getInstance().m_isStartGame)
            {
                exitRoom();
            }
            else
            {
                GameUtil.showGameObject(m_buttonStartGame.gameObject);
            }
        }
    }

    void onReceive_ResumeGame(string data)
    {
        try
        {
            NetLoading.getInstance().Close();

            clearData();

            JsonData jd = JsonMapper.ToObject(data);

            int roomState = (int) jd["roomState"];
            
            string zhuangjiaUID = jd["zhuangjiaUID"].ToString();
            string curMaiDiPlayer = jd["curMaiDiPlayer"].ToString();
            string curOutPokerPlayer = jd["curOutPokerPlayer"].ToString();
            string curRoundFirstPlayer = jd["curRoundFirstPlayer"].ToString();
            string curChaoDiPlayer = jd["curChaoDiPlayer"].ToString();

            GameData.getInstance().setGameRoomType(jd["gameroomtype"].ToString());
            GameData.getInstance().m_levelPokerNum = (int) jd["levelPokerNum"];
            GameData.getInstance().m_myLevelPoker = (int) jd["myLevelPoker"];
            GameData.getInstance().m_otherLevelPoker = (int) jd["otherLevelPoker"];
            GameData.getInstance().m_masterPokerType = (int) jd["masterPokerType"];
            GameData.getInstance().m_getAllScore = (int) jd["getAllScore"];

            initUI();
            startGame_InitUI(data);
            setCommonUI(GameData.getInstance().getGameRoomType());

            // 闲家抓到的分数
            m_textScore.text = GameData.getInstance().m_getAllScore.ToString();

            // 是否使用了记牌器
            {
                bool isUseJiPaiQi = (bool) jd["isUseJiPaiQi"];
                if (isUseJiPaiQi)
                {
                    m_buttonJiPaiQi.transform.localScale = new Vector3(1, 1, 1);
                    m_hasJiPaiQiUse = true;
                }
            }

            // 我的手牌
            {
                for (int i = 0; i < jd["myPokerList"].Count; i++)
                {
                    int num = (int) jd["myPokerList"][i]["num"];
                    int pokerType = (int) jd["myPokerList"][i]["pokerType"];

                    GameData.getInstance().m_myPokerList
                        .Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));
                }

                sortMyPokerList(GameData.getInstance().m_masterPokerType); // 对我的牌进行排序
                createMyPokerObj(); // 创建我的牌对象
                checkShowZhuPaiLogo();
            }

            // 最后埋底的人
            {
                GameData.getInstance().m_lastMaiDiPlayer = jd["lastMaiDiPlayer"].ToString();
                if (GameData.getInstance().m_lastMaiDiPlayer.CompareTo(UserData.uid) == 0)
                {
                    m_buttonDiPai.transform.localScale = new Vector3(1, 1, 1);
                    m_buttonDiPai.interactable = true;
                    CommonUtil.setImageSprite(m_buttonDiPai.GetComponent<Image>(), "Sprites/Game/game_btn_green");
                }
                else
                {
                    m_buttonDiPai.transform.localScale = new Vector3(1, 1, 1);
                    m_buttonDiPai.interactable = false;
                    CommonUtil.setImageSprite(m_buttonDiPai.GetComponent<Image>(), "Sprites/Game/game_btn_gray");
                }
            }

            // 底牌
            {
                GameData.getInstance().m_dipaiList.Clear();

                for (int i = 0; i < jd["diPokerList"].Count; i++)
                {
                    int num = (int) jd["diPokerList"][i]["num"];
                    int pokerType = (int) jd["diPokerList"][i]["pokerType"];

                    GameData.getInstance().m_dipaiList
                        .Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));
                }
            }

            // 主牌花色
            {
                //if(GameData.getInstance().m_masterPokerType != -1)
                {
                    GameUtil.showGameObject(m_imageMasterPokerType.gameObject);
                    GameData.getInstance().m_masterPokerType = GameData.getInstance().m_masterPokerType;
                    CommonUtil.setImageSprite(m_imageMasterPokerType, GameUtil.getMasterPokerIconPath(GameData.getInstance().m_masterPokerType));
                }
            }

            // 判断谁是庄家
            {
                if (zhuangjiaUID.CompareTo("") != 0)
                {
                    if (zhuangjiaUID.CompareTo(UserData.uid) == 0)
                    {
                        m_myUserInfoUI.GetComponent<MyUIScript>().m_imageZhuangJiaIcon.transform.localScale = new Vector3(1, 1, 1);
                        GameData.getInstance().m_isBanker = 1;
                    }
                    else
                    {
                        for (int i = 0; i < GameData.getInstance().m_otherPlayerUIObjList.Count; i++)
                        {
                            if (GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_uid.CompareTo(zhuangjiaUID) == 0)
                            {
                                GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_imageZhuangJiaIcon.transform.localScale = new Vector3(1, 1, 1);

                                if (GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_uid.CompareTo(GameData.getInstance().m_teammateUID) == 0)
                                {
                                    GameData.getInstance().m_isBanker = 1;
                                }
                                else
                                {
                                    GameData.getInstance().m_isBanker = 0;
                                }
                            }
                        }
                    }
                }
            }

            //----------------------------------------------------------------------------------------------------------------
            switch (roomState)
            {
                case (int) TLJCommon.Consts.RoomState.RoomState_waiting:
                {
                    // 不需要处理
                }
                    break;

                case (int) TLJCommon.Consts.RoomState.RoomState_qiangzhu:
                {
                    GameData.getInstance().m_myPokerList.Clear();

                    // 已经发的牌
                    {
                        for (int i = 0; i < jd["allotPokerList"].Count; i++)
                        {
                            int num = (int) jd["allotPokerList"][i]["num"];
                            int pokerType = (int) jd["allotPokerList"][i]["pokerType"];

                            GameData.getInstance().m_myPokerList
                                .Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));
                        }

                        sortMyPokerList(GameData.getInstance().m_masterPokerType); // 对我的牌进行排序
                        createMyPokerObj(); // 创建我的牌对象

                        if (jd["allotPokerList"].Count == 25)
                        {
                            // 开始倒计时
                            m_timerScript.start(GameData.getInstance().m_qiangZhuTime,
                                TimerScript.TimerType.TimerType_QiangZhu, true);
                        }
                    }
                }
                    break;

                case (int) TLJCommon.Consts.RoomState.RoomState_zhuangjiamaidi:
                {
                    GameUtil.hideGameObject(m_liangzhuObj);

                    // 判断谁是庄家
                    {
                        // 庄家开始埋底
                        if (curMaiDiPlayer.CompareTo(UserData.uid) == 0)
                        {
                            ToastScript.createToast("开始埋底");

                            // 开始埋底倒计时
                            m_timerScript.start(GameData.getInstance().m_maiDiTime,
                                TimerScript.TimerType.TimerType_MaiDi, true);
                            setTimerPos(curMaiDiPlayer);

                            // 启用埋底按钮
                            m_buttonMaiDi.transform.localScale = new Vector3(1, 1, 1);

                            checkShowZhuPaiLogo();
                        }
                        else
                        {
                            ToastScript.createToast("等待庄家埋底");

                            // 开始埋底倒计时
                            m_timerScript.start(GameData.getInstance().m_maiDiTime,
                                TimerScript.TimerType.TimerType_MaiDi, false);
                            setTimerPos(curMaiDiPlayer);
                        }
                    }
                }
                    break;

                case (int) TLJCommon.Consts.RoomState.RoomState_chaodi:
                {
                    // 检测是否轮到自己抄底
                    {
                        if (curChaoDiPlayer.CompareTo(UserData.uid) == 0)
                        {
                            if (m_liangzhuObj != null)
                            {
                                Destroy(m_liangzhuObj);
                                m_liangzhuObj = null;
                            }

                            m_liangzhuObj = LiangZhu.create(this);
                            m_liangzhuObj.GetComponent<LiangZhu>().setUseType(LiangZhu.UseType.UseType_chaodi);
                            m_liangzhuObj.GetComponent<LiangZhu>().UpdateUi(GameData.getInstance().m_myPokerList,
                            GameData.getInstance().m_beforeQiangzhuPokerList);

                            // 开始抄底倒计时
                            m_timerScript.start(GameData.getInstance().m_chaodiTime,
                            TimerScript.TimerType.TimerType_ChaoDi, true);
                            setTimerPos(curChaoDiPlayer);
                        }
                        else
                        {
                            ToastScript.createToast("等待玩家抄底");

                            GameUtil.hideGameObject(m_liangzhuObj);

                            // 开始抄底倒计时
                            m_timerScript.start(GameData.getInstance().m_chaodiTime,
                            TimerScript.TimerType.TimerType_ChaoDi, false);
                            setTimerPos(curChaoDiPlayer);
                        }
                    }
                }
                    break;

                case (int) TLJCommon.Consts.RoomState.RoomState_othermaidi:
                {
                    GameUtil.hideGameObject(m_liangzhuObj);

                    {
                        // 庄家开始埋底
                        if (curMaiDiPlayer.CompareTo(UserData.uid) == 0)
                        {
                            ToastScript.createToast("开始埋底");

                            // 开始埋底倒计时
                            m_timerScript.start(GameData.getInstance().m_maiDiTime,
                                TimerScript.TimerType.TimerType_MaiDi, true);
                            setTimerPos(curMaiDiPlayer);

                            // 启用埋底按钮
                            m_buttonMaiDi.transform.localScale = new Vector3(1, 1, 1);

                            checkShowZhuPaiLogo();
                        }
                        else
                        {
                            ToastScript.createToast("等待玩家埋底");

                            // 开始埋底倒计时
                            m_timerScript.start(GameData.getInstance().m_maiDiTime,
                                TimerScript.TimerType.TimerType_MaiDi, false);
                            setTimerPos(curMaiDiPlayer);
                        }
                    }
                }
                    break;

                case (int) TLJCommon.Consts.RoomState.RoomState_gaming:
                {
                    GameUtil.hideGameObject(m_liangzhuObj);
                    GameUtil.showGameObject(m_timer);
                        
                    // 显示当前回合出的牌
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            PlayerData playerData = GameData.getInstance().m_playerDataList[i];

                            List<TLJCommon.PokerInfo> outPokerList = new List<TLJCommon.PokerInfo>();
                            for (int j = 0; j < jd["player" + i + "OutPokerList"].Count; j++)
                            {
                                int num = (int) jd["player" + i + "OutPokerList"][j]["num"];
                                int pokerType = (int) jd["player" + i + "OutPokerList"][j]["pokerType"];

                                outPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));

                                // 当前回合第一个玩家出的牌
                                if (playerData.m_uid.CompareTo(curRoundFirstPlayer) == 0)
                                {
                                    GameData.getInstance().m_curRoundFirstOutPokerList
                                        .Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));
                                }
                            }

                            showOtherOutPoker(outPokerList, playerData.m_uid);
                        }
                    }

                    // 当前出牌的人
                    {
                        if (curOutPokerPlayer.CompareTo("") != 0)
                        {
                            if (curOutPokerPlayer.CompareTo(UserData.uid) == 0)
                            {
                                int isFreeOutPoker = (int) jd["isFreeOutPoker"];
                                if (isFreeOutPoker == 1)
                                {
                                    GameData.getInstance().m_isFreeOutPoker = true;
                                    //ToastScript.createToast("轮到你出牌：任意出");
                                }
                                else
                                {
                                    GameData.getInstance().m_isFreeOutPoker = false;
                                    //ToastScript.createToast("轮到你出牌：跟牌");
                                }

                                m_buttonOutPoker.transform.localScale = new Vector3(1, 1, 1);

                                // 开始出牌倒计时
                                m_timerScript.start(GameData.getInstance().m_outPokerTime,
                                    TimerScript.TimerType.TimerType_OutPoker, true);
                                setTimerPos(curOutPokerPlayer);

                                if ((GameData.getInstance().getGameRoomType()
                                         .CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ChuJi) == 0) ||
                                    (GameData.getInstance().getGameRoomType()
                                         .CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_ChuJi) == 0))
                                {
                                    m_buttonTiShi.transform.localScale = new Vector3(1, 1, 1);
                                }
                            }
                            else
                            {
                                // 开始出牌倒计时
                                m_timerScript.start(GameData.getInstance().m_outPokerTime,
                                    TimerScript.TimerType.TimerType_OutPoker, false);
                                setTimerPos(curOutPokerPlayer);

                                m_buttonTiShi.transform.localScale = new Vector3(0, 0, 0);
                            }
                        }
                    }

                    // 恢复记牌器数据
                    {
                        List<TLJCommon.PokerInfo> list = new List<TLJCommon.PokerInfo>();
                        for (int i = 0; i < jd["allOutPokerList"].Count; i++)
                        {
                            list.Clear();

                            int num = (int) jd["allOutPokerList"][i]["num"];
                            int pokerType = (int) jd["allOutPokerList"][i]["pokerType"];

                            list.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType) pokerType));

                            print(list.Count);
                            m_jiPaiGameObject.GetComponent<RememberPokerHelper>().UpdateUi(list);
                        }
                    }
                }
                    break;

                case (int) TLJCommon.Consts.RoomState.RoomState_end:
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

    //----------------------------------------------------------接收数据 end--------------------------------------------------

    void startGame()
    {
    }

    void createMyPokerObj()
    {
        // 先删掉旧的
        for (int i = GameData.getInstance().m_myPokerObjList.Count - 1; i >= 0; i--)
        {
            Destroy(GameData.getInstance().m_myPokerObjList[i]);
            GameData.getInstance().m_myPokerObjList.RemoveAt(i);
        }

        for (int i = 0; i < GameData.getInstance().m_myPokerList.Count; i++)
        {
            GameObject poker = PokerScript.createPoker();
            poker.transform.SetParent(GameObject.Find("Canvas").transform);
            poker.transform.localScale = new Vector3(1, 1, 1);

            poker.GetComponent<PokerScript>().initPoker(GameData.getInstance().m_myPokerList[i].m_num,
                (int) GameData.getInstance().m_myPokerList[i].m_pokerType);
            poker.GetComponent<PokerScript>().m_canTouch = true;

            GameData.getInstance().m_myPokerObjList.Add(poker);
        }

        initMyPokerPos(GameData.getInstance().m_myPokerObjList);
    }

    void checkShowZhuPaiLogo()
    {
        for (int i = 0; i < GameData.getInstance().m_myPokerObjList.Count; i++)
        {
            int num = GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().getPokerNum();
            int pokerType = GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().getPokerType();

            if (GameData.getInstance().m_levelPokerNum == num)
            {
                GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().showZhuPaiLogo();
            }
            else if ((GameData.getInstance().m_masterPokerType != -1) &&
                     (GameData.getInstance().m_masterPokerType == pokerType))
            {
                GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().showZhuPaiLogo();
            }
            else
            {
                GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().closeZhuPaiLogo();
            }
        }
    }

    void initOutPokerPos(List<GameObject> objList, OtherPlayerUIScript.Direction direc)
    {
        int jiange = 30;

        switch (direc)
        {
            case OtherPlayerUIScript.Direction.Direction_Up:
            {
                for (int i = 0; i < objList.Count; i++)
                {
                    int x = CommonUtil.getPosX(objList.Count, jiange, i, 0);
                    objList[i].transform.localPosition = new Vector3(x, 130, 0);

                    // 设置最后渲染
                    //objList[i].transform.SetAsLastSibling();
                }
            }
                break;

            case OtherPlayerUIScript.Direction.Direction_Left:
            {
                for (int i = 0; i < objList.Count; i++)
                {
                    float startX = -440;
                    objList[i].transform.localPosition = new Vector3(startX + (i * jiange), 0, 0);

                    // 设置最后渲染
                    //objList[i].transform.SetAsLastSibling();
                }
            }
                break;

            case OtherPlayerUIScript.Direction.Direction_Right:
            {
                for (int i = 0; i < objList.Count; i++)
                {
                    float endX = 480;
                    objList[i].transform.localPosition = new Vector3(endX - ((objList.Count - i) * jiange), 0, 0);

                    // 设置最后渲染
                    //objList[i].transform.SetAsLastSibling();
                }
            }
                break;

            case OtherPlayerUIScript.Direction.Direction_Down:
            {
                for (int i = 0; i < objList.Count; i++)
                {
                    int x = CommonUtil.getPosX(objList.Count, jiange, i, 0);
                    objList[i].transform.localPosition = new Vector3(x, -85, 0);

                    // 设置最后渲染
                    //objList[i].transform.SetAsLastSibling();
                }
            }
                break;
        }
    }

    void initMyPokerPos(List<GameObject> objList)
    {
        int jiange = 35;

        for (int i = 0; i < objList.Count; i++)
        {
            int x = CommonUtil.getPosX(objList.Count, jiange, i, 0);
            objList[i].transform.localPosition = new Vector3(x, -225, 0);
            objList[i].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            // 设置最后渲染
            objList[i].transform.SetAsLastSibling();
        }

        //// 让当前已出的牌（包括抢主的牌）显示在最上面
        //for (int i = 0; i < GameData.getInstance().m_curRoundOutPokerList.Count; i++)
        //{
        //    for (int j = i; j < GameData.getInstance().m_curRoundOutPokerList[i].Count; j++)
        //    {
        //        GameData.getInstance().m_curRoundOutPokerList[i][j].transform.SetAsLastSibling();
        //    }
        //}
    }

    void sortMyPokerList(int ZhuPokerType)
    {
        // 大小王
        List<TLJCommon.PokerInfo> wangPokerList = new List<TLJCommon.PokerInfo>();
        {
            for (int i = 0; i < GameData.getInstance().m_myPokerList.Count; i++)
            {
                if (GameData.getInstance().m_myPokerList[i].m_pokerType == TLJCommon.Consts.PokerType.PokerType_Wang)
                {
                    wangPokerList.Add(GameData.getInstance().m_myPokerList[i]);
                }
            }

            // 排序
            for (int i = 0; i < wangPokerList.Count - 1; i++)
            {
                for (int j = (i + 1); j < wangPokerList.Count; j++)
                {
                    if (wangPokerList[j].m_num > wangPokerList[i].m_num)
                    {
                        TLJCommon.PokerInfo temp = wangPokerList[j];
                        wangPokerList[j] = wangPokerList[i];
                        wangPokerList[i] = temp;
                    }
                }
            }
        }

        // 级牌
        List<TLJCommon.PokerInfo> levelPokerList = new List<TLJCommon.PokerInfo>();
        {
            for (int i = 0; i < GameData.getInstance().m_myPokerList.Count; i++)
            {
                if (GameData.getInstance().m_myPokerList[i].m_num == GameData.getInstance().m_levelPokerNum)
                {
                    levelPokerList.Add(GameData.getInstance().m_myPokerList[i]);
                }
            }

            // 排序
            for (int i = 0; i < levelPokerList.Count - 1; i++)
            {
                for (int j = (i + 1); j < levelPokerList.Count; j++)
                {
                    if ((int) levelPokerList[j].m_pokerType == ZhuPokerType)
                    {
                        TLJCommon.PokerInfo temp = levelPokerList[j];
                        levelPokerList[j] = levelPokerList[i];
                        levelPokerList[i] = temp;
                    }
                    else
                    {
                        if (((int) levelPokerList[i].m_pokerType != ZhuPokerType) &&
                            (levelPokerList[j].m_pokerType > levelPokerList[i].m_pokerType))
                        {
                            TLJCommon.PokerInfo temp = levelPokerList[j];
                            levelPokerList[j] = levelPokerList[i];
                            levelPokerList[i] = temp;
                        }
                    }

                    //if (((int)levelPokerList[j].m_pokerType == ZhuPokerType) || (levelPokerList[j].m_pokerType > levelPokerList[i].m_pokerType))
                    //{
                    //    TLJCommon.PokerInfo temp = levelPokerList[j];
                    //    levelPokerList[j] = levelPokerList[i];
                    //    levelPokerList[i] = temp;
                    //}
                }
            }
        }

        // 黑桃
        List<TLJCommon.PokerInfo> heitaoPokerList = new List<TLJCommon.PokerInfo>();
        {
            for (int i = 0; i < GameData.getInstance().m_myPokerList.Count; i++)
            {
                if (GameData.getInstance().m_levelPokerNum != GameData.getInstance().m_myPokerList[i].m_num)
                {
                    if (GameData.getInstance().m_myPokerList[i].m_pokerType ==
                        TLJCommon.Consts.PokerType.PokerType_HeiTao)
                    {
                        heitaoPokerList.Add(GameData.getInstance().m_myPokerList[i]);
                    }
                }
            }

            // 排序
            for (int i = 0; i < heitaoPokerList.Count - 1; i++)
            {
                for (int j = (i + 1); j < heitaoPokerList.Count; j++)
                {
                    if (heitaoPokerList[j].m_num > heitaoPokerList[i].m_num)
                    {
                        TLJCommon.PokerInfo temp = heitaoPokerList[j];
                        heitaoPokerList[j] = heitaoPokerList[i];
                        heitaoPokerList[i] = temp;
                    }
                }
            }
        }

        // 红桃
        List<TLJCommon.PokerInfo> hongtaoPokerList = new List<TLJCommon.PokerInfo>();
        {
            for (int i = 0; i < GameData.getInstance().m_myPokerList.Count; i++)
            {
                if (GameData.getInstance().m_levelPokerNum != GameData.getInstance().m_myPokerList[i].m_num)
                {
                    if (GameData.getInstance().m_myPokerList[i].m_pokerType ==
                        TLJCommon.Consts.PokerType.PokerType_HongTao)
                    {
                        hongtaoPokerList.Add(GameData.getInstance().m_myPokerList[i]);
                    }
                }
            }

            // 排序
            for (int i = 0; i < hongtaoPokerList.Count - 1; i++)
            {
                for (int j = (i + 1); j < hongtaoPokerList.Count; j++)
                {
                    if (hongtaoPokerList[j].m_num > hongtaoPokerList[i].m_num)
                    {
                        TLJCommon.PokerInfo temp = hongtaoPokerList[j];
                        hongtaoPokerList[j] = hongtaoPokerList[i];
                        hongtaoPokerList[i] = temp;
                    }
                }
            }
        }

        // 梅花
        List<TLJCommon.PokerInfo> meihuaPokerList = new List<TLJCommon.PokerInfo>();
        {
            for (int i = 0; i < GameData.getInstance().m_myPokerList.Count; i++)
            {
                if (GameData.getInstance().m_levelPokerNum != GameData.getInstance().m_myPokerList[i].m_num)
                {
                    if (GameData.getInstance().m_myPokerList[i].m_pokerType ==
                        TLJCommon.Consts.PokerType.PokerType_MeiHua)
                    {
                        meihuaPokerList.Add(GameData.getInstance().m_myPokerList[i]);
                    }
                }
            }

            // 排序
            for (int i = 0; i < meihuaPokerList.Count - 1; i++)
            {
                for (int j = (i + 1); j < meihuaPokerList.Count; j++)
                {
                    if (meihuaPokerList[j].m_num > meihuaPokerList[i].m_num)
                    {
                        TLJCommon.PokerInfo temp = meihuaPokerList[j];
                        meihuaPokerList[j] = meihuaPokerList[i];
                        meihuaPokerList[i] = temp;
                    }
                }
            }
        }

        // 方块
        List<TLJCommon.PokerInfo> fangkuaiPokerList = new List<TLJCommon.PokerInfo>();
        {
            for (int i = 0; i < GameData.getInstance().m_myPokerList.Count; i++)
            {
                if (GameData.getInstance().m_levelPokerNum != GameData.getInstance().m_myPokerList[i].m_num)
                {
                    if (GameData.getInstance().m_myPokerList[i].m_pokerType ==
                        TLJCommon.Consts.PokerType.PokerType_FangKuai)
                    {
                        fangkuaiPokerList.Add(GameData.getInstance().m_myPokerList[i]);
                    }
                }
            }

            // 排序
            for (int i = 0; i < fangkuaiPokerList.Count - 1; i++)
            {
                for (int j = (i + 1); j < fangkuaiPokerList.Count; j++)
                {
                    if (fangkuaiPokerList[j].m_num > fangkuaiPokerList[i].m_num)
                    {
                        TLJCommon.PokerInfo temp = fangkuaiPokerList[j];
                        fangkuaiPokerList[j] = fangkuaiPokerList[i];
                        fangkuaiPokerList[i] = temp;
                    }
                }
            }
        }

        // 无主牌
        if ((ZhuPokerType == -1) || (ZhuPokerType == 4) || (ZhuPokerType == 15) || (ZhuPokerType == 16))
        {
            GameData.getInstance().m_myPokerList.Clear();

            // 王
            for (int i = 0; i < wangPokerList.Count; i++)
            {
                GameData.getInstance().m_myPokerList.Add(wangPokerList[i]);
            }

            // 级牌
            for (int i = 0; i < levelPokerList.Count; i++)
            {
                GameData.getInstance().m_myPokerList.Add(levelPokerList[i]);
            }

            // 黑桃
            for (int i = 0; i < heitaoPokerList.Count; i++)
            {
                GameData.getInstance().m_myPokerList.Add(heitaoPokerList[i]);
            }

            // 红桃
            for (int i = 0; i < hongtaoPokerList.Count; i++)
            {
                GameData.getInstance().m_myPokerList.Add(hongtaoPokerList[i]);
            }

            //梅花
            for (int i = 0; i < meihuaPokerList.Count; i++)
            {
                GameData.getInstance().m_myPokerList.Add(meihuaPokerList[i]);
            }

            //方块
            for (int i = 0; i < fangkuaiPokerList.Count; i++)
            {
                GameData.getInstance().m_myPokerList.Add(fangkuaiPokerList[i]);
            }
        }
        // 有主牌
        else
        {
            GameData.getInstance().m_myPokerList.Clear();

            // 王
            for (int i = 0; i < wangPokerList.Count; i++)
            {
                GameData.getInstance().m_myPokerList.Add(wangPokerList[i]);
            }

            // 级牌
            for (int i = 0; i < levelPokerList.Count; i++)
            {
                GameData.getInstance().m_myPokerList.Add(levelPokerList[i]);
            }

            List<List<TLJCommon.PokerInfo>> list = new List<List<TLJCommon.PokerInfo>>();

            switch (ZhuPokerType)
            {
                case (int) TLJCommon.Consts.PokerType.PokerType_HeiTao:
                {
                    list.Add(heitaoPokerList);
                    list.Add(hongtaoPokerList);
                    list.Add(meihuaPokerList);
                    list.Add(fangkuaiPokerList);
                }
                    break;

                case (int) TLJCommon.Consts.PokerType.PokerType_HongTao:
                {
                    list.Add(hongtaoPokerList);
                    list.Add(heitaoPokerList);
                    list.Add(meihuaPokerList);
                    list.Add(fangkuaiPokerList);
                }
                    break;

                case (int) TLJCommon.Consts.PokerType.PokerType_MeiHua:
                {
                    list.Add(meihuaPokerList);
                    list.Add(heitaoPokerList);
                    list.Add(hongtaoPokerList);
                    list.Add(fangkuaiPokerList);
                }
                    break;

                case (int) TLJCommon.Consts.PokerType.PokerType_FangKuai:
                {
                    list.Add(fangkuaiPokerList);
                    list.Add(heitaoPokerList);
                    list.Add(hongtaoPokerList);
                    list.Add(meihuaPokerList);
                }
                    break;
            }

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Count; j++)
                {
                    GameData.getInstance().m_myPokerList.Add(list[i][j]);
                }
            }
        }
    }

    void showOtherOutPoker(List<TLJCommon.PokerInfo> pokerList, string uid)
    {
        if (pokerList.Count == 0)
        {
            return;
        }

        // 创建现在出的牌
        List<GameObject> tempList = new List<GameObject>();
        for (int i = 0; i < pokerList.Count; i++)
        {
            GameObject poker = PokerScript.createPoker();
            poker.transform.SetParent(GameObject.Find("Canvas").transform);
            poker.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            poker.GetComponent<PokerScript>().initPoker(pokerList[i].m_num, (int) pokerList[i].m_pokerType);

            tempList.Add(poker);

            // 出的牌的主牌标识
            {
                if (GameData.getInstance().m_levelPokerNum == pokerList[i].m_num)
                {
                    poker.GetComponent<PokerScript>().showZhuPaiLogo();
                }
                else if ((GameData.getInstance().m_masterPokerType != -1) &&
                         (GameData.getInstance().m_masterPokerType == (int) pokerList[i].m_pokerType))
                {
                    poker.GetComponent<PokerScript>().showZhuPaiLogo();
                }
            }
        }

        GameData.getInstance().getPlayerDataByUid(uid).m_outPokerObjList = tempList;

        // 显示在正确的座位上
        if (uid.CompareTo(UserData.uid) == 0)
        {
            initOutPokerPos(tempList, OtherPlayerUIScript.Direction.Direction_Down);
        }
        else
        {
            for (int i = 0; i < GameData.getInstance().m_otherPlayerUIObjList.Count; i++)
            {
                if (GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_uid
                        .CompareTo(uid) == 0)
                {
                    initOutPokerPos(tempList,
                        GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>()
                            .m_direction);
                    break;
                }
            }
        }
    }

    void setTimerPos(string uid)
    {
        if (uid.CompareTo(UserData.uid) == 0)
        {
            //m_timer.transform.localPosition = new Vector3(-552,-185,0);
            m_timer.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            for (int i = 0; i < GameData.getInstance().m_otherPlayerUIObjList.Count; i++)
            {
                if (GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_uid
                        .CompareTo(uid) == 0)
                {
                    switch (GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>()
                        .m_direction)
                    {
                        case OtherPlayerUIScript.Direction.Direction_Up:
                        {
                            m_timer.transform.localPosition = new Vector3(103, 288, 0);
                        }
                            break;

                        case OtherPlayerUIScript.Direction.Direction_Left:
                        {
                            m_timer.transform.localPosition = new Vector3(-470, 102, 0);
                        }
                            break;

                        case OtherPlayerUIScript.Direction.Direction_Right:
                        {
                            m_timer.transform.localPosition = new Vector3(590, 104, 0);
                        }
                            break;
                    }
                }
            }
        }
    }

    void showIsChaoDi(string uid,bool isChaoDi)
    {
        string imgPath = "";
        if (isChaoDi)
        {
            imgPath = "Sprites/Game/img_game_chaodi";
        }
        else
        {
            imgPath = "Sprites/Game/img_game_buchaodi";
        }

        if (uid.CompareTo(UserData.uid) == 0)
        {
            ShowImageScript.create(imgPath, new Vector3(-427.33f, -234.64f, 0));
        }
        else
        {
            for (int i = 0; i < GameData.getInstance().m_otherPlayerUIObjList.Count; i++)
            {
                if (GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_uid.CompareTo(uid) == 0)
                {
                    switch (GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_direction)
                    {
                        case OtherPlayerUIScript.Direction.Direction_Up:
                            {
                                ShowImageScript.create(imgPath, new Vector3(-152.2f, 289.5f, 0));
                            }
                            break;

                        case OtherPlayerUIScript.Direction.Direction_Left:
                            {
                                ShowImageScript.create(imgPath, new Vector3(-410f, 76.5f, 0));
                            }
                            break;

                        case OtherPlayerUIScript.Direction.Direction_Right:
                            {
                                ShowImageScript.create(imgPath, new Vector3(405.8f, 59.1f, 0));
                            }
                            break;
                    }
                }
            }
        }
    }

    public void useProp_jipaiqi()
    {
        if (!m_hasJiPaiQiUse)
        {
            if (!isPVP())
            {
                if(GameData.getInstance().m_isStartGame)
                //if (m_isStartGame)
                {
                    if (!m_hasJiPaiQiUse)
                    {
                        m_buttonJiPaiQi.transform.localScale = new Vector3(1, 1, 1);
                        m_hasJiPaiQiUse = true;

                        // 休闲场有记牌器的情况下自动使用
                        if (m_hasJiPaiQiUse)
                        {
                            reqUseBuff((int) TLJCommon.Consts.Prop.Prop_jipaiqi);
                        }
                    }
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------

    void onSocketReceive_Play(string data)
    {
        onReceive(data);
    }

    void onSocketConnect_Play(bool result)
    {
        NetLoading.getInstance().Close();

        if (result)
        {
            //LogUtil.Log("连接服务器成功");

            //ToastScript.createToast("连接Play服务器成功");

            NetLoading.getInstance().Close();
            NetErrorPanelScript.getInstance().Close();

            // 检查是否已经加入房间，已经加入的话则恢复房间
            reqIsJoinRoom();

            // 检测服务器是否连接
            checkNet();
        }
        else
        {
            //LogUtil.Log("连接服务器失败，尝试重新连接");

            //NetErrorPanelScript.getInstance().Show();
            //NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Play);
            //NetErrorPanelScript.getInstance().setContentText("连接游戏服务器失败，请重新连接");

            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(exitRoom);
            NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，点击确认回到主界面");
        }
    }

    void onSocketClose_Play()
    {
        //GameNetErrorPanelScript.create();

        //LogicEnginerScript.Instance.Stop();
        //PlayServiceSocket.s_instance.Stop();

        //NetErrorPanelScript.getInstance().Show();
        //NetErrorPanelScript.getInstance().setOnClickButton(onClickBack);
        //NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，点击确定回到主界面");

        checkNet();
    }

    public void onReceive_Main(string data)
    {
        LogUtil.Log("GameScript.onReceive_Main----" + data);
        JsonData jd = JsonMapper.ToObject(data);
        string tag = (string) jd["tag"];

        // 强制离线
        if (tag.CompareTo(TLJCommon.Consts.Tag_ForceOffline) == 0)
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
        // 救济金
        else if (tag.CompareTo(TLJCommon.Consts.Tag_SupplyGold) == 0)
        {
            int todayCount = (int) jd["todayCount"];
            int goldNum = (int) jd["goldNum"];

            GameUtil.changeData("1:" + goldNum);

            m_myUserInfoUI.GetComponent<MyUIScript>().setGoldNum(UserData.gold);

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

    void onSocketConnect_Logic(bool result)
    {
        NetLoading.getInstance().Close();

        if (result)
        {
            //LogUtil.Log("连接服务器成功");

            //ToastScript.createToast("连接Logic服务器成功");

            NetLoading.getInstance().Close();
            NetErrorPanelScript.getInstance().Close();
            // 检测服务器是否连接
            checkNet();
        }
        else
        {
            //LogUtil.Log("连接服务器失败，尝试重新连接");

            //NetErrorPanelScript.getInstance().Show();
            //NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
            //NetErrorPanelScript.getInstance().setContentText("连接逻辑服务器失败，请重新连接");

            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(exitRoom);
            NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，点击确认回到主界面");
        }
    }

    void onSocketClose_Logic()
    {
        //LogicEnginerScript.Instance.Stop();
        //PlayServiceSocket.s_instance.Stop();

        //NetErrorPanelScript.getInstance().Show();
        //NetErrorPanelScript.getInstance().setOnClickButton(onClickBack);
        //NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，点击确定回到主界面");

        checkNet();
    }

    // 点击网络断开弹框中的重连按钮:logic
    void onClickChongLian_Logic()
    {
        NetLoading.getInstance().Show();
        NetErrorPanelScript.getInstance().Close();
        LogicEnginerScript.Instance.startConnect();
    }

    // 点击网络断开弹框中的重连按钮:play
    void onClickChongLian_Play()
    {
        NetLoading.getInstance().Show();
        NetErrorPanelScript.getInstance().Close();
        PlayServiceSocket.s_instance.startConnect();
    }

    // 检测服务器是否连接
    void checkNet()
    {
        if (!LogicEnginerScript.Instance.isConnecion())
        {
            //NetErrorPanelScript.getInstance().Show();
            //NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
            //NetErrorPanelScript.getInstance().setContentText("与逻辑服务器断开连接，请重新连接");

            Destroy(LogicEnginerScript.Instance.gameObject);

            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(exitRoom);
            NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，点击确认回到主界面");
        }
        else if (!PlayServiceSocket.s_instance.isConnecion())
        {
            //NetErrorPanelScript.getInstance().Show();
            //NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Play);
            //NetErrorPanelScript.getInstance().setContentText("与游戏服务器断开连接，请重新连接");
            
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

    void onClickBack()
    {
        // 清空本局数据
        {
            clearData();
            initUI();
        }

        SceneManager.LoadScene("MainScene");
    }
    
    void onTimerEventTimeEnd()
    {
        // 全部设为未选中状态
        PokerScript.setAllPokerWeiXuanZe();

        switch (m_timerScript.getTimerType())
        {
            // 抢主
            case TimerScript.TimerType.TimerType_QiangZhu:
            {
                //ToastScript.createToast("抢主时间结束");
                //reqQiangZhuEnd();
            }
                break;

            // 埋底
            case TimerScript.TimerType.TimerType_MaiDi:
            {
                //ToastScript.createToast("时间到，自动埋底");

                //for (int i = GameData.getInstance().m_myPokerObjList.Count - 1; i >= (GameData.getInstance().m_myPokerObjList.Count - 8); i--)
                //{
                //    GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
                //}

                //reqMaiDi();
            }
                break;

            // 抄底
            case TimerScript.TimerType.TimerType_ChaoDi:
            {
                //ToastScript.createToast("时间到，不抄底");

                m_liangzhuObj.transform.localScale = new Vector3(0, 0, 0);

                //List<TLJCommon.PokerInfo> list = new List<TLJCommon.PokerInfo>();
                //onClickChaoDi(list);
            }
                break;

            // 庄家以外的3人埋底
            case TimerScript.TimerType.TimerType_OtherMaiDi:
            {
                //ToastScript.createToast("时间到，自动埋底");

                //for (int i = GameData.getInstance().m_myPokerObjList.Count - 1; i >= (GameData.getInstance().m_myPokerObjList.Count - 8); i--)
                //{
                //    GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
                //}

                //reqMaiDi();
            }
                break;

            // 出牌
            case TimerScript.TimerType.TimerType_OutPoker:
            {
                //if (!GameData.getInstance().m_isTuoGuan)
                //{
                //    m_tuoguanObj = TuoGuanPanelScript.create(this);

                //    GameData.getInstance().m_isTuoGuan = true;
                //}
            }
                break;
        }
    }

    //--------------------------------------------------------------------------------------------------
    void onPauseCallBack()
    {
        //LogicEnginerScript.Instance.Stop();
        //PlayServiceSocket.s_instance.Stop();
    }

    void onResumeCallBack()
    {
        //NetErrorPanelScript.getInstance().Show();
        //NetErrorPanelScript.getInstance().setOnClickButton(onClickBack);
        //NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，点击确定回到主界面");

        checkNet();
    }
}