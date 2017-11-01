using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameNetErrorPanelScript : MonoBehaviour {

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/GameNetErrorPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_High").transform);

        return obj;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickBack()
    {
        SceneManager.LoadScene("MainScene");
    }
}
