using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoScript : MonoBehaviour {
    public Text nickName;
    public Text account;
    public Text gold;
    public Text yuanBaoCount;
    public Text shengLv;
    public Text zongDuiJu;
    public Text taoPaoLv;
    public Text meiLiZhi;
    public Image userImage;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/UserInfoPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

        return obj;
    }

    private void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        nickName.text = UserData.name;
        account.text = UserData.name;
        gold.text = UserData.gold.ToString();
        yuanBaoCount.text = UserData.yuanbao.ToString();
        shengLv.text = "100%";
        zongDuiJu.text = "0";
        taoPaoLv.text = "0%";
        meiLiZhi.text = "100";

        userImage.sprite = Resources.Load<Sprite>("Sprites/Head/head_1");
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
