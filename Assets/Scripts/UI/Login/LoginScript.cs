using LitJson;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoginScript : MonoBehaviour
{
    Stopwatch stopwatch = new Stopwatch();
    int TestCount = 0;
    int TestAllCout = 500;

    public GameObject m_debugLog;
    static public DebugLogScript m_debugLogScript;

    public Button m_button_guanfang;
    public Button m_button_qq;
    public Button m_button_wechat;
    public Button m_button_defaultLogin;

    public GameObject m_healthTipPanel;
    public GameObject m_panel_choicePlatform;
    public GameObject m_panel_login;
    public GameObject m_panel_register;

    public InputField m_inputAccount;
    public InputField m_inputPassword;

    public InputField m_inputAccount_register;
    public InputField m_inputPassword_register;
    public InputField m_inputSecondPassword_register;

    public Text m_text_tips;

    public Toggle ToggleAgree;
    private GameObject exitGameObject;

    private void Awake()
    {
        OtherData.s_loginScript = this;
    }

    void Start()
    {
        {
            // 禁止多点触摸
            Input.multiTouchEnabled = false;

            // 永不息屏
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            OtherData.s_screenSize = new Vector2(Screen.width, Screen.height);

            // 安卓回调
            AndroidCallBack.s_onPauseCallBack = onPauseCallBack;
            AndroidCallBack.s_onResumeCallBack = onResumeCallBack;
//            if (string.IsNullOrEmpty(OtherData.s_apkVersion))
//            {
//                OtherData.s_apkVersion = PlatformHelper.GetVersionName();
//            }

        }

        ToastScript.clear();

        {
            GameUtil.hideGameObject(m_debugLog);

            // 用于打印屏幕日志
            m_debugLogScript = m_debugLog.GetComponent<DebugLogScript>();
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
        else
        {
            NetLoading.getInstance().Show();

            // 获取数值表
            OtherData.s_getNetEntityFile.getNetFile();
        }

        // 健康忠告提示文字
        m_text_tips.text = GameUtil.getOneTips();

        setLogonTypeUI();
    }

    public void onGetAllNetFile()
    {
        NetLoading.getInstance().Close();
        init();
    }

    void onInvokeHealthPanel()
    {
        m_healthTipPanel.transform.localScale = new Vector3(0, 0, 0);

        // 拉取数值表
        {
            NetLoading.getInstance().Show();
            NetConfig.reqNetConfig();

            PropData.getInstance().reqNet();
            ChatData.getInstance().reqNet();
            HuDongData.getInstance().reqNet();
            SensitiveWordUtil.reqNet();
            VipData.reqNet();
        }
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
            LogUtil.Log(ex.Message);
        }
    }

    void OnDestroy()
    {
        OtherData.s_loginScript = null;
        //        LoginServiceSocket.s_instance.Stop();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitGameObject == null)
            {
                exitGameObject = ExitGamePanelScript.create();
            }
        }
    }

    void setLogonTypeUI()
    {
        {
            int defaultLoginType = PlayerPrefs.GetInt("DefaultLoginType", (int)OtherData.s_defaultLoginType);

            switch (defaultLoginType)
            {
                case (int)OtherData.DefaultLoginType.DefaultLoginType_Default:
                    {
                        GameUtil.showGameObject(m_button_guanfang.gameObject);
                        GameUtil.showGameObject(m_button_qq.gameObject);
                        GameUtil.showGameObject(m_button_wechat.gameObject);

                        GameUtil.hideGameObject(m_button_defaultLogin.gameObject);
                    }
                    break;

                case (int)OtherData.DefaultLoginType.DefaultLoginType_GuanFang:
                    {
                        GameUtil.hideGameObject(m_button_guanfang.gameObject);
                        GameUtil.hideGameObject(m_button_qq.gameObject);
                        GameUtil.hideGameObject(m_button_wechat.gameObject);

                        GameUtil.showGameObject(m_button_defaultLogin.gameObject);

                        m_button_defaultLogin.transform.Find("Text_LoginType").GetComponent<Text>().text = "账号登录";
                    }
                    break;

                case (int)OtherData.DefaultLoginType.DefaultLoginType_QQ:
                    {
                        GameUtil.hideGameObject(m_button_guanfang.gameObject);
                        GameUtil.hideGameObject(m_button_qq.gameObject);
                        GameUtil.hideGameObject(m_button_wechat.gameObject);

                        GameUtil.showGameObject(m_button_defaultLogin.gameObject);
                        m_button_defaultLogin.transform.Find("Text_LoginType").GetComponent<Text>().text = "QQ登录";
                    }
                    break;

                case (int)OtherData.DefaultLoginType.DefaultLoginType_WeChat:
                    {
                        GameUtil.hideGameObject(m_button_guanfang.gameObject);
                        GameUtil.hideGameObject(m_button_qq.gameObject);
                        GameUtil.hideGameObject(m_button_wechat.gameObject);

                        GameUtil.showGameObject(m_button_defaultLogin.gameObject);
                        m_button_defaultLogin.transform.Find("Text_LoginType").GetComponent<Text>().text = "微信登录";
                    }
                    break;
            }
        }
    }

    public void OnToggleAgree()
    {
        var flag = ToggleAgree.isOn;
        var Panel_EnterLogin = this.transform.Find("Panel_EnterLogin");
        for (int i = 0; i < 3; i++)
        {
            Panel_EnterLogin.transform.GetChild(i).GetComponent<Button>().interactable = flag;
        }
        LogUtil.Log(flag);
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

    public void onClickChangeLoginType()
    {
        PlayerPrefs.SetInt("DefaultLoginType", (int)OtherData.DefaultLoginType.DefaultLoginType_Default);

        setLogonTypeUI();
    }

    public void onClickDefaultLogin()
    {
        int defaultLoginType = PlayerPrefs.GetInt("DefaultLoginType", (int)OtherData.s_defaultLoginType);

        switch (defaultLoginType)
        {
            case (int)OtherData.DefaultLoginType.DefaultLoginType_GuanFang:
                {
                    onClickLogin();
                }
                break;

            case (int)OtherData.DefaultLoginType.DefaultLoginType_QQ:
                {
                    onClickLogin_qq();
                }
                break;

            case (int)OtherData.DefaultLoginType.DefaultLoginType_WeChat:
                {
                    onClickLogin_wechat();
                }
                break;
        }
    }

    // 微信登录
    public void onClickLogin_wechat()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();
        PlatformHelper.Login("AndroidCallBack", "GetLoginResult", "weixin");

        PlayerPrefs.SetInt("DefaultLoginType", (int)OtherData.DefaultLoginType.DefaultLoginType_WeChat);
    }

    // QQ登录
    public void onClickLogin_qq()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();
        PlatformHelper.Login("AndroidCallBack", "GetLoginResult", "qq");


        PlayerPrefs.SetInt("DefaultLoginType", (int)OtherData.DefaultLoginType.DefaultLoginType_QQ);
    }

    // 官方登录
    public void onClickLogin()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();
        reqLogin();

        PlayerPrefs.SetInt("DefaultLoginType", (int)OtherData.DefaultLoginType.DefaultLoginType_GuanFang);
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
        if (++TestCount == TestAllCout)
        {
            stopwatch.Stop();
            LogUtil.Log("时间：" + stopwatch.ElapsedMilliseconds);
        }

        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int) jd["code"];

        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            string uid = jd["uid"].ToString();

            // 压力测试的时候拦截掉下面的逻辑
            // return;

            PlayerPrefs.SetString("account", m_inputAccount.text);
            PlayerPrefs.SetString("password", m_inputPassword.text);

            UserData.uid = uid;

            SceneManager.LoadScene("MainScene");
        }
        else if (code == (int) TLJCommon.Consts.Code.Code_PasswordError)
        {
            ToastScript.createToast("密码错误");
        }
        else if (code == (int) TLJCommon.Consts.Code.Code_AccountNoExist)
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

        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            string uid = jd["uid"].ToString();
            UserData.uid = uid;
            SceneManager.LoadScene("MainScene");
        }
        else if (code == (int) TLJCommon.Consts.Code.Code_PasswordError)
        {
            ToastScript.createToast("密码错误");
        }
        else if (code == (int) TLJCommon.Consts.Code.Code_AccountNoExist)
        {
            ToastScript.createToast("用户不存在");
        }
        else if (code == (int) TLJCommon.Consts.Code.Code_CommonFail)
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
        else if (code == (int) TLJCommon.Consts.Code.Code_CommonFail)
        {
            var msg = (string)jd["msg"];
            ToastScript.createToast(msg);
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
            string md5 = CommonUtil.GetMD5(m_inputPassword.text);
            LogUtil.Log(md5);
            data["password"] = md5;
            data["passwordtype"] = 1;

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
        if (SensitiveWordUtil.IsSensitiveWord(m_inputAccount_register.text))
        {
            ToastScript.createToast("您的账号有敏感词");

            return;
        }

        if (m_inputAccount_register.text.Length > 10)
        {
            ToastScript.createToast("账号长度不可超过10个字符");

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

            if (m_inputPassword_register.text.Length < 6)
            {
                ToastScript.createToast("密码至少6位");
                return;
            }

            if (m_inputPassword_register.text.Length > 30)
            {
                ToastScript.createToast("密码不能超过30位");
                return;
            }
        }

        NetLoading.getInstance().Show();

        {
            JsonData data = new JsonData();

            data["tag"] = "QuickRegister";
//            string result = Regex.Replace(m_inputAccount_register.text, @"\p{Cs}", "");//屏蔽emoji 
            data["account"] = m_inputAccount_register.text;
            data["password"] = CommonUtil.GetMD5(m_inputSecondPassword_register.text);

            LoginServiceSocket.s_instance.sendMessage(data.ToJson());
        }
    }

    //-------------------------------------------------------------------------------------------------------
    void onSocketConnect(bool result)
    {
        NetLoading.getInstance().Close();

        if (result)
        {
            //LogUtil.Log("连接服务器成功");

            NetLoading.getInstance().Close();
            NetErrorPanelScript.getInstance().Close();
        }
        else
        {
            //LogUtil.Log("连接服务器失败，尝试重新连接");

            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian);
            NetErrorPanelScript.getInstance().setContentText("连接服务器失败，请重新连接");
        }
    }

    void onSocketReceive(string data)
    {
        //LogUtil.Log("收到服务器消息:" + data);

        onReceive(data);
    }

    void onSocketClose()
    {
        //LogUtil.Log("被动与服务器断开连接,尝试重新连接");

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian);
        NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");
    }

    void onSocketStop()
    {
        //LogUtil.Log("主动与服务器断开连接");

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian);
        NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");
    }

    // 点击网络断开弹框中的重连按钮
    void onClickChongLian()
    {
        NetLoading.getInstance().Show();
        NetErrorPanelScript.getInstance().Close();
        LoginServiceSocket.s_instance.startConnect();
    }

    public void OnClickXieYi()
    {
        GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/Panel/UserAgreeMentPanel"),
            GameObject.Find("Canvas_Middle").transform);
    }

    //--------------------------------------------------------------------------------------------------
    void onPauseCallBack()
    {
        //LoginServiceSocket.s_instance.Stop();
        //LogicEnginerScript.Instance.Stop();
        //PlayServiceSocket.s_instance.Stop();
    }

    void onResumeCallBack()
    {
        //NetErrorPanelScript.getInstance().Show();
        //NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian);
        //NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");

        checkNet();
    }

    // 检测服务器是否连接
    void checkNet()
    {
        if (!LoginServiceSocket.s_instance.isConnecion())
        {
            //ToastScript.createToast("与Logic服务器断开连接");
            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian);
            NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");
        }
    }

    //--------------------------------------------------------------------------------------------------------------

    // 测试登录接口压力
    public void testLoginYaLi()
    {
        stopwatch.Start();
        Thread thread = new Thread(thread_test);
        thread.Start();
    }

    private void thread_test()
    {
        {
            JsonData data = new JsonData();

            data["tag"] = "Login";
            data["account"] = "123";
            data["password"] = "123";
            data["passwordtype"] = 1;

            for (int i = 0; i < TestAllCout; i++)
            {
                LoginServiceSocket.s_instance.sendMessage(data.ToJson());
            }
        }
    }
}