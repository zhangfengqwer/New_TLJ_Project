using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VipPanelScript : MonoBehaviour
{
    public GameObject Content;
    public Image VipImage;
    public Text MedalNumText;
    public Text OnceGoldNumText;
    public Image OncePropImage;
    public Text OncePropNumText;
    public Image MyVipImage;
    public Text VipExplain;
    public Text SlideText;
    public Slider SliderVip;

    public Text WeeklyGoldNumText;
    public Text WeeklyDiamondNumText;
    public static List<VipData> vipDatas;

    // Use this for initialization
    void Start()
    {

        InitVip();
        if (vipDatas == null || vipDatas.Count == 0)
        {
            ToastScript.createToast("贵族奖励配置表返回失败");
            return;
        }
       

        for (int i = 0; i < 10; i++)
        {
            VipData vipData = vipDatas[i];
            var VipTab = Content.transform.GetChild(i).gameObject;
            VipTab.transform.Find("Text").GetComponent<Text>().text = "贵族" + (i + 1);
            VipTab.transform.Find("Text").GetComponent<Text>().font = Resources.Load<Font>("Fonts/FANGZHENGKATONG");

            var toggle = VipTab.GetComponent<Toggle>();

            toggle.onValueChanged.AddListener(delegate(bool isOn)
            {
                if (isOn)
                {
                    VipImage.sprite = Resources.Load<Sprite>("Sprites/Vip/user_vip_" + vipData.vipLevel);
                    MedalNumText.text = "*" + vipData.medalNum;
                    //设置会员一次领取奖励
                    OnceGoldNumText.text = "*" + vipData.vipOnce.goldNum;
                    var props = vipData.vipOnce.prop.Split(':');
                    int propId = Convert.ToInt32(props[0]);
                    OncePropNumText.text = "*" + props[1];
                    CommonUtil.setImageSprite(OncePropImage, GameUtil.getPropIconPath(propId));
                    //设置会员每周领取奖励
                    WeeklyGoldNumText.text = "*" + vipData.vipWeekly.goldNum;
                    WeeklyDiamondNumText.text = "*" + vipData.vipWeekly.diamondNum;
                }
            });

            if (i == 0)
            {
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }
        }
    }

    private void InitVip()
    {
        int vipLevel = VipUtil.GetVipLevel(UserData.rechargeVip);
        int currentVipToTal = VipUtil.GetCurrentVipTotal(vipLevel);
        MyVipImage.sprite = Resources.Load<Sprite>("Sprites/Vip/shop_vip_" + vipLevel);

        var vipText = string.Format("累计充值" + "<color=#FF0000FF>{0}</color>" + ",即可升级到" + "<color=#FF0000FF>{1}</color>",
            currentVipToTal + "元", "贵族" + (vipLevel + 1));
        if (vipLevel >= 10)
        {
            vipText = string.Format("<color=#FF0000FF>{0}</color>", "贵族等级已满");
        }
        VipExplain.text = vipText;

        SlideText.text = UserData.rechargeVip + "/" + currentVipToTal;
        SliderVip.value = UserData.rechargeVip / (float)currentVipToTal;
    }
}