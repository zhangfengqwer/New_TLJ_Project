using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class BindTuiGuangCodeRequest : Request
{
    public delegate void RespondCallBack(string result);
    public RespondCallBack CallBack = null;

    public bool flag = false;
    public string result;

    public string tuiguangcode;

    private void Awake()
    {
        Tag = Consts.Tag_BindTuiGuangCode;
    }

    void Update()
    {
        if (flag)
        {
            if (CallBack != null)
            {
                CallBack(result);
            }

            flag = false;
        }
    }

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BindTuiGuangCodeRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BindTuiGuangCodeRequest_hotfix", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["tuiguangcode"] = tuiguangcode;

        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BindTuiGuangCodeRequest_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BindTuiGuangCodeRequest_hotfix", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
