using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

class PropData
{
    static PropData s_instance = null;

    List<PropInfo> m_propInfoList = new List<PropInfo>();

    public static PropData getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new PropData();
        }

        return s_instance;
    }

    public void reqNet()
    {
        UnityWebReqUtil.Instance.Get(OtherData.s_webStorageUrl + "prop.json", httpCallBack);
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
        m_propInfoList.Clear();

        //// 使用本地配置文件
        //string jsonData = Resources.Load("Entity/prop").ToString();

        JsonData jd = JsonMapper.ToObject(jsonData);

        for (int i = 0; i < jd.Count; i++)
        {
            PropInfo temp = new PropInfo();

            temp.m_id = (int) jd[i]["prop_id"];
            temp.m_type = (int) jd[i]["type"];

            temp.m_name = (string) jd[i]["prop_name"];
            temp.m_desc = (string) jd[i]["desc"];
            temp.m_icon = (string) jd[i]["icon"];

            m_propInfoList.Add(temp);
        }
    }

    public List<PropInfo> getPropInfoList()
    {
        return m_propInfoList;
    }

    public PropInfo getPropInfoById(int id)
    {
        PropInfo propInfo = null;

        for (int i = 0; i < m_propInfoList.Count; i++)
        {
            if (m_propInfoList[i].m_id == id)
            {
                propInfo = m_propInfoList[i];
                break;
            }
        }

        return propInfo;
    }
}

public class PropInfo
{
    public int m_id = 0;
    public int m_type = 0; // 0:可以直接使用    1:不可以直接使用

    public string m_name = "";
    public string m_desc = "";
    public string m_icon = "";
}