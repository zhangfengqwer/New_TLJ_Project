﻿using LitJson;
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

    public Button m_button_bag;
    public Button m_button_set;
    public Button m_button_exit;

    public Button m_buttonStartGame;
    public Button m_buttonOutPoker;
    public Button m_buttonTiShi;
    public Button m_buttonMaiDi;
    public Button m_buttonChat;
    public Button m_buttonTuoGuan;
    public Button m_buttonJiPaiQi;
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
    string m_tag = "";

    NetErrorPanelScript m_netErrorPanelScript;

    void Start()
    {
        ToastScript.clear();

        m_netErrorPanelScript = NetErrorPanelScript.create();
        AudioScript.getAudioScript().stopMusic();
        
        // 3秒后播放背景音乐,每隔75秒重复播放背景音乐
        InvokeRepeating("onInvokeStartMusic", 3,75);

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
        m_tag = GameData.getInstance().m_tag;
        PlayServiceSocket.s_instance.setOnPlayService_Connect(null);
        PlayServiceSocket.s_instance.setOnPlayService_Receive(onSocketReceive);
        PlayServiceSocket.s_instance.setOnPlayService_Close(onSocketClose);
    }

    void initUI()
    {
        m_gameInfoSign.transform.localScale = new Vector3(0,0,0);
        m_buttonJiPaiQi.transform.localScale = new Vector3(0, 0, 0);

        // 初始化定时器
        {
            m_timer = TimerScript.createTimer();
            m_timerScript = m_timer.GetComponent<TimerScript>();
            m_timerScript.setOnTimerEvent_TimeEnd(onTimerEventTimeEnd);
        }

        // 初始化亮主
        {
            m_liangzhuObj = LiangZhu.create(this);
            m_liangzhuObj.GetComponent<LiangZhu>().setUseType(LiangZhu.UseType.UseType_liangzhu);
            m_liangzhuObj.transform.localScale = new Vector3(0, 0, 0);
        }

        m_buttonOutPoker.transform.localScale = new Vector3(0, 0, 0);
        m_buttonTiShi.transform.localScale = new Vector3(0, 0, 0);
        m_buttonMaiDi.transform.localScale = new Vector3(0, 0, 0);
        m_buttonChat.transform.localScale = new Vector3(0, 0, 0);
        m_buttonTuoGuan.transform.localScale = new Vector3(0, 0, 0);

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
            m_myUserInfoUI.GetComponent<MyUIScript>().setGoldNum(UserData.gold);
            
        }
        // 比赛场
        else
        {
            m_button_bag.transform.localScale = new Vector3(0, 0, 0);
            m_buttonTuoGuan.transform.localScale = new Vector3(0, 0, 0);

            // 从比赛场过来，直接开始游戏
            {
                startGame_InitUI(GameData.getInstance().m_startGameJsonData);
            }

            // 比赛场把金币图标替换为积分图标
            CommonUtil.setImageSprite(m_myUserInfoUI.transform.Find("Image_goldIcon").GetComponent<Image>(), "Sprites/Icon/icon_jifen");
        }
    }

    void startGame_InitUI(string jsonData)
    {
        try
        {
            JsonData jd = JsonMapper.ToObject(jsonData);

            {
                if (!isPVP())
                {
                    m_buttonTuoGuan.transform.localScale = new Vector3(1, 1, 1);
                }

                m_buttonStartGame.transform.localScale = new Vector3(0, 0, 0);          // 禁用开始游戏按钮
                m_buttonChat.transform.localScale = new Vector3(1, 1, 1);
                m_liangzhuObj.transform.localScale = new Vector3(1, 1, 1);

                // 显示左上角提示牌信息
                m_gameInfoSign.transform.localScale = new Vector3(1, 1, 1);

                {
                    // 上边的玩家
                    {
                        GameObject obj = OtherPlayerUIScript.create();
                        obj.transform.localPosition = new Vector3(0, 280, 0);
                        obj.GetComponent<OtherPlayerUIScript>().m_direction = OtherPlayerUIScript.Direction.Direction_Up;

                        if (isPVP())
                        {
                            CommonUtil.setImageSprite(obj.transform.Find("Image_icon_gold").GetComponent<Image>(), "Sprites/Icon/icon_jifen");
                        }
                        

                        GameData.getInstance().m_otherPlayerUIObjList.Add(obj);
                    }

                    // 左边的玩家
                    {
                        GameObject obj = OtherPlayerUIScript.create();
                        obj.transform.localPosition = new Vector3(-550, 0, 0);
                        obj.GetComponent<OtherPlayerUIScript>().m_direction = OtherPlayerUIScript.Direction.Direction_Left;

                        if (isPVP())
                        {
                            CommonUtil.setImageSprite(obj.transform.Find("Image_icon_gold").GetComponent<Image>(), "Sprites/Icon/icon_jifen");
                        }

                        GameData.getInstance().m_otherPlayerUIObjList.Add(obj);
                    }

                    // 右边的玩家
                    {
                        GameObject obj = OtherPlayerUIScript.create();
                        obj.transform.localPosition = new Vector3(550, 0, 0);
                        obj.GetComponent<OtherPlayerUIScript>().m_direction = OtherPlayerUIScript.Direction.Direction_Right;

                        if (isPVP())
                        {
                            CommonUtil.setImageSprite(obj.transform.Find("Image_icon_gold").GetComponent<Image>(), "Sprites/Icon/icon_jifen");
                        }

                        GameData.getInstance().m_otherPlayerUIObjList.Add(obj);
                    }
                }
            }

            // 分数
            m_textScore.text = "0";
            
            // 级牌
            GameData.getInstance().m_levelPokerNum = (int)jd["levelPokerNum"];

            // 我方级数
            {
                GameData.getInstance().m_myLevelPoker = (int)jd["myLevelPoker"];
                m_text_myLevelPoker.text = GameData.getInstance().m_myLevelPoker.ToString();
            }

            // 对方级数
            {
                GameData.getInstance().m_otherLevelPoker = (int)jd["otherLevelPoker"];
                m_text_otherLevelPoker.text = GameData.getInstance().m_otherLevelPoker.ToString();
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
                            GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][2]["uid"].ToString();
                            GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][3]["uid"].ToString();
                            GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][1]["uid"].ToString();
                        }
                        break;

                    case 1:
                        {
                            GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][3]["uid"].ToString();
                            GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][0]["uid"].ToString();
                            GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][2]["uid"].ToString();
                        }
                        break;

                    case 2:
                        {
                            GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][0]["uid"].ToString();
                            GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][1]["uid"].ToString();
                            GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][3]["uid"].ToString();
                        }
                        break;

                    case 3:
                        {
                            GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][1]["uid"].ToString();
                            GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][2]["uid"].ToString();
                            GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][0]["uid"].ToString();
                        }
                        break;
                }
            }

            // 本桌所有人信息
            for (int i = 0; i < jd["userList"].Count; i++)
            {
                string uid = jd["userList"][i]["uid"].ToString();

                GameData.getInstance().m_playerDataList.Add(new PlayerData(uid));

                GameData.getInstance().getPlayerDataByUid(uid).m_score = (int)jd["userList"][i]["score"];

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
                        m_myUserInfoUI.GetComponent<MyUIScript>().m_textGoldNum.text = jd["userList"][i]["score"].ToString();
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
            Debug.Log("startGame_InitUI()报错：" + ex.Message);
        }
    }

    public void showWaitMatchPanel(float time)
    {
        m_waitMatchPanel = WaitMatchPanelScript.create(GameData.getInstance().getGameRoomType());
        WaitMatchPanelScript script = m_waitMatchPanel.GetComponent<WaitMatchPanelScript>();
        script.setOnTimerEvent_TimeEnd(onTimerEvent_TimeEnd);
        script.start(time);
    }

    public void onTimerEvent_TimeEnd()
    {
        Debug.Log("暂时没有匹配到玩家,请求匹配机器人");

        // 让服务端匹配机器人
        reqWaitMatchTimeOut();
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
            for (int i = GameData.getInstance().m_curRoundOutPokerList.Count - 1; i >= 0; i--)
            {
                for (int j = GameData.getInstance().m_curRoundOutPokerList[i].Count - 1; j >= 0; j--)
                {
                    Destroy(GameData.getInstance().m_curRoundOutPokerList[i][j]);
                }
            }
            GameData.getInstance().m_curRoundOutPokerList.Clear();
        }

        GameData.getInstance().clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //onClickExitRoom();
        }

        bool useMouse = true;

        // 鼠标触摸
        if (useMouse)
        {
            Vector3 touchPos = Input.mousePosition;
            touchPos.z = m_screenPos.z; // 这个很关键  
            touchPos = Camera.main.ScreenToWorldPoint(touchPos);
            touchPos = touchPos * 100.0f;

            if (Input.GetMouseButtonDown(0))
            {
                onTouchBegan(touchPos);
            }
            else if (Input.GetMouseButton(0))
            {
                onTouchMove(touchPos);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                onTouchEnd(touchPos);
            }
        }
        // 手机触摸
        else
        {
            if (Input.touchCount > 0)
            {
                Vector3 touchPos = Input.GetTouch(0).position;

                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    onTouchBegan(touchPos);
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    onTouchMove(touchPos);
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    onTouchEnd(touchPos);
                }
            }
        }
    }

    void onTouchBegan(Vector3 vec3)
    {
        //bool isTouchPoker = false;
        //for (int i = GameData.getInstance().m_myPokerObjList.Count - 1; i >= 0; i--)
        //{
        //    if (GameUtil.CheckGameObjectContainPoint(GameData.getInstance().m_myPokerObjList[i], vec3))
        //    {
        //        isTouchPoker = true;
        //        GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
        //        break;
        //    }            
        //}

        //// 点击空白处，所有已选中的牌都变成未选中
        //if (!isTouchPoker)
        //{
        //    for (int i = GameData.getInstance().m_myPokerObjList.Count - 1; i >= 0; i--)
        //    {
        //        if (GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().getIsSelect())
        //        {
        //            GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
        //        }
        //    }
        //}
    }

    void onTouchMove(Vector3 vec3)
    {
        //for (int i = GameData.getInstance().m_myPokerObjList.Count - 1; i >= 0; i--)
        //{
        //    if (GameUtil.CheckGameObjectContainPoint(GameData.getInstance().m_myPokerObjList[i], vec3))
        //    {
        //        if (!GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().getIsSelect())
        //        {
        //            GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
        //        }
        //        break;
        //    }
        //}
    }

    void onTouchEnd(Vector3 vec3)
    {
    }

    void OnDestroy()
    {
    }

    public void onClickBag()
    {
        BagPanelScript.create(true);
    }

    public void onClickSet()
    {
        SetScript.create(true);
    }

    public void onClickJoinRoom()
    {
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
        reqOutPoker();
    }

    public void onClickTiShi()
    {
        tishi();
    }

    public void onClickQiangZhu(List<TLJCommon.PokerInfo> pokerList)
    {
        //m_liangzhuObj.transform.localScale = new Vector3(0, 0, 0);
        reqQiangZhu(pokerList);
    }

    public void onClickChaoDi(List<TLJCommon.PokerInfo> pokerList)
    {
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
        m_tuoguanObj = TuoGuanPanelScript.create(this);

        GameData.getInstance().m_isTuoGuan = true;

        if (GameData.getInstance().m_curOutPokerPlayerUid.CompareTo(UserData.uid) == 0)
        {
            autoOutPoker();
        }
    }

    public void onClickCancelTuoGuan()
    {
        GameData.getInstance().m_isTuoGuan = false;

        if (GameData.getInstance().m_curOutPokerPlayerUid.CompareTo(UserData.uid) == 0)
        {
            CancelInvoke("onInvokeTuoGuan");
        }
    }

    public void OnClickJiPaiQi()
    {
        bool canUse = false;
        for (int i = 0; i < UserData.buffData.Count; i++)
        {
            if ((UserData.buffData[i].prop_id == 101) && (UserData.buffData[i].buff_num > 0))
            {
                canUse = true;
                break;
            }
        }

        if (canUse)
        {
            if (m_jiPaiGameObject == null)
            {
                m_jiPaiGameObject = RememberPokerHelper.create();
            }
            else
            {
                m_jiPaiGameObject.GetComponentInChildren<RememberPokerHelper>().OnClickShow();
            }
        }
        else
        {
            ToastScript.createToast("您没有记牌器可用");
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
        for (int i = 0; i < GameData.getInstance().m_myPokerObjList.Count; i++)
        {
            if (GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().getIsSelect())
            {
                GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
            }
        }

        // 自由出牌
        if (GameData.getInstance().m_isFreeOutPoker)
        {
           
            GameData.getInstance().m_myPokerObjList[GameData.getInstance().m_myPokerObjList.Count - 1].GetComponent<PokerScript>().onClickPoker();
        }
        // 跟牌
        else
        {
            List<TLJCommon.PokerInfo> listPoker = PlayRuleUtil.GetPokerWhenTuoGuan(GameData.getInstance().m_curRoundFirstOutPokerList, GameData.getInstance().m_myPokerList, GameData.getInstance().m_levelPokerNum, GameData.getInstance().m_masterPokerType);
            if (listPoker.Count == GameData.getInstance().m_curRoundFirstOutPokerList.Count)
            {
                for (int i = 0; i < listPoker.Count; i++)
                {
                    for (int j = GameData.getInstance().m_myPokerObjList.Count - 1; j >= 0; j--)
                    {
                        PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[j].GetComponent<PokerScript>();

                        if ((pokerScript.getPokerNum() == listPoker[i].m_num) && (pokerScript.getPokerType() == (int)listPoker[i].m_pokerType))
                        {
                            if (!pokerScript.getIsSelect())
                            {
                                pokerScript.onClickPoker();
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    //----------------------------------------------------------发送数据 start--------------------------------------------------

    // 请求加入房间
    public void reqJoinRoom()
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_JoinGame;
        data["gameroomtype"] = GameData.getInstance().getGameRoomType();

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    public void reqWaitMatchTimeOut()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = m_tag;
        jsonData["uid"] = UserData.uid;
        jsonData["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_WaitMatchTimeOut;

        PlayServiceSocket.s_instance.sendMessage(jsonData.ToJson());
    }

    // 请求退出房间
    public void reqExitRoom()
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_ExitGame;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求出牌
    public void reqOutPoker()
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_PlayerOutPoker;

        bool hasOutPoker = false;
        List<TLJCommon.PokerInfo> myOutPokerList = new List<TLJCommon.PokerInfo>();

        // 自己出的牌
        {
            JsonData jarray = new JsonData();
            for (int i = 0; i < GameData.getInstance().m_myPokerObjList.Count; i++)
            {
                PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>();
                if (pokerScript.getIsSelect())
                {
                    hasOutPoker = true;

                    JsonData jd = new JsonData();
                    jd["num"] = pokerScript.getPokerNum();
                    jd["pokerType"] = pokerScript.getPokerType();
                    jarray.Add(jd);

                    myOutPokerList.Add(new TLJCommon.PokerInfo(pokerScript.getPokerNum(), (TLJCommon.Consts.PokerType)pokerScript.getPokerType()));
                }
            }
            data["pokerList"] = jarray;
        }

        if (hasOutPoker)
        {
            // 检测出牌合理性
            {
                if (!CheckOutPoker.checkOutPoker(GameData.getInstance().m_isFreeOutPoker, myOutPokerList, GameData.getInstance().m_curRoundFirstOutPokerList,
                    GameData.getInstance().m_myPokerList, GameData.getInstance().m_levelPokerNum, GameData.getInstance().m_masterPokerType))
                {
                    ToastScript.createToast("出的牌不合规则");
                    return;
                }
            }

            CancelInvoke("onInvokeCleanOutPoker");

            PlayServiceSocket.s_instance.sendMessage(data.ToJson());

            m_buttonOutPoker.transform.localScale = new Vector3(0, 0, 0);

            m_timerScript.stop();
        }
        else
        {
            ToastScript.createToast("请选择你要出的牌");
        }
    }

    // 获取游戏内玩家信息
    public void reqUserInfo_Game(string uid)
    {
        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_UserInfo_Game;
        data["uid"] = uid;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求换桌
    public void reqChangeRoom()
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_ChangeRoom;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求继续游戏
    public void reqContinueGame()
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_ContinueGame;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求抢主
    public void reqQiangZhu(List<TLJCommon.PokerInfo> pokerList)
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_QiangZhu;

        JsonData jarray = new JsonData();
        for (int i = 0; i < pokerList.Count; i++)
        {
            JsonData jd = new JsonData();
            jd["num"] = pokerList[i].m_num;
            jd["pokerType"] = (int)pokerList[i].m_pokerType;
            jarray.Add(jd);
        }

        data["pokerList"] = jarray;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求炒底
    public void reqChaoDi(List<TLJCommon.PokerInfo> pokerList)
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_PlayerChaoDi;

        if (pokerList.Count > 0)
        {
            JsonData jarray = new JsonData();
            for (int i = 0; i < pokerList.Count; i++)
            {
                data["hasPoker"] = 1;

                JsonData jd = new JsonData();
                jd["num"] = pokerList[i].m_num;
                jd["pokerType"] = (int)pokerList[i].m_pokerType;
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

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_MaiDi;

        int selectNum = 0;
        List<TLJCommon.PokerInfo> myOutPokerList = new List<TLJCommon.PokerInfo>();

        // 自己出的牌
        {
            JsonData jarray = new JsonData();
            for (int i = 0; i < GameData.getInstance().m_myPokerObjList.Count; i++)
            {
                PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>();
                if (pokerScript.getIsSelect())
                {
                    ++selectNum;

                    JsonData jd = new JsonData();
                    jd["num"] = pokerScript.getPokerNum();
                    jd["pokerType"] = pokerScript.getPokerType();
                    jarray.Add(jd);

                    myOutPokerList.Add(new TLJCommon.PokerInfo(pokerScript.getPokerNum(), (TLJCommon.Consts.PokerType)pokerScript.getPokerType()));
                }
            }
            data["diPokerList"] = jarray;
        }

        if (selectNum == 8)
        {

            // 从我的牌堆里删除8张
            {
                for (int i = 0; i < myOutPokerList.Count; i++)
                {
                    int num = myOutPokerList[i].m_num;
                    int pokerType = (int)myOutPokerList[i].m_pokerType;

                    for (int j = GameData.getInstance().m_myPokerObjList.Count - 1; j >= 0; j--)
                    {
                        PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[j].GetComponent<PokerScript>();
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
                        if ((GameData.getInstance().m_myPokerList[j].m_num == num) && ((int)GameData.getInstance().m_myPokerList[j].m_pokerType == pokerType))
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
            }

            m_timerScript.stop();
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

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_QiangZhu;
        data["pokerType"] = -1;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 抢主结束
    public void reqQiangZhuEnd()
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_QiangZhuEnd;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 发送聊天信息
    public void reqChat(int content_id)
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_Chat;
        data["content_id"] = content_id;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }
    //----------------------------------------------------------发送数据 end--------------------------------------------------

    //----------------------------------------------------------接收数据 start--------------------------------------------------
    void onReceive(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        string tag = (string)jd["tag"];

        if (tag.CompareTo(m_tag) == 0)
        {
            onReceive_PlayGame(data);
        }
        else if (tag.CompareTo(TLJCommon.Consts.Tag_UserInfo_Game) == 0)
        {
            onReceive_UserInfo_Game(data);
        }
    }

    void onReceive_PlayGame(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int playAction = (int)jd["playAction"];

        switch (playAction)
        {
            // 加入游戏
            case (int)TLJCommon.Consts.PlayAction.PlayAction_JoinGame:
                {
                    int code = (int)jd["code"];

                    switch (code)
                    {
                        case (int)TLJCommon.Consts.Code.Code_OK:
                            {
                                int roomId = (int)jd["roomId"];
                                ToastScript.createToast("加入房间成功：");

                                // 禁用开始游戏按钮
                                m_buttonStartGame.transform.localScale = new Vector3(0, 0, 0);

                                //m_waitOtherPlayer = WaitOtherPlayerScript.create();

                                showWaitMatchPanel(10);
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
            case (int)TLJCommon.Consts.PlayAction.PlayAction_ExitGame:
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
            case (int)TLJCommon.Consts.PlayAction.PlayAction_StartGame:
                {
                    //Destroy(m_waitOtherPlayer);
                    Destroy(m_waitMatchPanel);
                    Destroy(m_pvpGameResultPanel);
                    JueShengJuTiShiPanelScript.checkClose();

                    startGame_InitUI(data);
                }
                break;

            // 发牌
            case (int)TLJCommon.Consts.PlayAction.PlayAction_FaPai:
                {
                    int num = (int)jd["num"];
                    int pokerType = (int)jd["pokerType"];

                    int isEnd = (int)jd["isEnd"];

                    GameData.getInstance().m_myPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));

                    sortMyPokerList(-1);        // 对我的牌进行排序
                    createMyPokerObj();         // 创建我的牌对象

                    if (isEnd == 1)
                    {
                        {
                            // 开始倒计时
                            m_timerScript.start(GameData.getInstance().m_qiangZhuTime, TimerScript.TimerType.TimerType_QiangZhu, true);
                        }
                    }

                    m_liangzhuObj.GetComponent<LiangZhu>().UpdateUi(GameData.getInstance().m_myPokerList, GameData.getInstance().m_beforeQiangzhuPokerList);
                }
                break;

            // 抢主
            case (int)TLJCommon.Consts.PlayAction.PlayAction_QiangZhu:
                {
                    ToastScript.createToast("玩家抢主");

                    GameData.getInstance().m_beforeQiangzhuPokerList.Clear();

                    List<TLJCommon.PokerInfo> outPokerList = new List<TLJCommon.PokerInfo>();

                    // 抢主所用的牌
                    {
                        for (int i = 0; i < jd["pokerList"].Count; i++)
                        {
                            int num = (int)jd["pokerList"][i]["num"];
                            int pokerType = (int)jd["pokerList"][i]["pokerType"];

                            GameData.getInstance().m_masterPokerType = pokerType;

                            outPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                            GameData.getInstance().m_beforeQiangzhuPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                        }
                    }

                    // 显示出的牌
                    showOtherOutPoker(outPokerList, (string)jd["uid"]);

                    initMyPokerPos(GameData.getInstance().m_myPokerObjList);
                }
                break;

            // 抢主结束
            case (int)TLJCommon.Consts.PlayAction.PlayAction_QiangZhuEnd:
                {
                    // 删除当前已出的牌对象
                    {
                        for (int i = GameData.getInstance().m_curRoundOutPokerList.Count - 1; i >= 0; i--)
                        {
                            for (int j = GameData.getInstance().m_curRoundOutPokerList[i].Count - 1; j >= 0; j--)
                            {
                                Destroy(GameData.getInstance().m_curRoundOutPokerList[i][j]);
                            }
                        }
                        GameData.getInstance().m_curRoundOutPokerList.Clear();
                    }

                    m_timerScript.stop();
                    m_liangzhuObj.transform.localScale = new Vector3(0, 0, 0);

                    // 主牌花色
                    {
                        GameData.getInstance().m_masterPokerType = (int)jd["masterPokerType"];

                        if (GameData.getInstance().m_masterPokerType != -1)
                        {
                            CommonUtil.setImageSprite(m_imageMasterPokerType, GameUtil.getMasterPokerIconPath(GameData.getInstance().m_masterPokerType));
                        }
                        else
                        {
                            CommonUtil.setImageSprite(m_imageMasterPokerType, GameUtil.getMasterPokerIconPath(GameData.getInstance().m_masterPokerType));
                            ToastScript.createToast("本局打无主牌");
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
                        string uid = jd["uid"].ToString();
                        if (uid.CompareTo(UserData.uid) == 0)
                        {
                            m_myUserInfoUI.GetComponent<MyUIScript>().m_imageZhuangJiaIcon.transform.localScale = new Vector3(1, 1, 1);
                        }
                        else
                        {
                            for (int i = 0; i < GameData.getInstance().m_otherPlayerUIObjList.Count; i++)
                            {
                                if (GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_uid.CompareTo(uid) == 0)
                                {
                                    GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_imageZhuangJiaIcon.transform.localScale = new Vector3(1, 1, 1);
                                }
                            }
                        }
                    }

                    // 判断身份：庄家一方、普通人一方
                    {
                        GameData.getInstance().m_isBanker = (int)jd["isBanker"];
                    }

                    // 所有牌设为未选中状态
                    for (int i = 0; i < GameData.getInstance().m_myPokerObjList.Count; i++)
                    {
                        PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>();
                        if (pokerScript.getIsSelect())
                        {
                            pokerScript.onClickPoker();
                        }
                    }
                }
                break;

            // 埋底
            case (int)TLJCommon.Consts.PlayAction.PlayAction_MaiDi:
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
                                    int num = (int)jd["diPokerList"][i]["num"];
                                    int pokerType = (int)jd["diPokerList"][i]["pokerType"];

                                    GameData.getInstance().m_myPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                                }

                                sortMyPokerList(GameData.getInstance().m_masterPokerType);
                                createMyPokerObj();
                            }

                            // 开始埋底倒计时
                            m_timerScript.start(GameData.getInstance().m_maiDiTime, TimerScript.TimerType.TimerType_MaiDi, true);
                            setTimerPos(uid);

                            // 启用埋底按钮
                            m_buttonMaiDi.transform.localScale = new Vector3(1, 1, 1);
                        }
                        else
                        {
                            ToastScript.createToast("等待庄家埋底");

                            // 开始埋底倒计时
                            m_timerScript.start(GameData.getInstance().m_maiDiTime, TimerScript.TimerType.TimerType_MaiDi, false);
                            setTimerPos(uid);
                        }
                    }
                }
                break;

            // 通知某人炒底
            case (int)TLJCommon.Consts.PlayAction.PlayAction_CallPlayerChaoDi:
                {
                    try
                    {
                        m_timerScript.stop();

                        string uid = (string)jd["uid"];

                        // 检测是否轮到自己炒底
                        {
                            if (uid.CompareTo(UserData.uid) == 0)
                            {
                                m_liangzhuObj = LiangZhu.create(this);
                                m_liangzhuObj.GetComponent<LiangZhu>().setUseType(LiangZhu.UseType.UseType_chaodi);
                                m_liangzhuObj.GetComponent<LiangZhu>().UpdateUi(GameData.getInstance().m_myPokerList, GameData.getInstance().m_beforeQiangzhuPokerList);

                                // 开始炒底倒计时
                                m_timerScript.start(GameData.getInstance().m_chaodiTime, TimerScript.TimerType.TimerType_ChaoDi, true);
                                setTimerPos(uid);
                            }
                            else
                            {
                                ToastScript.createToast("等待玩家炒底");

                                // 开始炒底倒计时
                                m_timerScript.start(GameData.getInstance().m_chaodiTime, TimerScript.TimerType.TimerType_ChaoDi, false);
                                setTimerPos(uid);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ToastScript.createToast("异常：" + ex.Message);
                    }
                }
                break;

            // 玩家炒底
            case (int)TLJCommon.Consts.PlayAction.PlayAction_PlayerChaoDi:
                {
                    m_timerScript.stop();

                    string uid = jd["uid"].ToString();

                    // 炒底用的牌
                    {
                        if ((int)jd["hasPoker"] == 1)
                        {
                            GameData.getInstance().m_beforeQiangzhuPokerList.Clear();

                            string str = "有人炒底：";
                            for (int i = 0; i < jd["pokerList"].Count; i++)
                            {
                                int num = (int)jd["pokerList"][i]["num"];
                                int pokerType = (int)jd["pokerList"][i]["pokerType"];

                                GameData.getInstance().m_beforeQiangzhuPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));

                                str += (num + "  ");
                            }

                            ToastScript.createToast(str);

                            {
                                // 庄家开始埋底
                                if (uid.CompareTo(UserData.uid) == 0)
                                {
                                    ToastScript.createToast("开始埋底");

                                    // 把底牌加上去
                                    {
                                        for (int i = 0; i < jd["diPokerList"].Count; i++)
                                        {
                                            int num = (int)jd["diPokerList"][i]["num"];
                                            int pokerType = (int)jd["diPokerList"][i]["pokerType"];

                                            GameData.getInstance().m_myPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                                        }

                                        sortMyPokerList(GameData.getInstance().m_masterPokerType);
                                        createMyPokerObj();
                                    }

                                    // 开始埋底倒计时
                                    m_timerScript.start(GameData.getInstance().m_maiDiTime, TimerScript.TimerType.TimerType_OtherMaiDi, true);
                                    setTimerPos(uid);

                                    // 启用埋底按钮
                                    m_buttonMaiDi.transform.localScale = new Vector3(1, 1, 1);
                                }
                                else
                                {
                                    ToastScript.createToast("等待庄家埋底");

                                    // 开始埋底倒计时
                                    m_timerScript.start(GameData.getInstance().m_maiDiTime, TimerScript.TimerType.TimerType_OtherMaiDi, false);
                                    setTimerPos(uid);
                                }
                            }
                        }
                        else
                        {
                            ToastScript.createToast("不炒底");
                        }
                    }
                }
                break;

            // 通知某人出牌
            case (int)TLJCommon.Consts.PlayAction.PlayAction_CallPlayerOutPoker:
                {
                    // 禁用埋底按钮
                    m_buttonMaiDi.transform.localScale = new Vector3(0, 0, 0);

                    try
                    {
                        // 所有牌设为未选中
                        {
                            for (int i = 0; i < GameData.getInstance().m_myPokerObjList.Count; i++)
                            {
                                if (GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().getIsSelect())
                                {
                                    GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
                                }
                            }
                        }

                        // 闲家抓到的分数
                        {
                            int getScore = (int)jd["getScore"];
                            GameData.getInstance().m_getAllScore += getScore;
                            m_textScore.text = GameData.getInstance().m_getAllScore.ToString();
                        }

                        int hasPlayerOutPoker = (int)jd["hasPlayerOutPoker"];
                        if (hasPlayerOutPoker == 1)
                        {
                            int isCurRoundFirstPlayer = (int)jd["isCurRoundFirstPlayer"];
                            string pre_uid = (string)jd["pre_uid"];

                            // 显示出的牌
                            {
                                // 收到此回合第一个人出的牌
                                if (isCurRoundFirstPlayer == 1)
                                {
                                    GameData.getInstance().m_curRoundFirstOutPokerList.Clear();

                                    // 清空每个人座位上的牌
                                    {
                                        for (int i = 0; i < GameData.getInstance().m_curRoundOutPokerList.Count; i++)
                                        {
                                            for (int j = 0; j < GameData.getInstance().m_curRoundOutPokerList[i].Count; j++)
                                            {
                                                Destroy(GameData.getInstance().m_curRoundOutPokerList[i][j]);
                                            }
                                        }

                                        GameData.getInstance().m_curRoundOutPokerList.Clear();
                                    }
                                }

                                List<TLJCommon.PokerInfo> outPokerList = new List<TLJCommon.PokerInfo>();
                                for (int i = 0; i < jd["pre_outPokerList"].Count; i++)
                                {
                                    int num = (int)jd["pre_outPokerList"][i]["num"];
                                    int pokerType = (int)jd["pre_outPokerList"][i]["pokerType"];

                                    outPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));

                                    if (isCurRoundFirstPlayer == 1)
                                    {
                                        GameData.getInstance().m_curRoundFirstOutPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                                    }
                                }

                                showOtherOutPoker(outPokerList, pre_uid);

                                // 刷新记牌器
                                if (m_jiPaiGameObject != null)
                                {
                                    m_jiPaiGameObject.GetComponent<RememberPokerHelper>().UpdateUi(outPokerList);
                                }
                            }

                            // 如果前一次是自己出的牌，那么就得删掉这些牌
                            if (pre_uid.CompareTo(UserData.uid) == 0)
                            {
                                for (int i = 0; i < jd["pre_outPokerList"].Count; i++)
                                {
                                    int num = (int)jd["pre_outPokerList"][i]["num"];
                                    int pokerType = (int)jd["pre_outPokerList"][i]["pokerType"];

                                    for (int j = GameData.getInstance().m_myPokerObjList.Count - 1; j >= 0; j--)
                                    {
                                        PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[j].GetComponent<PokerScript>();
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
                                        if ((GameData.getInstance().m_myPokerList[j].m_num == num) && ((int)GameData.getInstance().m_myPokerList[j].m_pokerType == pokerType))
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
                            }
                        }

                        // 检测是否轮到自己出牌
                        {
                            string uid = (string)jd["cur_uid"];
                            GameData.getInstance().m_curOutPokerPlayerUid = uid;
                            if (uid.CompareTo(UserData.uid) == 0)
                            {
                                int isFreeOutPoker = (int)jd["isFreeOutPoker"];
                                if (isFreeOutPoker == 1)
                                {
                                    GameData.getInstance().m_isFreeOutPoker = true;
                                    //ToastScript.createToast("轮到你出牌：任意出");

                                    Invoke("onInvokeCleanOutPoker",1);
                                }
                                else
                                {
                                    GameData.getInstance().m_isFreeOutPoker = false;
                                    //ToastScript.createToast("轮到你出牌：跟牌");
                                }

                                m_buttonOutPoker.transform.localScale = new Vector3(1, 1, 1);

                                // 开始出牌倒计时
                                m_timerScript.start(GameData.getInstance().m_outPokerTime, TimerScript.TimerType.TimerType_OutPoker, true);
                                setTimerPos(uid);

                                if (GameData.getInstance().m_isTuoGuan)
                                {
                                    Invoke("onInvokeTuoGuan", GameData.getInstance().m_tuoGuanOutPokerTime);
                                }

                                if ((GameData.getInstance().getGameRoomType().CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ChuJi) == 0) ||
                                    (GameData.getInstance().getGameRoomType().CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_ChuJi) == 0))
                                {
                                    m_buttonTiShi.transform.localScale = new Vector3(1, 1, 1);
                                }
                            }
                            else
                            {
                                // 开始出牌倒计时
                                m_timerScript.start(GameData.getInstance().m_outPokerTime, TimerScript.TimerType.TimerType_OutPoker, false);
                                setTimerPos(uid);

                                m_buttonTiShi.transform.localScale = new Vector3(0, 0, 0);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ToastScript.createToast("异常：" + ex.Message);
                    }
                }
                break;

            // 有人甩牌
            case (int)TLJCommon.Consts.PlayAction.PlayAction_ShuaiPai:
                {
                    try
                    {
                        string uid = (string)jd["uid"];

                        // 显示出的牌
                        {
                            //ToastScript.createToast("有人尝试甩牌");
                            GameData.getInstance().m_curRoundFirstOutPokerList.Clear();

                            // 清空每个人座位上的牌
                            {
                                for (int i = 0; i < GameData.getInstance().m_curRoundOutPokerList.Count; i++)
                                {
                                    for (int j = 0; j < GameData.getInstance().m_curRoundOutPokerList[i].Count; j++)
                                    {
                                        Destroy(GameData.getInstance().m_curRoundOutPokerList[i][j]);
                                    }
                                }

                                GameData.getInstance().m_curRoundOutPokerList.Clear();
                            }

                            List<TLJCommon.PokerInfo> outPokerList = new List<TLJCommon.PokerInfo>();
                            for (int i = 0; i < jd["pokerList"].Count; i++)
                            {
                                int num = (int)jd["pokerList"][i]["num"];
                                int pokerType = (int)jd["pokerList"][i]["pokerType"];

                                outPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                            }

                            showOtherOutPoker(outPokerList, uid);
                        }
                    }
                    catch (Exception ex)
                    {
                        ToastScript.createToast("异常：" + ex.Message);
                    }
                }
                break;

            // 游戏结束
            case (int)TLJCommon.Consts.PlayAction.PlayAction_GameOver:
                {
                    try
                    {
                        //ToastScript.createToast("游戏结束");

                        // 闲家抓到的分数
                        {
                            GameData.getInstance().m_getAllScore = (int)jd["getAllScore"];
                            m_textScore.text = GameData.getInstance().m_getAllScore.ToString();
                        }

                        {
                            string pre_uid = (string)jd["pre_uid"];
                            // 如果前一次是自己出的牌，那么就得删掉这些牌
                            if (pre_uid.CompareTo(UserData.uid) == 0)
                            {
                                for (int i = 0; i < jd["pre_outPokerList"].Count; i++)
                                {
                                    int num = (int)jd["pre_outPokerList"][i]["num"];
                                    int pokerType = (int)jd["pre_outPokerList"][i]["pokerType"];

                                    for (int j = GameData.getInstance().m_myPokerObjList.Count - 1; j >= 0; j--)
                                    {
                                        PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[j].GetComponent<PokerScript>();
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
                                        if ((GameData.getInstance().m_myPokerList[j].m_num == num) && ((int)GameData.getInstance().m_myPokerList[j].m_pokerType == pokerType))
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
                            }

                            // 显示出的牌
                            {
                                List<TLJCommon.PokerInfo> outPokerList = new List<TLJCommon.PokerInfo>();
                                for (int i = 0; i < jd["pre_outPokerList"].Count; i++)
                                {
                                    int num = (int)jd["pre_outPokerList"][i]["num"];
                                    int pokerType = (int)jd["pre_outPokerList"][i]["pokerType"];

                                    outPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                                }

                                showOtherOutPoker(outPokerList, pre_uid);

                                // 刷新记牌器
                                if (m_jiPaiGameObject != null)
                                {
                                    m_jiPaiGameObject.GetComponent<RememberPokerHelper>().UpdateUi(outPokerList);
                                }
                            }
                        }

                        // 判断输赢
                        {
                            int isBankerWin = (int)jd["isBankerWin"];
                            if (GameData.getInstance().m_isBanker == isBankerWin)
                            {
                                // 显示pvp结算界面
                                if (GameData.getInstance().m_isPVP)
                                {
                                    m_pvpGameResultPanel = PVPGameResultPanelScript.create(this);
                                    PVPGameResultPanelScript script = m_pvpGameResultPanel.GetComponent<PVPGameResultPanelScript>();
                                    script.setData(true);
                                }
                                // 显示休闲场结算界面
                                else
                                {
                                    GameObject obj = GameResultPanelScript.create(this);
                                    GameResultPanelScript script = obj.GetComponent<GameResultPanelScript>();
                                    script.setData(true, GameData.getInstance().m_getAllScore, (int)jd["score"]);
                                }
                            }
                            else
                            {
                                // 显示pvp结算界面
                                if (GameData.getInstance().m_isPVP)
                                {
                                    m_pvpGameResultPanel = PVPGameResultPanelScript.create(this);
                                    PVPGameResultPanelScript script = m_pvpGameResultPanel.GetComponent<PVPGameResultPanelScript>();
                                    script.setData(false);
                                }
                                // 显示休闲场结算界面
                                else
                                {
                                    GameObject obj = GameResultPanelScript.create(this);
                                    GameResultPanelScript script = obj.GetComponent<GameResultPanelScript>();
                                    script.setData(false, GameData.getInstance().m_getAllScore, (int)jd["score"]);
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
                        ToastScript.createToast("异常：" + ex.Message);
                    }
                }
                break;

            // PVP决胜局通知
            case (int)TLJCommon.Consts.PlayAction.PlayAction_JueShengJuTongZhi:
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
                        ToastScript.createToast("异常：" + ex.Message);
                    }
                }
                break;

            // PVP游戏结束
            case (int)TLJCommon.Consts.PlayAction.PlayAction_PVPGameOver:
                {
                    try
                    {
                        Destroy(m_pvpGameResultPanel);

                        ToastScript.createToast("游戏结束，稍后请在邮箱查看奖励");

                        int mingci = (int)jd["mingci"];
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
                        }
                    }
                    catch (Exception ex)
                    {
                        ToastScript.createToast("异常：" + ex.Message);
                    }
                }
                break;

            // 换桌
            case (int)TLJCommon.Consts.PlayAction.PlayAction_ChangeRoom:
                {
                    int code = (int)jd["code"];

                    switch (code)
                    {
                        case (int)TLJCommon.Consts.Code.Code_OK:
                            {
                                int roomId = (int)jd["roomId"];
                                ToastScript.createToast("加入房间成功：");

                                // 禁用开始游戏按钮
                                m_buttonStartGame.transform.localScale = new Vector3(0, 0, 0);

                                //m_waitOtherPlayer = WaitOtherPlayerScript.create();
                                showWaitMatchPanel(10);
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

            // 继续游戏
            case (int)TLJCommon.Consts.PlayAction.PlayAction_ContinueGame:
                {
                    int code = (int)jd["code"];

                    switch (code)
                    {
                        case (int)TLJCommon.Consts.Code.Code_OK:
                            {
                                ToastScript.createToast("继续游戏成功，等待同桌玩家");

                                // 禁用开始游戏按钮
                                m_buttonStartGame.transform.localScale = new Vector3(0, 0, 0);

                                //m_waitOtherPlayer = WaitOtherPlayerScript.create();
                                showWaitMatchPanel(10);
                            }
                            break;

                        case (int)TLJCommon.Consts.Code.Code_CommonFail:
                            {
                                ToastScript.createToast("同桌玩家退出，无法继续游戏");

                                // 启用开始游戏按钮
                                m_buttonStartGame.transform.localScale = new Vector3(1, 1, 1);
                                //Destroy(m_waitOtherPlayer);
                                Destroy(m_waitMatchPanel);

                            }
                            break;
                    }

                }
                break;

            // 继续游戏失败
            case (int)TLJCommon.Consts.PlayAction.PlayAction_ContinueGameFail:
                {
                    ToastScript.createToast("同桌玩家退出，无法继续游戏");

                    // 启用开始游戏按钮
                    m_buttonStartGame.transform.localScale = new Vector3(1, 1, 1);
                    //Destroy(m_waitOtherPlayer);
                    Destroy(m_waitMatchPanel);
                }
                break;

            // 聊天
            case (int)TLJCommon.Consts.PlayAction.PlayAction_Chat:
                {
                    try
                    {
                        string uid = (string)jd["uid"];
                        string content_text = "";
                        if (ChatData.getInstance().getChatTextById((int)jd["content_id"]) != null)
                        {
                            content_text = ChatData.getInstance().getChatTextById((int)jd["content_id"]).m_text;
                        }

                        if (uid.CompareTo(UserData.uid) == 0)
                        {
                            ChatContentScript.createChatContent(content_text, new Vector2(-260, -290), TextAnchor.MiddleLeft);
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
                                                ChatContentScript.createChatContent(content_text, new Vector2(0, 270), TextAnchor.MiddleCenter);
                                            }
                                            break;

                                        case OtherPlayerUIScript.Direction.Direction_Left:
                                            {
                                                ChatContentScript.createChatContent(content_text, new Vector2(-260, 0), TextAnchor.MiddleLeft);
                                            }
                                            break;

                                        case OtherPlayerUIScript.Direction.Direction_Right:
                                            {
                                                ChatContentScript.createChatContent(content_text, new Vector2(380, 0), TextAnchor.MiddleRight);
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ToastScript.createToast("异常：" + ex.Message);
                    }
                }
                break;
        }
    }

    void onReceive_UserInfo_Game(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            string uid = (string)jd["uid"];

            GameData.getInstance().getPlayerDataByUid(uid).m_name = (string)jd["name"];
            GameData.getInstance().getPlayerDataByUid(uid).m_head = "Sprites/Head/head_" + (int)jd["head"];
            GameData.getInstance().getPlayerDataByUid(uid).m_gold = (int)jd["gold"];
            GameData.getInstance().getPlayerDataByUid(uid).m_allGameCount = (int)jd["gameData"]["allGameCount"];
            GameData.getInstance().getPlayerDataByUid(uid).m_winCount = (int)jd["gameData"]["winCount"];
            GameData.getInstance().getPlayerDataByUid(uid).m_runCount = (int)jd["gameData"]["runCount"];
            GameData.getInstance().getPlayerDataByUid(uid).m_meiliZhi = (int)jd["gameData"]["meiliZhi"];

            GameData.getInstance().setOtherPlayerUI(uid,isPVP());
        }
    }
    //----------------------------------------------------------接收数据 end--------------------------------------------------

    void startGame()
    {

    }

    // 清空每个人座位上的牌
    void onInvokeCleanOutPoker()
    {
        for (int i = 0; i < GameData.getInstance().m_curRoundOutPokerList.Count; i++)
        {
            for (int j = 0; j < GameData.getInstance().m_curRoundOutPokerList[i].Count; j++)
            {
                Destroy(GameData.getInstance().m_curRoundOutPokerList[i][j]);
            }
        }

        GameData.getInstance().m_curRoundOutPokerList.Clear();
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

            poker.GetComponent<PokerScript>().initPoker(GameData.getInstance().m_myPokerList[i].m_num, (int)GameData.getInstance().m_myPokerList[i].m_pokerType);
            poker.GetComponent<PokerScript>().m_canTouch = true;

            GameData.getInstance().m_myPokerObjList.Add(poker);
        }

        initMyPokerPos(GameData.getInstance().m_myPokerObjList);
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
                        objList[i].transform.localPosition = new Vector3(x, -110, 0);

                        // 设置最后渲染
                        //objList[i].transform.SetAsLastSibling();
                    }
                }
                break;
        }
    }

    void initMyPokerPos(List<GameObject> objList)
    {
        int jiange = 30;

        for (int i = 0; i < objList.Count; i++)
        {
            int x = CommonUtil.getPosX(objList.Count, jiange, i, 0);
            objList[i].transform.localPosition = new Vector3(x, -240, 0);
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
                    //if (levelPokerList[j].m_pokerType > levelPokerList[i].m_pokerType)
                    //{
                    //    TLJCommon.PokerInfo temp = levelPokerList[j];
                    //    levelPokerList[j] = levelPokerList[i];
                    //    levelPokerList[i] = temp;
                    //}

                    if (((int)levelPokerList[j].m_pokerType == ZhuPokerType) || (levelPokerList[j].m_pokerType > levelPokerList[i].m_pokerType))
                    {
                        TLJCommon.PokerInfo temp = levelPokerList[j];
                        levelPokerList[j] = levelPokerList[i];
                        levelPokerList[i] = temp;
                    }
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
                    if (GameData.getInstance().m_myPokerList[i].m_pokerType == TLJCommon.Consts.PokerType.PokerType_HeiTao)
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
                    if (GameData.getInstance().m_myPokerList[i].m_pokerType == TLJCommon.Consts.PokerType.PokerType_HongTao)
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
                    if (GameData.getInstance().m_myPokerList[i].m_pokerType == TLJCommon.Consts.PokerType.PokerType_MeiHua)
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
                    if (GameData.getInstance().m_myPokerList[i].m_pokerType == TLJCommon.Consts.PokerType.PokerType_FangKuai)
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
        if (ZhuPokerType == -1)
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
                case (int)TLJCommon.Consts.PokerType.PokerType_HeiTao:
                    {
                        list.Add(heitaoPokerList);
                        list.Add(hongtaoPokerList);
                        list.Add(meihuaPokerList);
                        list.Add(fangkuaiPokerList);
                    }
                    break;

                case (int)TLJCommon.Consts.PokerType.PokerType_HongTao:
                    {
                        list.Add(hongtaoPokerList);
                        list.Add(heitaoPokerList);
                        list.Add(meihuaPokerList);
                        list.Add(fangkuaiPokerList);
                    }
                    break;

                case (int)TLJCommon.Consts.PokerType.PokerType_MeiHua:
                    {
                        list.Add(meihuaPokerList);
                        list.Add(heitaoPokerList);
                        list.Add(hongtaoPokerList);
                        list.Add(fangkuaiPokerList);
                    }
                    break;

                case (int)TLJCommon.Consts.PokerType.PokerType_FangKuai:
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
        // 创建现在出的牌
        List<GameObject> tempList = new List<GameObject>();
        for (int i = 0; i < pokerList.Count; i++)
        {
            GameObject poker = PokerScript.createPoker();
            poker.transform.SetParent(GameObject.Find("Canvas").transform);
            poker.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            poker.GetComponent<PokerScript>().initPoker(pokerList[i].m_num, (int)pokerList[i].m_pokerType);

            tempList.Add(poker);
        }
        GameData.getInstance().m_curRoundOutPokerList.Add(tempList);


        // 显示在正确的座位上
        if (uid.CompareTo(UserData.uid) == 0)
        {
            initOutPokerPos(tempList, OtherPlayerUIScript.Direction.Direction_Down);
        }
        else
        {
            for (int i = 0; i < GameData.getInstance().m_otherPlayerUIObjList.Count; i++)
            {
                if (GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_uid.CompareTo(uid) == 0)
                {
                    initOutPokerPos(tempList, GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_direction);
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
            m_timer.transform.localPosition = new Vector3(0,0, 0);
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

    //-------------------------------------------------------------------------------------------------------
    //void onSocketConnect(bool result)
    //{
    //    if (result)
    //    {
    //        Debug.Log("连接服务器成功");
    //        m_isConnServerSuccess = true;
    //    }
    //    else
    //    {
    //        Debug.Log("连接服务器失败，尝试重新连接");
    //        PlayServiceSocket.s_instance.getSocketUtil().start();
    //    }
    //}

    void onSocketReceive(string data)
    {
        onReceive(data);
    }

    void onSocketClose()
    {
        //GameNetErrorPanelScript.create();

        m_netErrorPanelScript.Show();
        m_netErrorPanelScript.setOnClickButton(onClickBack);
        m_netErrorPanelScript.setContentText("游戏内：与服务器断开连接，点击确定回到主界面");
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

    //void onSocketClose()
    //{
    //    Debug.Log("被动与服务器断开连接,尝试重新连接");
    //    PlayServiceSocket.s_instance.getSocketUtil().start();
    //}

    //void onSocketStop()
    //{
    //    Debug.Log("主动与服务器断开连接");
    //}

    void onInvokeTuoGuan()
    {
        autoOutPoker();
    }

    void onTimerEventTimeEnd()
    {
        // 全部设为未选中状态
        for (int i = 0; i < GameData.getInstance().m_myPokerObjList.Count; i++)
        {
            if (GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().getIsSelect())
            {
                GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
            }
        }

        switch (m_timerScript.getTimerType())
        {
            // 抢主
            case TimerScript.TimerType.TimerType_QiangZhu:
                {
                    //ToastScript.createToast("抢主时间结束");
                    reqQiangZhuEnd();
                }
                break;

            // 埋底
            case TimerScript.TimerType.TimerType_MaiDi:
                {
                    //ToastScript.createToast("时间到，自动埋底");

                    for(int i = GameData.getInstance().m_myPokerObjList.Count - 1; i >= (GameData.getInstance().m_myPokerObjList.Count - 8); i--)
                    {
                        GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
                    }

                    reqMaiDi();
                }
                break;

            // 炒底
            case TimerScript.TimerType.TimerType_ChaoDi:
                {
                    //ToastScript.createToast("时间到，不炒底");

                    m_liangzhuObj.transform.localScale = new Vector3(0, 0, 0);

                    List<TLJCommon.PokerInfo> list = new List<TLJCommon.PokerInfo>();
                    onClickChaoDi(list);
                }
                break;

            // 庄家以外的3人埋底
            case TimerScript.TimerType.TimerType_OtherMaiDi:
                {
                    //ToastScript.createToast("时间到，自动埋底");

                    for (int i = GameData.getInstance().m_myPokerObjList.Count - 1; i >= (GameData.getInstance().m_myPokerObjList.Count - 8); i--)
                    {
                        GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
                    }

                    reqMaiDi();
                }
                break;

            // 出牌
            case TimerScript.TimerType.TimerType_OutPoker:
                {
                    if (!GameData.getInstance().m_isTuoGuan)
                    {
                        m_tuoguanObj = TuoGuanPanelScript.create(this);

                        GameData.getInstance().m_isTuoGuan = true;
                    }

                    autoOutPoker();
                }
                break;
        }
    }

    // 时间到，自动出牌
    void autoOutPoker()
    {
        // 自由出牌
        if (GameData.getInstance().m_isFreeOutPoker)
        {
            //ToastScript.createToast("时间到，自动出牌：自由出牌");
            GameData.getInstance().m_myPokerObjList[GameData.getInstance().m_myPokerObjList.Count - 1].GetComponent<PokerScript>().onClickPoker();
            reqOutPoker();
        }
        // 跟牌
        else
        {
            List<TLJCommon.PokerInfo> listPoker = PlayRuleUtil.GetPokerWhenTuoGuan(GameData.getInstance().m_curRoundFirstOutPokerList, GameData.getInstance().m_myPokerList, GameData.getInstance().m_levelPokerNum, GameData.getInstance().m_masterPokerType);
            if (listPoker.Count == GameData.getInstance().m_curRoundFirstOutPokerList.Count)
            {
                for (int i = 0; i < listPoker.Count; i++)
                {
                    //ToastScript.createToast("自动出牌：" + listPoker[i].m_num + "  " + listPoker[i].m_pokerType);
                    for (int j = GameData.getInstance().m_myPokerObjList.Count - 1; j >= 0; j--)
                    {
                        PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[j].GetComponent<PokerScript>();

                        if ((pokerScript.getPokerNum() == listPoker[i].m_num) && (pokerScript.getPokerType() == (int)listPoker[i].m_pokerType))
                        {
                            if (!pokerScript.getIsSelect())
                            {
                                pokerScript.onClickPoker();
                                break;
                            }
                        }
                    }
                }

                reqOutPoker();
            }
            else
            {
                ToastScript.createToast("自动出牌失败：张数不对");
            }
            
        }
    }
}
