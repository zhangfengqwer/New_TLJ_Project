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
    public Text m_textFuWuFei;

    public string m_uid;

    // Use this for initialization
    void Start ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MyUIScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MyUIScript_hotfix", "Start", null, null);
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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MyUIScript_hotfix", "setHead"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MyUIScript_hotfix", "setHead", null, headPath);
            return;
        }

        gameObject.GetComponent<Image>().sprite = Resources.Load("Sprites/Head/head_1", typeof(Sprite)) as Sprite;
    }

    public void setName(string name)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MyUIScript_hotfix", "setName"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MyUIScript_hotfix", "setName", null, name);
            return;
        }

        m_textName.text = name;
        GameUtil.setNickNameFontColor(m_textName, UserData.vipLevel);
    }

    public void setGoldNum(int goldNum)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MyUIScript_hotfix", "setGoldNum"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MyUIScript_hotfix", "setGoldNum", null, goldNum);
            return;
        }

        m_textGoldNum.text = goldNum.ToString();
    }

    public void onClickHead()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MyUIScript_hotfix", "onClickHead"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MyUIScript_hotfix", "onClickHead", null, null);
            return;
        }

        GameUserInfoPanelScript.create(UserData.uid);
    }
}
