using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyGoodsPanelScript : MonoBehaviour
{
    public MainScript m_mainScript = null;

    public Text m_text_goods_name;
    public Text m_text_goods_num;
    public Text m_text_goods_allPrice;
    public Image m_text_goods_icon;

    public Button m_button_jian;
    public Button m_button_jia;
    public Button m_button_max;
    public Button m_button_buy;
    public Text m_text_shiduyule;

    ShopData m_shopData = null;

    int m_goods_num = 1;
    int m_goods_buy_maxNum = 10;

    public static GameObject create(MainScript mainScript, int goods_id)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BuyGoodsPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<BuyGoodsPanelScript>().setGoodsId(goods_id);
        obj.GetComponent<BuyGoodsPanelScript>().m_mainScript = mainScript;

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

            // 道具图标
            {
                List<string> list_str = new List<string>();
                CommonUtil.splitStr(m_shopData.props, list_str, ':');
                LogUtil.Log(GameUtil.getPropIconPath(int.Parse(list_str[0])));
                CommonUtil.setImageSprite(m_text_goods_icon, GameUtil.getPropIconPath(int.Parse(list_str[0])));
            }

            refreshPrice();

            if (m_shopData.goods_type != 3)
            {
                m_text_goods_num.transform.localScale = new Vector3(0, 0, 0);

                m_button_jian.transform.localScale = new Vector3(0, 0, 0);
                m_button_jia.transform.localScale = new Vector3(0, 0, 0);
                m_button_max.transform.localScale = new Vector3(0, 0, 0);

                m_text_shiduyule.transform.localScale = new Vector3(1,1,1);
            }
            else
            {
                m_text_shiduyule.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

    void refreshPrice()
    {
        if (m_shopData.money_type == 1)
        {
            m_text_goods_allPrice.text = "金币：" + (m_shopData.price * m_goods_num).ToString();
        }
        else if (m_shopData.money_type == 2)
        {
            m_text_goods_allPrice.text = "元宝：" + (m_shopData.price * m_goods_num).ToString();
        }
        else if (m_shopData.money_type == 3)
        {
            m_text_goods_allPrice.text = "¥：" + (m_shopData.price * m_goods_num).ToString();
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

    public void onClickBuy()
    {
        int totalPrice = m_shopData.price * m_goods_num;
        switch (m_shopData.money_type)
        {
            case 1:
                if (UserData.gold < totalPrice)
                {
                    ToastScript.createToast("金币不足,请前去充值");
                }
                else
                {
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().setGoodsInfo(m_shopData.goods_id, m_goods_num);
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().CallBack = onReceive_BuyGoods;
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().OnRequest();

                }
                break;
            case 2:
                if (UserData.yuanbao < totalPrice)
                {
                    ToastScript.createToast("元宝不足,请前去充值");
                }
                else
                {
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().setGoodsInfo(m_shopData.goods_id, m_goods_num);
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().CallBack = onReceive_BuyGoods;
                    LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().OnRequest();

                }
                break;
            //人民币购买
            case 3:
                //ToastScript.createToast("元宝购买暂未开放,敬请期待");
                if (UserData.IsRealName)
                {
                    PayTypePanelScript.create(m_shopData);
                }
                else
                {
                    CommonExitPanelScript commonExit = CommonExitPanelScript.create().GetComponent<CommonExitPanelScript>();
                    commonExit.TextContent.text = "您还未实名,无法购买";
                    commonExit.ButtonClose.gameObject.SetActive(true);
                    commonExit.ButtonConfirm.onClick.AddListener(delegate()
                    {
                        RealNameScript.create();
                        Destroy(commonExit.gameObject);
                    });
                }

                break;
        }
    }

    public void GetPayResult(string data)
    {
        if ("支付成功".Equals(data))
        {
            LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().setGoodsInfo(m_shopData.goods_id, m_goods_num);
            LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().OnRequest();
        }
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