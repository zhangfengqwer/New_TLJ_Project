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

    public void OnHeadClick()
    {
        GameObject userInfo = Resources.Load<GameObject>("Prefabs/UI/userInfoPanel");
        GameObject.Instantiate(userInfo,this.transform);
        
    }
}
