using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class ProgressTaskRequest : Request
{

    public delegate void CallBack(string result);
    public CallBack m_callBack = null;

    private bool flag = false;
    private string result;

    private void Awake()
    {
        Tag = Consts.Tag_ProgressTask;
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
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["task_id"] = 213;

        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        result = data;
        flag = true;
    }
}
