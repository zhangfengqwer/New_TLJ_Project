using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

    public Button m_buttonStartGame;
    public Button m_buttonOutPoker;

    List<string> m_dataList = new List<string>();
    bool m_isConnServerSuccess = false;

    List<TLJCommon.PokerInfo> myPokerList = new List<TLJCommon.PokerInfo>();
    List<GameObject> m_myPokerObjList = new List<GameObject>();
    List<GameObject> m_outPokerObjList = new List<GameObject>();
    List<GameObject> m_otherPlayerUIObjList = new List<GameObject>();
    
    GameObject m_timer;
    TimerScript m_timerScript;

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
        m_buttonOutPoker.interactable = false;

        // 上边的玩家
        {
            GameObject obj = OtherPlayerUIScript.create();
            obj.transform.localPosition = new Vector3(0, 300, 0);

            m_otherPlayerUIObjList.Add(obj);
        }

        // 左边的玩家
        {
            GameObject obj = OtherPlayerUIScript.create();
            obj.transform.localPosition = new Vector3(-550, 0, 0);

            m_otherPlayerUIObjList.Add(obj);
        }

        // 右边的玩家
        {
            GameObject obj = OtherPlayerUIScript.create();
            obj.transform.localPosition = new Vector3(550, 0, 0);

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
        m_buttonOutPoker.interactable = false;
        m_timerScript.stop();

        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
        data["uid"] = UserDataScript.getInstance().getUserInfo().m_uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_OutPoker;
        
        // 自己出的牌
        {
            JsonData jarray = new JsonData();
            for (int i = 0; i < m_myPokerObjList.Count; i++)
            {
                PokerScript pokerScript = m_myPokerObjList[i].GetComponent<PokerScript>();
                if (pokerScript.getIsSelect())
                {
                    JsonData jd = new JsonData();
                    jd["num"] = pokerScript.getPokerNum();
                    jd["pokerType"] = pokerScript.getPokerType();
                    jarray.Add(jd);
                }
            }
            data["pokerList"] = jarray;
        }

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
                }
                break;

            case (int)TLJCommon.Consts.PlayAction.PlayAction_StartGame:
                {
                    //string str = string.Format("人数已凑齐，可以开赛：{0}、{1}、{2}、{3}", jd["userList"][0]["uid"],jd["userList"][1]["uid"],jd["userList"][2]["uid"],jd["userList"][3]["uid"]);
                    //ToastScript.createToast(str);

                    for (int i = 0; i < jd["pokerList"].Count; i++)
                    {
                        int num = (int)jd["pokerList"][i]["num"];
                        int pokerType = (int)jd["pokerList"][i]["pokerType"];

                        myPokerList.Add(new TLJCommon.PokerInfo(num, (TLJCommon.Consts.PokerType)pokerType));
                    }

                    // 显示其他玩家的头像、昵称、金币
                    for (int i = 0; i < m_otherPlayerUIObjList.Count; i++)
                    {
                        m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().setHead("");
                        m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().setName("昵称");
                        m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().setGoldNum(1000);
                    }

                    startGame();
                }
                break;

            case (int)TLJCommon.Consts.PlayAction.PlayAction_OutPoker:
                {
                    try
                    {
                        int hasPlayerOutPoker = (int)jd["hasPlayerOutPoker"];
                        if (hasPlayerOutPoker == 1)
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
                                }

                                initPokerPos(m_myPokerObjList, true);
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
                                showOtherOutPoker(outPokerList);
                            }
                        }

                        // 轮到自己出牌
                        {
                            string uid = (string)jd["cur_uid"];
                            if (uid.CompareTo(UserDataScript.getInstance().getUserInfo().m_uid) == 0)
                            {
                                m_buttonOutPoker.interactable = true;
                                ToastScript.createToast("轮到你出牌");

                                // 如果是自己出牌，就设置倒计时回调
                                m_timerScript.setOnTimerEvent_TimeEnd(onTimerEventTimeEnd);
                            }
                            else
                            {
                                // 如果不是自己出牌，就取消倒计时回调
                                m_timerScript.setOnTimerEvent_TimeEnd(null);
                            }
                        }

                        // 开始倒计时
                        m_timerScript.start(10);
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
        for (int i = 0; i < myPokerList.Count; i++)
        {
            GameObject poker = PokerScript.createPoker();
            poker.transform.SetParent(GameObject.Find("Canvas").transform);
            poker.transform.localScale = new Vector3(1,1,1);

            poker.GetComponent<PokerScript>().initPoker(myPokerList[i].m_num, (int)myPokerList[i].m_pokerType);

            m_myPokerObjList.Add(poker);
        }

        initPokerPos(m_myPokerObjList,true);
    }

    void initPokerPos(List<GameObject> objList,bool isMyPokerList)
    {
        for (int i = 0; i < objList.Count; i++)
        {
            // 我的牌堆
            if (isMyPokerList)
            {
                int x = CommonUtil.getPosX(objList.Count, 40, i, 0);
                objList[i].transform.localPosition = new Vector3(x, -180, 0);
            }
            // 上一个玩家出的牌的牌堆
            else
            {
                int x = CommonUtil.getPosX(objList.Count, 30, i, 0);
                objList[i].transform.localPosition = new Vector3(x, 0, 0);
            }

            // 设置最后渲染
            objList[i].transform.SetAsLastSibling();
        }
    }

    void showOtherOutPoker(List<TLJCommon.PokerInfo> pokerList)
    {
        // 先删除之前的出的牌
        for (int i = m_outPokerObjList.Count - 1; i >= 0; i--)
        {
            Destroy(m_outPokerObjList[i]);
            m_outPokerObjList.RemoveAt(i);
        }

        // 再显示现在出的牌
        for (int i = 0; i < pokerList.Count; i++)
        {
            GameObject poker = PokerScript.createPoker();
            poker.transform.SetParent(GameObject.Find("Canvas").transform);
            poker.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            poker.GetComponent<PokerScript>().initPoker(pokerList[i].m_num, (int)pokerList[i].m_pokerType);

            // 禁止点击
            Destroy(poker.GetComponent<Button>());

            m_outPokerObjList.Add(poker);
        }

        initPokerPos(m_outPokerObjList,false);
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
        ToastScript.createToast("时间到，自动出牌");

        if (m_myPokerObjList.Count > 0)
        {
            m_myPokerObjList[m_myPokerObjList.Count - 1].GetComponent<PokerScript>().onClickPoker();
        }
        
        reqOutPoker();
    }
}
