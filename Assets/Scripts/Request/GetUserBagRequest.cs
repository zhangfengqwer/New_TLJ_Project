﻿using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class GetUserBagRequest : Request {
    public  List<UserPropData> _userPropDatas;
    public static GetUserBagRequest Instance;

    public delegate void GetUserBagCallBack(string result);
    public GetUserBagCallBack CallBack = null;

    private bool flag = false;
    private string result;

    private void Awake()
    {
        Tag = Consts.Tag_GetBag;
        if (Instance == null)
        {
            Instance = this;
        }
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

    // Use this for initialization
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
        if (code == (int) Consts.Code.Code_OK)
        {
            _userPropDatas = JsonMapper.ToObject<List<UserPropData>>(jsonData["prop_list"].ToString());

            result = data;
            flag = true;
        }
        else
        {
            ToastScript.createToast("用户背包数据错误");
        }
    }

    public  List<UserPropData> GetPropList()
    {
        return _userPropDatas;
    }
}
