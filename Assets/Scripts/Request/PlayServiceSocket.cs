using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayServiceSocket: MonoBehaviour
{
    public static PlayServiceSocket s_instance = null;

    SocketUtil m_socketUtil;
    bool m_isConnecion = false;
    bool m_isCloseSocket = false;
    int m_connectState = 2;             // 0:连接失败  1:连接成功   2:无状态
    List<string> m_dataList = new List<string>();
    
    public delegate void OnPlayService_Receive(string data);            // 收到服务器消息
    OnPlayService_Receive m_onPlayService_Receive = null;

    public delegate void OnPlayService_Close();                         // 与服务器断开
    OnPlayService_Close m_onPlayService_Close = null;

    public delegate void OnPlayService_Connect(bool result);           // 连接服务器结果
    OnPlayService_Connect m_onPlayService_Connect = null;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/Logic/PlayEnginer") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab);

        return obj;
    }

    void Awake()
    {
        s_instance = this.GetComponent<PlayServiceSocket>();

        m_socketUtil = new SocketUtil();
        m_socketUtil.init(NetConfig.s_playService_ip, NetConfig.s_playService_port);

        m_socketUtil.setOnSocketEvent_Connect(onSocketConnect);
        m_socketUtil.setOnSocketEvent_Receive(onSocketReceive);
        m_socketUtil.setOnSocketEvent_Close(onSocketClose);
        m_socketUtil.setOnSocketEvent_Stop(onSocketStop);

        m_socketUtil.init(NetConfig.s_playService_ip, NetConfig.s_playService_port);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // 断开连接
        if (m_isCloseSocket)
        {
            m_isCloseSocket = false;

            if (m_onPlayService_Close != null)
            {
                m_onPlayService_Close();
            }
        }

        // 连接失败
        if (m_connectState == 0)
        {
            m_connectState = 2;

            if (m_onPlayService_Connect != null)
            {
                m_onPlayService_Connect(false);
            }
        }
        // 连接成功
        else if (m_connectState == 1)
        {
            m_connectState = 2;

            if (m_onPlayService_Connect != null)
            {
                m_onPlayService_Connect(true);
            }
        }

        //for (int i = 0; i < m_dataList.Count; i++)
        //{
        //    if (m_onPlayService_Receive != null)
        //    {
        //        m_onPlayService_Receive(m_dataList[i]);
        //        m_dataList.RemoveAt(i);
        //    }
        //}

        if (m_dataList.Count > 0)
        {
            if (m_onPlayService_Receive != null)
            {
                m_onPlayService_Receive(m_dataList[0]);
                m_dataList.RemoveAt(0);
            }
        }
    }

    void OnDestroy()
    {
        m_socketUtil.stop();
    }

    public void startConnect()
    {
        m_socketUtil.start();
    }

    public void Stop()
    {
        m_socketUtil.stop();
    }

    public bool isConnecion()
    {
        return m_isConnecion;
    }

    public void sendMessage(string str)
    {
        m_socketUtil.sendMessage(str);
    }

    public void setOnPlayService_Connect(OnPlayService_Connect onPlayService_Connect)
    {
        m_onPlayService_Connect = onPlayService_Connect;
    }

    public void setOnPlayService_Receive(OnPlayService_Receive onPlayService_Receive)
    {
        m_onPlayService_Receive = onPlayService_Receive;
    }

    public void setOnPlayService_Close(OnPlayService_Close onPlayService_Close)
    {
        m_onPlayService_Close = onPlayService_Close;
    }

    void onSocketConnect(bool result)
    {
        if (result)
        {
            LogUtil.Log("Play:连接服务器成功");
            m_connectState = 1;
            m_isConnecion = true;
        }
        else
        {
            LogUtil.Log("Play:连接服务器失败，尝试重新连接");
            m_connectState = 0;
            m_isConnecion = false;
        }
    }

    void onSocketReceive(string data)
    {
        LogUtil.Log("收到服务器消息:" + data);

        m_dataList.Add(data);
    }

    void onSocketClose()
    {
        LogUtil.Log("Play:被动与服务器断开连接,尝试重新连接");

        m_isCloseSocket = true;
        m_isConnecion = false;
    }

    void onSocketStop()
    {
        LogUtil.Log("Play:主动与服务器断开连接");
        m_isConnecion = false;
    }
}
