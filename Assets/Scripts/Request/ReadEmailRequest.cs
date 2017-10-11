using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class ReadEmailRequest : Request
{
    private int emailId;

    public void setEmailId(int id)
    {
        emailId = id;
    }

    private void Awake()
    {
        Tag = Consts.Tag_GetBag;
    }

    public override void OnRequest()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["email_id"] = emailId;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMessage(requestData);
    }

    public override void OnResponse(string data)
    {

    }
}
