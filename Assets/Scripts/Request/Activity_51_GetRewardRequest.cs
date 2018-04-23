﻿using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class Activity_51_GetRewardRequest : Request
{
    public delegate void GetSign30RecordCallBack(string result);
    public GetSign30RecordCallBack CallBack = null;

    public bool flag = false;
    public string result;

    public int m_id;

    private void Awake()
    {
        Tag = "Activity_51_GetReward";
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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_51_GetReward_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_51_GetReward_hotfix", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["id"] = m_id;

        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Activity_51_GetReward_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Activity_51_GetReward_hotfix", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
