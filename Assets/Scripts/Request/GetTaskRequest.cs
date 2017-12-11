using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class GetTaskRequest : Request
{
    public delegate void GetTaskCallBack(string result);
    public GetTaskCallBack CallBack = null;

    private bool flag = false;
    private string result;

    private void Awake()
    {
        Tag = Consts.Tag_GetTask;
    }

    void Update()
    {
        if (flag)
        {
            if (CallBack != null)
            {
                CallBack(result);
            }

            if (OtherData.s_mainScript != null)
            {
                OtherData.s_mainScript.checkRedPoint();
            }
            flag = false;
        }
    }

    public override void OnRequest()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
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

            TaskDataScript.getInstance().initJson(data);
        }
        else
        {
            LogUtil.Log("返回任务数据错误:" + code);
        }
    }
}
