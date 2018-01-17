using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeDetailScript : MonoBehaviour {

    public NoticePanelScript m_parentScript;

    public Text m_title;
    public Text m_content;
    public Text m_time;
    public Text m_from;

    public NoticeData m_noticeData = null;

    public static GameObject create(int notice_id, NoticePanelScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/NoticeDetailPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<NoticeDetailScript>().setNoticeId(notice_id);
        obj.GetComponent<NoticeDetailScript>().m_parentScript = parentScript;

        return obj;
    }

    // Use this for initialization
    void Start()
    {
        OtherData.s_noticeDetailScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NoticeDetailScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NoticeDetailScript_hotfix", "Start", null, null);
            return;
        }
    }

    public void setNoticeId(int notice_id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NoticeDetailScript_hotfix", "setNoticeId"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NoticeDetailScript_hotfix", "setNoticeId", null, notice_id);
            return;
        }

        m_noticeData = NoticelDataScript.getInstance().getNoticeDataById(notice_id);
        m_title.text = m_noticeData.title_limian;

        string content = m_noticeData.content.Replace("^", "\r\n");
        LogUtil.Log(content);
        m_content.text = content;

        //m_time.text = m_noticeData.start_time;
        m_from.text = m_noticeData.from;
    }
}
