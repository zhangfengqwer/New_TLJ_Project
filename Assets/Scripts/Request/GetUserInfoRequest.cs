using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class GetUserInfoRequest : Request {

    private void Awake()
    {
        Tag = Consts.Tag_UserInfo;
    }

    public override void OnRequest()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        JsonData jsonData = JsonMapper.ToObject(data);
        var code = (int)jsonData["code"];
        if (code == (int) Consts.Code.Code_OK)
        {
            LogicEnginerScript.IsSuccessList.Add(true);
            UserData.name = (string) jsonData["name"];
            UserData.phone = (string) jsonData["phone"];
            UserData.gold = (int) jsonData["gold"];
            UserData.yuanbao = (int) jsonData["yuanbao"];
        }
        else
        {
            ToastScript.createToast("用户信息数据错误");
        }

     
    }
}
