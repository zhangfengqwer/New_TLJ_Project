using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class SetSecondPswRequest : Request
{
    public bool flag;
    public string result;
    public string data;

    public delegate void SetSecondPSWCallBack(string result);
    public SetSecondPSWCallBack CallBack = null;

    private void Awake()
    {
        Tag = Consts.Tag_SetSecondPSW;
    }

    private void Update()
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

    public void SetData(string password)
    {
        data = password;
    }

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SetSecondPswRequest", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SetSecondPswRequest", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["password"] =CommonUtil.GetMD5(data);

        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SetSecondPswRequest", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SetSecondPswRequest", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
