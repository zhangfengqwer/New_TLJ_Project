using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class UsePropRequest : Request {
    private bool flag;
    private string result;

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
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["prop_id"] = propId;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        result = data;
        flag = true;
    }
}
