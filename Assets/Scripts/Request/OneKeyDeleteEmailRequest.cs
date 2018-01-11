using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class OneKeyDeleteEmailRequest : Request
{
    public int emailId;
    public bool flag;
    public string result;

    private void Awake()
    {
        Tag = Consts.Tag_OneKeyDeleteMail;
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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OneKeyDeleteEmailRequest", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OneKeyDeleteEmailRequest", "OnRequest", null, null);
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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OneKeyDeleteEmailRequest", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OneKeyDeleteEmailRequest", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
