using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalDuiHuanRecordData {
    public static MedalDuiHuanRecordData s_instance = null;

    public List<MedalDuiHuanRecordDataContent> m_dataList = new List<MedalDuiHuanRecordDataContent>();

    public static MedalDuiHuanRecordData getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new MedalDuiHuanRecordData();
        }

        return s_instance;
    }

    public bool initJson(string json)
    {
        try
        {
            // 优先使用热更新的代码
            if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanRecordData_hotfix", "initJson"))
            {
                bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanRecordData_hotfix", "initJson", null, json);
                return b;
            }

            m_dataList.Clear();

            JsonData jsonData = JsonMapper.ToObject(json);
            m_dataList = JsonMapper.ToObject<List<MedalDuiHuanRecordDataContent>>(jsonData["medalDuiHuanRecordDataList"].ToString());

            return true;
        }
        catch (Exception ex)
        {
            LogUtil.Log("异常：" + ex);

            return false;
            //throw ex;
        }
    }

    public List<MedalDuiHuanRecordDataContent> getDataList()
    {
        return m_dataList;
    }
}

public class MedalDuiHuanRecordDataContent
{
    public int goods_id;
    public int num;
    public string time;
}