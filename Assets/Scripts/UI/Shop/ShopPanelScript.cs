using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelScript : MonoBehaviour
{
    public Image VipImage;
    public Text VipExplain;
    public Text SlideText;
    public Slider SliderVip;
    public Toggle YuanbaoToggle;
    public Toggle GoldToggle;
    public Toggle PropToggle;
    public Toggle MedalToggle;
    public GameObject Shop;
    public GameObject Vip;
    public Text UserYuanBao;
    public Text UserGold;
    public Text UserMedal;

    public UIWarpContent uiWarpContent;
    public List<string> _list;
    public static List<ShopData> shopDataList;
    public static ShopPanelScript Instance;
    //商品类型，1：金币，2：元宝，3：道具
    public int _type = 2;

    public List<ShopData> _shopItemDatas;

    public static GameObject create(int type)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ShopPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<ShopPanelScript>().SetType(type);
        return obj;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }   
    }

    // Use this for initialization
    void Start()
    {
        OtherData.s_shopPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopPanelScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopPanelScript", "Start", null, null);
            return;
        }

        Shop.SetActive(true);
        Vip.SetActive(false);


        InitVip();
        InitUserInfo();
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

    public void InitUserInfo()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopPanelScript", "InitUserInfo"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopPanelScript", "InitUserInfo", null, null);
            return;
        }

        UserGold.text = UserData.gold+"";
        UserYuanBao.text = UserData.yuanbao + "";
        UserMedal.text = UserData.medal + "";
    }

    public void SetType(int type)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopPanelScript", "SetType"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopPanelScript", "SetType", null, type);
            return;
        }

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

    public void InitVip()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopPanelScript", "InitVip"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopPanelScript", "InitVip", null, null);
            return;
        }

        int vipLevel = VipUtil.GetVipLevel(UserData.rechargeVip);
        int currentVipToTal = VipUtil.GetCurrentVipTotal(vipLevel);
        VipImage.sprite = Resources.Load<Sprite>("Sprites/Vip/shop_vip_" + vipLevel);

        var vipText = string.Format("累计充值" + "<color=#FF0000FF>{0}</color>" + ",即可升级到" + "<color=#FF0000FF>{1}</color>",
            currentVipToTal + "元", "贵族" + (vipLevel + 1));
        if (vipLevel >= 10)
        {
            vipText = string.Format("<color=#FF0000FF>{0}</color>", "贵族等级已满");
        }
        VipExplain.text = vipText;
        
        SlideText.text = UserData.rechargeVip + "/" + currentVipToTal;
        SliderVip.value = UserData.rechargeVip / (float) currentVipToTal;
    }


    public void Init()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopPanelScript", "Init"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopPanelScript", "Init", null, null);
            return;
        }

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
            LogUtil.Log(e.Message);
        }
    }

    public void onInitializeItem(GameObject go, int dataindex)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopPanelScript", "onInitializeItem"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopPanelScript", "onInitializeItem", null, go, dataindex);
            return;
        }

        ShopData shopItemData = _shopItemDatas[dataindex];
        string[] strings = shopItemData.props.Split(':');
        int id = Convert.ToInt32(strings[0]);

        Image goods_image = go.transform.Find("goods_image").GetComponent<Image>();
        Text goods_des = go.transform.Find("goods_des").GetComponent<Text>();
        GameObject goods_bg = go.transform.Find("goods_bg").gameObject;
        var Image_price_bg = goods_bg.transform.GetChild(0);
        var Image_price_bg1 = goods_bg.transform.GetChild(1);
        var goods_icon = Image_price_bg.transform.GetChild(0).GetComponent<Image>();
        var Text_Price = Image_price_bg.transform.GetChild(1).GetComponent<Text>();
        var goods_icon1 = Image_price_bg1.transform.GetChild(0).GetComponent<Image>();
        var Text_Price1 = Image_price_bg1.transform.GetChild(1).GetComponent<Text>();

        //首充
        Image Image_first_recharge = go.transform.Find("Image_first_recharge").GetComponent<Image>();
        GameObject Image_jiazeng = go.transform.Find("Image_jiazeng").gameObject;

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
            if (!string.IsNullOrEmpty(shopItemData.extra_reward))
            {
                Image_first_recharge.gameObject.SetActive(true);
                Image_jiazeng.gameObject.SetActive(true);

                Image_jiazeng.transform.GetChild(0).GetComponent<Text>().text = shopItemData.extra_reward.Split(':')[1] + "元宝";
            }
        }
        else
        {
            goods_image.sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/icon_jinbi");
        }

        //设置已经首充
        for (int i = 0; i < UserData.userRecharge.Count; i++)
        {
            if (UserData.userRecharge[i].goods_id == shopItemData.goods_id)
            {
                Image_first_recharge.gameObject.SetActive(false);
                Image_jiazeng.gameObject.SetActive(false);
            }
        }

        //设置第一个价格
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
            goods_icon.sprite = Resources.Load<Sprite>("Sprites/Icon/icon_rmb_white");
        }
        Text_Price.text = shopItemData.price + "";
        //设置第二个价格
        if (shopItemData.price2 > 0)
        {
            Image_price_bg1.gameObject.SetActive(true);
            goods_icon1.sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/icon_huizhang");

            Text_Price1.text = shopItemData.price2 + "";
        }
        else
        {
            Image_price_bg1.gameObject.SetActive(false);
        }


        Button button = go.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate() { BuyGoodsPanelScript.create(_shopItemDatas[dataindex].goods_id); });
    }

    public void onReceive_GetShop(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopPanelScript", "onReceive_GetShop"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopPanelScript", "onReceive_GetShop", null, data);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopPanelScript", "IsJinbiToggle"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopPanelScript", "IsJinbiToggle", null, IsClick);
            return;
        }

        if (IsClick)
        {
            _type = 1;
            Init();
        }
    }

    public void IsYuanBaoToggle(bool IsClick)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopPanelScript", "IsYuanBaoToggle"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopPanelScript", "IsYuanBaoToggle", null, IsClick);
            return;
        }

        if (IsClick)
        {
            _type = 2;
            Init();
        }
    }

    public void IsPropToggle(bool IsClick)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopPanelScript", "IsPropToggle"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopPanelScript", "IsPropToggle", null, IsClick);
            return;
        }

        if (IsClick)
        {
            _type = 3;
            Init();
        }
    }

    public void IsMedalToggle(bool IsClick)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopPanelScript", "IsMedalToggle"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopPanelScript", "IsMedalToggle", null, IsClick);
            return;
        }

        if (IsClick)
        {
            ToastScript.createToast("徽章兑换暂未开放");
            _type = 4;
            Init();
        }
    }

    public void OnClickVipHelp()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopPanelScript", "OnClickVipHelp"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopPanelScript", "OnClickVipHelp", null, null);
            return;
        }

        Shop.SetActive(false);
        Vip.SetActive(true);
    }

    public void OnClickVipBack()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShopPanelScript", "OnClickVipBack"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShopPanelScript", "OnClickVipBack", null, null);
            return;
        }

        Shop.SetActive(true);
        Vip.SetActive(false);
    }
}