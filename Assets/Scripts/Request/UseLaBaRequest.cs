using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class UseLaBaRequest : Request
{
    bool flag;
    string result;
    string text;

    public delegate void UseLabaCallBack(string result);
    public UseLabaCallBack CallBack;

    private void Awake()
    {
        Tag = Consts.Tag_UseLaBa;
    }

    private void Update()
    {
        if (flag)
        {
            CallBack(result);
            flag = false;
        }
    }

    public void SetText(string str)
    {
        text = str;
    }
   
    public override void OnRequest()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["text"] = text;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        result = data;
        flag = true;
    }
}
