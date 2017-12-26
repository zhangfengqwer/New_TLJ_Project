using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class SetSecondPswRequest : Request
{
    bool flag;
    string result;
    string data;

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
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["password"] = CommonUtil.GetMD5(data);

        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        result = data;
        flag = true;
    }
}
