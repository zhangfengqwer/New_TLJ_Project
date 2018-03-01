﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sign30RecordData {

    public static Sign30RecordData s_instance = null;

    public List<int> m_sign30RecordList = new List<int>();
    public List<int> m_sign30LeiJiRecordList = new List<int>();

    public static Sign30RecordData getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new Sign30RecordData();
        }

        return s_instance;
    }

    public bool initJson(string json)
    {
        try
        {
            // 优先使用热更新的代码
            if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30RecordData_hotfix", "initJson"))
            {
                bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30RecordData_hotfix", "initJson", null, json);
                return b;
            }

            m_sign30RecordList.Clear();
            m_sign30LeiJiRecordList.Clear();

            JsonData jsonData = JsonMapper.ToObject(json);
            string record = jsonData["record"].ToString();

            List<string> list = new List<string>();
            CommonUtil.splitStr(record, list, ',');

            for (int i = 0; i < list.Count; i++)
            {
                int id = int.Parse(list[i]);
                if (Sign30Data.getInstance().getSign30DataById(id).type == 1)
                {
                    m_sign30RecordList.Add(id);
                }
                else
                {
                    m_sign30LeiJiRecordList.Add(id);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
            //throw ex;
        }
    }

    public List<int> getSign30RecordList()
    {
        return m_sign30RecordList;
    }

    public List<int> getSign30LeiJiRecordList()
    {
        return m_sign30LeiJiRecordList;
    }

    public bool todayIsSign()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30RecordData_hotfix", "todayIsSign"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30RecordData_hotfix", "todayIsSign", null, null);
            return b;
        }

        if (Sign30Data.getInstance().getSign30DataContentList().Count == 0)
        {
            Debug.Log("签到奖励配置表未赋值");
            return false;
        }

        for (int i = 0; i < m_sign30RecordList.Count; i++)
        {
            if (Sign30Data.getInstance().getSign30DataById(m_sign30RecordList[i]).day == CommonUtil.getCurDay())
            {
                return true;
            }
        }

        return false;
    }
}