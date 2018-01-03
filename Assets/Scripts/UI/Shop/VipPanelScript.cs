using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VipPanelScript : MonoBehaviour
{
    public GameObject Content;
    public Image VipImage;
    public Image VipImage2;
    public Text VipText;
    public Text VipText2;
    public Text VipText3;
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

    public GameObject VipWeekOnce;
    public static List<VipData> vipDatas;
    public List<GameObject> VipWeekOnceChild = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        InitVip();
        if (vipDatas == null || vipDatas.Count == 0)
        {
            ToastScript.createToast("贵族奖励配置表返回失败");
            return;
        }

        int childCount = VipWeekOnce.transform.childCount;
        LogUtil.Log(childCount);

        for (int j = 0; j < childCount; j++)
        {
            Transform child = VipWeekOnce.transform.GetChild(j);
            VipWeekOnceChild.Add(child.gameObject);
        }


        for (int i = 0; i < 10; i++)
        {
            VipData vipData = vipDatas[i];
            var VipTab = Content.transform.GetChild(i).gameObject;
            VipTab.transform.Find("Text").GetComponent<Text>().text = "贵族" + (i + 1);
            VipTab.transform.Find("Text").GetComponent<Text>().font = Resources.Load<Font>("Fonts/STXINWEI");

            var toggle = VipTab.GetComponent<Toggle>();


            var i2 = i;
            toggle.onValueChanged.AddListener(delegate(bool isOn)
            {
                var h = i2;
                if (isOn)
                {
                    VipImage2.sprite = Resources.Load<Sprite>("Sprites/Vip/user_vip_" + vipData.vipLevel);
                    if (vipData.medalNum > 0)
                    {
                        VipText2.gameObject.SetActive(true);
                    }
                    else
                    {
                        VipText2.gameObject.SetActive(false);
                    }

                    if (vipData.turnTableCount > 0)
                    {
                        VipText3.gameObject.SetActive(true);
                    }
                    else
                    {
                        VipText3.gameObject.SetActive(false);
                    }

                  
                    string temp = string.Format(@"比赛场第一名额外        *{0}",vipData.medalNum);
                    VipText2.text = temp;
                    string temp2 = string.Format(@",每日转盘免费次数加{0}", vipData.turnTableCount);
                    VipText3.text = temp2;

                    VipWeekOnceChild[0].transform.GetChild(0).GetComponent<Text>().text = "*" + vipData.vipOnce.goldNum;
                    string[] props = vipData.vipOnce.prop.Split(';');
                    foreach (var child in VipWeekOnceChild)
                    {
                        child.SetActive(false);
                    }

                    for (int j = 0; j <= props.Length; j++)
                    {
                        VipWeekOnceChild[j].SetActive(true);
                    }


                    //处理ui居中
                    if (h <= 2)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            Vector3 current = VipWeekOnceChild[j].transform.localPosition;
                            VipWeekOnceChild[j].transform.localPosition = new Vector3(current.x, -39f, current.z);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            Vector3 current = VipWeekOnceChild[j].transform.localPosition;
                            VipWeekOnceChild[j].transform.localPosition = new Vector3(current.x, -12.5f, current.z);
                        }
                    }
                    if (h == 0)
                    {
                        VipText.transform.localPosition = new Vector3(0, 1.7f, 0);
                    }
                    else
                    {
                        VipText.transform.localPosition = new Vector3(0, 31.5f, 0);
                    }

                    if (h >= 3 && h <= 6)
                    {
                        Vector3 current = VipWeekOnceChild[2].transform.localPosition;
                        VipWeekOnceChild[2].transform.localPosition = new Vector3(0f, current.y, current.z);
                    }
                    else
                    {
                        Vector3 current = VipWeekOnceChild[2].transform.localPosition;
                        VipWeekOnceChild[2].transform.localPosition = new Vector3(-104.2f, current.y, current.z);
                    }

                    for (int j = 0; j < props.Length; j++)
                    {
                        var prop = props[j];
                        GameObject go = VipWeekOnceChild[j + 1];
                        Image propImage = go.GetComponent<Image>();
                        Text propNum = go.transform.GetChild(0).GetComponent<Text>();
                        string[] strings = prop.Split(':');
                        int propId = Convert.ToInt32(strings[0]);
                        propNum.text = "*" + strings[1];
                        CommonUtil.setImageSprite(propImage, GameUtil.getPropIconPath(propId));
                    }

                    //设置会员每周领取奖励
                    WeeklyGoldNumText.text = "*" + vipData.vipWeekly.goldNum;
                    WeeklyDiamondNumText.text = "*" + vipData.vipWeekly.diamondNum;
                }
            });

            if (i == VipUtil.GetVipLevel(UserData.rechargeVip) - 1)
            {
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }

            if (VipUtil.GetVipLevel(UserData.rechargeVip) == 0)
            {
                if (i == 0)
                {
                    toggle.isOn = true;
                }
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