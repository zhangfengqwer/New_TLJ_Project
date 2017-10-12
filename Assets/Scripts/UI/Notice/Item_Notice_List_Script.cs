using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Notice_List_Script : MonoBehaviour {

    public NoticePanelScript m_parentScript;

    public Text m_text_title;
    public Text m_text_time;
    public Image m_redPoint;

    public Button m_button_item;

    NoticeData m_noticeData;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setNoticeData(NoticeData noticeData)
    {
        m_noticeData = noticeData;

        {
            m_text_title.text = m_noticeData.m_title;
            m_text_time.text = m_noticeData.m_time;

            // 已读
            if (m_noticeData.m_state == 1)
            {
                m_redPoint.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

    public NoticeData getNoticeData()
    {
        return m_noticeData;
    }

    public void onClickItem()
    {
        Debug.Log("onClickItem");

        int notice_id = int.Parse(gameObject.transform.name);
        m_parentScript.setNoticeReaded(notice_id);

        //LogicEnginerScript.Instance.GetComponent<ReadEmailRequest>().setEmailId(int.Parse(gameObject.transform.name));
        //LogicEnginerScript.Instance.GetComponent<ReadEmailRequest>().CallBack = onReceive_ReadMail;
        //LogicEnginerScript.Instance.GetComponent<ReadEmailRequest>().OnRequest();
    }

    public void onReceive_ReadMail(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];
        int notice_id = (int)jd["notice_id"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            m_parentScript.setNoticeReaded(notice_id);
        }

        //MailDetailScript.create(int.Parse(gameObject.transform.name), m_parentScript);
    }
}
