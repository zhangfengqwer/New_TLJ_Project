using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class GetSign30RewardRequest : Request
{
    public delegate void GetSign30RewardCallBack(string result);
    public GetSign30RewardCallBack CallBack = null;

    public bool flag = false;
    public string result;

    private void Awake()
    {
        Tag = Consts.Tag_GetSignReward_30;
    }

    void Update()
    {
        if (flag)
        {
            if (CallBack != null)
            {
                CallBack(result);
            }

            LogicEnginerScript.Instance.GetComponent<GetSign30RecordRequest>().OnRequest();

            flag = false;
        }
    }

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GetSign30RewardRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GetSign30RewardRequest_hotfix", "OnRequest", null, null);
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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GetSign30RewardRequest_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GetSign30RewardRequest_hotfix", "OnResponse", null, data);
            return;
        }

        Sign30Data.getInstance().initJson(data);

        result = data;
        flag = true;
    }
}
