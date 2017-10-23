using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class GetShopRequest : Request
{
    public delegate void GetShopCallBack(string result);
    public GetShopCallBack CallBack = null;

    private bool flag = false;
    private string result;

    private void Awake()
    {
        Tag = Consts.Tag_GetShop;
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
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        JsonData jsonData = JsonMapper.ToObject(data);
        ShopDataScript.getInstance().initJson(data);
        result = data;
        flag = true;
    }
}
