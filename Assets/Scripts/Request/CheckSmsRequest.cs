using LitJson;
using TLJCommon;
using UnityEngine;

public class CheckSmsRequest : Request {

    private bool flag = false;
    private string result;
    private string phoneNum;
    private string code;
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
        result = data;
        flag = true;
    }
}
