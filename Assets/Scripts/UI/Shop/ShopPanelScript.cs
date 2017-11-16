using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelScript : MonoBehaviour
{
    private MainScript m_mainScript = null;
    public Image VipImage;
    public Text VipExplain;
    public Text SlideText;
    public Slider SliderVip;
    public Toggle YuanbaoToggle;
    public Toggle GoldToggle;
    public Toggle PropToggle;
    public Toggle MedalToggle;


    private UIWarpContent uiWarpContent;
    private List<string> _list;
    private static List<ShopData> shopDataList;

    //商品类型，1：金币，2：元宝，3：道具,4:徽章
    private int _type = 2;

    private List<ShopData> _shopItemDatas;

    public static GameObject create(MainScript mainScript,int type)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ShopPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<ShopPanelScript>().m_mainScript = mainScript;
        obj.GetComponent<ShopPanelScript>().SetType(type);
        return obj;
    }

    // Use this for initialization
    void Start()
    {
        InitVip();

        uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
        uiWarpContent.onInitializeItem = onInitializeItem;

        if (shopDataList == null || shopDataList.Count == 0)
        {
            // 拉取商店数据
            {
                LogicEnginerScript.Instance.GetComponent<GetShopRequest>().CallBack = onReceive_GetShop;
                LogicEnginerScript.Instance.GetComponent<GetShopRequest>().OnRequest();
            }
        }
        else
        {
            Init();
        }
    }

    public void SetType(int type)
    {
        uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
        uiWarpContent.onInitializeItem = onInitializeItem;
        switch (type)
        {
            case 1:
                GoldToggle.isOn = true;
                break;
            case 2:
                YuanbaoToggle.isOn = true;
                break;
            case 3:
                PropToggle.isOn = true;
                break;
            case 4:
                MedalToggle.isOn = true;
                break;
        }
    }

    private void InitVip()
    {
        int vipLevel = VipUtil.GetVipLevel(UserData.rechargeVip);
        int currentVipToTal = VipUtil.GetCurrentVipToTal(vipLevel);
        VipImage.sprite = Resources.Load<Sprite>("Sprites/Vip/shop_vip_" + vipLevel);
//        Vip.text = "vip" + vipLevel;
       

        int left = currentVipToTal - UserData.rechargeVip;

        VipExplain.text =string.Format("累计充值" + "<color=#FF0000FF>{0}</color>" + ",即可升级到" + "<color=#FF0000FF>{1}</color>",
            currentVipToTal+"元",
            "VIP"+ (vipLevel + 1));
            
        SlideText.text = UserData.rechargeVip + "/" + currentVipToTal;
        SliderVip.value = UserData.rechargeVip / (float) currentVipToTal;
    }


    private void Init()
    {
        try
        {
            for (int i = shopDataList.Count - 1; i >= 0; i--)
            {
                uiWarpContent.DelItem(i);
            }
            _shopItemDatas = new List<ShopData>();
            for (int i = 0; i < shopDataList.Count; i++)
            {
                ShopData shopData = shopDataList[i];
                if (shopData.goods_type == _type)
                {
                    _shopItemDatas.Add(shopData);
                }
            }
            uiWarpContent.Init(_shopItemDatas.Count);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void onInitializeItem(GameObject go, int dataindex)
    {
        ShopData shopItemData = _shopItemDatas[dataindex];
        string[] strings = shopItemData.props.Split(':');
        int id = Convert.ToInt32(strings[0]);

        Text goods_price = go.transform.Find("goods_price").GetComponent<Text>();
        Text goods_price2 = go.transform.Find("goods_price2").GetComponent<Text>();
        Image goods_image = go.transform.Find("goods_image").GetComponent<Image>();
        Image goods_icon = go.transform.Find("goods_icon").GetComponent<Image>();
        Text goods_des = go.transform.Find("goods_des").GetComponent<Text>();

        go.transform.Find("goods_price").localScale = Vector3.one;
        go.transform.Find("goods_price2").localScale = Vector3.zero;

        goods_des.text = shopItemData.goods_name;

        if (_type == 3)
        {
            for (int i = 0; i < PropData.getInstance().getPropInfoList().Count; i++)
            {
                PropInfo propInfo = PropData.getInstance().getPropInfoList()[i];
                if (id == propInfo.m_id)
                {
                    goods_image.sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/" + propInfo.m_icon);
                }
            }
        }
        else if (_type == 2)
        {
            goods_image.sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/icon_yuanbao");
        }
        else
        {
            goods_image.sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/icon_jinbi");
        }

        //设置价格
        string price = null;
        if (_shopItemDatas[dataindex].money_type == 1)
        {
            goods_icon.sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/icon_jinbi");
        }
        else if (_shopItemDatas[dataindex].money_type == 2)
        {
            goods_icon.sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/icon_yuanbao");
        }
        else
        {
            price = "¥";
            go.transform.Find("goods_icon").localScale = Vector3.zero;
            go.transform.Find("goods_price2").localScale = Vector3.one;
            go.transform.Find("goods_price").localScale = Vector3.zero;
            goods_price2.text = price + _shopItemDatas[dataindex].price;
        }
        goods_price.text = price + _shopItemDatas[dataindex].price;

        Button button = go.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate()
        {
            BuyGoodsPanelScript.create(m_mainScript, _shopItemDatas[dataindex].goods_id);
        });
    }

    public void onReceive_GetShop(string data)
    {
        ShopDataScript.getInstance().initJson(data);
        shopDataList = ShopDataScript.getInstance().getShopDataList();
        //去除1005；
        for (int i = 0; i < shopDataList.Count; i++)
        {
            ShopData shopData = shopDataList[i];
            if (shopData.goods_id == 1005)
            {
                shopDataList.Remove(shopData);
            }
        }

        Init();
    }

    public void IsJinbiToggle(bool IsClick)
    {
        if (IsClick)
        {
            _type = 1;
            Init();
        }
    }

    public void IsYuanBaoToggle(bool IsClick)
    {
        if (IsClick)
        {
            _type = 2;
            Init();
        }
    }

    public void IsPropToggle(bool IsClick)
    {
        if (IsClick)
        {
            _type = 3;
            Init();
        }
    }

     public void IsMedalToggle(bool IsClick)
    {
        if (IsClick)
        {
            ToastScript.createToast("徽章兑换暂未开放");
            _type = 4;
            Init();
        }
    }

    public void OnClickVipHelp()
    {
        ToastScript.createToast("vip特权完善中");
    }
}