using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class GetEmailRequest : Request {

    public delegate void GetMailCallBack(string result);
    public GetMailCallBack CallBack = null;

    public bool flag = false;
    public string result;

    private void Awake()
    {
        Tag = Consts.Tag_GetMail;
    }

    void Update()
    {
        if (flag)
        {
            if (CallBack != null)
            {
                CallBack(result);
            }

            if (OtherData.s_mainScript != null)
            {
                OtherData.s_mainScript.checkRedPoint();
            }
            flag = false;
        }
    }

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GetEmailRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GetEmailRequest_hotfix", "OnRequest", null, null);
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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GetEmailRequest_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GetEmailRequest_hotfix", "OnResponse", null, data);
            return;
        }

        JsonData jsonData = JsonMapper.ToObject(data);
        var code = (int) jsonData["code"];
        if (code == (int) Consts.Code.Code_OK)
        {
            LogicEnginerScript.IsSuccessList.Add(true);
            UserMailData.getInstance().initJson(data);

            result = data;
            flag = true;
        }
        else
        {
            LogUtil.Log("返回邮箱数据错误:" + code);
        }
    }
}
