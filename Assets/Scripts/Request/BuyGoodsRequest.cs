using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class BuyGoodsRequest : Request
{
    public delegate void BuyGoodsCallBack(string result);
    public BuyGoodsCallBack CallBack;

    public int m_goods_id;
    public int m_goods_num;
    public int m_money_type;

    public bool flag;
    public string result;

    public void setGoodsInfo(int goods_id, int goods_num, int money_type)
    {
        m_goods_id = goods_id;
        m_goods_num = goods_num;
        m_money_type = money_type;
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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BuyGoodsRequest_hotfix", "OnRequest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BuyGoodsRequest_hotfix", "OnRequest", null, null);
            return;
        }

        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        jsonData["goods_id"] = m_goods_id;
        jsonData["goods_num"] = m_goods_num;
        jsonData["money_type"] = m_money_type;
        
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BuyGoodsRequest_hotfix", "OnResponse"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BuyGoodsRequest_hotfix", "OnResponse", null, data);
            return;
        }

        result = data;
        flag = true;
    }
}
