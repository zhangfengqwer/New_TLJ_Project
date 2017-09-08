using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {

    List<string> m_dataList = new List<string>();
    bool m_isConnServerSuccess = false;

    // Use this for initialization
    void Start ()
    {
        // 设置Socket事件
        SocketUtil.getInstance().setOnSocketEvent_Connect(onSocketConnect);
        SocketUtil.getInstance().setOnSocketEvent_Receive(onSocketReceive);
        SocketUtil.getInstance().setOnSocketEvent_Close(onSocketClose);
        SocketUtil.getInstance().setOnSocketEvent_Stop(onSocketStop);

        SocketUtil.getInstance().init(NetConfig.s_playService_ip, NetConfig.s_playService_port);
        SocketUtil.getInstance().start();
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
                    string str = string.Format("人数已凑齐，可以开赛：{0}、{1}、{2}、{3}", jd["userList"][0]["uid"],jd["userList"][1]["uid"],jd["userList"][2]["uid"],jd["userList"][3]["uid"]);
                    ToastScript.createToast(str);
                    //for (int i = 0; i < jd["userList"].Count; i++)
                    //{
                    //    Debug.Log(jd["userList"][i]["name"]);
                    //    Debug.Log(jd["userList"][i]["uid"]);
                    //}
                }
                break;

            case (int)TLJCommon.Consts.PlayAction.PlayAction_OutPoker:
                {

                }
                break;
        }
    }

    //----------------------------------------------------------接收数据 end--------------------------------------------------

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
}
