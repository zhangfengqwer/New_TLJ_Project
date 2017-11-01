using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoScript : MonoBehaviour
{
    public Text nickName;
    public Text account;
    public Text gold;
    public Text yuanBaoCount;
    public Text shengLv;
    public Text allGame;
    public Text taoPaoLv;
    public Text meiLiZhi;
    public Image Vip;
    public GameObject headIcon;
    public GameObject AlReadyRN;
    public Button ButtonRealName;
    public Button ButtonBindPhone;
    public Button ButtonChangePhone;
    public static UserInfoScript Instance;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/UserInfoPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    private void Start()
    {
        Instance = this;
        InitUI();
    }

    public void InitUI()
    {
        nickName.text = UserData.name;
        account.text = UserData.uid;
        gold.text = UserData.gold.ToString();
        yuanBaoCount.text = UserData.yuanbao.ToString();
        meiLiZhi.text = UserData.gameData.meiliZhi+"";
        int vipLevel = CommonUtil.GetVipLevel(UserData.rechargeVip);

        Vip.sprite = Resources.Load<Sprite>("Sprites/Vip/user_vip_" + vipLevel);
        if (UserData.IsRealName)
        {
            ButtonRealName.interactable = false;
            ButtonRealName.transform.localScale = Vector3.zero;
            AlReadyRN.transform.localScale = Vector3.one;

        }
        else
        {
            ButtonRealName.interactable = true;
            ButtonRealName.transform.localScale = Vector3.one;
            AlReadyRN.transform.localScale = Vector3.zero;
        }

        if (string.IsNullOrEmpty(UserData.phone))
        {
            ButtonBindPhone.interactable = true;
            ButtonBindPhone.transform.localScale = Vector3.one;
            ButtonChangePhone.transform.localScale = Vector3.zero;
        }
        else
        {
            ButtonBindPhone.interactable = false;
            ButtonBindPhone.transform.localScale = Vector3.zero;
            ButtonChangePhone.transform.localScale = Vector3.one;
        }


        headIcon.GetComponent<HeadIconScript>().setIcon(UserData.head);

        if (UserData.gameData.allGameCount == 0)
        {
            shengLv.text = "0%";
            allGame.text = "0";
            taoPaoLv.text = "0%";
        }
        else
        {
            //默认为保留两位
            shengLv.text = String.Format("{0:F}", (UserData.gameData.winCount / (float)UserData.gameData.allGameCount) * 100) + "%";
            allGame.text = UserData.gameData.allGameCount+"";
            taoPaoLv.text = String.Format("{0:F}", (UserData.gameData.runCount / (float)UserData.gameData.allGameCount) * 100) + "%"; ;
        }
        
        headIcon.GetComponent<HeadIconScript>().setIcon(UserData.head);
    }

    public void OnCloseClick()
    {
        Destroy(this.gameObject);
    }

    public void OnBindPhoneClick()
    {
        BindPhoneScript.create();
    }

    public void OnRealNameClick()
    {
       RealNameScript.create();
    }
}