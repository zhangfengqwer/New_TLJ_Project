using UnityEngine;
using UnityEngine.UI;

public class BindPhoneScript : MonoBehaviour {
    public InputField PhoneField;
    public InputField VerificationCodeField;
    private bool _isCorrectPhone;
    private bool _isCorrectCode;
    private string _phoneNum;
    private string _verificationCode;
    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BindPhonePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start()
    {
        PhoneField.onEndEdit.AddListener(delegate { GetPhoneNum(PhoneField); });
        VerificationCodeField.onEndEdit.AddListener(delegate { GetVerificationCode(VerificationCodeField); });
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
            LogicEnginerScript.Instance.GetComponent<RealNameRequest>().CallBack = realNameCallBack;
            LogicEnginerScript.Instance.GetComponent<RealNameRequest>().OnRequest(_phoneNum, _verificationCode);
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
            print("发送验证码");
        }
        else
        {
            ToastScript.createToast("请输入正确的手机号");
        }
    }

    private void realNameCallBack(string result)
    {
        print(result);
    }
}
