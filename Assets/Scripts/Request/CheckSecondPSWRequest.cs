using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class CheckSecondPSWRequest : Request
{
    public delegate void CallBack(string result);
    public CallBack m_callBack = null;

    string m_secondPSW;

    private bool flag = false;
    private string result;

    private void Awake()
    {
        Tag = Consts.Tag_Login;
    }

    void Update()
    {
        if (flag)
        {
            if (m_callBack != null)
            {
                m_callBack(result);
            }

            flag = false;
        }
    }

    public void setData(string secondPSW)
    {
        m_secondPSW = secondPSW;
    }

    public override void OnRequest()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["account"] = UserData.name;
        jsonData["password"] = CommonUtil.GetMD5(m_secondPSW);
        jsonData["passwordtype"] = 3;

        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        result = data;
        flag = true;
    }
}
