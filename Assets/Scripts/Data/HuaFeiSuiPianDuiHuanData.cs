using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HuaFeiSuiPianDuiHuanData {
    public static HuaFeiSuiPianDuiHuanData s_instance = null;

    public List<HuaFeiSuiPianDuiHuanDataContent> m_dataList = new List<HuaFeiSuiPianDuiHuanDataContent>();

    public static HuaFeiSuiPianDuiHuanData getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new HuaFeiSuiPianDuiHuanData();
        }

        return s_instance;
    }

    public bool initJson(string json)
    {
        try
        {
            // 优先使用热更新的代码
            if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HuaFeiSuiPianDuiHuanData_hotfix", "initJson"))
            {
                bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HuaFeiSuiPianDuiHuanData_hotfix", "initJson", null, json);
                return b;
            }

            m_dataList.Clear();

            JsonData jsonData = JsonMapper.ToObject(json);
            m_dataList = JsonMapper.ToObject<List<HuaFeiSuiPianDuiHuanDataContent>>(jsonData["dataList"].ToString());

            return true;
        }
        catch (Exception ex)
        {
            LogUtil.Log("异常：" + ex);

            return false;
            //throw ex;
        }
    }

    public List<HuaFeiSuiPianDuiHuanDataContent> getDataList()
    {
        return m_dataList;
    }

    public HuaFeiSuiPianDuiHuanDataContent getDataById(int id)
    {
        HuaFeiSuiPianDuiHuanDataContent temp = null;

        for (int i = 0; i < m_dataList.Count; i++)
        {
            if (m_dataList[i].duihuan_id == id)
            {
                return m_dataList[i];
            }
        }

        return temp;
    }
}

public class HuaFeiSuiPianDuiHuanDataContent
{
    public int duihuan_id;
    public int material_id;     // 合成需要的材料道具id
    public int material_num;    // 合成需要的材料道具数量
    public int Synthesis_id;    // 合成后的道具id
    public int Synthesis_num;   // 合成后的道具数量
}