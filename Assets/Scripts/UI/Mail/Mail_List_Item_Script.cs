using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mail_List_Item_Script : MonoBehaviour {

    public Text m_text_title;
    public Text m_text_time;

    public Button m_button_item;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setTitle(string str)
    {
        m_text_title.text = str;
    }

    public void setTime(string str)
    {
        m_text_time.text = str;
    }

    public void onClickItem()
    {

    }
}
