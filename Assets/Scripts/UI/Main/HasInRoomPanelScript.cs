using LitJson;
using System.Collections;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

public class HasInRoomPanelScript : MonoBehaviour {

    public delegate void OnClickButton();
    OnClickButton m_OnClickButton = null;

    public Text m_content;

    public static GameObject create(string text, OnClickButton onClickButton)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/HasInRoomPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<HasInRoomPanelScript>().m_content.text = text;
        obj.GetComponent<HasInRoomPanelScript>().m_OnClickButton = onClickButton;

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickOK()
    {
        if (m_OnClickButton != null)
        {
            m_OnClickButton();
        }
    }
}
