using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailDetailScript : MonoBehaviour {

    public Text m_content;
    MailData m_mailData = null;

    public static GameObject create(int email_id)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/MailDetailPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

        obj.GetComponent<MailDetailScript>().setEmailId(email_id);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setEmailId(int email_id)
    {
        for (int i = 0; i < UserMailData.getInstance().getUserMailDataList().Count; i++)
        {
            if (UserMailData.getInstance().getUserMailDataList()[i].m_email_id == email_id)
            {
                m_mailData = UserMailData.getInstance().getUserMailDataList()[i];
                m_content.text = m_mailData.m_content;

                break;
            }
        }
    }

    public void onClickDelete()
    {

    }
}
