using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

    public Button m_buttonStartGame;
    public Button m_buttonOutPoker;
    public Button m_buttonMaiDi;
    public Button m_buttonChat;
    public Button m_buttonTuoGuan;
    public Text m_textScore;
    public Image m_imageMasterPokerType;

    // 倒计时
    GameObject m_timer;
    TimerScript m_timerScript;

    public GameObject m_myUserInfoUI;
    GameObject m_waitOtherPlayer;
    GameObject m_liangzhuObj;

    bool m_isConnServerSuccess = false;

    List<string> m_dataList = new List<string>();

    //List<TLJCommon.PokerInfo> m_myPokerList = new List<TLJCommon.PokerInfo>();
    //List<TLJCommon.PokerInfo> m_qiangzhuPokerList = new List<TLJCommon.PokerInfo>();
    //List<GameObject> m_myPokerObjList = new List<GameObject>();
    //List<List<GameObject>> m_curRoundOutPokerList = new List<List<GameObject>>();
    //List<GameObject> m_otherPlayerUIObjList = new List<GameObject>();

    //int m_outPokerTime = 5;             // 出牌时间 
    //int m_tuoGuanOutPokerTime = 1;      // 托管出牌时间 
    //int m_qiangZhuTime = 10;            // 抢主时间
    //int m_maiDiTime = 20;               // 埋底时间
    //int m_chaodiTime = 10;              // 选择是否炒底时间 

    //public static int m_levelPokerNum = -1;           // 级牌
    //public static int m_masterPokerType = -1;         // 主牌花色

    //string m_teammateUID;               // 我的队友uid
    //int m_isBanker;                     // 是否属于庄家一方

    //int m_getAllScore;                  // 庄家对家抓到的分数

    //string m_curOutPokerPlayerUid;

    //bool m_isTuoGuan = false;
    //bool m_isFreeOutPoker = false;
    //List<TLJCommon.PokerInfo> m_curRoundFirstOutPokerList = new List<TLJCommon.PokerInfo>();

    // Use this for initialization
    void Start ()
    {
        initData();

        initUI();
    }

    void initData()
    {
        // 设置Socket事件
        SocketUtil.getInstance().setOnSocketEvent_Connect(onSocketConnect);
        SocketUtil.getInstance().setOnSocketEvent_Receive(onSocketReceive);
        SocketUtil.getInstance().setOnSocketEvent_Close(onSocketClose);
        SocketUtil.getInstance().setOnSocketEvent_Stop(onSocketStop);

        SocketUtil.getInstance().init(NetConfig.s_playService_ip, NetConfig.s_playService_port);
        SocketUtil.getInstance().start();

        // 初始化定时器
        {
            m_timer = TimerScript.createTimer();
            m_timerScript = m_timer.GetComponent<TimerScript>();
            m_timerScript.setOnTimerEvent_TimeEnd(onTimerEventTimeEnd);
        }
    }

    void initUI()
    {
        {
            m_liangzhuObj = LiangZhu.create(this,LiangZhu.UseType.UseType_liangzhu);
            m_liangzhuObj.transform.localScale = new Vector3(0, 0, 0);
        }

        m_buttonOutPoker.transform.localScale = new Vector3(0, 0, 0);
        m_buttonMaiDi.transform.localScale = new Vector3(0, 0, 0);
        m_buttonChat.transform.localScale = new Vector3(0, 0, 0);
        m_buttonTuoGuan.transform.localScale = new Vector3(0, 0, 0);

        // 我的信息
        {
            m_myUserInfoUI.GetComponent<MyUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon(UserData.head);
            m_myUserInfoUI.GetComponent<MyUIScript>().setName(UserData.name);
            m_myUserInfoUI.GetComponent<MyUIScript>().setGoldNum(UserData.gold);
            m_myUserInfoUI.GetComponent<MyUIScript>().m_uid = UserDataScript.getInstance().getUserInfo().m_uid;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (m_isConnServerSuccess)
        {
            ToastScript.createToast("连接服务器成功");
            m_isConnServerSuccess = false;
        }

        for (int i = 0; i < m_dataList.Count; i++)
        {
            onReceive(m_dataList[i]);
            m_dataList.RemoveAt(i);
        }
    }

    void OnDestroy()
    {
        SocketUtil.getInstance().stop();
    }

    public void onClickBag()
    {
        BagPanelScript.create(false);
    }

    public void onClickSet()
    {
        SetScript.create();
    }

    public void onClickJoinRoom()
    {
        reqJoinRoom();
    }

    public void onClickExitRoom()
    {
        reqExitRoom();
    }

    public void onClickOutPoker()
    {
        reqOutPoker();
    }

    public void onClickQiangZhu(List<TLJCommon.PokerInfo> pokerList)
    {
        //Destroy(m_liangzhuObj);
        reqQiangZhu(pokerList);
    }

    public void onClickChaoDi(List<TLJCommon.PokerInfo> pokerList)
    {
        Destroy(m_liangzhuObj);
        reqChaoDi(pokerList);
    }

    public void onClickMaiDi()
    {
        reqMaiDi();
    }

    public void onClickOtherMaiDi()
    {
        reqOtherMaiDi();
    }

    public void onClickChat()
    {
        ChatPanelScript.create(this);
    }

    public void onClickTuoGuan()
    {
        {
            TuoGuanPanelScript.create(this);
        }

        GameData.getInstance().m_isTuoGuan = true;

        if (GameData.getInstance().m_curOutPokerPlayerUid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
        {
            autoOutPoker();
        }
    }

    public void onClickCancelTuoGuan()
    {
        GameData.getInstance().m_isTuoGuan = false;

        if (GameData.getInstance().m_curOutPokerPlayerUid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
        {
            CancelInvoke("onInvokeTuoGuan");
        }
    }

    //----------------------------------------------------------发送数据 start--------------------------------------------------

    // 请求加入房间
    public void reqJoinRoom()
    {
        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
        data["uid"] = UserDataScript.getInstance().getUserInfo().m_uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_JoinGame;

        SocketUtil.getInstance().sendMessage(data.ToJson());
    }

    // 请求退出房间
    public void reqExitRoom()
    {
        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
        data["uid"] = UserDataScript.getInstance().getUserInfo().m_uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_ExitGame;

        SocketUtil.getInstance().sendMessage(data.ToJson());
    }

    // 请求出牌
    public void reqOutPoker()
    {
        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
        data["uid"] = UserDataScript.getInstance().getUserInfo().m_uid;
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

            SocketUtil.getInstance().sendMessage(data.ToJson());

            m_buttonOutPoker.transform.localScale = new Vector3(0, 0, 0);
            
            m_timerScript.stop();
        }
        else
        {
            ToastScript.createToast("请选择你要出的牌");
        }
    }

    public void reqQiangZhu(List<TLJCommon.PokerInfo> pokerList)
    {
        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
        data["uid"] = UserDataScript.getInstance().getUserInfo().m_uid;
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
        
        SocketUtil.getInstance().sendMessage(data.ToJson());
    }

    public void reqChaoDi(List<TLJCommon.PokerInfo> pokerList)
    {
        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
        data["uid"] = UserDataScript.getInstance().getUserInfo().m_uid;
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

        SocketUtil.getInstance().sendMessage(data.ToJson());
    }

    public void reqMaiDi()
    {
        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
        data["uid"] = UserDataScript.getInstance().getUserInfo().m_uid;
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
            SocketUtil.getInstance().sendMessage(data.ToJson());

            m_buttonMaiDi.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            ToastScript.createToast("请选择8张牌");
        }
    }
    
    public void reqOtherMaiDi()
    {
        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
        data["uid"] = UserDataScript.getInstance().getUserInfo().m_uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_OtherMaiDi;

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
            SocketUtil.getInstance().sendMessage(data.ToJson());
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

        data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
        data["uid"] = UserDataScript.getInstance().getUserInfo().m_uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_QiangZhu;
        data["pokerType"] = -1;

        SocketUtil.getInstance().sendMessage(data.ToJson());
    }

    // 抢主结束
    public void reqQiangZhuEnd()
    {
        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
        data["uid"] = UserDataScript.getInstance().getUserInfo().m_uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_QiangZhuEnd;

        SocketUtil.getInstance().sendMessage(data.ToJson());
    }

    // 发送聊天信息
    public void reqChat(int content_id)
    {
        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
        data["uid"] = UserDataScript.getInstance().getUserInfo().m_uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_Chat;
        data["content_id"] = content_id;

        SocketUtil.getInstance().sendMessage(data.ToJson());
    }
    //----------------------------------------------------------发送数据 end--------------------------------------------------

    //----------------------------------------------------------接收数据 start--------------------------------------------------
    void onReceive(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        string tag = (string)jd["tag"];

        if (tag.CompareTo(TLJCommon.Consts.Tag_XiuXianChang) == 0)
        {
            onReceive_XiuXianChang(data);
        }
    }

    void onReceive_XiuXianChang(string data)
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
                                ToastScript.createToast("加入房间成功：" + roomId);

                                // 禁用开始游戏按钮
                                m_buttonStartGame.transform.localScale = new Vector3(0,0,0);

                                m_waitOtherPlayer = WaitOtherPlayerScript.create();
                            }
                            break;

                        case (int)TLJCommon.Consts.Code.Code_CommonFail:
                            {
                                ToastScript.createToast("加入房间失败，已经加入房间");
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
                                ToastScript.createToast("退出房间成功：" + roomId);
                            }
                            break;

                        case (int)TLJCommon.Consts.Code.Code_CommonFail:
                            {
                                ToastScript.createToast("退出房间失败，当前并没有加入房间");
                            }
                            break;
                    }

                    SceneManager.LoadScene("MainScene");
                }
                break;

            // 开始游戏
            case (int)TLJCommon.Consts.PlayAction.PlayAction_StartGame:
                {
                    Destroy(m_waitOtherPlayer);

                    {
                        m_buttonChat.transform.localScale = new Vector3(1, 1, 1);
                        m_buttonTuoGuan.transform.localScale = new Vector3(1, 1, 1);
                        m_liangzhuObj.transform.localScale = new Vector3(1, 1, 1);

                        {
                            // 上边的玩家
                            {
                                GameObject obj = OtherPlayerUIScript.create();
                                obj.transform.localPosition = new Vector3(0, 200, 0);
                                obj.GetComponent<OtherPlayerUIScript>().m_direction = OtherPlayerUIScript.Direction.Direction_Up;

                                GameData.getInstance().m_otherPlayerUIObjList.Add(obj);
                            }

                            // 左边的玩家
                            {
                                GameObject obj = OtherPlayerUIScript.create();
                                obj.transform.localPosition = new Vector3(-550, 0, 0);
                                obj.GetComponent<OtherPlayerUIScript>().m_direction = OtherPlayerUIScript.Direction.Direction_Left;

                                GameData.getInstance().m_otherPlayerUIObjList.Add(obj);
                            }

                            // 右边的玩家
                            {
                                GameObject obj = OtherPlayerUIScript.create();
                                obj.transform.localPosition = new Vector3(550, 0, 0);
                                obj.GetComponent<OtherPlayerUIScript>().m_direction = OtherPlayerUIScript.Direction.Direction_Right;

                                GameData.getInstance().m_otherPlayerUIObjList.Add(obj);
                            }
                        }
                    }

                    // 级牌
                    GameData.getInstance().m_levelPokerNum = (int)jd["levelPokerNum"];

                    // 我的队友uid
                    GameData.getInstance().m_teammateUID = jd["teammateUID"].ToString();

                    // 显示所有玩家的头像、昵称、金币
                    {
                        int myIndex = 0;
                        for (int i = 0; i < jd["userList"].Count; i++)
                        {
                            if (jd["userList"][i]["uid"].ToString().CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
                            {
                                myIndex = i;

                                break;
                            }
                        }

                        switch (myIndex)
                        {
                            case 0:
                                {
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon("Sprites/Head/head_10");
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][2]["uid"].ToString());
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setGoldNum(2);
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][2]["uid"].ToString();

                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon("Sprites/Head/head_10");
                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][3]["uid"].ToString());
                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setGoldNum(3);
                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][3]["uid"].ToString();

                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon("Sprites/Head/head_10");
                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][1]["uid"].ToString());
                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setGoldNum(1);
                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][1]["uid"].ToString();
                                }
                                break;

                            case 1:
                                {
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon("Sprites/Head/head_10");
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][3]["uid"].ToString());
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setGoldNum(3);
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][3]["uid"].ToString();

                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon("Sprites/Head/head_10");
                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][0]["uid"].ToString());
                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setGoldNum(0);
                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][0]["uid"].ToString();

                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon("Sprites/Head/head_10");
                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][2]["uid"].ToString());
                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setGoldNum(2);
                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][2]["uid"].ToString();
                                }
                                break;

                            case 2:
                                {
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon("Sprites/Head/head_10");
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][0]["uid"].ToString());
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setGoldNum(0);
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][0]["uid"].ToString();

                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon("Sprites/Head/head_10");
                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][1]["uid"].ToString());
                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setGoldNum(1);
                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][1]["uid"].ToString();

                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon("Sprites/Head/head_10");
                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][3]["uid"].ToString());
                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setGoldNum(3);
                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][3]["uid"].ToString();
                                }
                                break;

                            case 3:
                                {
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon("Sprites/Head/head_10");
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][1]["uid"].ToString());
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setGoldNum(1);
                                    GameData.getInstance().m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][1]["uid"].ToString();

                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon("Sprites/Head/head_10");
                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][2]["uid"].ToString());
                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setGoldNum(2);
                                    GameData.getInstance().m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][2]["uid"].ToString();

                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon("Sprites/Head/head_10");
                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][0]["uid"].ToString());
                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setGoldNum(0);
                                    GameData.getInstance().m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][0]["uid"].ToString();
                                }
                                break;
                        }
                    }
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
                            ToastScript.createToast("开始抢主,本局打" + GameData.getInstance().m_levelPokerNum.ToString());

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
                    GameData.getInstance().m_beforeQiangzhuPokerList.Clear();

                    string str = "有人抢主：";
                    // 主牌花色
                    {
                        for (int i = 0; i < jd["pokerList"].Count; i++)
                        {
                            int num = (int)jd["pokerList"][i]["num"];
                            int pokerType = (int)jd["pokerList"][i]["pokerType"];

                            GameData.getInstance().m_masterPokerType = pokerType;

                            GameData.getInstance().m_beforeQiangzhuPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));

                            str += (num + "  ");
                        }
                    }
                    ToastScript.createToast(str);
                }
                break;

            // 抢主结束
            case (int)TLJCommon.Consts.PlayAction.PlayAction_QiangZhuEnd:
                {
                    m_timerScript.stop();
                    Destroy(m_liangzhuObj);

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
                        if (uid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
                        {
                            ToastScript.createToast("我是庄家");

                            m_myUserInfoUI.GetComponent<MyUIScript>().m_imageZhuangJiaIcon.transform.localScale = new Vector3(1, 1, 1);
                        }
                        else
                        {
                            ToastScript.createToast(uid + "是庄家");
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
                        if (GameData.getInstance().m_isBanker == 1)
                        {
                            ToastScript.createToast("我是庄家一方");
                        }
                        else
                        {
                            ToastScript.createToast("我是普通人一方");
                        }
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
                        if (uid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
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

                            // 启用埋底按钮
                            m_buttonMaiDi.transform.localScale = new Vector3(1, 1, 1);
                        }
                        else
                        {
                            ToastScript.createToast("等待庄家埋底");
                            
                            // 开始埋底倒计时
                            m_timerScript.start(GameData.getInstance().m_maiDiTime, TimerScript.TimerType.TimerType_MaiDi, false);
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
                            if (uid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
                            {
                                m_liangzhuObj = LiangZhu.create(this, LiangZhu.UseType.UseType_chaodi);
                                m_liangzhuObj.GetComponent<LiangZhu>().UpdateUi(GameData.getInstance().m_myPokerList, GameData.getInstance().m_beforeQiangzhuPokerList);

                                // 开始炒底倒计时
                                m_timerScript.start(GameData.getInstance().m_chaodiTime, TimerScript.TimerType.TimerType_ChaoDi, true);
                            }
                            else
                            {
                                ToastScript.createToast("等待玩家炒底：" + uid);

                                // 开始炒底倒计时
                                m_timerScript.start(GameData.getInstance().m_chaodiTime, TimerScript.TimerType.TimerType_ChaoDi, false);
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
                                if (uid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
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

                                    // 启用埋底按钮
                                    m_buttonMaiDi.transform.localScale = new Vector3(1, 1, 1);
                                }
                                else
                                {
                                    ToastScript.createToast("等待庄家埋底");

                                    // 开始埋底倒计时
                                    m_timerScript.start(GameData.getInstance().m_maiDiTime, TimerScript.TimerType.TimerType_OtherMaiDi, false);
                                }
                            }
                        }
                        else
                        {
                            ToastScript.createToast("玩家不炒底：" + uid);
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
                                GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().setIsSelect(false);
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
                            // 如果前一次是自己出的牌，那么就得删掉这些牌
                            if (pre_uid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
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
                                if (isCurRoundFirstPlayer == 1)
                                {
                                    ToastScript.createToast("收到此回合第一个人出的牌");
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
                            }
                        }

                        // 检测是否轮到自己出牌
                        {
                            string uid = (string)jd["cur_uid"];
                            GameData.getInstance().m_curOutPokerPlayerUid = uid;
                            if (uid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
                            {
                                int isFreeOutPoker = (int)jd["isFreeOutPoker"];
                                if (isFreeOutPoker == 1)
                                {
                                    GameData.getInstance().m_isFreeOutPoker = true;
                                    ToastScript.createToast("轮到你出牌：任意出");
                                }
                                else
                                {
                                    GameData.getInstance().m_isFreeOutPoker = false;
                                    ToastScript.createToast("轮到你出牌：跟牌");
                                }

                                m_buttonOutPoker.transform.localScale = new Vector3(1, 1, 1);
                                
                                // 开始出牌倒计时
                                m_timerScript.start(GameData.getInstance().m_outPokerTime, TimerScript.TimerType.TimerType_OutPoker, true);

                                if (GameData.getInstance().m_isTuoGuan)
                                {
                                    Invoke("onInvokeTuoGuan", GameData.getInstance().m_tuoGuanOutPokerTime);
                                }
                            }
                            else
                            {
                                // 开始出牌倒计时
                                m_timerScript.start(GameData.getInstance().m_outPokerTime, TimerScript.TimerType.TimerType_OutPoker,false);
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
                            ToastScript.createToast("有人尝试甩牌");
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
                        ToastScript.createToast("游戏结束");

                        // 闲家抓到的分数
                        {
                            GameData.getInstance().m_getAllScore = (int)jd["getAllScore"];
                            m_textScore.text = GameData.getInstance().m_getAllScore.ToString();
                        }
                        
                        {
                            string pre_uid = (string)jd["pre_uid"];
                            // 如果前一次是自己出的牌，那么就得删掉这些牌
                            if (pre_uid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
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
                            }
                        }

                        // 判断输赢
                        {
                            int isBankerWin = (int)jd["isBankerWin"];
                            if (GameData.getInstance().m_isBanker == isBankerWin)
                            {
                                //ToastScript.createToast("我方胜利");

                                GameObject obj = GameResultPanelScript.create(this);
                                GameResultPanelScript script = obj.GetComponent<GameResultPanelScript>();
                                script.setData(true, GameData.getInstance().m_getAllScore, 1000);
                            }
                            else
                            {
                                //ToastScript.createToast("对方胜利");
                                GameObject obj = GameResultPanelScript.create(this);
                                GameResultPanelScript script = obj.GetComponent<GameResultPanelScript>();
                                script.setData(false, GameData.getInstance().m_getAllScore, -1000);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ToastScript.createToast("异常：" + ex.Message);
                    }
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

                        if (uid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
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

            poker.GetComponent<PokerScript>().initPoker(GameData.getInstance().m_myPokerList[i].m_num, (int)GameData.getInstance().m_myPokerList[i].m_pokerType);

            GameData.getInstance().m_myPokerObjList.Add(poker);
        }

        initMyPokerPos(GameData.getInstance().m_myPokerObjList);
    }

    void initOutPokerPos(List<GameObject> objList,OtherPlayerUIScript.Direction direc)
    {
        int jiange = 30;

        switch (direc)
        {
            case OtherPlayerUIScript.Direction.Direction_Up:
                {
                    for (int i = 0; i < objList.Count; i++)
                    {
                        int x = CommonUtil.getPosX(objList.Count, jiange, i, 0);
                        objList[i].transform.localPosition = new Vector3(x, 300, 0);

                        // 设置最后渲染
                        objList[i].transform.SetAsLastSibling();
                    }
                }
                break;

            case OtherPlayerUIScript.Direction.Direction_Left:
                {
                    for (int i = 0; i < objList.Count; i++)
                    {
                        float startX = -540;
                        objList[i].transform.localPosition = new Vector3(startX + (i * jiange), 0, 0);

                        // 设置最后渲染
                        objList[i].transform.SetAsLastSibling();
                    }
                }
                break;

            case OtherPlayerUIScript.Direction.Direction_Right:
                {
                    for (int i = 0; i < objList.Count; i++)
                    {
                        float endX = 540;                
                        objList[i].transform.localPosition = new Vector3(endX - ((objList.Count - i) * jiange), 0, 0);

                        // 设置最后渲染
                        objList[i].transform.SetAsLastSibling();
                    }
                }
                break;

            case OtherPlayerUIScript.Direction.Direction_Down:
                {
                    for (int i = 0; i < objList.Count; i++)
                    {
                        int x = CommonUtil.getPosX(objList.Count, jiange, i, 0);
                        objList[i].transform.localPosition = new Vector3(x, -300, 0);

                        // 设置最后渲染
                        objList[i].transform.SetAsLastSibling();
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
            objList[i].transform.localPosition = new Vector3(x, -220, 0);

            // 设置最后渲染
            objList[i].transform.SetAsLastSibling();
        }
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

    void showOtherOutPoker(List<TLJCommon.PokerInfo> pokerList,string uid)
    {
        // 创建现在出的牌
        List<GameObject> tempList = new List<GameObject>();
        for (int i = 0; i < pokerList.Count; i++)
        {
            GameObject poker = PokerScript.createPoker();
            poker.transform.SetParent(GameObject.Find("Canvas").transform);
            poker.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            poker.GetComponent<PokerScript>().initPoker(pokerList[i].m_num, (int)pokerList[i].m_pokerType);

            // 禁止点击
            Destroy(poker.GetComponent<Button>());

            tempList.Add(poker);
        }
        GameData.getInstance().m_curRoundOutPokerList.Add(tempList);


        // 显示在正确的座位上
        if (uid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
        {
            initOutPokerPos(tempList, OtherPlayerUIScript.Direction.Direction_Down);
        }
        else
        {
            for (int i = 0; i < GameData.getInstance().m_otherPlayerUIObjList.Count; i++)
            {
                if(GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_uid.CompareTo(uid) == 0)
                {
                    initOutPokerPos(tempList, GameData.getInstance().m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_direction);
                    break;
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------
    void onSocketConnect(bool result)
    {
        if (result)
        {
            Debug.Log("连接服务器成功");
            m_isConnServerSuccess = true;
        }
        else
        {
            Debug.Log("连接服务器失败，尝试重新连接");
            SocketUtil.getInstance().start();
        }
    }

    void onSocketReceive(string data)
    {
        Debug.Log("收到服务器消息:" + data);

        m_dataList.Add(data);
    }

    void onSocketClose()
    {
        Debug.Log("被动与服务器断开连接,尝试重新连接");
        SocketUtil.getInstance().start();
    }

    void onSocketStop()
    {
        Debug.Log("主动与服务器断开连接");
    }

    void onInvokeTuoGuan()
    {
        autoOutPoker();
    }

    void onTimerEventTimeEnd()
    {
        switch (m_timerScript.getTimerType())
        {
            // 抢主
            case TimerScript.TimerType.TimerType_QiangZhu:
                {
                    ToastScript.createToast("抢主时间结束");
                    reqQiangZhuEnd();
                }
                break;

            // 埋底
            case TimerScript.TimerType.TimerType_MaiDi:
                {
                    ToastScript.createToast("时间到，自动埋底");

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
                    ToastScript.createToast("时间到，不炒底");

                    Destroy(m_liangzhuObj);

                    List<TLJCommon.PokerInfo> list = new List<TLJCommon.PokerInfo>();
                    onClickChaoDi(list);
                }
                break;

            // 庄家以外的3人埋底
            case TimerScript.TimerType.TimerType_OtherMaiDi:
                {
                    ToastScript.createToast("时间到，自动埋底");

                    for (int i = GameData.getInstance().m_myPokerObjList.Count - 1; i >= (GameData.getInstance().m_myPokerObjList.Count - 8); i--)
                    {
                        GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
                    }

                    reqOtherMaiDi();
                }
                break;

            // 出牌
            case TimerScript.TimerType.TimerType_OutPoker:
                {
                    autoOutPoker();
                }
                break;
        }
    }

    // 时间到，自动出牌
    void autoOutPoker()
    {
        // 全部设为未选中状态
        for (int i = 0; i < GameData.getInstance().m_myPokerObjList.Count ; i++)
        {
            if (GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().getIsSelect())
            {
                GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
            }
        }

        // 自由出牌
        if (GameData.getInstance().m_isFreeOutPoker)
        {
            ToastScript.createToast("时间到，自动出牌：自由出牌");
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
