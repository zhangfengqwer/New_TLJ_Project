using System.Text.RegularExpressions;
using LitJson;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

public class RealNameScript : MonoBehaviour
{
    public InputField RealNameInputField;
    public InputField IdentificationInputField;
    private bool _isCorrectRealName;
    private bool _isCorrectIdentification;
    private string _realName;
    private string _identification;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/RealNamePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        return obj;
    }

    // Use this for initialization
    void Start()
    {
        RealNameInputField.onEndEdit.AddListener(delegate { GetRealName(RealNameInputField); });
        IdentificationInputField.onEndEdit.AddListener(delegate { GetIdentification(IdentificationInputField); });
    }

    public void GetRealName(InputField input)
    {
        _isCorrectRealName = VerifyRuleUtil.CheckRealName(input.text);
        bool isSensitiveWord = SensitiveWordUtil.IsSensitiveWord(input.text);
        if (isSensitiveWord)
        {
            _isCorrectRealName = false;
            ToastScript.createToast("您的名字有敏感词");
        }

        if (_isCorrectRealName)
        {
            _realName = input.text;
        }
        else
        {
            ToastScript.createToast("请输入正确的姓名");
        }
    }

    public void GetIdentification(InputField input)
    {
        _isCorrectIdentification = VerifyRuleUtil.CheckIDCard(input.text);
        if (_isCorrectIdentification)
        {
            _identification = input.text;
        }
        else
        {
            ToastScript.createToast("请输入正确的身份证");
        }
    }

    public void OnClickRealName()
    {
        if (_isCorrectRealName && _isCorrectIdentification)
        {
            LogicEnginerScript.Instance.GetComponent<RealNameRequest>().CallBack = realNameCallBack;
            LogicEnginerScript.Instance.GetComponent<RealNameRequest>().OnRequest(_realName, _identification);
        }
        else
        {
//            ToastScript.createToast("输入的信息有误");
        }
    }

    private void realNameCallBack(string result)
    {
        JsonData jsonData = JsonMapper.ToObject(result);
        var code = (int) jsonData["code"];
        if (code == (int) Consts.Code.Code_OK)
        {
            UserData.IsRealName = true;
            if (UserInfoScript.Instance != null)
            {
                UserInfoScript.Instance.InitUI();
            }
           
            ToastScript.createToast("实名认证成功,请去邮箱领取奖励！");
            
            LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().OnRequest();
            Destroy(this.gameObject);
        }
        else
        {
            ToastScript.createToast("实名认证错误");
        }
    }
}