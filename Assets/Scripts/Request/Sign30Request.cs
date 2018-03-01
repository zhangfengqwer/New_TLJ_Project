using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class Sign30Request : Request
{
    public delegate void Sign30CallBack(string result);
    public Sign30CallBack CallBack = null;

    public bool flag = false;
    public string result;

    public int m_id = 0;
    public int m_type = 0;

    private void Awake()
    {
        Tag = Consts.Tag_Sign_30;
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

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30Request_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30Request_hotfix", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["id"] = m_id;
        jsonData["type"] = m_type;

        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30Request_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30Request_hotfix", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
