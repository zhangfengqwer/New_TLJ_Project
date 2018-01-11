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

    public string goods_prop;

    public void OnRequest(string goodProp)
    {
        goods_prop = goodProp;
        OnRequest();
    }

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SignRequest", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SignRequest", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SignRequest", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SignRequest", "OnResponse", null, data);
            return;
        }

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
