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
    public Button m_buttonQiangZhu;
    public Button m_buttonMaiDi;
    public Text m_textScore;

    List<string> m_dataList = new List<string>();
    bool m_isConnServerSuccess = false;

    List<TLJCommon.PokerInfo> myPokerList = new List<TLJCommon.PokerInfo>();
    List<GameObject> m_myPokerObjList = new List<GameObject>();
    //List<GameObject> m_outPokerObjList = new List<GameObject>();
    List<List<GameObject>> m_curRoundOutPokerList = new List<List<GameObject>>();
    List<GameObject> m_otherPlayerUIObjList = new List<GameObject>();
    public GameObject m_myUserInfoUI;

    // 倒计时
    GameObject m_timer;
    TimerScript m_timerScript;

    int m_outPokerTime = 10;            // 出牌时间 
    int m_qiangZhuTime = 10;            // 抢主时间
    int m_maiDiTime = 20;               // 埋底时间

    int m_levelPokerNum = -1;           // 级牌
    int m_masterPokerType = -1;         // 主牌花色

    string m_teammateUID;               // 我的队友uid
    int m_isBanker;                     // 是否属于庄家一方

    int m_getAllScore;                  // 庄家对家抓到的分数

    bool m_isFreeOutPoker = false;
    List<TLJCommon.PokerInfo> m_curRoundFirstOutPokerList = new List<TLJCommon.PokerInfo>();

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
        m_buttonOutPoker.transform.localScale = new Vector3(0, 0, 0);
        m_buttonQiangZhu.transform.localScale = new Vector3(0,0,0);
        m_buttonMaiDi.transform.localScale = new Vector3(0, 0, 0);

        // 上边的玩家
        {
            GameObject obj = OtherPlayerUIScript.create();
            obj.transform.localPosition = new Vector3(0, 300, 0);
            obj.GetComponent<OtherPlayerUIScript>().m_direction = OtherPlayerUIScript.Direction.Direction_Up;

            m_otherPlayerUIObjList.Add(obj);
        }

        // 左边的玩家
        {
            GameObject obj = OtherPlayerUIScript.create();
            obj.transform.localPosition = new Vector3(-550, 0, 0);
            obj.GetComponent<OtherPlayerUIScript>().m_direction = OtherPlayerUIScript.Direction.Direction_Left;

            m_otherPlayerUIObjList.Add(obj);
        }

        // 右边的玩家
        {
            GameObject obj = OtherPlayerUIScript.create();
            obj.transform.localPosition = new Vector3(550, 0, 0);
            obj.GetComponent<OtherPlayerUIScript>().m_direction = OtherPlayerUIScript.Direction.Direction_Right;

            m_otherPlayerUIObjList.Add(obj);
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

    public void onClickQiangZhu()
    {
        reqQiangZhu();
    }

    public void onClickMaiDi()
    {
        reqMaiDi();
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
            for (int i = 0; i < m_myPokerObjList.Count; i++)
            {
                PokerScript pokerScript = m_myPokerObjList[i].GetComponent<PokerScript>();
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
                if (!CheckOutPoker.checkOutPoker(m_isFreeOutPoker, myOutPokerList, m_curRoundFirstOutPokerList, myPokerList))
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

    public void reqQiangZhu()
    {
        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
        data["uid"] = UserDataScript.getInstance().getUserInfo().m_uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_QiangZhu;

        int select = -1;
        // 自己出的牌
        for (int i = 0; i < m_myPokerObjList.Count; i++)
        {
            PokerScript pokerScript = m_myPokerObjList[i].GetComponent<PokerScript>();
            if (pokerScript.getIsSelect())
            {
                select = pokerScript.getPokerNum();

                data["pokerType"] = pokerScript.getPokerType();
                break;
            }
        }

        if (select != -1)
        {
            if (select == m_levelPokerNum)
            {
                m_timerScript.stop();
                SocketUtil.getInstance().sendMessage(data.ToJson());
            }
            else
            {
                ToastScript.createToast("请选择正确的牌");
            }
        }
        else
        {
            ToastScript.createToast("请选中你的牌");
        }
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
            for (int i = 0; i < m_myPokerObjList.Count; i++)
            {
                PokerScript pokerScript = m_myPokerObjList[i].GetComponent<PokerScript>();
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

                    for (int j = m_myPokerObjList.Count - 1; j >= 0; j--)
                    {
                        PokerScript pokerScript = m_myPokerObjList[j].GetComponent<PokerScript>();
                        if ((pokerScript.getPokerNum() == num) && (pokerScript.getPokerType() == pokerType))
                        {
                            // 出的牌从自己的牌堆里删除
                            {
                                Destroy(m_myPokerObjList[j]);
                                m_myPokerObjList.RemoveAt(j);
                            }

                            break;
                        }
                    }

                    for (int j = myPokerList.Count - 1; j >= 0; j--)
                    {
                        if ((myPokerList[j].m_num == num) && ((int)myPokerList[j].m_pokerType == pokerType))
                        {
                            // 出的牌从自己的牌堆里删除
                            {
                                myPokerList.RemoveAt(j);
                            }

                            break;
                        }
                    }
                }

                initMyPokerPos(m_myPokerObjList);
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
                    // 级牌
                    m_levelPokerNum = (int)jd["levelPokerNum"];
                    
                    // 我的队友uid
                    m_teammateUID = jd["teammateUID"].ToString();

                    // 我的牌
                    for (int i = 0; i < jd["pokerList"].Count; i++)
                    {
                        int num = (int)jd["pokerList"][i]["num"];
                        int pokerType = (int)jd["pokerList"][i]["pokerType"];

                        myPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                    }

                    // 显示所有玩家的头像、昵称、金币
                    {
                        int myIndex = 0;
                        for (int i = 0; i < jd["userList"].Count; i++)
                        {
                            if (jd["userList"][i]["uid"].ToString().CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
                            {
                                m_myUserInfoUI.GetComponent<MyUIScript>().setHead("");
                                m_myUserInfoUI.GetComponent<MyUIScript>().setName(jd["userList"][i]["uid"].ToString());
                                m_myUserInfoUI.GetComponent<MyUIScript>().setGoldNum(i);
                                m_myUserInfoUI.GetComponent<MyUIScript>().m_uid = UserDataScript.getInstance().getUserInfo().m_uid;

                                myIndex = i;

                                break;
                            }
                        }

                        switch (myIndex)
                        {
                            case 0:
                                {
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setHead("");
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][2]["uid"].ToString());
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setGoldNum(2);
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][2]["uid"].ToString();

                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setHead("");
                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][3]["uid"].ToString());
                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setGoldNum(3);
                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][3]["uid"].ToString();

                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setHead("");
                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][1]["uid"].ToString());
                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setGoldNum(1);
                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][1]["uid"].ToString();
                                }
                                break;

                            case 1:
                                {
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setHead("");
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][3]["uid"].ToString());
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setGoldNum(3);
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][3]["uid"].ToString();

                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setHead("");
                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][0]["uid"].ToString());
                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setGoldNum(0);
                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][0]["uid"].ToString();

                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setHead("");
                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][2]["uid"].ToString());
                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setGoldNum(2);
                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][2]["uid"].ToString();
                                }
                                break;

                            case 2:
                                {
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setHead("");
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][0]["uid"].ToString());
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setGoldNum(0);
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][0]["uid"].ToString();

                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setHead("");
                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][1]["uid"].ToString());
                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setGoldNum(1);
                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][1]["uid"].ToString();

                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setHead("");
                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][3]["uid"].ToString());
                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setGoldNum(3);
                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][3]["uid"].ToString();
                                }
                                break;

                            case 3:
                                {
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setHead("");
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][1]["uid"].ToString());
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().setGoldNum(1);
                                    m_otherPlayerUIObjList[0].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][1]["uid"].ToString();

                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setHead("");
                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][2]["uid"].ToString());
                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().setGoldNum(2);
                                    m_otherPlayerUIObjList[1].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][2]["uid"].ToString();

                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setHead("");
                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setName(jd["userList"][0]["uid"].ToString());
                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().setGoldNum(0);
                                    m_otherPlayerUIObjList[2].GetComponent<OtherPlayerUIScript>().m_uid = jd["userList"][0]["uid"].ToString();
                                }
                                break;
                        }
                    }
                    
                    sortMyPokerList(-1);        // 对我的牌进行排序
                    createMyPokerObj();         // 创建我的牌对象
                    startGame();
                }
                break;

            // 抢主
            case (int)TLJCommon.Consts.PlayAction.PlayAction_QiangZhu:
                {
                    m_timerScript.stop();

                    // 主牌花色
                    {
                        m_masterPokerType = (int)jd["pokerType"];

                        if (m_masterPokerType != -1)
                        {
                            GameObject poker = PokerScript.createPoker();
                            poker.transform.SetParent(GameObject.Find("Canvas").transform);
                            poker.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                            poker.transform.localPosition = new Vector3(-600, 315, 0);

                            poker.GetComponent<PokerScript>().initPoker(m_levelPokerNum, m_masterPokerType);

                            // 禁止点击
                            Destroy(poker.GetComponent<Button>());
                        }
                        else
                        {
                            GameObject poker = PokerScript.createPoker();
                            poker.transform.SetParent(GameObject.Find("Canvas").transform);
                            poker.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                            poker.transform.localPosition = new Vector3(-600, 315, 0);

                            poker.GetComponent<PokerScript>().initPoker(m_levelPokerNum, m_masterPokerType);

                            // 禁止点击
                            Destroy(poker.GetComponent<Button>());

                            ToastScript.createToast("本局打无主牌");
                        }                        
                    }

                    // 对我的牌重新排序
                    {
                        if (m_masterPokerType != -1)
                        {
                            sortMyPokerList(m_masterPokerType);
                            createMyPokerObj();
                        }
                    }

                    // 判断谁是庄家
                    {
                        string uid = jd["uid"].ToString();
                        if (uid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
                        {
                            ToastScript.createToast("我是庄家");

                            m_myUserInfoUI.GetComponent<MyUIScript>().m_imageZhuangJiaIcon.transform.localScale = new Vector3(1,1,1);
                        }
                        else
                        {
                            ToastScript.createToast(uid + "是庄家");
                            for (int i = 0; i < m_otherPlayerUIObjList.Count; i++)
                            {
                                if (m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_uid.CompareTo(uid) == 0)
                                {
                                    m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_imageZhuangJiaIcon.transform.localScale = new Vector3(1, 1, 1);
                                }
                            }
                        }
                    }

                    // 判断身份：庄家一方、普通人一方
                    {
                        m_isBanker = (int)jd["isBanker"];
                        if (m_isBanker == 1)
                        {
                            ToastScript.createToast("我是庄家一方");
                        }
                        else
                        {
                            ToastScript.createToast("我是普通人一方");
                        }
                    }

                    // 禁用抢主按钮
                    m_buttonQiangZhu.transform.localScale = new Vector3(0, 0, 0);

                    // 所有牌设为未选中状态
                    for (int i = 0; i < m_myPokerObjList.Count; i++)
                    {
                        PokerScript pokerScript = m_myPokerObjList[i].GetComponent<PokerScript>();
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
                    // 禁用埋底按钮
                    m_buttonMaiDi.transform.localScale = new Vector3(0, 0, 0);

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

                                    myPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                                }

                                sortMyPokerList(m_masterPokerType);
                                createMyPokerObj();
                            }

                            // 开始埋底倒计时
                            m_timerScript.start(m_maiDiTime, TimerScript.TimerType.TimerType_MaiDi, true);

                            // 启用埋底按钮
                            m_buttonMaiDi.transform.localScale = new Vector3(1, 1, 1);
                        }
                        else
                        {
                            ToastScript.createToast("等待庄家埋底");
                            
                            // 开始埋底倒计时
                            m_timerScript.start(m_maiDiTime, TimerScript.TimerType.TimerType_MaiDi, false);
                        }
                    }
                }
                break;

            // 通知某人出牌
            case (int)TLJCommon.Consts.PlayAction.PlayAction_CallPlayerOutPoker:
                {
                    try
                    {
                        // 所有牌设为未选中
                        {
                            for (int i = 0; i < m_myPokerObjList.Count; i++)
                            {
                                m_myPokerObjList[i].GetComponent<PokerScript>().setIsSelect(false);
                            }
                        }

                        // 庄家对家抓到的分数
                        {
                            int getScore = (int)jd["getScore"];
                            m_getAllScore += getScore;
                            m_textScore.text = "分数：" + m_getAllScore.ToString();
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

                                    for (int j = m_myPokerObjList.Count - 1; j >= 0; j--)
                                    {
                                        PokerScript pokerScript = m_myPokerObjList[j].GetComponent<PokerScript>();
                                        if ((pokerScript.getPokerNum() == num) && (pokerScript.getPokerType() == pokerType))
                                        {
                                            // 出的牌从自己的牌堆里删除
                                            {
                                                Destroy(m_myPokerObjList[j]);
                                                m_myPokerObjList.RemoveAt(j);
                                            }

                                            break;
                                        }
                                    }

                                    for (int j = myPokerList.Count - 1; j >= 0; j--)
                                    {
                                        if ((myPokerList[j].m_num == num) && ((int)myPokerList[j].m_pokerType == pokerType))
                                        {
                                            // 出的牌从自己的牌堆里删除
                                            {
                                                myPokerList.RemoveAt(j);
                                            }

                                            break;
                                        }
                                    }
                                }

                                initMyPokerPos(m_myPokerObjList);
                            }

                            // 显示出的牌
                            {
                                if (isCurRoundFirstPlayer == 1)
                                {
                                    ToastScript.createToast("收到此回合第一个人出的牌");
                                    m_curRoundFirstOutPokerList.Clear();

                                    // 清空每个人座位上的牌
                                    {
                                        for (int i = 0; i < m_curRoundOutPokerList.Count; i++)
                                        {
                                            for (int j = 0; j < m_curRoundOutPokerList[i].Count; j++)
                                            {
                                                Destroy(m_curRoundOutPokerList[i][j]);
                                            }
                                        }

                                        m_curRoundOutPokerList.Clear();
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
                                        m_curRoundFirstOutPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                                    }
                                }

                                showOtherOutPoker(outPokerList, pre_uid);
                            }
                        }

                        // 检测是否轮到自己出牌
                        {
                            string uid = (string)jd["cur_uid"];
                            if (uid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
                            {
                                int isFreeOutPoker = (int)jd["isFreeOutPoker"];
                                if (isFreeOutPoker == 1)
                                {
                                    m_isFreeOutPoker = true;
                                    ToastScript.createToast("轮到你出牌：任意出");
                                }
                                else
                                {
                                    m_isFreeOutPoker = false;
                                    ToastScript.createToast("轮到你出牌：跟牌");
                                }

                                m_buttonOutPoker.transform.localScale = new Vector3(1, 1, 1);
                                
                                // 开始出牌倒计时
                                m_timerScript.start(m_outPokerTime, TimerScript.TimerType.TimerType_OutPoker, true);
                            }
                            else
                            {
                                // 开始出牌倒计时
                                m_timerScript.start(m_outPokerTime, TimerScript.TimerType.TimerType_OutPoker,false);
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
                            m_curRoundFirstOutPokerList.Clear();

                            // 清空每个人座位上的牌
                            {
                                for (int i = 0; i < m_curRoundOutPokerList.Count; i++)
                                {
                                    for (int j = 0; j < m_curRoundOutPokerList[i].Count; j++)
                                    {
                                        Destroy(m_curRoundOutPokerList[i][j]);
                                    }
                                }

                                m_curRoundOutPokerList.Clear();
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

                        // 庄家对家抓到的分数
                        {
                            m_getAllScore = (int)jd["getAllScore"];
                            m_textScore.text = "分数：" + m_getAllScore.ToString();
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

                                    for (int j = m_myPokerObjList.Count - 1; j >= 0; j--)
                                    {
                                        PokerScript pokerScript = m_myPokerObjList[j].GetComponent<PokerScript>();
                                        if ((pokerScript.getPokerNum() == num) && (pokerScript.getPokerType() == pokerType))
                                        {
                                            // 出的牌从自己的牌堆里删除
                                            {
                                                Destroy(m_myPokerObjList[j]);
                                                m_myPokerObjList.RemoveAt(j);
                                            }

                                            break;
                                        }
                                    }

                                    for (int j = myPokerList.Count - 1; j >= 0; j--)
                                    {
                                        if ((myPokerList[j].m_num == num) && ((int)myPokerList[j].m_pokerType == pokerType))
                                        {
                                            // 出的牌从自己的牌堆里删除
                                            {
                                                myPokerList.RemoveAt(j);
                                            }

                                            break;
                                        }
                                    }
                                }

                                initMyPokerPos(m_myPokerObjList);
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
                            if (m_isBanker == isBankerWin)
                            {
                                //ToastScript.createToast("我方胜利");
                                GameResultScript.create().GetComponent<GameResultScript>().setText("胜利");
                            }
                            else
                            {
                                //ToastScript.createToast("对方胜利");
                                GameResultScript.create().GetComponent<GameResultScript>().setText("失败");
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
        m_buttonQiangZhu.transform.localScale = new Vector3(1, 1, 1);
        ToastScript.createToast("开始抢主,本局打" + m_levelPokerNum.ToString());

        // 开始倒计时
        m_timerScript.start(m_qiangZhuTime, TimerScript.TimerType.TimerType_QiangZhu,true);
    }

    void createMyPokerObj()
    {
        // 先删掉旧的
        for (int i = m_myPokerObjList.Count - 1; i >= 0; i--)
        {
            Destroy(m_myPokerObjList[i]);
            m_myPokerObjList.RemoveAt(i);
        }

        for (int i = 0; i < myPokerList.Count; i++)
        {
            GameObject poker = PokerScript.createPoker();
            poker.transform.SetParent(GameObject.Find("Canvas").transform);
            poker.transform.localScale = new Vector3(1, 1, 1);

            poker.GetComponent<PokerScript>().initPoker(myPokerList[i].m_num, (int)myPokerList[i].m_pokerType);

            m_myPokerObjList.Add(poker);
        }

        initMyPokerPos(m_myPokerObjList);
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
            for (int i = 0; i < myPokerList.Count; i++)
            {
                if (myPokerList[i].m_pokerType == TLJCommon.Consts.PokerType.PokerType_Wang)
                {
                    wangPokerList.Add(myPokerList[i]);
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
            for (int i = 0; i < myPokerList.Count; i++)
            {
                if (myPokerList[i].m_num == m_levelPokerNum)
                {
                    levelPokerList.Add(myPokerList[i]);
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
            for (int i = 0; i < myPokerList.Count; i++)
            {
                if (m_levelPokerNum != myPokerList[i].m_num)
                {
                    if (myPokerList[i].m_pokerType == TLJCommon.Consts.PokerType.PokerType_HeiTao)
                    {
                        heitaoPokerList.Add(myPokerList[i]);
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
            for (int i = 0; i < myPokerList.Count; i++)
            {
                if (m_levelPokerNum != myPokerList[i].m_num)
                {
                    if (myPokerList[i].m_pokerType == TLJCommon.Consts.PokerType.PokerType_HongTao)
                    {
                        hongtaoPokerList.Add(myPokerList[i]);
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
            for (int i = 0; i < myPokerList.Count; i++)
            {
                if (m_levelPokerNum != myPokerList[i].m_num)
                {
                    if (myPokerList[i].m_pokerType == TLJCommon.Consts.PokerType.PokerType_MeiHua)
                    {
                        meihuaPokerList.Add(myPokerList[i]);
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
            for (int i = 0; i < myPokerList.Count; i++)
            {
                if (m_levelPokerNum != myPokerList[i].m_num)
                {
                    if (myPokerList[i].m_pokerType == TLJCommon.Consts.PokerType.PokerType_FangKuai)
                    {
                        fangkuaiPokerList.Add(myPokerList[i]);
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
            myPokerList.Clear();

            // 王
            for (int i = 0; i < wangPokerList.Count; i++)
            {
                myPokerList.Add(wangPokerList[i]);
            }

            // 级牌
            for (int i = 0; i < levelPokerList.Count; i++)
            {
                myPokerList.Add(levelPokerList[i]);
            }

            // 黑桃
            for (int i = 0; i < heitaoPokerList.Count; i++)
            {
                myPokerList.Add(heitaoPokerList[i]);
            }

            // 红桃
            for (int i = 0; i < hongtaoPokerList.Count; i++)
            {
                myPokerList.Add(hongtaoPokerList[i]);
            }

            //梅花
            for (int i = 0; i < meihuaPokerList.Count; i++)
            {
                myPokerList.Add(meihuaPokerList[i]);
            }

            //方块
            for (int i = 0; i < fangkuaiPokerList.Count; i++)
            {
                myPokerList.Add(fangkuaiPokerList[i]);
            }
        }
        // 有主牌
        else
        {
            myPokerList.Clear();

            // 王
            for (int i = 0; i < wangPokerList.Count; i++)
            {
                myPokerList.Add(wangPokerList[i]);
            }

            // 级牌
            for (int i = 0; i < levelPokerList.Count; i++)
            {
                myPokerList.Add(levelPokerList[i]);
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
                    myPokerList.Add(list[i][j]);
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
        m_curRoundOutPokerList.Add(tempList);


        // 显示在正确的座位上
        if (uid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
        {
            initOutPokerPos(tempList, OtherPlayerUIScript.Direction.Direction_Down);
        }
        else
        {
            for (int i = 0; i < m_otherPlayerUIObjList.Count; i++)
            {
                if(m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_uid.CompareTo(uid) == 0)
                {
                    initOutPokerPos(tempList, m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_direction);
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

    void onTimerEventTimeEnd()
    {
        switch (m_timerScript.getTimerType())
        {
            // 抢主
            case TimerScript.TimerType.TimerType_QiangZhu:
                {
                    ToastScript.createToast("时间到，放弃抢主");
                    reqGiveUpQiangZhu();
                }
                break;

            // 埋底
            case TimerScript.TimerType.TimerType_MaiDi:
                {
                    ToastScript.createToast("时间到，自动埋底");

                    for(int i = m_myPokerObjList.Count - 1; i >= (m_myPokerObjList.Count - 8); i--)
                    {
                        m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
                    }

                    reqMaiDi();
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
        for (int i = 0; i < m_myPokerObjList.Count ; i++)
        {
            if (m_myPokerObjList[i].GetComponent<PokerScript>().getIsSelect())
            {
                m_myPokerObjList[i].GetComponent<PokerScript>().onClickPoker();
            }
        }

        // 自由出牌
        if (m_isFreeOutPoker)
        {
            ToastScript.createToast("时间到，自动出牌：自由出牌");
            m_myPokerObjList[m_myPokerObjList.Count - 1].GetComponent<PokerScript>().onClickPoker();
            reqOutPoker();
        }
        // 跟牌
        else
        {
            int count = m_curRoundFirstOutPokerList.Count;

            if (m_myPokerObjList.Count >= count)
            {
                switch (CheckOutPoker.checkOutPokerType(m_curRoundFirstOutPokerList))
                {
                    case CheckOutPoker.OutPokerType.OutPokerType_TuoLaJi:
                        {
                            ToastScript.createToast("时间到，自动出牌：拖拉机");

                            List<TLJCommon.PokerInfo> firstOutPokerList_single = GameUtil.choiceSinglePoker(m_curRoundFirstOutPokerList, m_curRoundFirstOutPokerList[0].m_pokerType);
                            List<TLJCommon.PokerInfo> firstOutPokerList_double = GameUtil.choiceDoublePoker(m_curRoundFirstOutPokerList, m_curRoundFirstOutPokerList[0].m_pokerType);

                            List<TLJCommon.PokerInfo> myPokerList_single = GameUtil.choiceSinglePoker(myPokerList, m_curRoundFirstOutPokerList[0].m_pokerType);
                            List<TLJCommon.PokerInfo> myPokerList_double = GameUtil.choiceDoublePoker(myPokerList, m_curRoundFirstOutPokerList[0].m_pokerType);

                            int needAllPokerNum = firstOutPokerList_single.Count + firstOutPokerList_double.Count * 2;

                            // 先找拖拉机
                            {
                                for (int i = myPokerList_double.Count - 1; i >= firstOutPokerList_double.Count - 1; i--)
                                {
                                    bool find = true;
                                    for (int j = 0; j < firstOutPokerList_double.Count - 1; j++)
                                    {
                                        if ((myPokerList_double[i - j].m_num - myPokerList_double[i - j - 1].m_num) != -1)
                                        {
                                            find = false;
                                        }
                                    }

                                    if (find)
                                    {
                                        for (int j = 0; i < firstOutPokerList_double.Count; j++)
                                        {
                                            // 对子要找两次
                                            {
                                                for (int k = m_myPokerObjList.Count - 1; k >= 0; k--)
                                                {
                                                    if ((m_myPokerObjList[k].GetComponent<PokerScript>().getPokerNum() == myPokerList_double[i - j].m_num) &&
                                                       (m_myPokerObjList[k].GetComponent<PokerScript>().getPokerType() == (int)m_curRoundFirstOutPokerList[0].m_pokerType))
                                                    {
                                                        if (!m_myPokerObjList[k].GetComponent<PokerScript>().getIsSelect())
                                                        {
                                                            m_myPokerObjList[k].GetComponent<PokerScript>().onClickPoker();
                                                            break;
                                                        }
                                                    }
                                                }
                                                
                                                for (int k = m_myPokerObjList.Count - 1; k >= 0; k--)
                                                {
                                                    if ((m_myPokerObjList[k].GetComponent<PokerScript>().getPokerNum() == myPokerList_double[i - j].m_num) &&
                                                       (m_myPokerObjList[k].GetComponent<PokerScript>().getPokerType() == (int)m_curRoundFirstOutPokerList[0].m_pokerType))
                                                    {
                                                        if (!m_myPokerObjList[k].GetComponent<PokerScript>().getIsSelect())
                                                        {
                                                            m_myPokerObjList[k].GetComponent<PokerScript>().onClickPoker();
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        reqOutPoker();

                                        return;
                                    }
                                }
                            }

                            // 没有拖拉机就按正常流程走
                            {
                                {
                                    int findAllNum = 0;

                                    // 选择同花色单牌
                                    {
                                        if (firstOutPokerList_single.Count > 0)
                                        {
                                            int hasFindNum = 0;
                                            for (int i = myPokerList_single.Count - 1; i >= 0; i--)
                                            {
                                                TLJCommon.PokerInfo pokerInfo = myPokerList_single[i];

                                                for (int j = m_myPokerObjList.Count - 1; j >= 0; j--)
                                                {
                                                    if ((m_myPokerObjList[j].GetComponent<PokerScript>().getPokerNum() == pokerInfo.m_num) &&
                                                       (m_myPokerObjList[j].GetComponent<PokerScript>().getPokerType() == (int)m_curRoundFirstOutPokerList[0].m_pokerType))
                                                    {
                                                        if (!m_myPokerObjList[j].GetComponent<PokerScript>().getIsSelect())
                                                        {
                                                            m_myPokerObjList[j].GetComponent<PokerScript>().onClickPoker();

                                                            ++hasFindNum;

                                                            break;
                                                        }
                                                    }
                                                }

                                                if (hasFindNum == firstOutPokerList_single.Count)
                                                {
                                                    break;
                                                }
                                            }

                                            findAllNum += hasFindNum;
                                        }
                                    }

                                    // 选择同花色对子
                                    {
                                        if (firstOutPokerList_double.Count > 0)
                                        {
                                            int hasFindNum = 0;
                                            for (int i = myPokerList_double.Count - 1; i >= 0; i--)
                                            {
                                                TLJCommon.PokerInfo pokerInfo = myPokerList_double[i];

                                                // 对子要找两次
                                                {
                                                    for (int j = m_myPokerObjList.Count - 1; j >= 0; j--)
                                                    {
                                                        if ((m_myPokerObjList[j].GetComponent<PokerScript>().getPokerNum() == pokerInfo.m_num) &&
                                                           (m_myPokerObjList[j].GetComponent<PokerScript>().getPokerType() == (int)m_curRoundFirstOutPokerList[0].m_pokerType))
                                                        {
                                                            if (!m_myPokerObjList[j].GetComponent<PokerScript>().getIsSelect())
                                                            {
                                                                m_myPokerObjList[j].GetComponent<PokerScript>().onClickPoker();

                                                                ++hasFindNum;

                                                                break;
                                                            }
                                                        }
                                                    }

                                                    for (int j = m_myPokerObjList.Count - 1; j >= 0; j--)
                                                    {
                                                        if ((m_myPokerObjList[j].GetComponent<PokerScript>().getPokerNum() == pokerInfo.m_num) &&
                                                           (m_myPokerObjList[j].GetComponent<PokerScript>().getPokerType() == (int)m_curRoundFirstOutPokerList[0].m_pokerType))
                                                        {
                                                            if (!m_myPokerObjList[j].GetComponent<PokerScript>().getIsSelect())
                                                            {
                                                                m_myPokerObjList[j].GetComponent<PokerScript>().onClickPoker();

                                                                ++hasFindNum;

                                                                break;
                                                            }
                                                        }
                                                    }
                                                }

                                                if (hasFindNum == (firstOutPokerList_double.Count * 2))
                                                {
                                                    break;
                                                }
                                            }

                                            findAllNum += hasFindNum;
                                        }
                                    }

                                    // 如果还差的话则拿剩余的同花色补充
                                    {
                                        if (findAllNum < needAllPokerNum)
                                        {
                                            for (int i = m_myPokerObjList.Count - 1; i >= 0; i--)
                                            {
                                                PokerScript pokerScript = m_myPokerObjList[i].GetComponent<PokerScript>();

                                                if (pokerScript.getPokerType() == (int)m_curRoundFirstOutPokerList[0].m_pokerType)
                                                {
                                                    if (!pokerScript.getIsSelect())
                                                    {
                                                        pokerScript.onClickPoker();

                                                        if ((++findAllNum) == needAllPokerNum)
                                                        {
                                                            reqOutPoker();

                                                            return;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            reqOutPoker();

                                            return;
                                        }
                                    }

                                    // 如果还差的话则拿其他花色补充
                                    {
                                        if (findAllNum < needAllPokerNum)
                                        {
                                            for (int i = m_myPokerObjList.Count - 1; i >= 0; i--)
                                            {
                                                PokerScript pokerScript = m_myPokerObjList[i].GetComponent<PokerScript>();

                                                if (pokerScript.getPokerType() != (int)m_curRoundFirstOutPokerList[0].m_pokerType)
                                                {
                                                    if (!pokerScript.getIsSelect())
                                                    {
                                                        pokerScript.onClickPoker();

                                                        if ((++findAllNum) == needAllPokerNum)
                                                        {
                                                            reqOutPoker();

                                                            return;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            reqOutPoker();

                                            return;
                                        }
                                    }

                                    ToastScript.createToast("拖拉机跟牌类型自动出牌失败：没找到足够的张数");
                                }
                            }
                        }
                        break;

                    case CheckOutPoker.OutPokerType.OutPokerType_Single:
                    case CheckOutPoker.OutPokerType.OutPokerType_Double:
                    case CheckOutPoker.OutPokerType.OutPokerType_ShuaiPai:
                    case CheckOutPoker.OutPokerType.OutPokerType_Error:
                        {
                            List<TLJCommon.PokerInfo> firstOutPokerList_single =  GameUtil.choiceSinglePoker(m_curRoundFirstOutPokerList, m_curRoundFirstOutPokerList[0].m_pokerType);
                            List<TLJCommon.PokerInfo> firstOutPokerList_double = GameUtil.choiceDoublePoker(m_curRoundFirstOutPokerList, m_curRoundFirstOutPokerList[0].m_pokerType);

                            List<TLJCommon.PokerInfo> myPokerList_single = GameUtil.choiceSinglePoker(myPokerList, m_curRoundFirstOutPokerList[0].m_pokerType);
                            List<TLJCommon.PokerInfo> myPokerList_double = GameUtil.choiceDoublePoker(myPokerList, m_curRoundFirstOutPokerList[0].m_pokerType);

                            int needAllPokerNum = firstOutPokerList_single.Count + firstOutPokerList_double.Count * 2;

                            {
                                int findAllNum = 0;

                                // 选择同花色单牌
                                {
                                    if (firstOutPokerList_single.Count > 0)
                                    {
                                        int hasFindNum = 0;
                                        for (int i = myPokerList_single.Count - 1; i >= 0; i--)
                                        {
                                            TLJCommon.PokerInfo pokerInfo = myPokerList_single[i];

                                            for (int j = m_myPokerObjList.Count - 1; j >= 0; j--)
                                            {
                                                if ((m_myPokerObjList[j].GetComponent<PokerScript>().getPokerNum() == pokerInfo.m_num) &&
                                                   (m_myPokerObjList[j].GetComponent<PokerScript>().getPokerType() == (int)m_curRoundFirstOutPokerList[0].m_pokerType))
                                                {
                                                    if (!m_myPokerObjList[j].GetComponent<PokerScript>().getIsSelect())
                                                    {
                                                        m_myPokerObjList[j].GetComponent<PokerScript>().onClickPoker();

                                                        ++hasFindNum;

                                                        break;
                                                    }
                                                }
                                            }

                                            if (hasFindNum == firstOutPokerList_single.Count)
                                            {
                                                break;
                                            }
                                        }

                                        findAllNum += hasFindNum;
                                    }
                                }

                                // 选择同花色对子
                                {
                                    if (firstOutPokerList_double.Count > 0)
                                    {
                                        int hasFindNum = 0;
                                        for (int i = myPokerList_double.Count - 1; i >= 0; i--)
                                        {
                                            TLJCommon.PokerInfo pokerInfo = myPokerList_double[i];

                                            // 对子要找两次
                                            {
                                                for (int j = m_myPokerObjList.Count - 1; j >= 0; j--)
                                                {
                                                    if ((m_myPokerObjList[j].GetComponent<PokerScript>().getPokerNum() == pokerInfo.m_num) &&
                                                       (m_myPokerObjList[j].GetComponent<PokerScript>().getPokerType() == (int)m_curRoundFirstOutPokerList[0].m_pokerType))
                                                    {
                                                        if (!m_myPokerObjList[j].GetComponent<PokerScript>().getIsSelect())
                                                        {
                                                            m_myPokerObjList[j].GetComponent<PokerScript>().onClickPoker();

                                                            ++hasFindNum;

                                                            break;
                                                        }
                                                    }
                                                }

                                                for (int j = m_myPokerObjList.Count - 1; j >= 0; j--)
                                                {
                                                    if ((m_myPokerObjList[j].GetComponent<PokerScript>().getPokerNum() == pokerInfo.m_num) &&
                                                       (m_myPokerObjList[j].GetComponent<PokerScript>().getPokerType() == (int)m_curRoundFirstOutPokerList[0].m_pokerType))
                                                    {
                                                        if (!m_myPokerObjList[j].GetComponent<PokerScript>().getIsSelect())
                                                        {
                                                            m_myPokerObjList[j].GetComponent<PokerScript>().onClickPoker();

                                                            ++hasFindNum;

                                                            break;
                                                        }
                                                    }
                                                }
                                            }

                                            if (hasFindNum == (firstOutPokerList_double.Count * 2))
                                            {
                                                break;
                                            }
                                        }

                                        findAllNum += hasFindNum;
                                    }
                                }

                                // 如果还差的话则拿剩余的同花色补充
                                {
                                    if (findAllNum < needAllPokerNum)
                                    {
                                        for (int i = m_myPokerObjList.Count - 1; i >= 0; i--)
                                        {
                                            PokerScript pokerScript = m_myPokerObjList[i].GetComponent<PokerScript>();

                                            if (pokerScript.getPokerType() == (int)m_curRoundFirstOutPokerList[0].m_pokerType)
                                            {
                                                if (!pokerScript.getIsSelect())
                                                {
                                                    pokerScript.onClickPoker();

                                                    if ((++findAllNum) == needAllPokerNum)
                                                    {
                                                        reqOutPoker();

                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        reqOutPoker();

                                        return;
                                    }
                                }

                                // 如果还差的话则拿其他花色补充
                                {
                                    if (findAllNum < needAllPokerNum)
                                    {
                                        for (int i = m_myPokerObjList.Count - 1; i >= 0; i--)
                                        {
                                            PokerScript pokerScript = m_myPokerObjList[i].GetComponent<PokerScript>();

                                            if (pokerScript.getPokerType() != (int)m_curRoundFirstOutPokerList[0].m_pokerType)
                                            {
                                                if (!pokerScript.getIsSelect())
                                                {
                                                    pokerScript.onClickPoker();

                                                    if ((++findAllNum) == needAllPokerNum)
                                                    {
                                                        reqOutPoker();

                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        reqOutPoker();

                                        return;
                                    }
                                }
                                
                                ToastScript.createToast("拖拉机跟牌类型自动出牌失败：没找到足够的张数");
                            }
                        }
                        break;

                    default:
                        {
                            ToastScript.createToast("时间到，自动出牌：未知类型");
                        }
                        break;
                }
            }
            else
            {
                ToastScript.createToast("异常：牌的张数不足");
            }
        }
    }
}
