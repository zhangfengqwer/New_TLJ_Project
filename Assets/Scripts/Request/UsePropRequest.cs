﻿using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class UsePropRequest : Request {
    public bool flag;
    public string result;

    private void Awake()
    {
        Tag = Consts.Tag_UseProp;
    }

    private int propId;
    public void SetPropId(int id)
    {
        propId = id;
    }
    // Use this for initialization

    private void Update()
    {
        if (flag)
        {
            CallBack(result);
            flag = false;
        }
    }

    public delegate void UsePropCallBack(string result);
    public UsePropCallBack CallBack;

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("UsePropRequest", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.UsePropRequest", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["prop_id"] = propId;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("UsePropRequest", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.UsePropRequest", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
//        LogicEnginerScript.Instance._getUserBagRequest.OnRequest();
    }
}
