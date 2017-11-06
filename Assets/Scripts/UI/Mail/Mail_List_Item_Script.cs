using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mail_List_Item_Script : MonoBehaviour {

    public EmailPanelScript m_parentScript;

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

    public MailData getMailData()
    {
        return m_mailData;
    }

    public void onClickItem()
    {
        // 未领取的邮件先请求服务器
        if (m_mailData.m_state == 0)
        {
            LogicEnginerScript.Instance.GetComponent<ReadEmailRequest>().setEmailId(int.Parse(gameObject.transform.name));
            LogicEnginerScript.Instance.GetComponent<ReadEmailRequest>().CallBack = onReceive_ReadMail;
            LogicEnginerScript.Instance.GetComponent<ReadEmailRequest>().OnRequest();
        }
        // 已领取的直接显示
        else
        {
            MailDetailScript.create(int.Parse(gameObject.transform.name), m_parentScript);
        }
    }

    public void onReceive_ReadMail(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];
        int email_id = (int)jd["email_id"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            m_parentScript.setMailReaded(email_id);

            //ShowRewardPanelScript.create().GetComponent<ShowRewardPanelScript>().setData(m_mailData.m_reward);
            ShowRewardPanelScript.Show(m_mailData.m_reward);
        }

        MailDetailScript.create(int.Parse(gameObject.transform.name), m_parentScript);
    }
}
