using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
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
        SceneManager.LoadScene("GameScene");
    }

    public void onClickEnterJingJiChang()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickHead()
    {
        GameObject userInfo = Resources.Load<GameObject>("Prefabs/UI/Panel/UserInfoPanel");
        GameObject.Instantiate(userInfo,this.transform);
        
    }

    public void OnClickNotice()
    {
        GameObject NoticePanel = Resources.Load<GameObject>("Prefabs/UI/Panel/NoticePanel");
        GameObject.Instantiate(NoticePanel, this.transform);

    }

    public void OnClickSign()
    {
        GameObject WeeklySignPanel = Resources.Load<GameObject>("Prefabs/UI/Panel/WeeklySignPanel");
        GameObject.Instantiate(WeeklySignPanel, this.transform);
    }

    public void OnClickInventory()
    {
        GameObject InventoryPanel = Resources.Load<GameObject>("Prefabs/UI/Panel/InventoryPanel");
        GameObject.Instantiate(InventoryPanel, this.transform);
    }
    public void OnClickShop()
    {
        GameObject ShopPanel = Resources.Load<GameObject>("Prefabs/UI/Panel/ShopPanel");
        GameObject.Instantiate(ShopPanel, this.transform);
    }

    public void OnClickEmail()
    {
        GameObject EmailPanel = Resources.Load<GameObject>("Prefabs/UI/Panel/EmailPanel");
        GameObject.Instantiate(EmailPanel, this.transform);
    }
    public void OnClickSetting()
    {
        GameObject SettingPanel = Resources.Load<GameObject>("Prefabs/UI/Panel/SettingPanel");
        GameObject.Instantiate(SettingPanel, this.transform);
    }

}
