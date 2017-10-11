using LitJson;
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
        JsonData jd = JsonMapper.ToObject(json);
        
        for (int i = 0; i < jd["mailData"].Count; i++)
        {
            MailData mailData = new MailData();
            mailData.m_email_id = (int)jd["mailData"][i]["email_id"];
            mailData.m_title = jd["mailData"][i]["title"].ToString();
            mailData.m_content = jd["mailData"][i]["content"].ToString();
            mailData.m_state = (int)jd["mailData"][i]["state"];
            mailData.m_time = jd["mailData"][i]["time"].ToString();

            // 奖励
            {
                string reward_str = jd["mailData"][i]["reward"].ToString();

                if (reward_str.CompareTo("") != 0)
                {
                    List<string> list_str1 = new List<string>();
                    CommonUtil.splitStr(reward_str, list_str1, ';');

                    for (int j = 0; j < list_str1.Count; j++)
                    {
                        List<string> list_str2 = new List<string>();
                        CommonUtil.splitStr(list_str1[j], list_str2, ':');

                        mailData.m_rewardList.Add(new CommonClass.Reward(int.Parse(list_str2[0]), int.Parse(list_str2[1])));
                    }
                }
            }

            m_myMailDataList.Add(mailData);
        }
    }

    public List<MailData> getUserMailDataList()
    {
        return m_myMailDataList;
    }

    public void addMailData(MailData mailData)
    {
        m_myMailDataList.Add(mailData);
    }

    // 邮件设为已读
    public void setMailReaded(int email_id)
    {
        for (int i = 0; i < m_myMailDataList.Count; i++)
        {
            if (m_myMailDataList[i].m_email_id == email_id)
            {
                m_myMailDataList[i].m_state = 1;
                break;
            }
        }
    }

    // 所有邮件设为已读
    public void setAllMailReaded()
    {
        for (int i = 0; i < m_myMailDataList.Count; i++)
        {
            if (m_myMailDataList[i].m_state == 0)
            {
                m_myMailDataList[i].m_state = 1;
            }
        }
    }

    // 删除邮件
    public void deleteMail(int email_id)
    {
        for (int i = 0; i < m_myMailDataList.Count; i++)
        {
            if (m_myMailDataList[i].m_email_id == email_id)
            {
                m_myMailDataList.RemoveAt(i);
                break;
            }
        }
    }

    // 删除所有邮件:必须是已读的
    public void deleteAllMail()
    {
        for (int i = m_myMailDataList.Count - 1; i >= 0; i--)
        {
            if (m_myMailDataList[i].m_state == 1)
            {
                m_myMailDataList.RemoveAt(i);
            }
        }
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