using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyGoodsPanelScript : MonoBehaviour
{
    public Image m_buyCount;
    public Text m_text_goods_name;
    public Text m_text_goods_desc;

    public Text m_text_goods_num;

    //public Text m_text_goods_allPrice;
    public Image m_text_goods_icon;

    public Button m_button_jian;
    public Button m_button_jia;
    public Button m_button_max;
    public Button m_button_buy1;
    public Button m_button_buy2;
    public Text m_text_shiduyule;

    ShopData m_shopData = null;

    int m_goods_num = 1;
    int m_goods_buy_maxNum = 10;

    int m_money_type;

    public static GameObject create(int goods_id)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BuyGoodsPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<BuyGoodsPanelScript>().setGoodsId(goods_id);

        return obj;
    }

    // Use this for initialization
    void Start()
    {
        m_button_jian.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setGoodsId(int goods_id)
    {
        m_shopData = ShopDataScript.getInstance().getShopDataById(goods_id);

        if (m_shopData != null)
        {
            m_text_goods_name.text = m_shopData.goods_name;

            m_text_goods_num.text = m_goods_num.ToString();

            List<string> list_str = new List<string>();
            CommonUtil.splitStr(m_shopData.props, list_str, ':');
            int prop_id = int.Parse(list_str[0]);

            // 道具图标
            {
                
                CommonUtil.setImageSprite(m_text_goods_icon, GameUtil.getPropIconPath(prop_id));
            }

            // 道具描述
            {
                if ((prop_id != 1) && (prop_id != 2))
                {
                    PropInfo propInfo = PropData.getInstance().getPropInfoById(prop_id);
                    if (propInfo != null)
                    {
                        m_text_goods_desc.text = propInfo.m_desc;
                    }
                }
            }

            refreshPrice();

            if (m_shopData.goods_type != 3)
            {
                m_buyCount.transform.localScale = new Vector3(0, 0, 0);

                m_text_shiduyule.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                m_text_shiduyule.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

    void refreshPrice()
    {
        if (m_shopData.price != 0)
        {
            if (m_shopData.money_type == 1)
            {
                CommonUtil.setImageSprite(m_button_buy1.transform.Find("Image").GetComponent<Image>(),
                    GameUtil.getPropIconPath(1));
                m_button_buy1.transform.Find("Text_price").GetComponent<Text>().text =
                    (m_shopData.price * m_goods_num).ToString();
                if ((m_shopData.price * m_goods_num).ToString().Length >= 5)
                {
                    m_button_buy1.transform.Find("Text_price").GetComponent<Text>().fontSize = 23;
                }
                m_button_buy1.transform.Find("Text_money_type").GetComponent<Text>().text = "金币购买";
            }
            else if (m_shopData.money_type == 2)
            {
                CommonUtil.setImageSprite(m_button_buy1.transform.Find("Image").GetComponent<Image>(),
                    GameUtil.getPropIconPath(2));
                m_button_buy1.transform.Find("Text_price").GetComponent<Text>().text =
                    (m_shopData.price * m_goods_num).ToString();
                if ((m_shopData.price * m_goods_num).ToString().Length >= 5)
                {
                    m_button_buy1.transform.Find("Text_price").GetComponent<Text>().fontSize = 23;
                }
                m_button_buy1.transform.Find("Text_money_type").GetComponent<Text>().text = "元宝购买";
            }
            else if (m_shopData.money_type == 3)
            {
                CommonUtil.setImageSprite(m_button_buy1.transform.Find("Image").GetComponent<Image>(),
                    GameUtil.getPropIconPath(3));
                m_button_buy1.transform.Find("Text_price").GetComponent<Text>().text =
                    (m_shopData.price * m_goods_num).ToString();
                if ((m_shopData.price * m_goods_num).ToString().Length >= 5)
                {
                    m_button_buy1.transform.Find("Text_price").GetComponent<Text>().fontSize = 23;
                }
                m_button_buy1.transform.Find("Text_money_type").GetComponent<Text>().text = "购买";
                m_button_buy1.transform.Find("Image").localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
            else if (m_shopData.money_type == 4)
            {
                CommonUtil.setImageSprite(m_button_buy1.transform.Find("Image").GetComponent<Image>(),
                    GameUtil.getPropIconPath((int) TLJCommon.Consts.Prop.Prop_huizhang));
                m_button_buy1.transform.Find("Text_price").GetComponent<Text>().text =
                    (m_shopData.price * m_goods_num).ToString();
                if ((m_shopData.price * m_goods_num).ToString().Length >= 5)
                {
                    m_button_buy1.transform.Find("Text_price").GetComponent<Text>().fontSize = 23;
                }
                m_button_buy1.transform.Find("Text_money_type").GetComponent<Text>().text = "徽章购买";
            }
        }
        else
        {
            GameUtil.hideGameObject(m_button_buy1.gameObject);
        }

        if (m_shopData.price2 != 0)
        {
            if (m_shopData.money_type2 == 1)
            {
                CommonUtil.setImageSprite(m_button_buy2.transform.Find("Image").GetComponent<Image>(),
                    GameUtil.getPropIconPath(1));
                m_button_buy2.transform.Find("Text_price").GetComponent<Text>().text =
                    (m_shopData.price2 * m_goods_num).ToString();
                if ((m_shopData.price2 * m_goods_num).ToString().Length >= 5)
                {
                    m_button_buy2.transform.Find("Text_price").GetComponent<Text>().fontSize = 23;
                }
                m_button_buy2.transform.Find("Text_money_type").GetComponent<Text>().text = "金币购买";
            }
            else if (m_shopData.money_type2 == 2)
            {
                CommonUtil.setImageSprite(m_button_buy2.transform.Find("Image").GetComponent<Image>(),
                    GameUtil.getPropIconPath(2));
                m_button_buy2.transform.Find("Text_price").GetComponent<Text>().text =
                    (m_shopData.price2 * m_goods_num).ToString();
                if ((m_shopData.price2 * m_goods_num).ToString().Length >= 5)
                {
                    m_button_buy2.transform.Find("Text_price").GetComponent<Text>().fontSize = 23;
                }
                m_button_buy2.transform.Find("Text_money_type").GetComponent<Text>().text = "元宝购买";
            }
            else if (m_shopData.money_type2 == 3)
            {
                CommonUtil.setImageSprite(m_button_buy2.transform.Find("Image").GetComponent<Image>(),
                    GameUtil.getPropIconPath(3));
                m_button_buy2.transform.Find("Text_price").GetComponent<Text>().text =
                    (m_shopData.price2 * m_goods_num).ToString();
                if ((m_shopData.price2 * m_goods_num).ToString().Length >= 5)
                {
                    m_button_buy2.transform.Find("Text_price").GetComponent<Text>().fontSize = 23;
                }
                m_button_buy2.transform.Find("Text_money_type").GetComponent<Text>().text = "购买";
                m_button_buy2.transform.Find("Image").localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
            else if (m_shopData.money_type2 == 4)
            {
                CommonUtil.setImageSprite(m_button_buy2.transform.Find("Image").GetComponent<Image>(),
                    GameUtil.getPropIconPath((int) TLJCommon.Consts.Prop.Prop_huizhang));
                m_button_buy2.transform.Find("Text_price").GetComponent<Text>().text =
                    (m_shopData.price2 * m_goods_num).ToString();
                if ((m_shopData.price2 * m_goods_num).ToString().Length >= 5)
                {
                    m_button_buy2.transform.Find("Text_price").GetComponent<Text>().fontSize = 23;
                }
                m_button_buy2.transform.Find("Text_money_type").GetComponent<Text>().text = "徽章购买";
            }
        }
        else
        {
            m_button_buy1.transform.localPosition = new Vector3(0, -183.1f, 0);
            GameUtil.hideGameObject(m_button_buy2.gameObject);
        }
    }

    public void onClickJian()
    {
        m_button_jia.interactable = true;

        if ((--m_goods_num) == 1)
        {
            m_button_jian.interactable = false;
        }

        m_text_goods_num.text = m_goods_num.ToString();

        refreshPrice();
    }

    public void onClickJia()
    {
        m_button_jian.interactable = true;

        if ((++m_goods_num) == m_goods_buy_maxNum)
        {
            m_button_jia.interactable = false;
        }

        m_text_goods_num.text = m_goods_num.ToString();

        refreshPrice();
    }

    public void onClickMax()
    {
        m_goods_num = m_goods_buy_maxNum;
        m_text_goods_num.text = m_goods_num.ToString();

        m_button_jian.interactable = true;
        m_button_jia.interactable = false;

        refreshPrice();
    }

    public void onClickBuy1()
    {
        if (m_shopData.money_type == 4)
        {
            // 判断是否设置过徽章密码
            {
                if (!UserData.isSetSecondPsw)
                {
                    SetSecondPswPanelScript.create();
                    ToastScript.createToast("请先设置徽章密码");

                    return;
                }
            }

            // 校验徽章密码
            {
                if (!OtherData.s_hasCheckSecondPSW)
                {
                    CheckSecondPSWPanelScript.create();

                    return;
                }
            }
        }

        m_money_type = m_shopData.money_type;
        buy(m_shopData.money_type);
    }

    public void onClickBuy2()
    {
        if (m_shopData.money_type2 == 4)
        {
            // 判断是否设置过徽章密码
            {
                if (!UserData.isSetSecondPsw)
                {
                    SetSecondPswPanelScript.create();
                    ToastScript.createToast("请先设置徽章密码");

                    return;
                }
            }

            // 校验徽章密码
            {
                if (!OtherData.s_hasCheckSecondPSW)
                {
                    CheckSecondPSWPanelScript.create();

                    return;
                }
            }
        }
        else
        {
            LogUtil.Log("不是会长");
        }

        m_money_type = m_shopData.money_type2;
        buy(m_shopData.money_type2);
    }

    void buy(int money_type)
    {
        int totalPrice = 0;

        if (money_type == m_shopData.money_type)
        {
            totalPrice = m_shopData.price * m_goods_num;
        }
        else if (money_type == m_shopData.money_type2)
        {
            totalPrice = m_shopData.price2 * m_goods_num;
        }

        switch (money_type)
        {
            case 1:
                if (UserData.gold < totalPrice)
                {
                    ToastScript.createToast("金币不足,请前去充值");
                    return;
                }
                else
                {
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>()
                        .setGoodsInfo(m_shopData.goods_id, m_goods_num, money_type);
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().CallBack = onReceive_BuyGoods;
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().OnRequest();
                }
                break;

            case 2:
                if (UserData.yuanbao < totalPrice)
                {
                    ToastScript.createToast("元宝不足,请前去充值");
                    return;
                }
                else
                {
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>()
                        .setGoodsInfo(m_shopData.goods_id, m_goods_num, money_type);
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().CallBack = onReceive_BuyGoods;
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().OnRequest();
                }
                break;

            //人民币购买
            case 3:
                if (!OtherData.s_canRecharge)
                {
                    ToastScript.createToast("元宝购买暂未开放,敬请期待");
                    return;
                }

                if (UserData.IsRealName)
                {
                    if (!ChannelHelper.Is3RdLogin())
                    {
                        PayTypePanelScript.create(m_shopData);
                    }
                    else
                    {
                        PlatformHelper.pay(PlatformHelper.GetChannelName(), "AndroidCallBack", "GetPayResult", SetRequest(m_shopData));
                    }
                    Destroy(this.gameObject);
                }
                else
                {
                    CommonExitPanelScript commonExit =
                        CommonExitPanelScript.create().GetComponent<CommonExitPanelScript>();
                    commonExit.TextContent.text = "您还未实名,无法购买";
                    commonExit.ButtonClose.gameObject.SetActive(true);
                    commonExit.ButtonConfirm.onClick.AddListener(delegate()
                    {
                        RealNameScript.create();
                        Destroy(commonExit.gameObject);
                    });
                }
                break;

            case 4:
                if (UserData.medal < totalPrice)
                {
                    ToastScript.createToast("徽章不足,无法购买");
                    return;
                }
                else
                {
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>()
                        .setGoodsInfo(m_shopData.goods_id, m_goods_num, money_type);
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().CallBack = onReceive_BuyGoods;
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().OnRequest();
                }
                break;
        }
    }

    private string SetRequest(ShopData shopData)
    {
        JsonData data = new JsonData();
        data["uid"] = UserData.uid;
        data["goods_id"] = shopData.goods_id;
        data["goods_num"] = 1;
        data["goods_name"] = shopData.goods_name;
        data["price"] = shopData.price;
        return data.ToJson();
    }


    public void onReceive_BuyGoods(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int) jd["code"];

        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            if (m_shopData.goods_type == 1 || m_shopData.goods_type == 2)
            {
                ToastScript.createToast("购买成功");
            }
            else
            {
                ToastScript.createToast("购买成功,请去背包中查看");
            }

            //更新背包数据
            LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().CallBack = MainScript.onReceive_GetUserBag;
            LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().OnRequest();
            LogicEnginerScript.Instance.GetComponent<GetUserInfoRequest>().OnRequest();
            Destroy(gameObject);
        }
        else
        {
            ToastScript.createToast("购买失败");
        }
    }
}