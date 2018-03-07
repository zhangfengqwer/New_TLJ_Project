using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class GetSign30RecordRequest : Request
{
    public delegate void GetSign30RecordCallBack(string result);
    public GetSign30RecordCallBack CallBack = null;

    public bool flag = false;
    public string result;

    private void Awake()
    {
        Tag = Consts.Tag_GetSignRecord_30;
    }

    void Update()
    {
        if (flag)
        {
            if (CallBack != null)
            {
                CallBack(result);
            }

            Sign30RecordData.getInstance().initJson(result);

            if (OtherData.s_mainScript != null)
            {
                OtherData.s_mainScript.checkRedPoint();
            }

            flag = false;
        }
    }

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GetSign30RecordRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GetSign30RecordRequest_hotfix", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;

        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GetSign30RecordRequest_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GetSign30RecordRequest_hotfix", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
