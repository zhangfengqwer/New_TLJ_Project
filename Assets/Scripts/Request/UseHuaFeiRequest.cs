using LitJson;
using System.Collections;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

public class UseHuaFeiRequest : Request
{
    public bool flag;
    public string result;
    public int m_prop_id;
    public int m_prop_num;
    public string m_phone;

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

    public void SetData(int prop_id,int prop_num,string phone)
    {
        m_prop_id = prop_id;
        m_prop_num = prop_num;
        m_phone = phone;
    }

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("UseHuaFeiRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.UseHuaFeiRequest_hotfix", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["prop_id"] = m_prop_id;
        jsonData["prop_num"] = m_prop_num;
        jsonData["phone"] = m_phone;

        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("UseHuaFeiRequest_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.UseHuaFeiRequest_hotfix", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
