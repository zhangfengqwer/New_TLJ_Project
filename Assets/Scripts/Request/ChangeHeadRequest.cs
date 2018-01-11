using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class ChangeHeadRequest : Request
{
    public delegate void CallBack(string result);
    public CallBack m_callBack = null;

    public bool flag = false;
    public string result;

    public int head = 1;

    private void Awake()
    {
        Tag = Consts.Tag_ChangeHead;
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

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChangeHeadRequest", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChangeHeadRequest", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["head"] = head;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChangeHeadRequest", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChangeHeadRequest", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
