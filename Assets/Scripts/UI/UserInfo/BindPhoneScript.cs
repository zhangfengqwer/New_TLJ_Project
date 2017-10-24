﻿using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using LitJson;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

public class BindPhoneScript : MonoBehaviour
{
    public InputField PhoneField;
    public InputField VerificationCodeField;
    private bool _isCorrectPhone;
    private bool _isCorrectCode;
    private string _phoneNum;
    private string _verificationCode;
    public Button ButtonSendSms;
    private int time = 20;
    public static int totalTime;
    private bool IsStartTime;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BindPhonePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }
    private float nextTime = 1;//一秒之后执行
    private Text textSend;

    private void Timer1()
    {
        if (nextTime <= Time.time)
        {
            textSend.text = string.Format("{0:d2}",totalTime % 60);
            nextTime = Time.time + 1;//到达一秒后加1
            if (totalTime <= 0)
            {
                IsStartTime = false;
                ButtonSendSms.interactable = true;
                textSend.text = "发送";
            }
        }
    }

    private void Update()
    {
        if (IsStartTime)
        {
            Timer1();
        }
    }

    // Use this for initialization
    void Start()
    {
        PhoneField.onEndEdit.AddListener(delegate { GetPhoneNum(PhoneField); });
        VerificationCodeField.onEndEdit.AddListener(delegate { GetVerificationCode(VerificationCodeField); });
        textSend = ButtonSendSms.transform.Find("Text_Send").GetComponent<Text>();

        if (totalTime > 0)
        {
            textSend.text = string.Format("{0:d2}", totalTime % 60);
            ButtonSendSms.interactable = false;
            IsStartTime = true;
        }
    }

    public void GetPhoneNum(InputField input)
    {
        _isCorrectPhone = VerifyRuleUtil.CheckPhone(input.text);
        print(_isCorrectPhone);
        if (_isCorrectPhone)
        {
            _phoneNum = input.text;
        }
    }

    public void GetVerificationCode(InputField input)
    {
        _isCorrectCode = VerifyRuleUtil.CheckVerificationCode(input.text);
        print(_isCorrectCode);
        if (_isCorrectCode)
        {
            _verificationCode = input.text;
        }
    }

    public void OnClickBindPhone()
    {
        if (_isCorrectPhone && _isCorrectCode)
        {
            LogicEnginerScript.Instance.GetComponent<CheckSmsRequest>().CallBack = bindPhoneCallBack;
            LogicEnginerScript.Instance.GetComponent<CheckSmsRequest>().OnRequest(_phoneNum, _verificationCode);
        }
        else
        {
            ToastScript.createToast("输入的信息有误");
        }
    }

    public void OnClickSendCode()
    {
        if (_isCorrectPhone)
        {
            LogicEnginerScript.Instance.GetComponent<SendVerificationCodeRequest>().CallBack =
                sendVerificationCodeCallBack;
            LogicEnginerScript.Instance.GetComponent<SendVerificationCodeRequest>().OnRequest(_phoneNum);

        }
        else
        {
            ToastScript.createToast("请输入正确的手机号");
        }
    }

    //发送验证码
    private void sendVerificationCodeCallBack(string data)
    {
        try
        {
            JsonData jsonData = JsonMapper.ToObject(data);
            var result = (string) jsonData["result"];

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            XmlNodeList nodeList = xmlDoc.ChildNodes;
            XmlNode xmlNode = nodeList[1];
            XmlNodeList xmlNodeChildNodes = xmlNode.ChildNodes;
            foreach (XmlNode nodeChild in xmlNodeChildNodes)
            {
                print(nodeChild.Name + ":" + nodeChild.InnerText);
                string value = nodeChild.InnerText;
                if (nodeChild.Name.Equals("ResultCode"))
                {
                    //发送验证码成功
                    if (value.Equals("1"))
                    {
                        totalTime = time;
                        IsStartTime = true;
                        ButtonSendSms.interactable = false;
                    }
                }else if (nodeChild.Name.Equals("ResultMessageDetails"))
                {
                    ToastScript.createToast(value);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    //绑定手机回调
    private void bindPhoneCallBack(string data)
    {
        JsonData jsonData = JsonMapper.ToObject(data);
        var code = (int)jsonData["code"];
        if (code == (int)Consts.Code.Code_OK)
        {
            UserData.phone = _phoneNum;
            UserInfoScript.Instance.InitUI();
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("绑定手机失败：" + code);
        }
    }
}