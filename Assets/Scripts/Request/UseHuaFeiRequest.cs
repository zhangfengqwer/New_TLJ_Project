using LitJson;
using System.Collections;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

public class UseHuaFeiRequest : Request
{
    bool flag;
    string result;
    int m_prop_id;
    string m_phone;

    public delegate void UseHuaFeiCallBack(string result);
    public UseHuaFeiCallBack CallBack;

    private void Awake()
    {
        Tag = Consts.Tag_UseHuaFei;
    }

    private void Update()
    {
        if (flag)
        {
            CallBack(result);
            flag = false;
        }
    }

    public void SetData(int prop_id,string phone)
    {
        m_prop_id = prop_id;
        m_phone = phone;
    }

    public override void OnRequest()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["prop_id"] = m_prop_id;
        jsonData["phone"] = m_phone;

        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        result = data;
        flag = true;
    }
}
