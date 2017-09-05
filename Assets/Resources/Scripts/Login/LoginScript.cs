using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScript : MonoBehaviour {

    List<string> m_dataList = new List<string>();

    void Start ()
    {
        // 设置Socket事件
        SocketUtil.getInstance().setOnSocketEvent_Connect(onSocketConnect);
        SocketUtil.getInstance().setOnSocketEvent_Receive(onSocketReceive);
        SocketUtil.getInstance().setOnSocketEvent_Close(onSocketClose);
        SocketUtil.getInstance().setOnSocketEvent_Close(onSocketStop);

        SocketUtil.getInstance().start();
    }
    
    void Update ()
    {
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
            data["account"] = "123";
            data["password"] = "123";

            SocketUtil.getInstance().sendMessage(data.ToJson());
        }
    }

    // 请求注册
    public void reqQuickRegister()
    {
        {
            JsonData data = new JsonData();

            data["tag"] = "QuickRegister";
            data["account"] = "123";
            data["password"] = "123";
            
            SocketUtil.getInstance().sendMessage(data.ToJson());
        }
    }

    //-------------------------------------------------------------------------------------------------------
    void onSocketConnect()
    {
        Debug.Log("onSocketConnect");
    }

    void onSocketReceive(string data)
    {
        Debug.Log("onSocketReceive:" + data);

        m_dataList.Add(data);
    }

    void onSocketClose()
    {
        Debug.Log("onSocketClose");
    }

    void onSocketStop()
    {
        Debug.Log("onSocketStop");
    }
}