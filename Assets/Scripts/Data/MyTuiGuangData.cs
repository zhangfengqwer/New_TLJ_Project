using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyTuiGuangData {

    public static MyTuiGuangData s_instance = null;

    public List<MyTuiGuangDataContent> m_myTuiGuangDataContentList = new List<MyTuiGuangDataContent>();

    public static MyTuiGuangData getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new MyTuiGuangData();
        }

        return s_instance;
    }

    public bool initJson(string json)
    {
        try
        {
            // 优先使用热更新的代码
            if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MyTuiGuangData_hotfix", "initJson"))
            {
                bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MyTuiGuangData_hotfix", "initJson", null, json);
                return b;
            }

            m_myTuiGuangDataContentList.Clear();

            JsonData jsonData = JsonMapper.ToObject(json);
            m_myTuiGuangDataContentList = JsonMapper.ToObject<List<MyTuiGuangDataContent>>(jsonData["myTuiGuangYouLiDataList"].ToString());

            return true;
        }
        catch (Exception ex)
        {
            LogUtil.Log("MyTuiGuangData解析出错：" + ex);

            return false;
            //throw ex;
        }
    }

    public List<MyTuiGuangDataContent> getMyTuiGuangDataList()
    {
        return m_myTuiGuangDataContentList;
    }

    public MyTuiGuangDataContent getMyTuiGuangDataContentByUId(string uid)
    {
        MyTuiGuangDataContent data = null;
        for (int i = 0; i < m_myTuiGuangDataContentList.Count; i++)
        {
            if (m_myTuiGuangDataContentList[i].uid.CompareTo(uid) == 0)
            {
                data = m_myTuiGuangDataContentList[i];
                break;
            }
        }

        return data;
    }
}

public class MyTuiGuangDataContent
{
    public string uid;
    public string name;
    public int task1_state;     //（1：未完成  2、完成未领取  3、完成已领取）
    public int task2_state;
}
