using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class CompleteTaskRequest : Request
{
    public delegate void CompleteTaskCallBack(string result);
    public CompleteTaskCallBack CallBack;

    private int taskId;
    private bool flag;
    private string result;

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
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["task_id"] = taskId;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        result = data;
        flag = true;
    }
}
