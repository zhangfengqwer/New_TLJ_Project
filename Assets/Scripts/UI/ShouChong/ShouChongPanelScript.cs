using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShouChongPanelScript : MonoBehaviour {

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ShouChongPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickChongZhi()
    {
        ShopPanelScript.create(2);
        Destroy(gameObject);
    }
}
