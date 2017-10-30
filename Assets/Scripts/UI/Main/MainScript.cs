using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public Button m_button_xiuxianchang;
    public Button m_button_jingjichang;
    public GameObject m_xiuxianchang;
    public Text UserAccount;
    public Text UserGold;
    public Text MyGold;
    public Text UserYuanBao;

    public GameObject m_laba;
    public GameObject m_headIcon;

    LaBaScript m_laBaScript;

    //发送验证码的倒计时
    private float nextTime = 1;//一秒之后执行

    //public GameObject m_loadingPanel;

    // Use this for initialization
    void Start ()
	{
	    // 游戏打牌服务器
	    {
	        if (LogicEnginerScript.Instance == null)
	        {
	            LogicEnginerScript.create();
	        }
	        else
	        {
	            LogicEnginerScript.Instance.GetComponent<GetUserInfoRequest>().OnRequest();
                LogicEnginerScript.Instance.GetComponent<GetRankRequest>().OnRequest();
                LogicEnginerScript.Instance.GetComponent<GetSignRecordRequest>().OnRequest();
	           
            }
	    }

        m_laBaScript = m_laba.GetComponent<LaBaScript>();
        LogicEnginerScript.Instance.GetComponent<MainRequest>().CallBack = onReceive_Main;

	    // 游戏打牌服务器
	    {
	        if (PlayServiceSocket.s_instance == null)
	        {
	            PlayServiceSocket.create();
	        }

	        PlayServiceSocket.s_instance.setOnPlayService_Receive(onSocketReceive_Play);
	        PlayServiceSocket.s_instance.startConnect();
	    }

        if (!OtherData.s_isMainInited)
        {
            OtherData.s_isMainInited = true;

            AudioScript.getAudioScript().playMusic_GameBg();
            //m_loadingPanel = LoadingScript.create();
        }
        else
        {
            refreshUI();
        }
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
    }

    void OnDestroy()
    {
//        LogicEnginerScript.Instance.m_socketUtil.stop();
        //PlayServiceSocket.getInstance().Stop();
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

        //// 删除loading界面
        //if (m_loadingPanel.transform.IsChildOf(GameObject.Find("Canvas").transform))
        //{
        //    Destroy(m_loadingPanel);
        //}
    }


    public void showWaitMatchPanel(float time)
    {
        WaitMatchPanelScript script = WaitMatchPanelScript.create().GetComponent<WaitMatchPanelScript>();
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

        {
            LogicEnginerScript.Instance.GetComponent<GetPVPRoomRequest>().OnRequest();
        }
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
        BagPanelScript.create(true);
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
        SetScript.create();
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

    //-----------------------------------------------------------------------------

    public void reqWaitMatchTimeOut()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = TLJCommon.Consts.Tag_JingJiChang;
        jsonData["uid"] = UserData.uid;
        jsonData["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_WaitMatchTimeOut;

        PlayServiceSocket.s_instance.sendMessage(jsonData.ToJson());
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
                case (int)TLJCommon.Consts.PlayAction.PlayAction_JoinGame:
                {
                    doTask_PlayAction_JoinGame(data);
                }
                break;
                
                // 退出游戏
                case (int)TLJCommon.Consts.PlayAction.PlayAction_ExitGame:
                {
                    doTask_PlayAction_ExitGame(data);
                }
                break;

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
                    ToastScript.createToast("报名成功");

                    showWaitMatchPanel(10);

                    // 扣除报名费
                    {
                        string gameroomtype = (string)jd["gameroomtype"].ToString();
                        string baomingfei = PVPGameRoomDataScript.getInstance().getDataByRoomType(gameroomtype).baomingfei;
                        if (baomingfei.CompareTo("0") != 0)
                        {
                            List<string> tempList = new List<string>();
                            CommonUtil.splitStr(baomingfei, tempList, ':');
                            GameUtil.changeData(int.Parse(tempList[0]), -int.Parse(tempList[1]));

                            refreshUI();
                        }
                    }
                }
                break;

            case (int)TLJCommon.Consts.Code.Code_CommonFail:
                {
                    ToastScript.createToast("加入房间失败，已经加入房间");
                }
                break;
        }
    }

    void doTask_PlayAction_ExitGame(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];

        switch (code)
        {
            case (int)TLJCommon.Consts.Code.Code_OK:
            {
                int roomId = (int)jd["roomId"];
                ToastScript.createToast("退赛成功");
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

    //-----------------------------------------------------------------------------
}
