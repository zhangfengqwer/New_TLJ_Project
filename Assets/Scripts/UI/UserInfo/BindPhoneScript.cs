using System;
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
    public bool _isCorrectPhone;
    public bool _isCorrectCode;
    public string _phoneNum;
    public string _verificationCode;
    public Button ButtonSendSms;
    public int time = 20;
    public static int totalTime;

    public bool IsStartTime;

    //0:绑定手机 1：修改手机
    public static int phone_type;

    public GameObject TitleChange;
    public GameObject TitleBind;


    public static GameObject create(int type)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BindPhonePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        phone_type = type;
        return obj;
    }

    public float nextTime = 1; //一秒之后执行
    public Text textSend;

    public void Timer1()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BindPhoneScript", "Timer1"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BindPhoneScript", "Timer1", null, null);
            return;
        }

        if (nextTime <= Time.time)
        {
            textSend.text = string.Format("{0:d2}", totalTime % 60);
            nextTime = Time.time + 1; //到达一秒后加1
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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BindPhoneScript", "Update"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BindPhoneScript", "Update", null, null);
            return;
        }

        if (IsStartTime)
        {
            Timer1();
        }
    }

    // Use this for initialization
    void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BindPhoneScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BindPhoneScript", "Start", null, null);
            return;
        }

        if (phone_type == 0)
        {
            TitleBind.transform.localScale = Vector3.one;
            TitleChange.transform.localScale = Vector3.zero;
        }
        else if (phone_type == 1)
        {
            TitleBind.transform.localScale = Vector3.zero;
            TitleChange.transform.localScale = Vector3.one;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BindPhoneScript", "GetPhoneNum"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BindPhoneScript", "GetPhoneNum", null, input);
            return;
        }

        _isCorrectPhone = VerifyRuleUtil.CheckPhone(input.text);
        LogUtil.Log(_isCorrectPhone);
        if (_isCorrectPhone)
        {
            _phoneNum = input.text;
        }
    }

    public void GetVerificationCode(InputField input)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BindPhoneScript", "GetVerificationCode"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BindPhoneScript", "GetVerificationCode", null, input);
            return;
        }

        _isCorrectCode = VerifyRuleUtil.CheckVerificationCode(input.text);
        LogUtil.Log(_isCorrectCode);
        if (_isCorrectCode)
        {
            _verificationCode = input.text;
        }
    }

    public void OnClickBindPhone()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BindPhoneScript", "OnClickBindPhone"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BindPhoneScript", "OnClickBindPhone", null, null);
            return;
        }

        if (string.IsNullOrEmpty(PhoneField.text) || string.IsNullOrEmpty(VerificationCodeField.text))
        {
            ToastScript.createToast("输入的内容不能为空");

            return;
        }
        if (!_isCorrectPhone)
        {
            ToastScript.createToast("请输入正确的手机号");

            return;
        }

        if (!_isCorrectCode)
        {
            ToastScript.createToast("请输入正确的验证码");

            return;
        }
        
        LogicEnginerScript.Instance.GetComponent<CheckSmsRequest>().CallBack = bindPhoneCallBack;
        LogicEnginerScript.Instance.GetComponent<CheckSmsRequest>().OnRequest(_phoneNum, _verificationCode);
    }

    public void OnClickSendCode()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BindPhoneScript", "OnClickSendCode"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BindPhoneScript", "OnClickSendCode", null, null);
            return;
        }

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
    public void sendVerificationCodeCallBack(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BindPhoneScript", "sendVerificationCodeCallBack"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BindPhoneScript", "sendVerificationCodeCallBack", null, null);
            return;
        }

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
                LogUtil.Log(nodeChild.Name + ":" + nodeChild.InnerText);
                string value = nodeChild.InnerText;
                if (nodeChild.Name.Equals("ResultCode"))
                {
                    //发送验证码成功
                    totalTime = time;
                    IsStartTime = true;
                    ButtonSendSms.interactable = false;
                }
                else if (nodeChild.Name.Equals("ResultMessageDetails"))
                {
                    ToastScript.createToast(value);
                }
            }
        }
        catch (Exception e)
        {
            LogUtil.Log(e.Message);
        }
    }

    //绑定手机回调
    public void bindPhoneCallBack(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BindPhoneScript", "bindPhoneCallBack"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BindPhoneScript", "bindPhoneCallBack", null, data);
            return;
        }

        JsonData jsonData = JsonMapper.ToObject(data);
        var code = (int) jsonData["code"];
        var msg = (string) jsonData["msg"];
        if (code == (int) Consts.Code.Code_OK)
        {
            if (phone_type == 0)
            {
                ToastScript.createToast("绑定手机成功,请去邮箱领取奖励！");
            }
            else
            {
                ToastScript.createToast("修改手机成功！");
            }

            LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().OnRequest();
            UserData.phone = _phoneNum;
            UserInfoScript.Instance.InitUI();
            Destroy(this.gameObject);
        }
        else
        {
            LogUtil.Log("绑定手机失败：" + code);
            ToastScript.createToast(msg);
        }
    }
}