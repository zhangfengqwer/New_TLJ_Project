using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
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
        GameObject userInfo = Resources.Load<GameObject>("Prefabs/UI/UserInfoPanel");
        GameObject.Instantiate(userInfo,this.transform);
        
    }

    public void OnClickSign()
    {
        GameObject WeeklySignPanel = Resources.Load<GameObject>("Prefabs/UI/WeeklySignPanel");
        GameObject.Instantiate(WeeklySignPanel, this.transform);
    }

    public void OnClickInventory()
    {
        GameObject WeeklySignPanel = Resources.Load<GameObject>("Prefabs/UI/InventoryPanel");
        GameObject.Instantiate(WeeklySignPanel, this.transform);
    }
}
