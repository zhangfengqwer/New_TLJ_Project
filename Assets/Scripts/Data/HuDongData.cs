using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HuDongData
{
    public static HuDongData s_instance = null;

    public List<HuDongProp> m_hudongDataList = new List<HuDongProp>();

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
        UnityWebReqUtil.Instance.Get(OtherData.getWebUrl() + "hudong.json", httpCallBack);
    }

    public void httpCallBack(string tag, string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HuDongData", "httpCallBack"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HuDongData", "httpCallBack", null, tag, data);
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
            LogUtil.Log("获取互动配置文件出错：" + ex.Message);
            OtherData.s_getNetEntityFile.GetFileFail("hudong.json");

            //throw ex;
        }
    }

    public void init(string jsonData)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HuDongData", "init"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HuDongData", "init", null, jsonData);
            return;
        }

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

        OtherData.s_getNetEntityFile.GetFileSuccess("hudong.json");
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