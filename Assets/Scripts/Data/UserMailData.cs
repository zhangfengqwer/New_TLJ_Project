using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMailData {

    static UserMailData s_userMailData = null;

    List<MailData> m_myMailDataList = new List<MailData>();

    public static UserMailData getInstance()
    {
        if (s_userMailData == null)
        {
            s_userMailData = new UserMailData();
        }

        return s_userMailData;
    }

    public void initJson(string json)
    {

    }

    public List<MailData> getUserMailDataList()
    {
        return m_myMailDataList;
    }

    public void addMailData(MailData mailData)
    {
        m_myMailDataList.Add(mailData);
    }

    // 清空数据
    public static void clearData()
    {
        UserMailData.clearData();
    }
}

public class MailData
{
    public int m_email_id = 0;
    public string m_title = "";
    public string m_content = "";
    public int m_state = 0;
    public string m_time = "";
    public List<CommonClass.Reward> m_rewardList = new List<CommonClass.Reward>();
}