using System;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;
using TLJCommon;


public class LoginScript : MonoBehaviour
{
    SocketUtil m_socketUtil;

    List<string> m_dataList = new List<string>();
    bool m_isConnServerSuccess = false;

    public GameObject m_healthTipPanel;
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
            SensitiveWordUtil.InitWords();
        }

        m_inputAccount.text = PlayerPrefs.GetString("account", "");
        m_inputPassword.text = PlayerPrefs.GetString("password", "");

        m_panel_login.transform.localScale = new Vector3(0, 0, 0);
        m_panel_register.transform.localScale = new Vector3(0, 0, 0);

        if (!OtherData.s_isFromSetToLogin)
        {
            m_healthTipPanel.transform.localScale = new Vector3(1, 1, 1);
            Invoke("onInvokeHealthPanel", 2);
        }
    }
    void onInvokeHealthPanel()
    {
        m_healthTipPanel.transform.localScale = new Vector3(0,0,0);
    }

    // 等获取到服务器配置文件再调用
    public void init()
    {
        m_socketUtil = new SocketUtil();
        m_socketUtil.setOnSocketEvent_Connect(onSocketConnect);
        m_socketUtil.setOnSocketEvent_Receive(onSocketReceive);
        m_socketUtil.setOnSocketEvent_Close(onSocketClose);
        m_socketUtil.setOnSocketEvent_Stop(onSocketStop);

        m_socketUtil.init(NetConfig.s_loginService_ip, NetConfig.s_loginService_port);
        m_socketUtil.start();
    }

    void OnDestroy()
    {
        m_socketUtil.stop();
    }

    void Update()
    {
        if (m_isConnServerSuccess)
        {
            //ToastScript.createToast("连接服务器成功");
            m_isConnServerSuccess = false;
        }
        for (int i = 0; i < m_dataList.Count; i++)
        {
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
        m_inputAccount_register.text = "";
        m_inputPassword_register.text = "";
        m_inputSecondPassword_register.text = "";

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

    // QQ登录
    public void onClickLogin_qq()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();
        PlatformHelper.Login("Login", "GetLoginResult", "qq");
    }

    public void GetLoginResult(string data)
    {
        try
        {
            JsonData jsonData = JsonMapper.ToObject(data);
            var openId = (string) jsonData["openid"];
            var nickname = (string) jsonData["nickname"];
            var figureurl = (string) jsonData["figureurl"];
            var platform = (string) jsonData["platform"];

            JsonData jd = new JsonData();
            jd["tag"] = Consts.Tag_Third_Login;
            jd["nickname"] = nickname;
            jd["third_id"] = openId;
            jd["platform"] = platform;

            m_socketUtil.sendMessage(jd.ToJson());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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

    public void onClickLogin_close()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();

        m_panel_choicePlatform.transform.localScale = new Vector3(1, 1, 1);
        m_panel_login.transform.localScale = new Vector3(0, 0, 0);
        m_panel_register.transform.localScale = new Vector3(0, 0, 0);
    }

    public void onClickRegister_close()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();

        OnEnterLoginClick();
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
        else if (tag.CompareTo(TLJCommon.Consts.Tag_Third_Login) == 0)
        {
            onReceive_Third_Login(data);
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

            PlayerPrefs.SetString("account", m_inputAccount.text);
            PlayerPrefs.SetString("password", m_inputPassword.text);

            UserData.uid = uid;

            SceneManager.LoadScene("MainScene");
//            m_socketUtil.stop();
//            GameObject LogicEnginer = Resources.Load<GameObject>("Prefabs/Logic/LogicEnginer");
//            GameObject.Instantiate(LogicEnginer);
        }
        else
        {
            ToastScript.createToast("登录失败：" + code.ToString());
        }
    }

    private void onReceive_Third_Login(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int) jd["code"];

        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            string uid = jd["uid"].ToString();
            UserData.uid = uid;
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            ToastScript.createToast("：" + code.ToString());
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

            PlayerPrefs.SetString("account", m_inputAccount_register.text);
            PlayerPrefs.SetString("password", m_inputPassword_register.text);

            UserData.uid = uid;

            //            m_socketUtil.stop();
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

            m_socketUtil.sendMessage(data.ToJson());
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

        // 检测账号是否合格
        if(SensitiveWordUtil.IsSensitiveWord(m_inputAccount_register.text))
        {
            ToastScript.createToast("您的账号有敏感词");

            return;
        }

        // 检测密码是否合格
        {
            for (int i = 0; i < m_inputPassword_register.text.Length; i++)
            {
                string str = m_inputPassword_register.text[i].ToString();
                if (((CommonUtil.charToAsc(str) >= 48) && (CommonUtil.charToAsc(str) <= 57) ||
                    ((CommonUtil.charToAsc(str) >= 65) && (CommonUtil.charToAsc(str) <= 90) ||
                    ((CommonUtil.charToAsc(str) >= 97) && (CommonUtil.charToAsc(str) <= 122)))))
                {

                }
                else
                {
                    ToastScript.createToast("密码格式不对");

                    return;
                }
            }
        }

        {
            JsonData data = new JsonData();

            data["tag"] = "QuickRegister";
            data["account"] = m_inputAccount_register.text;
            data["password"] = m_inputSecondPassword_register.text;

            m_socketUtil.sendMessage(data.ToJson());
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
        Debug.Log("被动与服务器断开连接,尝试重新连接");
        m_socketUtil.start();
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

        m_socketUtil.sendMessage(data.ToJson());
    }

    public void OnClickXieYi()
    {
        print("xieyi");
    }
}