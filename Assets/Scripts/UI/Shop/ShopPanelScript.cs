using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelScript : MonoBehaviour {
    private UIWarpContent uiWarpContent;
    private List<string> _list;
    private List<ShopData> shopDataList;
    private int type = 1;
    private List<ShopData> _shopDatas;

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
        print(shopDataList.Count);
        for (int i = shopDataList.Count -1; i>=0; i--)
        {
            uiWarpContent.DelItem(i);
        }

        _shopDatas = new List<ShopData>();
        for (int i = 0; i < shopDataList.Count; i++)
        {
            ShopData shopData = shopDataList[i];
            if (shopData.goods_type == type)
            {
                _shopDatas.Add(shopData);
            }
        }

        uiWarpContent.Init(_shopDatas.Count);
    }

    private void onInitializeItem(GameObject go, int dataindex)
    {
       
        Text goods_name = go.transform.Find("goods_name").GetComponent<Text>();
        goods_name.text = _shopDatas[dataindex].props;

        Button button = go.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate()
        {
            ToastScript.createToast(_shopDatas[dataindex].props);
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
