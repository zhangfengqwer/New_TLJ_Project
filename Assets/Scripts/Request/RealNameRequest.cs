using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class RealNameRequest : Request {

    public bool flag = false;
    public string result;
    public string realName;
    public string identification;

    private void Awake()
    {
        Tag = Consts.Tag_RealName;
    }
    void Update()
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
    public delegate void RealNameCallBack(string result);

    public RealNameCallBack CallBack = null;

    public void OnRequest(string reaname, string identfy)
    {
        this.realName = reaname;
        this.identification = identfy;
        OnRequest();
    }

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("RealNameRequest", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.RealNameRequest", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["realName"] = realName;
        jsonData["identification"] = identification;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("RealNameRequest", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.RealNameRequest", "OnResponse", null, data);
            return;
        }

        JsonData jsonData = JsonMapper.ToObject(data);
        var code = (int)jsonData["code"];
        if (code == (int)Consts.Code.Code_OK)
        {
            result = data;
            flag = true;
        }
        else
        {
            LogUtil.Log("返回实名认证数据错误：" + code);
        }
    }
}
