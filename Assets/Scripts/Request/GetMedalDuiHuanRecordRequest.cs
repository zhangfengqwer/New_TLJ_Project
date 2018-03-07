using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class GetMedalDuiHuanRecordRequest : Request
{
    public delegate void RespondCallBack(string result);
    public RespondCallBack CallBack = null;

    public bool flag = false;
    public string result;

    private void Awake()
    {
        Tag = Consts.Tag_GetMedalDuiHuanRecord;
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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GetMedalDuiHuanRecordRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GetMedalDuiHuanRecordRequest_hotfix", "OnRequest", null, null);
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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GetMedalDuiHuanRecordRequest_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GetMedalDuiHuanRecordRequest_hotfix", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
