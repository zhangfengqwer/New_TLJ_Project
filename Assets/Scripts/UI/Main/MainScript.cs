using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScript : MonoBehaviour {

    public Button m_button_xiuxianchang;
    public Button m_button_jingjichang;
    public GameObject m_xiuxianchang;

    // Use this for initialization
    void Start ()
	{
        AudioScript.getAudioScript().playMusic_GameBg();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy()
    {
        SocketUtil.getInstance().stop();
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
        SceneManager.LoadScene("GameScene");
    }

    public void onClickChaoDiChang()
    {
        SceneManager.LoadScene("GameScene");
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
        BagPanelScript.create();
    }

    public void OnClickShop()
    {
        ShopPanelScript.create();
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

}
