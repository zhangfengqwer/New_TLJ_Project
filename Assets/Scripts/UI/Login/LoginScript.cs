using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour
{
    List<string> m_dataList = new List<string>();
    bool m_isConnServerSuccess = false;

    public GameObject m_panel_choicePlatform;
    public GameObject m_panel_login;
    public GameObject m_panel_register;

    public InputField m_inputAccount;
    public InputField m_inputPassword;

    public InputField m_inputAccount_register;
    public InputField m_inputPassword_register;
    public InputField m_inputSecondPassword_register;

    void Start()
    {
        // 拉取数值表
        {
            NetConfig.reqNetConfig();
            PropData.getInstance().reqHttp();
            ChatData.getInstance().init();
            HuDongData.getInstance().init();
        }

        m_inputAccount.text = "123";
        m_inputPassword.text = "123";

        m_panel_login.transform.localScale = new Vector3(0, 0, 0);
        m_panel_register.transform.localScale = new Vector3(0, 0, 0);
    }

    // 等获取到服务器配置文件再调用
    public void init()
    {
        // 设置Socket事件
        SocketUtil.getInstance().setOnSocketEvent_Connect(onSocketConnect);
        SocketUtil.getInstance().setOnSocketEvent_Receive(onSocketReceive);
        SocketUtil.getInstance().setOnSocketEvent_Close(onSocketClose);
        SocketUtil.getInstance().setOnSocketEvent_Stop(onSocketStop);
        SocketUtil.getInstance().init(NetConfig.s_loginService_ip, NetConfig.s_loginService_port);
        SocketUtil.getInstance().start();
    }

    void OnDestroy()
    {
        SocketUtil.getInstance().stop();
    }

    void Update()
    {
        if (m_isConnServerSuccess)
        {
            ToastScript.createToast("连接服务器成功");
            m_isConnServerSuccess = false;
        }
        for (int i = 0; i < m_dataList.Count; i++)
        {
            print("取");
            onReceive(m_dataList[i]);
            m_dataList.RemoveAt(i);
        }
    }

    // 显示登录界面（输入账号密码）
    public void OnEnterLoginClick()
    {
        m_panel_choicePlatform.transform.localScale = new Vector3(0, 0, 0);
        m_panel_login.transform.localScale = new Vector3(1, 1, 1);
        m_panel_register.transform.localScale = new Vector3(0, 0, 0);
    }

    // 显示注册界面
    public void OnEnterRegisterClick()
    {
        m_panel_choicePlatform.transform.localScale = new Vector3(0, 0, 0);
        m_panel_login.transform.localScale = new Vector3(0, 0, 0);
        m_panel_register.transform.localScale = new Vector3(1, 1, 1);
    }

    // 微信登录
    public void onClickLogin_wechat()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();
        PlatformHelper.Login("Login", "GetLoginResult", "weixin");
        ToastScript.createToast("暂未开放");
    }

    public void GetLoginResult(string data)
    {
        print("anroid返回的数据：" + data);
    }

    // QQ登录
    public void onClickLogin_qq()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();

        ToastScript.createToast("暂未开放");
    }

    // 官方登录
    public void onClickLogin()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();
        reqLogin();
    }

    public void onClickQuickRegister()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();
        reqQuickRegister();
    }

    void onReceive(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        string tag = (string) jd["tag"];

        if (tag.CompareTo(TLJCommon.Consts.Tag_Login) == 0)
        {
            onReceive_Login(data);
        }
        else if (tag.CompareTo(TLJCommon.Consts.Tag_QuickRegister) == 0)
        {
            onReceive_QuickRegister(data);
        }
    }

    void onReceive_Login(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int) jd["code"];

        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            string uid = jd["userInfo"]["uid"].ToString();
            string name = jd["userInfo"]["name"].ToString();
            int goldNum = (int) (jd["userInfo"]["goldNum"]);

            UserData.uid = uid;

            SceneManager.LoadScene("MainScene");
//            SocketUtil.getInstance().stop();
//            GameObject LogicEnginer = Resources.Load<GameObject>("Prefabs/Logic/LogicEnginer");
//            GameObject.Instantiate(LogicEnginer);
        }
        else
        {
            ToastScript.createToast("登录失败：" + code.ToString());
        }
    }

    void onReceive_QuickRegister(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int) jd["code"];

        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            string uid = jd["userInfo"]["uid"].ToString();
            string name = jd["userInfo"]["name"].ToString();
            int goldNum = (int) (jd["userInfo"]["goldNum"]);

            UserData.uid = uid;

            //            SocketUtil.getInstance().stop();
            //            GameObject LogicEnginer = Resources.Load<GameObject>("Prefabs/Logic/LogicEnginer");
            //            GameObject.Instantiate(LogicEnginer);
            //ToastScript.createToast("注册成功");
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            ToastScript.createToast("注册失败：" + code.ToString());
        }
    }

    // 请求登录
    public void reqLogin()
    {
        if ((m_inputAccount.text.CompareTo("") == 0) || (m_inputPassword.text.CompareTo("") == 0))
        {
            ToastScript.createToast("请输入账号密码");
            return;
        }

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
        if ((m_inputAccount_register.text.CompareTo("") == 0) ||
            (m_inputSecondPassword_register.text.CompareTo("") == 0) ||
            (m_inputPassword_register.text.CompareTo("") == 0))
        {
            ToastScript.createToast("请输入账号密码");
            return;
        }

        if (m_inputSecondPassword_register.text.CompareTo(m_inputPassword_register.text) != 0)
        {
            ToastScript.createToast("密码不一致");
            return;
        }

        {
            JsonData data = new JsonData();

            data["tag"] = "QuickRegister";
            data["account"] = m_inputAccount_register.text;
            data["password"] = m_inputSecondPassword_register.text;

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

    private string account;
    private string password = "123";


    public void OnClickPalyer1()
    {
        account = "a";
        SendRequest();
    }

    public void OnClickPalyer2()
    {
        account = "b";
        SendRequest();
    }

    public void OnClickPalyer3()
    {
        account = "c";
        SendRequest();
    }

    public void OnClickPalyer4()
    {
        account = "d";
        SendRequest();
    }

    private void SendRequest()
    {
        JsonData data = new JsonData();

        data["tag"] = "Login";
        data["account"] = account;
        data["password"] = password;

        SocketUtil.getInstance().sendMessage(data.ToJson());
    }
}