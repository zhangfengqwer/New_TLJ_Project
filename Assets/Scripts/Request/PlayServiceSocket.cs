using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayServiceSocket: MonoBehaviour
{
    public static PlayServiceSocket s_instance = null;

    SocketUtil m_socketUtil;
    bool m_isConnServerSuccess = false;
    bool m_isCloseSocket = false;
    List<string> m_dataList = new List<string>();

    public delegate void OnPlayService_Receive(string data);    // 收到服务器消息
    OnPlayService_Receive m_onPlayService_Receive_Play = null;

    public delegate void OnPlayService_Close();      // 与服务器断开
    OnPlayService_Close m_onPlayService_Receive_Close = null;

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
        if (m_isConnServerSuccess)
        {
            m_isConnServerSuccess = false;
        }

        if (m_isCloseSocket)
        {
            m_isCloseSocket = false;

            if (m_onPlayService_Receive_Close != null)
            {
                m_onPlayService_Receive_Close();
            }
        }

        for (int i = 0; i < m_dataList.Count; i++)
        {
            m_onPlayService_Receive_Play(m_dataList[i]);
            m_dataList.RemoveAt(i);
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

    public void sendMessage(string str)
    {
        m_socketUtil.sendMessage(str);
    }

    public void setOnPlayService_Receive(OnPlayService_Receive onPlayService_Receive)
    {
        m_onPlayService_Receive_Play = onPlayService_Receive;
    }

    public void setOnPlayService_Close(OnPlayService_Close onPlayService_Close)
    {
        m_onPlayService_Receive_Close = onPlayService_Close;
    }

    void onSocketConnect(bool result)
    {
        if (result)
        {
            Debug.Log("Play:连接服务器成功");
            m_isConnServerSuccess = true;
        }
        else
        {
            Debug.Log("Play:连接服务器失败，尝试重新连接");
            m_socketUtil.start();
        }
    }

    void onSocketReceive(string data)
    {
        Debug.Log("收到服务器消息:" + data);

        m_dataList.Add(data);
    }

    void onSocketClose()
    {
        Debug.Log("Play:被动与服务器断开连接,尝试重新连接");

        m_isCloseSocket = true;

        m_socketUtil.start();
    }

    void onSocketStop()
    {
        Debug.Log("Play:主动与服务器断开连接");
    }
}
