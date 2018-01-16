using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class PropData
{
    public static PropData s_instance = null;

    public List<PropInfo> m_propInfoList = new List<PropInfo>();

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
        UnityWebReqUtil.Instance.Get(OtherData.getWebUrl() + "prop.json", httpCallBack);
    }

    public void httpCallBack(string tag, string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropData", "httpCallBack"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropData", "httpCallBack", null, tag, data);
            return;
        }

        try
        {
            // 读取配置文件
            {
                init(data);
            }
        }
        catch (Exception ex)
        {
            LogUtil.Log("获取道具表文件出错：" + ex.Message);
            OtherData.s_getNetEntityFile.GetFileFail("prop.json");

            //throw ex;
        }
    }

    public void init(string jsonData)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropData", "init"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropData", "init", null, jsonData);
            return;
        }

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

        OtherData.s_getNetEntityFile.GetFileSuccess("prop.json");
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