using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class MedalDuiHuanRequest : Request
{
    public delegate void RespondCallBack(string result);
    public RespondCallBack CallBack = null;

    public bool flag = false;
    public string result;

    public int goods_id;
    public int num;

    private void Awake()
    {
        Tag = Consts.Tag_MedalDuiHuan;
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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanRequest_hotfix", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["goods_id"] = goods_id;
        jsonData["num"] = num;

        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanRequest_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanRequest_hotfix", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
