using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class GetEmailRequest : Request {

    private void Awake()
    {
        Tag = Consts.Tag_GetMail;
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
        }
        else
        {
            ToastScript.createToast("返回邮箱数据错误");
        }
    }
}
