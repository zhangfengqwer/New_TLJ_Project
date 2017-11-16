using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginServiceSocket : MonoBehaviour {

    public static LoginServiceSocket s_instance = null;

    SocketUtil m_socketUtil;
    bool m_isCloseSocket = false;
    int m_connectState = 2;             // 0:连接失败  1:连接成功   2:无状态
    List<string> m_dataList = new List<string>();

    public delegate void OnLoginService_Receive(string data);           // 收到服务器消息
    OnLoginService_Receive m_onLoginService_Receive = null;

    public delegate void OnLoginService_Close();                        // 与服务器断开
    OnLoginService_Close m_onLoginService_Close = null;

    public delegate void OnLoginService_Connect(bool result);           // 连接服务器结果
    OnLoginService_Connect m_onLoginService_Connect = null;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/Logic/LoginEnginer") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab);

        return obj;
    }

    void Awake()
    {
        s_instance = this.GetComponent<LoginServiceSocket>();

        m_socketUtil = new SocketUtil();
        m_socketUtil.init(NetConfig.s_loginService_ip, NetConfig.s_loginService_port);

        m_socketUtil.setOnSocketEvent_Connect(onSocketConnect);
        m_socketUtil.setOnSocketEvent_Receive(onSocketReceive);
        m_socketUtil.setOnSocketEvent_Close(onSocketClose);
        m_socketUtil.setOnSocketEvent_Stop(onSocketStop);

        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // 断开连接
        if (m_isCloseSocket)
        {
            m_isCloseSocket = false;

            if (m_onLoginService_Close != null)
            {
                m_onLoginService_Close();
            }
        }

        // 连接失败
        if (m_connectState == 0)
        {
            m_connectState = 2;

            if (m_onLoginService_Connect != null)
            {
                m_onLoginService_Connect(false);
            }
        }
        // 连接成功
        else if (m_connectState == 1)
        {
            m_connectState = 2;

            if (m_onLoginService_Connect != null)
            {
                m_onLoginService_Connect(true);
            }
        }

        for (int i = 0; i < m_dataList.Count; i++)
        {
            m_onLoginService_Receive(m_dataList[i]);
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

    public void setOnLoginService_Connect(OnLoginService_Connect onLoginService_Connect)
    {
        m_onLoginService_Connect = onLoginService_Connect;
    }

    public void setOnLoginService_Receive(OnLoginService_Receive onLoginService_Receive)
    {
        m_onLoginService_Receive = onLoginService_Receive;
    }

    public void setOnLoginService_Close(OnLoginService_Close onLoginService_Close)
    {
        m_onLoginService_Close = onLoginService_Close;
    }

    void onSocketConnect(bool result)
    {
        if (result)
        {
            LogUtil.Log("Login:连接服务器成功");
            m_connectState = 1;
        }
        else
        {
            LogUtil.Log("Login:连接服务器失败，尝试重新连接");
            m_connectState = 0;
        }
    }

    void onSocketReceive(string data)
    {
        LogUtil.Log("收到服务器消息:" + data);

        m_dataList.Add(data);
    }

    void onSocketClose()
    {
        LogUtil.Log("Login:被动与服务器断开连接,尝试重新连接");

        m_isCloseSocket = true;
    }

    void onSocketStop()
    {
        LogUtil.Log("Login:主动与服务器断开连接");
    }
}
