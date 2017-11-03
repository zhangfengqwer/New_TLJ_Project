using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_hudong_Scroll_Script : MonoBehaviour {

    public Image m_icon;
    public GameUserInfoPanelScript m_parentScript;

    public Text m_text_price;

    HuDongProp m_huDongProp;

    // Use this for initialization
    void Start()
    {

    }

    public void setHuDongPropData(HuDongProp huDongProp)
    {
        m_huDongProp = huDongProp;

        {
            CommonUtil.setImageSprite(m_icon, GameUtil.getPropIconPath(m_huDongProp.m_id));
            m_text_price.text = "金币x" + m_huDongProp.m_price;
        }
    }

    public HuDongProp getHuDongPropData()
    {
        return m_huDongProp;
    }

    public void onClickItem()
    {
        Debug.Log(gameObject.transform.name);

        ToastScript.createToast("暂未开放");
    }
}
