using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mail_List_Item_Script : MonoBehaviour {

    public Text m_text_title;
    public Text m_text_time;
    public Image m_redPoint;

    public Button m_button_item;

    MailData m_mailData;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setMailData(MailData mailData)
    {
        m_mailData = mailData;

        {
            m_text_title.text = m_mailData.m_title;
            m_text_time.text = m_mailData.m_time;

            // 已读
            if (m_mailData.m_state == 1)
            {
                m_redPoint.transform.localScale = new Vector3(0,0,0);
            }
        }
    }

    public void onClickItem()
    {
        //LogicEnginerScript.Instance.GetComponent<ReadEmailRequest>().setEmailId(int.Parse(gameObject.transform.name));
        //LogicEnginerScript.Instance.GetComponent<ReadEmailRequest>().OnRequest();

        JsonData jsonData = new JsonData();
        jsonData["tag"] = TLJCommon.Consts.Tag_ReadMail;
        jsonData["uid"] = UserData.uid;
        jsonData["email_id"] = 7;
        string requestData = jsonData.ToJson();
        SocketUtil.getInstance().sendMessage(requestData);

        //MailDetailScript.create(int.Parse(gameObject.transform.name));
    }
}
