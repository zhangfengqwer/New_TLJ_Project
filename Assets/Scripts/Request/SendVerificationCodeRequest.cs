using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class SendVerificationCodeRequest : Request {

    private bool flag = false;
    private string result;
    private string phoneNum;
    private void Awake()
    {
        Tag = Consts.Tag_SendSMS;
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
    public delegate void SendVerificationCodeCallBack(string result);

    public SendVerificationCodeCallBack CallBack = null;

    public void OnRequest(string phone)
    {
        this.phoneNum = phone;
        OnRequest();
    }

    public override void OnRequest()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["phoneNum"] = phoneNum;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
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
