using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using TLJCommon;

public class SignRequest : Request {
    private void Awake()
    {
        Tag = Consts.Tag_Sign;
    }

    public delegate void SignCallBack(bool falg);

    public SignCallBack CallBack;

    public override void OnRequest()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.Uid;
        string requestData = jsonData.ToJson();
        print(requestData);
        LogicEnginerScript.Instance.SendMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        JsonData jsonData = JsonMapper.ToObject(data);
        var code = (int)jsonData["code"];
        if (code == (int) Consts.Code.Code_OK)
        {
            CallBack(true);
        }
        else
        {
            CallBack(false);
        }
    }
}
