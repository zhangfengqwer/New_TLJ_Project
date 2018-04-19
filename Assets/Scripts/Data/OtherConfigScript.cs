using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherConfigScript
{
    public static OtherConfigScript s_instance = null;

    public int m_CodeVersion = 1;

    public static OtherConfigScript getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new OtherConfigScript();
        }

        return s_instance;
    }

    public void reqNet()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OtherConfigScript_hotfix", "reqNet"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OtherConfigScript_hotfix", "reqNet", null, null);
            return;
        }

        UnityWebReqUtil.Instance.Get(OtherData.getWebUrl() + "OtherConfig.json", httpCallBack);
    }

    public void httpCallBack(string tag, string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OtherConfigScript_hotfix", "httpCallBack"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OtherConfigScript_hotfix", "httpCallBack", null, tag, data);
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
            LogUtil.Log("获取OtherConfig文件出错：" + ex.Message);
            OtherData.s_getNetEntityFile.GetFileFail("OtherConfig.json");

            //throw ex;
        }
    }

    public void init(string jsonData)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OtherConfigScript_hotfix", "init"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OtherConfigScript_hotfix", "init", null, jsonData);
            return;
        }

        JsonData jd = JsonMapper.ToObject(jsonData);

        string key = "CodeVersion-" + OtherData.s_apkVersion;
        m_CodeVersion = (int)jd[key];

        NetLoading.getInstance().Close();

        OtherData.s_loginScript.getOtherConfigOver();
    }
}
