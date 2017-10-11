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

        string uid = jd["userInfo"]["uid"].ToString();
        string name = jd["userInfo"]["name"].ToString();
        int goldNum = (int)(jd["userInfo"]["goldNum"]);

        UserDataScript.getInstance().getUserInfo().m_uid = uid;
        UserDataScript.getInstance().getUserInfo().m_name = name;
        UserDataScript.getInstance().getUserInfo().m_goldNum = goldNum;
        UserData.uid = uid;


        SocketUtil.getInstance().stop();
        GameObject LogicEnginer = Resources.Load<GameObject>("Prefabs/Logic/LogicEnginer");
        GameObject.Instantiate(LogicEnginer);
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