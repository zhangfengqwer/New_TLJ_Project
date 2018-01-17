using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class CheckSecondPSWRequest : Request
{
    public delegate void CallBack(string result);
    public CallBack m_callBack = null;

    public string m_secondPSW;

    public bool flag = false;
    public string result;

    private void Awake()
    {
        Tag = Consts.Tag_Login;
    }

    void Update()
    {
        if (flag)
        {
            if (m_callBack != null)
            {
                m_callBack(result);
            }

            flag = false;
        }
    }

    public void setData(string secondPSW)
    {
        m_secondPSW = secondPSW;
    }

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("CheckSecondPSWRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.CheckSecondPSWRequest_hotfix", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["account"] = UserData.name;
        jsonData["password"] = CommonUtil.GetMD5(m_secondPSW);
        jsonData["passwordtype"] = 3;

        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("CheckSecondPSWRequest_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.CheckSecondPSWRequest_hotfix", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
