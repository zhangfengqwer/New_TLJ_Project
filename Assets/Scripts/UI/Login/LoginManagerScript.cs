using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManagerScript : MonoBehaviour {
    public GameObject EnterLoginPanel;
    public GameObject LoginPanel;
    public GameObject RegisterPanel;

    public void OnEnterLoginClick()
    {
        EnterLoginPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    public void OnEnterRegisterClick()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }

    /// <summary>
    /// 点击发送验证码
    /// </summary>
    public void OnSendIDCodeClick()
    {
        print("发送验证码");
    }
}
