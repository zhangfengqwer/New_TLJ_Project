using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class BuyGoodsRequest : Request
{
    public delegate void BuyGoodsCallBack(string result);
    public BuyGoodsCallBack CallBack;

    private int gooldsId;
    private bool flag;
    private string result;
    public void setGoodsId(int id)
    {
        gooldsId = id;
    }

    private void Awake()
    {
        Tag = Consts.Tag_BuyGoods;
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
        jsonData["goods_id"] = gooldsId;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        result = data;
        flag = true;
    }
}
