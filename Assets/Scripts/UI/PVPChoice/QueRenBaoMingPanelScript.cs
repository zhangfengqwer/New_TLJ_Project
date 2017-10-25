using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueRenBaoMingPanelScript : MonoBehaviour {

    public Text m_text_roomname;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/Panel/QueRenBaoMingPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        //obj.GetComponent<QueRenBaoMingPanelScript>().setGameChangCiType(gameChangCiType);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickBaoMing()
    {

    }
}
