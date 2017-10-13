using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class BuyGoodsRequest : Request
{
    public delegate void BuyGoodsCallBack(string result);
    public BuyGoodsCallBack CallBack;

    int m_goods_id;
    int m_goods_num;

    private bool flag;
    private string result;

    public void setGoodsInfo(int goods_id, int goods_num)
    {
        m_goods_id = goods_id;
        m_goods_num = goods_num;
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
        jsonData["goods_id"] = m_goods_id;
        jsonData["goods_num"] = m_goods_num;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        result = data;
        flag = true;
    }
}
