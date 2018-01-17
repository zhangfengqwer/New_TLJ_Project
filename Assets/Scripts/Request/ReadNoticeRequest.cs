using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class ReadNoticeRequest : Request {
    public int noticeId;
    public bool flag;
    public string result;

    private void Awake()
    {
        Tag = Consts.Tag_ReadNotice;
    }

    public void setNoticeId(int id)
    {
        noticeId = id;
    }
    private void Update()
    {
        if (flag)
        {
            CallBack(result);
            flag = false;
        }
    }
    public delegate void ReadNoticeCallBack(string result);

    public ReadNoticeCallBack CallBack;
    // Use this for initialization
    public override void OnRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ReadNoticeRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ReadNoticeRequest_hotfix", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["notice_id"] = noticeId;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ReadNoticeRequest_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ReadNoticeRequest_hotfix", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
