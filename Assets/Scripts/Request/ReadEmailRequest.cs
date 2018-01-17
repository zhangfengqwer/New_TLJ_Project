using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class ReadEmailRequest : Request
{
    public delegate void ReadMailCallBack(string result);
    public ReadMailCallBack CallBack;

    public int emailId;
    public bool flag;
    public string result;

    public void setEmailId(int id)
    {
        emailId = id;
    }

    private void Awake()
    {
        Tag = Consts.Tag_ReadMail;
    }

    private void Update()
    {
        if (flag)
        {
            CallBack(result);
            flag = false;
        }
    }

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ReadEmailRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ReadEmailRequest_hotfix", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["email_id"] = emailId;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ReadEmailRequest_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ReadEmailRequest_hotfix", "OnResponse", null, null);
            return;
        }

        result = data;
        flag = true;
    }
}
