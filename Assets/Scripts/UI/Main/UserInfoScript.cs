using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoScript : MonoBehaviour {
    public Text nickName;
    public Text account;
    public Text coinCount;
    public Text yuanBaoCount;
    public Text shengLv;
    public Image userImage;

    private void Start()
    {
        nickName.text = "zfffff";
        account.text = "zhangfengqer";
        coinCount.text = 5000.ToString();
        yuanBaoCount.text = 20.ToString();
        shengLv.text = "23.21%";
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
