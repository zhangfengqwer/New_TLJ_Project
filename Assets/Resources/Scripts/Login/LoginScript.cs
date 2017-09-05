using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour {

    List<string> m_dataList = new List<string>();
    bool m_isConnServerSuccess = false;

    public InputField m_inputAccount;
    public InputField m_inputPassword;

    void Start ()
    {
        {
            // 设置Socket事件
            SocketUtil.getInstance().setOnSocketEvent_Connect(onSocketConnect);
            SocketUtil.getInstance().setOnSocketEvent_Receive(onSocketReceive);
            SocketUtil.getInstance().setOnSocketEvent_Close(onSocketClose);
            SocketUtil.getInstance().setOnSocketEvent_Stop(onSocketStop);

            SocketUtil.getInstance().init("10.224.5.110", 60001);
            SocketUtil.getInstance().start();
        }
    }

    void OnDestroy()
    {
        SocketUtil.getInstance().stop();
    }
    
    void Update ()
    {
        if (m_isConnServerSuccess)
        {
            ToastScript.createToast("连接服务器成功");
            m_isConnServerSuccess = false;
        }

        for (int i = 0; i < m_dataList.Count; i++)
        {
            ToastScript.createToast("收到消息:" + m_dataList[i]);
            m_dataList.RemoveAt(i);
        }
	}

    // 请求登录
    public void reqLogin()
    {
        {
            JsonData data = new JsonData();

            data["tag"] = "Login";
            data["account"] = m_inputAccount.text;
            data["password"] = m_inputPassword.text;

            SocketUtil.getInstance().sendMessage(data.ToJson());
        }
    }

    // 请求注册
    public void reqQuickRegister()
    {
        {
            JsonData data = new JsonData();

            data["tag"] = "QuickRegister";
            data["account"] = m_inputAccount.text;
            data["password"] = m_inputPassword.text;

            SocketUtil.getInstance().sendMessage(data.ToJson());
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
}