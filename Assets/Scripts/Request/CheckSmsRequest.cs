using LitJson;
using TLJCommon;
using UnityEngine;

public class CheckSmsRequest : Request {

    public bool flag = false;
    public string result;
    public string phoneNum;
    public string code;

    private void Awake()
    {
        Tag = Consts.Tag_CheckSMS;
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
    public delegate void BindPhoneCallBack(string result);

    public BindPhoneCallBack CallBack = null;

    public void OnRequest(string phone,string code)
    {
        this.phoneNum = phone;
        this.code = code;
        OnRequest();
    }

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("CheckSmsRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.CheckSmsRequest_hotfix", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["phoneNum"] = phoneNum;
        jsonData["verfityCode"] = code;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("CheckSmsRequest", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.CheckSmsRequest", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
