using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class ReadNoticeRequest : Request {
    private int noticeId;
    private bool flag;
    private string result;

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
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["notice_id"] = noticeId;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        result = data;
        flag = true;
    }
}
