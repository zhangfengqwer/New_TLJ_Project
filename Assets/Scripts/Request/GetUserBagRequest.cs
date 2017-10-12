﻿using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class GetUserBagRequest : Request
{ 
    private void Awake()
    {
        Tag = Consts.Tag_GetBag;
    }
    // Use this for initialization
    public override void OnRequest()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        JsonData jsonData = JsonMapper.ToObject(data);
        var code = (int)jsonData["code"];
        if (code == (int) Consts.Code.Code_OK)
        {
            UserBagData.getInstance().initJson(data);
        }
        else
        {
            ToastScript.createToast("用户背包数据错误");
        }
    }
}
