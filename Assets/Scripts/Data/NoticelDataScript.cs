using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NoticelDataScript
{
    static NoticelDataScript s_noticeData = null;

    List<NoticeData> m_noticeDataList = new List<NoticeData>();

    public static NoticelDataScript getInstance()
    {
        if (s_noticeData == null)
        {
            s_noticeData = new NoticelDataScript();
        }

        return s_noticeData;
    }

    public void initJson(string json)
    {
        m_noticeDataList.Clear();
        
        JsonData jsonData = JsonMapper.ToObject(json);
        m_noticeDataList = JsonMapper.ToObject<List<NoticeData>>(jsonData["notice_list"].ToString());
    }

    public List<NoticeData> getNoticeDataList()
    {
        return m_noticeDataList;
    }

    public NoticeData getNoticeDataById(int notice_id)
    {
        NoticeData noticeData = null;
        for (int i = 0; i < m_noticeDataList.Count; i++)
        {
            if (m_noticeDataList[i].notice_id == notice_id)
            {
                noticeData = m_noticeDataList[i];
                break;
            }
        }

        return noticeData;
    }

    // 设为已读
    public void setNoticeReaded(int notice_id)
    {
        for (int i = 0; i < m_noticeDataList.Count; i++)
        {
            if (m_noticeDataList[i].notice_id == notice_id)
            {
                m_noticeDataList[i].state = 1;
                break;
            }
        }
    }
}

public class NoticeData
{
    public int notice_id;
    public string title = "";
    public string title_limian = "";
    public string content = "";
    public int type = 0;        // 0:活动    1:公告
    public int state = 0;       // 0:未读    1:已读
    public string start_time;
    public string end_time;
    public string from;
}