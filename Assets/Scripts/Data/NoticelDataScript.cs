using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticelDataScript
{
    static NoticelDataScript s_noticeData = null;

    List<NoticeData> m_huodongDataList = new List<NoticeData>();
    List<NoticeData> m_gonggaoDataList = new List<NoticeData>();

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
        m_huodongDataList.Clear();
        m_gonggaoDataList.Clear();

        //JsonData jd = JsonMapper.ToObject(json);

        //for (int i = 0; i < jd["NoticeData"].Count; i++)
        //{
        //    NoticeData noticeData = new NoticeData();
        //    noticeData.m_notice_id = (int)jd["NoticeData"][i]["notice_id"];
        //    noticeData.m_title = jd["NoticeData"][i]["title"].ToString();
        //    noticeData.m_content = jd["NoticeData"][i]["content"].ToString();
        //    noticeData.m_state = (int)jd["NoticeData"][i]["state"];
        //    noticeData.m_type = (int)jd["NoticeData"][i]["type"];
        //    noticeData.m_time = jd["NoticeData"][i]["time"].ToString();

        //    if (noticeData.m_type == 0)
        //    {
        //        m_huodongDataList.Add(noticeData);
        //    }
        //    else
        //    {
        //        m_gonggaoDataList.Add(noticeData);
        //    }
            
        //}

        for (int i = 0; i < 10; i++)
        {
            NoticeData noticeData = new NoticeData();
            noticeData.m_notice_id = i;
            noticeData.m_title = "标题" + i;
            noticeData.m_content = "内容" + i;
            noticeData.m_state = 0;
            noticeData.m_type = 0;
            noticeData.m_time = "2017-10-12";
            
            m_huodongDataList.Add(noticeData);
        }

        for (int i = 10; i < 18; i++)
        {
            NoticeData noticeData = new NoticeData();
            noticeData.m_notice_id = i;
            noticeData.m_title = "标题" + i;
            noticeData.m_content = "内容" + i;
            noticeData.m_state = 0;
            noticeData.m_type = 1;
            noticeData.m_time = "2017-10-12";

            m_gonggaoDataList.Add(noticeData);
        }
    }

    public List<NoticeData> getHuoDongDataList()
    {
        return m_huodongDataList;
    }

    public List<NoticeData> getGongGaoDataList()
    {
        return m_gonggaoDataList;
    }

    // 设为已读
    public void setNoticeReaded(int notice_id)
    {
        for (int i = 0; i < m_huodongDataList.Count; i++)
        {
            if (m_huodongDataList[i].m_notice_id == notice_id)
            {
                m_huodongDataList[i].m_state = 1;
                break;
            }
        }

        for (int i = 0; i < m_gonggaoDataList.Count; i++)
        {
            if (m_gonggaoDataList[i].m_notice_id == notice_id)
            {
                m_gonggaoDataList[i].m_state = 1;
                break;
            }
        }
    }

    // 清空数据
    public static void clearData()
    {
        UserMailData.clearData();
    }
}

public class NoticeData
{
    public int m_notice_id = 0;
    public string m_title = "";
    public string m_content = "";
    public string m_time = "";
    public int m_type = 0;         // 0:活动    1:公告
    public int m_state = 0;
}