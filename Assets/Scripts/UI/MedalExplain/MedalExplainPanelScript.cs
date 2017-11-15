using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalExplainPanelScript : MonoBehaviour {

    public Button m_button_setSecondPsw;

    // Use this for initialization
    void Start ()
    {
        if (UserData.isSetSecondPsw)
        {
            m_button_setSecondPsw.transform.Find("Text").GetComponent<Text>().text = "已设置";
            Destroy(m_button_setSecondPsw.GetComponent<Button>());
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void onClickSetPsw()
    {
        SetSecondPswPanelScript.create();
        Destroy(gameObject);
    }
}
