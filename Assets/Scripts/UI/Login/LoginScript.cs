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
    public GameObject m_healthTipPanel;
    public GameObject m_panel_choicePlatform;
    public GameObject m_panel_login;
    public GameObject m_panel_register;

    public InputField m_inputAccount;
    public InputField m_inputPassword;

    public InputField m_inputAccount_register;
    public InputField m_inputPassword_register;
    public InputField m_inputSecondPassword_register;

    NetErrorPanelScript m_netErrorPanelScript;

    void Start()
    {
        m_netErrorPanelScript = NetErrorPanelScript.create();

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
            Invoke("onInvokeHealthPanel", 3);
        }
    }
    void onInvokeHealthPanel()
    {
        m_healthTipPanel.transform.localScale = new Vector3(0,0,0);
    }

    // 等获取到服务器配置文件再调用
    public void init()
    {
        try
        {
            LoginServiceSocket.create();

            NetLoading.getInstance().Show();

            LoginServiceSocket.s_instance.setOnLoginService_Connect(onSocketConnect);
            LoginServiceSocket.s_instance.setOnLoginService_Receive(onSocketReceive);
            LoginServiceSocket.s_instance.setOnLoginService_Close(onSocketClose);
            LoginServiceSocket.s_instance.startConnect();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    void OnDestroy()
    {
        LoginServiceSocket.s_instance.Stop();
    }

    void Update()
    {
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
            
            LoginServiceSocket.s_instance.sendMessage(jd.ToJson());
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
        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int) jd["code"];

        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            string uid = jd["uid"].ToString();

            PlayerPrefs.SetString("account", m_inputAccount.text);
            PlayerPrefs.SetString("password", m_inputPassword.text);

            UserData.uid = uid;

            SceneManager.LoadScene("MainScene");
        }
        else if (code == (int)TLJCommon.Consts.Code.Code_PasswordError)
        {
            ToastScript.createToast("密码错误");
        }
        else if (code == (int)TLJCommon.Consts.Code.Code_AccountNoExist)
        {
            ToastScript.createToast("用户不存在");
        }
        else
        {
            ToastScript.createToast("服务器内部错误");
        }
    }

    private void onReceive_Third_Login(string data)
    {
        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int) jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            string uid = jd["uid"].ToString();
            UserData.uid = uid;
            SceneManager.LoadScene("MainScene");
        }
        else if (code == (int)TLJCommon.Consts.Code.Code_PasswordError)
        {
            ToastScript.createToast("密码错误");
        }
        else if (code == (int)TLJCommon.Consts.Code.Code_AccountNoExist)
        {
            ToastScript.createToast("用户不存在");
        }
        else if (code == (int)TLJCommon.Consts.Code.Code_CommonFail)
        {
            ToastScript.createToast("登录失败");
        }
        else
        {
            ToastScript.createToast("服务器内部错误");
        }
    }

    void onReceive_QuickRegister(string data)
    {
        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int) jd["code"];

        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            string uid = jd["uid"].ToString();
         
            PlayerPrefs.SetString("account", m_inputAccount_register.text);
            PlayerPrefs.SetString("password", m_inputPassword_register.text);

            UserData.uid = uid;
            
            SceneManager.LoadScene("MainScene");
        }
        else if (code == (int)TLJCommon.Consts.Code.Code_CommonFail)
        {
            ToastScript.createToast("用户已存在");
        }
        else
        {
            ToastScript.createToast("服务器内部错误");
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

        NetLoading.getInstance().Show();

        {
            JsonData data = new JsonData();

            data["tag"] = "Login";
            data["account"] = m_inputAccount.text;
            data["password"] = m_inputPassword.text;

            LoginServiceSocket.s_instance.sendMessage(data.ToJson());
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

        NetLoading.getInstance().Show();

        {
            JsonData data = new JsonData();

            data["tag"] = "QuickRegister";
            data["account"] = m_inputAccount_register.text;
            data["password"] = m_inputSecondPassword_register.text;
            
            LoginServiceSocket.s_instance.sendMessage(data.ToJson());
        }
    }

    //-------------------------------------------------------------------------------------------------------
    void onSocketConnect(bool result)
    {
        NetLoading.getInstance().Close();

        if (result)
        {
            //Debug.Log("连接服务器成功");

            ToastScript.createToast("连接服务器成功");

            NetLoading.getInstance().Close();
            m_netErrorPanelScript.Close();
        }
        else
        {
            //Debug.Log("连接服务器失败，尝试重新连接");

            m_netErrorPanelScript.Show();
            m_netErrorPanelScript.setOnClickButton(onClickChongLian);
            m_netErrorPanelScript.setContentText("连接服务器失败，请重新连接");
        }
    }

    void onSocketReceive(string data)
    {
        //Debug.Log("收到服务器消息:" + data);
        
        onReceive(data);
    }

    void onSocketClose()
    {
        //Debug.Log("被动与服务器断开连接,尝试重新连接");

        m_netErrorPanelScript.Show();
        m_netErrorPanelScript.setOnClickButton(onClickChongLian);
        m_netErrorPanelScript.setContentText("与服务器断开连接，请重新连接");
    }

    void onSocketStop()
    {
        //Debug.Log("主动与服务器断开连接");

        m_netErrorPanelScript.Show();
        m_netErrorPanelScript.setOnClickButton(onClickChongLian);
        m_netErrorPanelScript.setContentText("与服务器断开连接，请重新连接");
    }

    // 点击网络断开弹框中的重连按钮
    void onClickChongLian()
    {
        NetLoading.getInstance().Show();
        m_netErrorPanelScript.Close();
        LoginServiceSocket.s_instance.startConnect();
    }

    public void OnClickXieYi()
    {
        GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Panel/UserAgreeMentPanel"),GameObject.Find("Canvas_Middle").transform);
    }
}