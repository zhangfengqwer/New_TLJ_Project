using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class UseTurntableRequest : Request
{
    public delegate void UseTurntableCallBack(string result);
    public UseTurntableCallBack CallBack = null;

    private bool flag = false;
    private string result;

    public int type = 1;

    private void Awake()
    {
        Tag = Consts.Tag_UseTurntable;
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
        jsonData["type"] = type;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        result = data;
        flag = true;
    }
}
