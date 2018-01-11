using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopDataScript
{
    public static ShopDataScript s_shopData = null;

    public List<ShopData> m_shopDataList = new List<ShopData>();

    public static ShopDataScript getInstance()
    {
        if (s_shopData == null)
        {
            s_shopData = new ShopDataScript();
        }

        return s_shopData;
    }

    public void initJson(string json)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopDataScript", "initJson"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopDataScript", "initJson", null, json);
            return;
        }

        m_shopDataList.Clear();

        JsonData jsonData = JsonMapper.ToObject(json);
        m_shopDataList = JsonMapper.ToObject<List<ShopData>>(jsonData["shop_list"].ToString());
    }

    public List<ShopData> getShopDataList()
    {
        return m_shopDataList;
    }

    public ShopData getShopDataById(int goods_id)
    {
        ShopData shopData = null;
        for (int i = 0; i < m_shopDataList.Count; i++)
        {
            if (m_shopDataList[i].goods_id == goods_id)
            {
                shopData = m_shopDataList[i];
                break;
            }
        }

        return shopData;
    }
}

public class ShopData
{
    public int goods_id;
    public string goods_name;
    public int goods_type;          // 1:金币 2:元宝 3:道具
    public string props;
    public string extra_reward;
    public int price;
    public int money_type;          // 1:金币  2:元宝  3:人民币  4:徽章
    public int price2;
    public int money_type2;          // 1:金币  2:元宝  3:人民币  4:徽章
}