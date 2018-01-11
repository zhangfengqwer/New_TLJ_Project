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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MyUIScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MyUIScript", "Start", null, null);
            return;
        }

        int vipLevel = VipUtil.GetVipLevel(UserData.rechargeVip);
        CommonUtil.setImageSprite(m_imageZhuanVipLevel, "Sprites/Vip/user_vip_" + vipLevel);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setHead(string headPath)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MyUIScript", "setHead"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MyUIScript", "setHead", null, headPath);
            return;
        }

        gameObject.GetComponent<Image>().sprite = Resources.Load("Sprites/Head/head_1", typeof(Sprite)) as Sprite;
    }

    public void setName(string name)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MyUIScript", "setName"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MyUIScript", "setName", null, name);
            return;
        }

        m_textName.text = name;
        GameUtil.setNickNameFontColor(m_textName, UserData.vipLevel);
    }

    public void setGoldNum(int goldNum)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MyUIScript", "setGoldNum"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MyUIScript", "setGoldNum", null, goldNum);
            return;
        }

        m_textGoldNum.text = goldNum.ToString();
    }

    public void onClickHead()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MyUIScript", "onClickHead"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MyUIScript", "onClickHead", null, null);
            return;
        }

        GameUserInfoPanelScript.create(UserData.uid);
    }
}
