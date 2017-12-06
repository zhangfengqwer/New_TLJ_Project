using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeHeadButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        gameObject.GetComponent<Image>().color = Color.gray;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickHeadItem()
    {
        ChangeHeadPanelScript.s_instance.m_choiceHead = int.Parse(gameObject.transform.name);

        gameObject.GetComponent<Image>().color = Color.white;

        for (int i = 0; i < 16; i++)
        {
            if (gameObject.transform.parent.Find((i + 1).ToString()).name != gameObject.transform.name)
            {
                gameObject.transform.parent.GetChild(i).GetComponent<Image>().color = Color.gray;
            }
        }
    }
}
