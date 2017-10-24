﻿using Boo.Lang;
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
        m_laBaScript = m_laba.GetComponent<LaBaScript>();
        LogicEnginerScript.Instance.GetComponent<MainRequest>().CallBack = onReceive_Main;

        {
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
        LogicEnginerScript.Instance.m_socketUtil.stop();
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
        //SceneManager.LoadScene("GameScene");

        PVPChoiceScript.create();
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
    }

    //-----------------------------------------------------------------------------
}
