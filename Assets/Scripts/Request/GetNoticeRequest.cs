﻿using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;
using System;

public class GetNoticeRequest : Request {

    public delegate void GetNoticeCallBack(string result);
    public GetNoticeCallBack CallBack = null;

    private bool flag = false;
    private string result;

    private void Awake()
    {
        Tag = Consts.Tag_GetNotice;
    }

    void Update()
    {
        if (flag)
        {
            if (CallBack != null)
            {
                CallBack(result);
            }

            GameObject.Find("Canvas").GetComponent<MainScript>().checkRedPoint();
            flag = false;
        }
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
        if (code == (int)Consts.Code.Code_OK)
        {
            NoticelDataScript.getInstance().initJson(data);

            result = data;
            flag = true;
        }
        else
        {
            Debug.Log("返回公告活动数据错误：" + code);
        }
    }
}