using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class TurntableBroadcastDataScript
{
    static TurntableBroadcastDataScript s_instance = null;

    List<TurntableBroadcastData> m_dataList = new List<TurntableBroadcastData>();

    public static TurntableBroadcastDataScript getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new TurntableBroadcastDataScript();
        }

        return s_instance;
    }

    public void addData(string name,int reward_id)
    {
        TurntableBroadcastData temp = new TurntableBroadcastData();

        temp.m_name = name;
        temp.m_reward_id = reward_id;

        m_dataList.Add(temp);

        if (m_dataList.Count > 5)
        {
            m_dataList.RemoveAt(0);
        }
    }

    public List<TurntableBroadcastData> getTurntableBroadcastDataList()
    {
        return m_dataList;
    }
}

public class TurntableBroadcastData
{
    public string m_name;
    public int m_reward_id;
}