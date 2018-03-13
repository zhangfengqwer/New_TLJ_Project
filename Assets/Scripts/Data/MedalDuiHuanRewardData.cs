using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalDuiHuanRewardData {

    public static MedalDuiHuanRewardData s_instance = null;

    public List<MedalDuiHuanRewardDataContent> m_dataList = new List<MedalDuiHuanRewardDataContent>();

    public static MedalDuiHuanRewardData getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new MedalDuiHuanRewardData();
        }

        return s_instance;
    }

    public bool initJson(string json)
    {
        try
        {
            // 优先使用热更新的代码
            if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanRewardData_hotfix", "initJson"))
            {
                bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanRewardData_hotfix", "initJson", null, json);
                return b;
            }

            m_dataList.Clear();

            JsonData jsonData = JsonMapper.ToObject(json);
            m_dataList = JsonMapper.ToObject<List<MedalDuiHuanRewardDataContent>>(jsonData["medalDuiHuanRewardDataList"].ToString());

            return true;
        }
        catch (Exception ex)
        {
            LogUtil.Log("异常initJson：" + ex);

            return false;
            //throw ex;
        }
    }

    public List<MedalDuiHuanRewardDataContent> getDataList()
    {
        return m_dataList;
    }

    public MedalDuiHuanRewardDataContent getMedalDuiHuanRewardDataContentByIndex(int index)
    {
        MedalDuiHuanRewardDataContent data = null;

        if (index < m_dataList.Count)
        {
            data = m_dataList[index];
        }

        return data;
    }

    public MedalDuiHuanRewardDataContent getMedalDuiHuanRewardDataContentById(int goods_id)
    {
        MedalDuiHuanRewardDataContent data = null;
        for (int i = 0; i < m_dataList.Count; i++)
        {
            if (m_dataList[i].goods_id == goods_id)
            {
                data = m_dataList[i];
                break;
            }
        }

        return data;
    }
}

public class MedalDuiHuanRewardDataContent
{
    public int goods_id;
    public int type;                // :(1:虚拟商品   2:实物)
    public string name;
    public string reward_prop;
    public string desc;
    public int price;
    public int vipLevel;
}