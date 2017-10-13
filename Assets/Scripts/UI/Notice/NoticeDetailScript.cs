using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeDetailScript : MonoBehaviour {

    public NoticePanelScript m_parentScript;

    public Text m_content;
    NoticeData m_noticeData = null;

    public static GameObject create(int notice_id, NoticePanelScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/NoticeDetailPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

        obj.GetComponent<NoticeDetailScript>().setNoticeId(notice_id);
        obj.GetComponent<NoticeDetailScript>().m_parentScript = parentScript;

        return obj;
    }

    // Use this for initialization
    void Start()
    {

    }

    public void setNoticeId(int notice_id)
    {
        m_noticeData = NoticelDataScript.getInstance().getNoticeDataById(notice_id);
        m_content.text = m_noticeData.content;
    }
}
