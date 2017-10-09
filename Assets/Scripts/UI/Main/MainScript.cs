using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour {

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
        SceneManager.LoadScene("GameScene");
    }

    public void onClickEnterJingJiChang()
    {
        AudioScript.getAudioScript().playSound_ButtonClick();
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickHead()
    {
        //AudioScript.getAudioScript().playSound_ButtonClick();
        GameObject userInfo = Resources.Load<GameObject>("Prefabs/UI/Panel/UserInfoPanel");
        GameObject.Instantiate(userInfo,this.transform);
        
    }

    public void OnClickNotice()
    {
        //AudioScript.getAudioScript().playSound_ButtonClick();
        GameObject NoticePanel = Resources.Load<GameObject>("Prefabs/UI/Panel/NoticePanel");
        GameObject.Instantiate(NoticePanel, this.transform);

    }

    public void OnClickSign()
    {
        //AudioScript.getAudioScript().playSound_ButtonClick();
        GameObject WeeklySignPanel = Resources.Load<GameObject>("Prefabs/UI/Panel/WeeklySignPanel");
        GameObject.Instantiate(WeeklySignPanel, this.transform);
    }

    public void OnClickInventory()
    {
        //AudioScript.getAudioScript().playSound_ButtonClick();
        GameObject InventoryPanel = Resources.Load<GameObject>("Prefabs/UI/Panel/InventoryPanel");
        GameObject.Instantiate(InventoryPanel, this.transform);
    }

    public void OnClickShop()
    {
        //AudioScript.getAudioScript().playSound_ButtonClick();
        GameObject ShopPanel = Resources.Load<GameObject>("Prefabs/UI/Panel/ShopPanel");
        GameObject.Instantiate(ShopPanel, this.transform);
    }

    public void OnClickEmail()
    {
        //AudioScript.getAudioScript().playSound_ButtonClick();
        GameObject EmailPanel = Resources.Load<GameObject>("Prefabs/UI/Panel/EmailPanel");
        GameObject.Instantiate(EmailPanel, this.transform);
    }
    public void OnClickSetting()
    {
        //AudioScript.getAudioScript().playSound_ButtonClick();
        GameObject SettingPanel = Resources.Load<GameObject>("Prefabs/UI/Panel/SettingPanel");
        GameObject.Instantiate(SettingPanel, this.transform);
    }

}
