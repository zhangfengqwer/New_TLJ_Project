using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class CompleteTaskRequest : Request
{
    public delegate void CompleteTaskCallBack(string result);
    public CompleteTaskCallBack CallBack;

    public int taskId;
    public bool flag;
    public string result;

    public void setTaskId(int id)
    {
        taskId = id;
    }

    private void Awake()
    {
        Tag = Consts.Tag_CompleteTask;
    }

    private void Update()
    {
        if (flag)
        {
            CallBack(result);
            flag = false;
        }
    }
    

    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("CompleteTaskRequest", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.CompleteTaskRequest", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["task_id"] = taskId;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("CompleteTaskRequest", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.CompleteTaskRequest", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
