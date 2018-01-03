using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyUIScript : MonoBehaviour {

    public GameObject m_headIcon;
    public Text m_textName;
    public Text m_textGoldNum;
    public Image m_imageZhuangJiaIcon;
    public Image m_imageZhuanVipLevel;
    public Image m_nickName_bg;

    public string m_uid;

    // Use this for initialization
    void Start ()
    {
        int vipLevel = VipUtil.GetVipLevel(UserData.rechargeVip);
        CommonUtil.setImageSprite(m_imageZhuanVipLevel, "Sprites/Vip/user_vip_" + vipLevel);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setHead(string headPath)
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load("Sprites/Head/head_1", typeof(Sprite)) as Sprite;
    }

    public void setName(string name)
    {
        m_textName.text = name;
        GameUtil.setNickNameFontColor(m_textName, UserData.vipLevel);
    }

    public void setGoldNum(int goldNum)
    {
        m_textGoldNum.text = goldNum.ToString();
    }

    public void onClickHead()
    {
        GameUserInfoPanelScript.create(UserData.uid);
    }
}
