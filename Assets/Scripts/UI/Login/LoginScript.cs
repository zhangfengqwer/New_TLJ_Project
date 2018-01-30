using LitJson;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoginScript : MonoBehaviour
{
    public Stopwatch stopwatch = new Stopwatch();
    public int TestCount = 0;
    public int TestAllCout = 500;

    public GameObject m_debugLog;
    static public DebugLogScript m_debugLogScript;

    public Button m_button_guanfang;
    public Button m_button_qq;
    public Button m_button_wechat;
    public Button m_button_defaultLogin;
    public Button m_button_3rdLogin;

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
    public Text m_text_chubanhao;

    public Toggle ToggleAgree;
    public GameObject exitGameObject;

    public int m_codeVersion = 1;

    private void Awake()
    {
        OtherData.s_channelName = PlatformHelper.GetChannelName();
        //OtherData.s_channelName = "ios";
        OtherData.s_loginScript = this;
    }

    void Start()
    {
        {
            m_panel_login.transform.localScale = new Vector3(0, 0, 0);
            m_panel_register.transform.localScale = new Vector3(0, 0, 0);

            // 出版号
            m_text_chubanhao.text = PlayerPrefs.GetString("banhao", "");

            if (!OtherData.s_isFromSetToLogin)
            {
                m_healthTipPanel.transform.localScale = new Vector3(1, 1, 1);
            }

            Set3rdLogin();
            setLogonTypeUI();
        }

        NetLoading.getInstance().Show();
        OtherConfigScript.getInstance().reqNet();

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "Start", null, null);
            return;
        }
    }

    public void getOtherConfigOver()
    {
        // 拉取热更dll
        {
            m_codeVersion = OtherConfigScript.getInstance().m_CodeVersion;

            NetLoading.getInstance().Show();
            ILRuntimeUtil.getInstance().downDll(OtherData.getWebUrl() + "hotfix/HotFix_Project-" + m_codeVersion + ".dll");
        }
    }

    public void Set3rdLogin()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "Set3rdLogin"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "Set3rdLogin", null, null);
            return;
        }

        bool is3RdLogin = ChannelHelper.Is3RdLogin();
        string channelAllName = ChannelHelper.GetChannelAllName();
        LogUtil.Log("渠道号:" + OtherData.s_channelName + ",渠道名:"+ channelAllName);

        bool isThirdLogin = PlatformHelper.IsThirdLogin();

        if (is3RdLogin)
        {
            if (isThirdLogin)
            {
                m_button_3rdLogin.gameObject.SetActive(true);
                m_button_defaultLogin.gameObject.SetActive(false);
                m_button_guanfang.gameObject.SetActive(false);
                m_button_qq.gameObject.SetActive(false);
                m_button_wechat.gameObject.SetActive(false);
                var childText = m_button_3rdLogin.transform.GetChild(0).GetComponent<Text>();
                childText.text = channelAllName + "账号登录";
            }
        }
        else
        {
            m_button_3rdLogin.gameObject.SetActive(false);
            m_button_defaultLogin.gameObject.SetActive(true);
            m_button_guanfang.gameObject.SetActive(true);
            m_button_qq.gameObject.SetActive(true);
            m_button_wechat.gameObject.SetActive(true);
            var childText = m_button_3rdLogin.transform.GetChild(0).GetComponent<Text>();
            childText.text = channelAllName + "账号登录";
        }

        m_button_3rdLogin.onClick.AddListener(() =>
        {
            AudioScript.getAudioScript().playSound_ButtonClick();
            PlatformHelper.Login("AndroidCallBack", "GetLoginResult", OtherData.s_channelName);
            NetLoading.getInstance().Show();
        });
    }

    public void onGetAllNetFile()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onGetAllNetFile"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onGetAllNetFile", null, null);
            return;
        }

        NetLoading.getInstance().Close();
        init();
    }

    public void onInvokeHealthPanel()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onInvokeHealthPanel"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onInvokeHealthPanel", null, null);
            return;
        }

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

        if (OtherData.s_isTest)
        {
            ToastScript.createToast("这是测试包");
        }
        else
        {
            LogUtil.Log("这是正式包");
        }
    }
    
    // 等获取到服务器配置文件再调用
    public void init()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "init"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "init", null, null);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "Update"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "Update", null, null);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LogUtil.Log("第三方返回:" + PlatformHelper.isThirdSDKQuit());

            if (PlatformHelper.isThirdSDKQuit())
            {
                PlatformHelper.thirdSDKQuit("AnroidCallBack", "", "");
            }
            else
            {
                if (exitGameObject == null)
                {
                    exitGameObject = ExitGamePanelScript.create();
                }
            }
        }
    }

    public void setLogonTypeUI()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "setLogonTypeUI"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "setLogonTypeUI", null, null);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "OnToggleAgree"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "OnToggleAgree", null, null);
            return;
        }

        var flag = ToggleAgree.isOn;
        var Panel_EnterLogin = this.transform.Find("Panel_EnterLogin");
        for (int i = 0; i < 5; i++)
        {
            Panel_EnterLogin.transform.GetChild(i).GetComponent<Button>().interactable = flag;
        }
        LogUtil.Log(flag);
    }

    // 显示登录界面（输入账号密码）
    public void OnEnterLoginClick()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "OnEnterLoginClick"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "OnEnterLoginClick", null, null);
            return;
        }

        m_panel_choicePlatform.transform.localScale = new Vector3(0, 0, 0);
        m_panel_login.transform.localScale = new Vector3(1, 1, 1);
        m_panel_register.transform.localScale = new Vector3(0, 0, 0);
    }

    // 显示注册界面
    public void OnEnterRegisterClick()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "OnEnterRegisterClick"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "OnEnterRegisterClick", null, null);
            return;
        }

        m_inputAccount_register.text = "";
        m_inputPassword_register.text = "";
        m_inputSecondPassword_register.text = "";

        m_panel_choicePlatform.transform.localScale = new Vector3(0, 0, 0);
        m_panel_login.transform.localScale = new Vector3(0, 0, 0);
        m_panel_register.transform.localScale = new Vector3(1, 1, 1);
    }

    public void onClickChangeLoginType()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onClickChangeLoginType"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onClickChangeLoginType", null, null);
            return;
        }

        PlayerPrefs.SetInt("DefaultLoginType", (int)OtherData.DefaultLoginType.DefaultLoginType_Default);

        setLogonTypeUI();
    }

    public void onClickDefaultLogin()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onClickDefaultLogin"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onClickDefaultLogin", null, null);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onClickLogin_wechat"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onClickLogin_wechat", null, null);
            return;
        }

        AudioScript.getAudioScript().playSound_ButtonClick();
        PlatformHelper.Login("AndroidCallBack", "GetLoginResult", "weixin");
        NetLoading.getInstance().Show();

        PlayerPrefs.SetInt("DefaultLoginType", (int)OtherData.DefaultLoginType.DefaultLoginType_WeChat);
    }

    // QQ登录
    public void onClickLogin_qq()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onClickLogin_qq"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onClickLogin_qq", null, null);
            return;
        }

        AudioScript.getAudioScript().playSound_ButtonClick();
        PlatformHelper.Login("AndroidCallBack", "GetLoginResult", "qq");
        NetLoading.getInstance().Show();

        PlayerPrefs.SetInt("DefaultLoginType", (int)OtherData.DefaultLoginType.DefaultLoginType_QQ);
    }

    // 官方登录
    public void onClickLogin()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onClickLogin"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onClickLogin", null, null);
            return;
        }

        AudioScript.getAudioScript().playSound_ButtonClick();
        reqLogin();
        PlayerPrefs.SetInt("DefaultLoginType", (int)OtherData.DefaultLoginType.DefaultLoginType_GuanFang);
    }

    public void onClickQuickRegister()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onClickQuickRegister"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onClickQuickRegister", null, null);
            return;
        }

        AudioScript.getAudioScript().playSound_ButtonClick();
        reqQuickRegister();
    }

    public void onClickLogin_close()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onClickLogin_close"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onClickLogin_close", null, null);
            return;
        }

        AudioScript.getAudioScript().playSound_ButtonClick();

        m_panel_choicePlatform.transform.localScale = new Vector3(1, 1, 1);
        m_panel_login.transform.localScale = new Vector3(0, 0, 0);
        m_panel_register.transform.localScale = new Vector3(0, 0, 0);
    }

    public void onClickRegister_close()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onClickRegister_close"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onClickRegister_close", null, null);
            return;
        }

        AudioScript.getAudioScript().playSound_ButtonClick();

        OnEnterLoginClick();
    }

    public void onApkVerisionIsLow()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onApkVerisionIsLow"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onApkVerisionIsLow", null, null);
            return;
        }

#if UNITY_ANDROID
        PlatformHelper.DownApk();
        GameObject go = GameObject.Find("NetErrorPanel(Clone)");
        if (go != null)
        {
            Destroy(go);
        }
#else

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onApkVerisionIsLow);
        NetErrorPanelScript.getInstance().setContentText("您的客户端版本过低，请更新到最新版本。");
#endif
    }

    public void onReceive(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onReceive"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onReceive", null, data);
            return;
        }

        JsonData jd = JsonMapper.ToObject(data);
        string tag = (string) jd["tag"];

        if (tag.CompareTo(TLJCommon.Consts.Tag_CheckVerisionCode) == 0)
        {
            onReceive_CheckVerisionCode(data);
        }
        else if (tag.CompareTo(TLJCommon.Consts.Tag_Login) == 0)
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

    public void onReceive_CheckVerisionCode(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onReceive_CheckVerisionCode"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onReceive_CheckVerisionCode", null, data);
            return;
        }

        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            bool isStopServer = (bool)jd["isStopServer"];
            if (isStopServer)
            {
                NetErrorPanelScript.getInstance().Show();
                NetErrorPanelScript.getInstance().setOnClickButton(onServerIsStop);
                NetErrorPanelScript.getInstance().setContentText("服务器正在维护，请稍后登录。");

                return;
            }

            OtherData.s_canRecharge = (bool)jd["canRecharge"];
            OtherData.s_canDebug = (bool)jd["canDebug"];

            string apkVersion = jd["apkVersion"].ToString();

            if (OtherData.s_apkVersion.CompareTo(apkVersion) < 0)
            {
                NetErrorPanelScript.getInstance().Show();
                NetErrorPanelScript.getInstance().setOnClickButton(onApkVerisionIsLow);
                NetErrorPanelScript.getInstance().setContentText("您的客户端版本过低，请更新到最新版本。");
            }

            //// 代码版本
            //{
            //    m_codeVersion = (int)jd["codeVersion"];

            //    NetLoading.getInstance().Show();
            //    ILRuntimeUtil.getInstance().downDll(OtherData.getWebUrl() + "hotfix/HotFix_Project-" + m_codeVersion + ".dll");
            //}

            {
                string banbao = jd["banhao"].ToString();
                PlayerPrefs.SetString("banhao", banbao);
            }
        }
        else
        {
            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(reqCheckVerisionCode);
            NetErrorPanelScript.getInstance().setContentText("服务器内部错误");
        }
    }

    public void onReceive_Login(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onReceive_Login"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onReceive_Login", null, data);
            return;
        }

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

    public void onReceive_Third_Login(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onReceive_Third_Login"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onReceive_Third_Login", null, data);
            return;
        }

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

    public void onReceive_QuickRegister(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onReceive_QuickRegister"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onReceive_QuickRegister", null, data);
            return;
        }

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

    public void onServerIsStop()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onServerIsStop"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onServerIsStop", null, null);
            return;
        }

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onServerIsStop);
        NetErrorPanelScript.getInstance().setContentText("服务器正在维护，请稍后登录。");
    }

    // 检查版本号
    public void reqCheckVerisionCode()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "reqCheckVerisionCode"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "reqCheckVerisionCode", null, null);
            return;
        }

        NetLoading.getInstance().Show();

        {
            JsonData data = new JsonData();
            data["tag"] = TLJCommon.Consts.Tag_CheckVerisionCode;

            LoginServiceSocket.s_instance.sendMessage(data.ToJson());
        }
    }

    // 请求登录
    public void reqLogin()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "reqLogin"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "reqLogin", null, null);
            return;
        }

        if ((m_inputAccount.text.CompareTo("") == 0) || (m_inputPassword.text.CompareTo("") == 0))
        {
            ToastScript.createToast("请输入账号密码");
            return;
        }

        NetLoading.getInstance().Show();

        {
            JsonData data = new JsonData();

            data["tag"] = TLJCommon.Consts.Tag_Login;
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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "reqQuickRegister"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "reqQuickRegister", null, null);
            return;
        }

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

            data["tag"] = TLJCommon.Consts.Tag_QuickRegister;
//            string result = Regex.Replace(m_inputAccount_register.text, @"\p{Cs}", "");//屏蔽emoji 
            data["account"] = m_inputAccount_register.text;
            data["password"] = CommonUtil.GetMD5(m_inputSecondPassword_register.text);
            data["channelname"] = PlatformHelper.GetChannelName();
            LoginServiceSocket.s_instance.sendMessage(data.ToJson());
        }
    }

    //-------------------------------------------------------------------------------------------------------
    public void onSocketConnect(bool result)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onSocketConnect"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onSocketConnect", null, result);
            return;
        }

        NetLoading.getInstance().Close();

        if (result)
        {
            //LogUtil.Log("连接服务器成功");

            NetLoading.getInstance().Close();
            NetErrorPanelScript.getInstance().Close();

            // 检查版本号
            reqCheckVerisionCode();
        }
        else
        {
            //LogUtil.Log("连接服务器失败，尝试重新连接");

            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian);
            NetErrorPanelScript.getInstance().setContentText("连接服务器失败，请重新连接");
        }
    }

    public void onSocketReceive(string data)
    {
        //LogUtil.Log("收到服务器消息:" + data);

        onReceive(data);
    }

    public void onSocketClose()
    {
        //LogUtil.Log("被动与服务器断开连接,尝试重新连接");

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian);
        NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");
    }

    public void onSocketStop()
    {
        //LogUtil.Log("主动与服务器断开连接");

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian);
        NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");
    }

    // 点击网络断开弹框中的重连按钮
    public void onClickChongLian()
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
    public void onPauseCallBack()
    {
        //LoginServiceSocket.s_instance.Stop();
        //LogicEnginerScript.Instance.Stop();
        //PlayServiceSocket.s_instance.Stop();
    }

    public void onResumeCallBack()
    {
        //NetErrorPanelScript.getInstance().Show();
        //NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian);
        //NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");

        checkNet();
    }

    // 检测服务器是否连接
    public void checkNet()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "checkNet"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "checkNet", null, null);
            return;
        }

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

    public void thread_test()
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

    public void showHealth()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "showHealth"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "showHealth", null, null);
            return;
        }

        Invoke("onInvokeHealthPanel", 3);
    }

    public void onDllGetOver()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LoginScript_hotfix", "onDllGetOver"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onDllGetOver", null, null);
            return;
        }

        {
            // 禁止多点触摸
            Input.multiTouchEnabled = false;

            // 永不息屏
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            OtherData.s_screenSize = new Vector2(Screen.width, Screen.height);

            // 安卓回调
            AndroidCallBack.s_onPauseCallBack = onPauseCallBack;
            AndroidCallBack.s_onResumeCallBack = onResumeCallBack;
        }

        ToastScript.clear();

        {
            GameUtil.hideGameObject(m_debugLog);

            // 用于打印屏幕日志
            m_debugLogScript = m_debugLog.GetComponent<DebugLogScript>();
        }

        m_inputAccount.text = PlayerPrefs.GetString("account", "");
        m_inputPassword.text = PlayerPrefs.GetString("password", "");

        if (!OtherData.s_isFromSetToLogin)
        {
            m_healthTipPanel.transform.localScale = new Vector3(1, 1, 1);
            showHealth();
        }
        else
        {
            NetLoading.getInstance().Show();

            // 获取数值表
            OtherData.s_getNetEntityFile.getNetFile();
        }

        // 健康忠告提示文字
        //m_text_tips.text = GameUtil.getOneTips();
    }
}