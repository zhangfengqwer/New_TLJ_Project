﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyGoodsPanelScript : MonoBehaviour {

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

    public static GameObject create(int goods_id)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BuyGoodsPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

        obj.GetComponent<BuyGoodsPanelScript>().setGoodsId(goods_id);

        return obj;
    }

    // Use this for initialization
    void Start () {
		
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
            m_text_goods_allPrice.text = (m_shopData.price * m_goods_num).ToString();

            if (m_shopData.goods_type != 3)
            {
                m_text_goods_num.transform.localScale = new Vector3(0, 0, 0);

                m_button_jian.transform.localScale = new Vector3(0,0,0);
                m_button_jia.transform.localScale = new Vector3(0, 0, 0);
                m_button_max.transform.localScale = new Vector3(0, 0, 0);
            }
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
    }

    public void onClickJia()
    {
        m_button_jian.interactable = true;

        if ((++m_goods_num) == m_goods_buy_maxNum)
        {
            m_button_jia.interactable = false;
        }

        m_text_goods_num.text = m_goods_num.ToString();
    }

    public void onClickMax()
    {
        m_goods_num = m_goods_buy_maxNum;
        m_text_goods_num.text = m_goods_num.ToString();

        m_button_jia.interactable = false;
    }

    public void onClickBuy()
    {
        ToastScript.createToast("暂时不可以购买");
    }
}
