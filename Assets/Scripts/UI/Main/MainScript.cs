using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TLJCommon;

public class MainScript : MonoBehaviour
{
    public Image m_notice_redPoint;
    public Image m_task_redPoint;
    public Image m_sign_redPoint;
    public Image m_mail_redPoint;

    public Image VipImage;

    public Button m_button_xiuxianchang;
    public Button m_button_jingjichang;
    public GameObject m_xiuxianchang;
    public Text UserAccount;
    public Text UserGold;
    public Text MyGold;
    public Text UserYuanBao;
    public Text UserMedal;

    public GameObject m_laba;
    public GameObject m_headIcon;

    private GameObject m_waitMatchPanel = null;
    private GameObject exitGameObject;
    LaBaScript m_laBaScript;

    //发送验证码的倒计时
    private float nextTime = 1;//一秒之后执行

    // Use this for initialization
    void Start ()
	{
        // 禁止多点触摸
        Input.multiTouchEnabled = false;

        ToastScript.clear();

        // 安卓回调
        AndroidCallBack.s_onPauseCallBack = onPauseCallBack;
        AndroidCallBack.s_onResumeCallBack = onResumeCallBack;
        
        AudioScript.getAudioScript().stopMusic();

        // 3秒后播放背景音乐,每隔55秒重复播放背景音乐
        InvokeRepeating("onInvokeStartMusic", 3, 55);

        // 逻辑服务器
        {
            if (LogicEnginerScript.Instance == null)
            {
                LogicEnginerScript.create();

                LogicEnginerScript.Instance.setOnLoginService_Connect(onSocketConnect_Logic);
                LogicEnginerScript.Instance.setOnLoginService_Close(onSocketClose_Logic);
                LogicEnginerScript.Instance.GetComponent<MainRequest>().CallBack = onReceive_Main;
            }
            else
            {
                LogicEnginerScript.Instance.setOnLoginService_Connect(onSocketConnect_Logic);
                LogicEnginerScript.Instance.setOnLoginService_Close(onSocketClose_Logic);
                LogicEnginerScript.Instance.GetComponent<MainRequest>().CallBack = onReceive_Main;

                if (LogicEnginerScript.Instance.isConnecion())
                {
                    LogicEnginerScript.Instance.GetComponent<GetUserInfoRequest>().OnRequest();
                    LogicEnginerScript.Instance.GetComponent<GetRankRequest>().OnRequest();
                    LogicEnginerScript.Instance.GetComponent<GetSignRecordRequest>().OnRequest();
                    LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().CallBack = onReceive_GetUserBag;
                    LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().OnRequest();
                    LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().OnRequest();
                    LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().OnRequest();
                }
                else
                {
                    NetErrorPanelScript.getInstance().Show();
                    NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
                    NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");
                }
            }
        }

	    // 游戏打牌服务器
	    {
            if (PlayServiceSocket.s_instance == null)
            {
                PlayServiceSocket.create();

                PlayServiceSocket.s_instance.setOnPlayService_Connect(onSocketConnect_Play);
                PlayServiceSocket.s_instance.setOnPlayService_Receive(onSocketReceive_Play);
                PlayServiceSocket.s_instance.setOnPlayService_Close(onSocketClose_Play);

                PlayServiceSocket.s_instance.startConnect();
            }
            else
            {
                PlayServiceSocket.s_instance.setOnPlayService_Connect(onSocketConnect_Play);
                PlayServiceSocket.s_instance.setOnPlayService_Receive(onSocketReceive_Play);
                PlayServiceSocket.s_instance.setOnPlayService_Close(onSocketClose_Play);
            }
        }

        m_laBaScript = m_laba.GetComponent<LaBaScript>();

        if (!OtherData.s_isMainInited)
        {
            OtherData.s_isMainInited = true;

            NetLoading.getInstance().Show();
        }
        else
        {
            refreshUI();
        }
    }

    void onInvokeStartMusic()
    {
        AudioScript.getAudioScript().playMusic_MainBg();
    }

    
    void Update ()
    {
	    if (BindPhoneScript.totalTime > 0)
	    {
	        if (nextTime <= Time.time)
	        {
	            BindPhoneScript.totalTime--;
	            nextTime = Time.time + 1;//到达一秒后加1
            }
	    }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitGameObject == null)
            {
                exitGameObject = ExitGamePanelScript.create();
            }
        }
    }

    void OnDestroy()
    {
//        LogicEnginerScript.Instance.m_socketUtil.stop();
        //PlayServiceSocket.getInstance().Stop();
    }

    // 检测服务器是否连接
    void checkNet()
    {
        if (!LogicEnginerScript.Instance.isConnecion())
        {
            //ToastScript.createToast("与Logic服务器断开连接");
            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
            NetErrorPanelScript.getInstance().setContentText("与逻辑服务器断开连接，请重新连接");
        }
        else if (!PlayServiceSocket.s_instance.isConnecion())
        {
            //ToastScript.createToast("与Play服务器断开连接");
            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Play);
            NetErrorPanelScript.getInstance().setContentText("与游戏服务器断开连接，请重新连接");
        }
        else
        {
            //ToastScript.createToast("两个服务器都成功连接");
        }
    }

    public void refreshUI()
    {
        // 头像
        m_headIcon.GetComponent<HeadIconScript>().setIcon(UserData.head);

        // 昵称
        UserAccount.text = UserData.name;

        // 金币
        UserGold.text = UserData.gold+"";
        MyGold.text = "我的金币:"+ UserData.gold;

        // 元宝
        UserYuanBao.text = UserData.yuanbao+""; 
        
        // 徽章
        UserMedal.text = UserData.medal+"";
        int vipLevel = VipUtil.GetVipLevel(UserData.rechargeVip);

        VipImage.sprite = Resources.Load<Sprite>("Sprites/Vip/user_vip_" + vipLevel);

        checkRedPoint();

        NetLoading.getInstance().Close();
    }


    public void showWaitMatchPanel(float time,string gameroomtype)
    {
        if (m_waitMatchPanel != null)
        {
            Destroy(m_waitMatchPanel);
            m_waitMatchPanel = null;
        }

        m_waitMatchPanel = WaitMatchPanelScript.create(gameroomtype);
        WaitMatchPanelScript script = m_waitMatchPanel.GetComponent<WaitMatchPanelScript>();
        script.setOnTimerEvent_TimeEnd(onTimerEvent_TimeEnd);
        script.start(time);
    }

    void onTimerEvent_TimeEnd()
    {
        Debug.Log("暂时没有匹配到玩家,请求匹配机器人");

        // 让服务端匹配机器人
        reqWaitMatchTimeOut();
    }

    public void onClickEnterXiuXianChang()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();
        //SceneManager.LoadScene("GameScene");

        m_button_xiuxianchang.transform.localScale = new Vector3(0,0,0);
        m_button_jingjichang.transform.localScale = new Vector3(0,0,0);
        m_xiuxianchang.transform.localScale = new Vector3(1, 1, 1);

        m_xiuxianchang.GetComponent<Animation>().Play("xiuxianchang_show");
    }

    public void onClickEnterJingJiChang()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();

        reqPVPRoom();
    }

    public void onClickJingDianChang()
    {
        GameLevelChoiceScript.create(GameLevelChoiceScript.GameChangCiType.GameChangCiType_jingdian);
    }

    public void onClickChaoDiChang()
    {
        GameLevelChoiceScript.create(GameLevelChoiceScript.GameChangCiType.GameChangCiType_chaodi);
    }

    public void onClickXiuXianChang_back()
    {
        m_button_xiuxianchang.transform.localScale = new Vector3(1, 1, 1);
        m_button_jingjichang.transform.localScale = new Vector3(1, 1, 1);
        m_xiuxianchang.transform.localScale = new Vector3(0, 0, 0);
    }

    public void OnClickHead()
    {
        UserInfoScript.create();
    }

    public void OnClickNotice()
    {
        NoticePanelScript.create();
    }

    public void OnClickSign()
    {
        WeeklySignScript.create();
    }

    public void OnClickInventory()
    {
        BagPanelScript.create(false);
    }

    public void OnClickShop()
    {
        ShopPanelScript.create(this);
    }

    public void OnClickEmail()
    {
        EmailPanelScript.create();
    }

    public void OnClickSetting()
    {
        SetScript.create(false);
    }

    public void OnClickKeFu()
    {
        KeFuPanelScript.create();
    }

    public void OnClickTask()
    {
        TaskPanelScript.create();
    }

    public void onClickLaBa()
    {
        LaBaPanelScript.create(this);
    }

    public void OnClickMedalHelp()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/MedalExplainPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
    }

    //-----------------------------------------------------------------------------

    public void reqWaitMatchTimeOut()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = TLJCommon.Consts.Tag_JingJiChang;
        jsonData["uid"] = UserData.uid;
        jsonData["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_WaitMatchTimeOut;

        PlayServiceSocket.s_instance.sendMessage(jsonData.ToJson());
    }

    public void reqPVPRoom()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = TLJCommon.Consts.Tag_GetPVPGameRoom;
        jsonData["uid"] = UserData.uid;
        string requestData = jsonData.ToJson();

        PlayServiceSocket.s_instance.sendMessage(requestData);
    }

    //-----------------------------------------------------------------------------
    public void onReceive_Main(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        string tag = (string)jd["tag"];

        // 有人使用喇叭
        if (tag.CompareTo(TLJCommon.Consts.Tag_Broadcast_LaBa) == 0)
        {
            string text = (string)jd["text"];

            m_laBaScript.addText(text);
        }
        // 强制离线
        else if(tag.CompareTo(TLJCommon.Consts.Tag_ForceOffline) == 0)
        {
            Destroy(LogicEnginerScript.Instance);
            Destroy(PlayServiceSocket.s_instance);

            GameObject obj = CommonExitPanelScript.create();
            obj.GetComponent<CommonExitPanelScript>().ButtonConfirm.onClick.RemoveAllListeners();
            obj.GetComponent<CommonExitPanelScript>().ButtonConfirm.onClick.AddListener(delegate ()
            {
                OtherData.s_isFromSetToLogin = true;
                SceneManager.LoadScene("LoginScene");
            });
        }
        else
        {
            Debug.Log("onReceive_Main：未知tag");
        }
    }

    //---------------------------------------------------------------------------------

    void onSocketReceive_Play(string data)
    {
        Debug.Log("Play:收到服务器消息:" + data);

        JsonData jd = JsonMapper.ToObject(data);
        string tag = (string)jd["tag"];

        if (tag.CompareTo(TLJCommon.Consts.Tag_JingJiChang) == 0)
        {
            int playAction = (int)jd["playAction"];

            switch (playAction)
            {
                // 加入游戏
                case (int)TLJCommon.Consts.PlayAction.PlayAction_JoinGame:
                {
                    doTask_PlayAction_JoinGame(data);
                }
                break;
                
                // 退出游戏
                case (int)TLJCommon.Consts.PlayAction.PlayAction_ExitPVP:
                {
                    doTask_PlayAction_ExitPVP(data);
                }
                break;

                // 开始游戏
                case (int)TLJCommon.Consts.PlayAction.PlayAction_StartGame:
                {
                    doTask_PlayAction_StartGame(data);
                }
                break;
            }
        }
        // 获取pvp场次信息
        else if (tag.CompareTo(TLJCommon.Consts.Tag_GetPVPGameRoom) == 0)
        {
            PVPGameRoomDataScript.getInstance().initJson(data);

            PVPChoiceScript.create();
        }
    }

    void doTask_PlayAction_JoinGame(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];

        switch (code)
        {
            case (int)TLJCommon.Consts.Code.Code_OK:
                {
                    int roomId = (int)jd["roomId"];
                    string gameroomtype = (string)jd["gameroomtype"].ToString();

                    ToastScript.createToast("报名成功");

                    showWaitMatchPanel(10,gameroomtype);

                    // 扣除报名费
                    {
                        string baomingfei = PVPGameRoomDataScript.getInstance().getDataByRoomType(gameroomtype).baomingfei;
                        if (baomingfei.CompareTo("0") != 0)
                        {
                            List<string> tempList = new List<string>();
                            CommonUtil.splitStr(baomingfei, tempList, ':');
                            GameUtil.changeData(int.Parse(tempList[0]), -int.Parse(tempList[1]));
                        }
                    }
                }
                break;

            case (int)TLJCommon.Consts.Code.Code_CommonFail:
                {
                    ToastScript.createToast("您已加入其它房间，无法开始新游戏");
                }
                break;
        }
    }

    void doTask_PlayAction_ExitPVP(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];

        switch (code)
        {
            case (int)TLJCommon.Consts.Code.Code_OK:
            {
                int roomId = (int)jd["roomId"];
                string gameroomtype = jd["gameroomtype"].ToString();
                if (gameroomtype.CompareTo(TLJCommon.Consts.GameRoomType_PVP_HuaFei_8) == 0)
                {
                        ToastScript.createToast("退赛成功");
                    }
                else
                {
                    ToastScript.createToast("退赛成功,请到邮箱领取报名费");

                    // 报名费返还通过邮件
                    LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().OnRequest();
                }
            }
            break;

            case (int)TLJCommon.Consts.Code.Code_CommonFail:
            {
                ToastScript.createToast("退赛失败，当前并没有加入房间");
            }
            break;
        }
    }

    void doTask_PlayAction_StartGame(string data)
    {
        GameData.getInstance().m_startGameJsonData = data;
        SceneManager.LoadScene("GameScene");
    }

    public void checkRedPoint()
    {
        // 活动
        {
            bool isShowRedPoint = false;
            for (int i = 0; i < NoticelDataScript.getInstance().getNoticeDataList().Count; i++)
            {
                if (NoticelDataScript.getInstance().getNoticeDataList()[i].state == 0)
                {
                    isShowRedPoint = true;
                    break;
                }
            }

            if (isShowRedPoint)
            {
                m_notice_redPoint.transform.localScale = new Vector3(1,1,1);
            }
            else
            {
                m_notice_redPoint.transform.localScale = new Vector3(0, 0, 0);
            }
        }

        // 任务
        {
            bool isShowRedPoint = false;
            for (int i = 0; i < TaskDataScript.getInstance().getTaskDataList().Count; i++)
            {
                if ((TaskDataScript.getInstance().getTaskDataList()[i].progress == TaskDataScript.getInstance().getTaskDataList()[i].target) &&
                    (TaskDataScript.getInstance().getTaskDataList()[i].isover == 0))
                {
                    isShowRedPoint = true;
                    break;
                }
            }

            if (isShowRedPoint)
            {
                m_task_redPoint.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                m_task_redPoint.transform.localScale = new Vector3(0, 0, 0);
            }
        }

        // 签到
        {
            if (!SignData.IsSign)
            {
                m_sign_redPoint.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                m_sign_redPoint.transform.localScale = new Vector3(0, 0, 0);
            }
        }

        // 邮件
        {
            bool isShowRedPoint = false;
            for (int i = 0; i < UserMailData.getInstance().getUserMailDataList().Count; i++)
            {
                if (UserMailData.getInstance().getUserMailDataList()[i].m_state == 0)
                {
                    isShowRedPoint = true;
                    break;
                }
            }

            if (isShowRedPoint)
            {
                m_mail_redPoint.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                m_mail_redPoint.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

    //-------------------------------------Logic服务器相关----------------------------------------

    void onSocketConnect_Logic(bool result)
    {
        NetLoading.getInstance().Close();

        if (result)
        {
            //Debug.Log("连接服务器成功");

            //ToastScript.createToast("连接Logic服务器成功");

            NetLoading.getInstance().Close();
            NetErrorPanelScript.getInstance().Close();

            {
                LogicEnginerScript.Instance.GetComponent<GetUserInfoRequest>().OnRequest();
                LogicEnginerScript.Instance.GetComponent<GetRankRequest>().OnRequest();
                LogicEnginerScript.Instance.GetComponent<GetSignRecordRequest>().OnRequest();
                LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().CallBack = onReceive_GetUserBag;
                LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().OnRequest();
                LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().OnRequest();
                LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().OnRequest();
            }

            // 检测服务器是否连接
            checkNet();
        }
        else
        {
            //Debug.Log("连接服务器失败，尝试重新连接");

            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
            NetErrorPanelScript.getInstance().setContentText("连接服务器失败，请重新连接");
        }
    }

    public static void onReceive_GetUserBag(string data)
    {
        {
            print("处理背包回调");
            JsonData jsonData = JsonMapper.ToObject(data);
            var code = (int)jsonData["code"];
            if (code == (int)Consts.Code.Code_OK)
            {
                UserData.propData = JsonMapper.ToObject<List<UserPropData>>(jsonData["prop_list"].ToString());
                for (int i = 0; i < PropData.getInstance().getPropInfoList().Count; i++)
                {
                    PropInfo propInfo = PropData.getInstance().getPropInfoList()[i];
                    for (int j = 0; j < UserData.propData.Count; j++)
                    {
                        UserPropData userPropData = UserData.propData[j];
                        if (propInfo.m_id == userPropData.prop_id)
                        {
                            userPropData.prop_icon = propInfo.m_icon;
                            userPropData.prop_name = propInfo.m_name;
                        }
                    }
                }
            }
            else
            {
                ToastScript.createToast("用户背包数据错误");
                return;
            }
        }
    }


    void onSocketClose_Logic()
    {
        //Debug.Log("被动与服务器断开连接,尝试重新连接");

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
        NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");
    }

    void onSocketStop_Logic()
    {
        //Debug.Log("主动与服务器断开连接");

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
        NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");
    }

    // 点击网络断开弹框中的重连按钮
    void onClickChongLian_Logic()
    {
        NetLoading.getInstance().Show();
        NetErrorPanelScript.getInstance().Close();
        LogicEnginerScript.Instance.startConnect();
    }


    //-------------------------------------Play服务器相关----------------------------------------

    void onSocketConnect_Play(bool result)
    {
        NetLoading.getInstance().Close();

        if (result)
        {
            //Debug.Log("连接服务器成功");

            //ToastScript.createToast("连接Play服务器成功");

            NetLoading.getInstance().Close();
            NetErrorPanelScript.getInstance().Close();

            // 检测服务器是否连接
            checkNet();
        }
        else
        {
            //Debug.Log("连接服务器失败，尝试重新连接");

            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Play);
            NetErrorPanelScript.getInstance().setContentText("连接逻辑服务器失败，请重新连接");
        }
    }

    void onSocketClose_Play()
    {
        //Debug.Log("被动与服务器断开连接,尝试重新连接");

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Play);
        NetErrorPanelScript.getInstance().setContentText("与游戏服务器断开连接，请重新连接");
    }

    void onSocketStop_Play()
    {
        //Debug.Log("主动与服务器断开连接");

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Play);
        NetErrorPanelScript.getInstance().setContentText("与游戏服务器断开连接，请重新连接");
    }

    // 点击网络断开弹框中的重连按钮
    void onClickChongLian_Play()
    {
        NetLoading.getInstance().Show();
        NetErrorPanelScript.getInstance().Close();
        PlayServiceSocket.s_instance.startConnect();
    }

    //--------------------------------------------------------------------------------------------------
    void onPauseCallBack()
    {
        LogicEnginerScript.Instance.Stop();
        PlayServiceSocket.s_instance.Stop();
    }

    void onResumeCallBack()
    {
        if (m_waitMatchPanel != null)
        {
            Destroy(m_waitMatchPanel);
            m_waitMatchPanel = null;
        }

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClickChongLian_Logic);
        NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");
    }
}
