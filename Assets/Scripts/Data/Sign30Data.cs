using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sign30Data
{
    public static Sign30Data s_instance = null;

    public List<Sign30DataContent> m_sign30DataContentList = new List<Sign30DataContent>();

    public static Sign30Data getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new Sign30Data();
        }

        return s_instance;
    }

    public bool initJson(string json)
    {
        try
        {
            // 优先使用热更新的代码
            if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30Data_hotfix", "initJson"))
            {
                bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30Data_hotfix", "initJson", null, json);
                return b;
            }

            m_sign30DataContentList.Clear();

            JsonData jsonData = JsonMapper.ToObject(json);
            m_sign30DataContentList = JsonMapper.ToObject<List<Sign30DataContent>>(jsonData["sign30Reward_list"].ToString());

            return true;
        }
        catch (Exception ex)
        {
            LogUtil.Log("解析SignReward_30出错：" + ex);

            return false;
            //throw ex;
        }
    }

    public List<Sign30DataContent> getSign30DataContentList()
    {
        return m_sign30DataContentList;
    }

    public Sign30DataContent getSign30DataById(int id)
    {
        Sign30DataContent data = null;
        for (int i = 0; i < m_sign30DataContentList.Count; i++)
        {
            if (m_sign30DataContentList[i].id == id)
            {
                data = m_sign30DataContentList[i];
                break;
            }
        }

        return data;
    }

    public Sign30DataContent getSign30DataContent(int day,int type)
    {
        Sign30DataContent data = null;
        for (int i = 0; i < m_sign30DataContentList.Count; i++)
        {
            if ((m_sign30DataContentList[i].day == day) && (m_sign30DataContentList[i].type == type))
            {
                data = m_sign30DataContentList[i];
                break;
            }
        }

        return data;
    }
}

public class Sign30DataContent
{
    public int id;
    public int type;                  // 1:普通签到  2:累计签到
    public int day;
    public string reward_name = "";
    public string reward_prop = "";
}
