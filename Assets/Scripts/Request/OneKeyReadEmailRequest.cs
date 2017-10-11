using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class OneKeyReadEmailRequest : Request
{
    private int emailId;
    private bool flag;
    private string result;

    private void Awake()
    {
        Tag = Consts.Tag_OneKeyReadMail;
    }

    private void Update()
    {
        if (flag)
        {
            CallBack(result);
            flag = false;
        }
    }
    public delegate void ReadMailCallBack(string result);

    public ReadMailCallBack CallBack;

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
        result = data;
        flag = true;
    }
}
