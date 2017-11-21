﻿using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HuDongData
{
    static HuDongData s_instance = null;

    List<HuDongProp> m_hudongDataList = new List<HuDongProp>();

    public static HuDongData getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new HuDongData();
        }

        return s_instance;
    }

    public void reqNet()
    {
        UnityWebReqUtil.Instance.Get("http://oru510uv8.bkt.clouddn.com/hudong.json", httpCallBack);
    }

    void httpCallBack(string tag, string data)
    {
        try
        {
            // 读取配置文件
            {
                init(data);
            }
        }
        catch (Exception ex)
        {
            LogUtil.Log("读取网络配置文件出错：" + ex.Message);
        }
    }

    public void init(string jsonData)
    {
        m_hudongDataList.Clear();

        // string jsonData = Resources.Load("Entity/hudong").ToString();

        JsonData jd = JsonMapper.ToObject(jsonData);

        for (int i = 0; i < jd.Count; i++)
        {
            HuDongProp temp = new HuDongProp();

            temp.m_id = (int) jd[i]["id"];
            temp.m_price = (int) jd[i]["price"];

            m_hudongDataList.Add(temp);
        }
    }

    public List<HuDongProp> getHuDongDataList()
    {
        return m_hudongDataList;
    }

    public HuDongProp getHuDongDataById(int id)
    {
        HuDongProp temp = null;

        for (int i = 0; i < m_hudongDataList.Count; i++)
        {
            if (m_hudongDataList[i].m_id == id)
            {
                temp = m_hudongDataList[i];
                break;
            }
        }

        return temp;
    }
}

public class HuDongProp
{
    public int m_id = 0;
    public int m_price = 0;
}