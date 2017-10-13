using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelScript : MonoBehaviour {
    private UIWarpContent uiWarpContent;
    private List<string> _list;
    private List<ShopData> shopDataList;
    private int type = 1;
    private List<ShopData> _shopItemDatas;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ShopPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
	{
	    uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
	    uiWarpContent.onInitializeItem = onInitializeItem;
        // 拉取商店数据
        {
	        LogicEnginerScript.Instance.GetComponent<GetShopRequest>().CallBack = onReceive_GetShop;
	        LogicEnginerScript.Instance.GetComponent<GetShopRequest>().OnRequest();
	    }
	}

    private void Init()
    {
        for (int i = shopDataList.Count -1; i>=0; i--)
        {
            uiWarpContent.DelItem(i);
        }

        _shopItemDatas = new List<ShopData>();
        for (int i = 0; i < shopDataList.Count; i++)
        {
            ShopData shopData = shopDataList[i];
            if (shopData.goods_type == type)
            {
                _shopItemDatas.Add(shopData);
            }
        }

        uiWarpContent.Init(_shopItemDatas.Count);
    }

    private void onInitializeItem(GameObject go, int dataindex)
    {
       
        Text goods_name = go.transform.Find("goods_name").GetComponent<Text>();
        Text goods_price = go.transform.Find("goods_price").GetComponent<Text>();
        goods_name.text = _shopItemDatas[dataindex].props;
        //设置价格
        string price = null;
        if (_shopItemDatas[dataindex].money_type == 1)
        {
            price = "金币:";
        }
        else if (_shopItemDatas[dataindex].money_type == 2)
        {
            price = "元宝:";
        }
        else
        {
            price = "¥:";
        }
        goods_price.text = price + _shopItemDatas[dataindex].price;

        Button button = go.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate()
        {
            BuyGoodsPanelScript.create(_shopItemDatas[dataindex].goods_id);
        });
    }

    public void onReceive_GetShop(string data)
    {
        ShopDataScript.getInstance().initJson(data);
        shopDataList = ShopDataScript.getInstance().getShopDataList();
        Init();
    }

    public void IsJinbiToggle(bool IsClick)
    {
        if (IsClick)
        {
            type = 1;
            Init();
        }
    }

    public void IsYuanBaoToggle(bool IsClick)
    {
        if (IsClick)
        {
            type = 2;
            Init();
        }
    }

    public void IsPropToggle(bool IsClick)
    {
        if (IsClick)
        {
            type = 3;
            Init();
        }
    }
}
