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
    public GameObject headIcon;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/UserInfoPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    private void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        nickName.text = UserData.name;
        account.text = UserData.uid;
        gold.text = UserData.gold.ToString();
        yuanBaoCount.text = UserData.yuanbao.ToString();
        meiLiZhi.text = UserData.gameData.meiliZhi+"";

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
        print("绑定手机");
    }

    public void OnRealNameClick()
    {
        print("实名认证");
    }
}