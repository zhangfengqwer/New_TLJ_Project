﻿using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class GetEmailRequest : Request {

    public delegate void GetMailCallBack(string result);
    public GetMailCallBack CallBack = null;

    private bool flag = false;
    private string result;

    private void Awake()
    {
        Tag = Consts.Tag_GetMail;
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
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        JsonData jsonData = JsonMapper.ToObject(data);
        var code = (int) jsonData["code"];
        if (code == (int) Consts.Code.Code_OK)
        {
            LogicEnginerScript.IsSuccessList.Add(true);
            UserMailData.getInstance().initJson(data);

            result = data;
            flag = true;
        }
        else
        {
            ToastScript.createToast("返回邮箱数据错误:" + code);
        }
    }
}
