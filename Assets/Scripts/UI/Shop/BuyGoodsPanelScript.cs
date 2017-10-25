﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyGoodsPanelScript : MonoBehaviour {

    public MainScript m_mainScript = null;

    public Text m_text_goods_name;
    public Text m_text_goods_num;
    public Text m_text_goods_allPrice;
    public Image m_text_goods_icon;

    public Button m_button_jian;
    public Button m_button_jia;
    public Button m_button_max;
    public Button m_button_buy;
    
    ShopData m_shopData = null;

    int m_goods_num = 1;
    int m_goods_buy_maxNum = 10;

    public static GameObject create(MainScript mainScript,int goods_id)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BuyGoodsPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<BuyGoodsPanelScript>().setGoodsId(goods_id);
        obj.GetComponent<BuyGoodsPanelScript>().m_mainScript = mainScript;

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        m_button_jian.interactable = false;
    }
	
	// Update is called once per frame
	void Update ()
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
                Debug.Log(GameUtil.getPropIconPath(int.Parse(list_str[0])));
                CommonUtil.setImageSprite(m_text_goods_icon, GameUtil.getPropIconPath(int.Parse(list_str[0])));
            }

            refreshPrice();

            if (m_shopData.goods_type != 3)
            {
                m_text_goods_num.transform.localScale = new Vector3(0, 0, 0);

                m_button_jian.transform.localScale = new Vector3(0,0,0);
                m_button_jia.transform.localScale = new Vector3(0, 0, 0);
                m_button_max.transform.localScale = new Vector3(0, 0, 0);
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
        switch (m_shopData.money_type)
        {
            case 1:
            case 2:
                LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().setGoodsInfo(m_shopData.goods_id, m_goods_num);
                LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().CallBack = onReceive_BuyGoods;
                LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>().OnRequest();
                break;
            //人民币购买
            case 3:
                PlatformHelper.pay(this.gameObject.name, "GetPayResult", "unity传给android的支付数据");
                break;
        }
    }

    public void GetPayResult(string data)
    {
        ToastScript.createToast("android回传的数据:" + data);
    }


    public void onReceive_BuyGoods(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            ToastScript.createToast("购买成功:" + m_shopData.goods_name);

            {
                List<string> list_str1 = new List<string>();
                CommonUtil.splitStr(m_shopData.props, list_str1, ';');

                for (int i = 0; i < list_str1.Count; i++)
                {
                    List<string> list_str2 = new List<string>();
                    CommonUtil.splitStr(list_str1[i], list_str2, ':');

                    int prop_id = int.Parse(list_str2[0]);
                    int prop_num = int.Parse(list_str2[1]);

                    // 金币
                    if (prop_id == 1)
                    {
                        UserData.gold += prop_num;

                        if (m_mainScript != null)
                        {
                            m_mainScript.refreshUI();
                        }
                    }
                    // 元宝
                    else if (prop_id == 2)
                    {
                        UserData.yuanbao += prop_num;

                        if (m_mainScript != null)
                        {
                            m_mainScript.refreshUI();
                        }
                    }
                }
            }
        }
    }
}
