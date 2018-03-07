using System;
using LitJson;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;

public class GetAcitivityRequest : Request
{
    public bool flag = false;
    public string result;

    public Action<string> CallBack;

    private void Awake()
    {
        Tag = Consts.Tag_GetActivityData;
    }

    void Update()
    {
        if (flag)
        {
            flag = false;
            CallBack(result);
        }
    }

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GetUserInfoRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GetUserInfoRequest_hotfix", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GetUserInfoRequest_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GetUserInfoRequest_hotfix", "OnResponse", null, data);
            return;
        }

        JsonData jsonData = JsonMapper.ToObject(data);
        var code = (int)jsonData["code"];
        if (code == (int) Consts.Code.Code_OK)
        {
            result = data;
            flag = true;
        }
           
        else
        {
            LogUtil.Log("用户信息数据错误：" + code);
        }
    }

}