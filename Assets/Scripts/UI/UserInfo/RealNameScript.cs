using System.Text.RegularExpressions;
using LitJson;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

public class RealNameScript : MonoBehaviour
{
    public InputField RealNameInputField;
    public InputField IdentificationInputField;
    public bool _isCorrectRealName;
    public bool _isCorrectIdentification;
    public string _realName;
    public string _identification;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/RealNamePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        return obj;
    }

    // Use this for initialization
    void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("RealNameScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.RealNameScript", "Start", null, null);
            return;
        }

        RealNameInputField.onEndEdit.AddListener(delegate { GetRealName(RealNameInputField); });
        IdentificationInputField.onEndEdit.AddListener(delegate { GetIdentification(IdentificationInputField); });
    }

    public void GetRealName(InputField input)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("RealNameScript", "GetRealName"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.RealNameScript", "GetRealName", null, input);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("RealNameScript", "GetIdentification"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.RealNameScript", "GetIdentification", null, input);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("RealNameScript", "OnClickRealName"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.RealNameScript", "OnClickRealName", null, null);
            return;
        }

        if (string.IsNullOrEmpty(RealNameInputField.text) || string.IsNullOrEmpty(IdentificationInputField.text))
        {
            ToastScript.createToast("输入的内容不能为空");

            return;
        }

        if (!_isCorrectRealName)
        {
            ToastScript.createToast("请输入正确的姓名");

            return;
        }

        if (!_isCorrectIdentification)
        {
            ToastScript.createToast("请输入正确的身份证");
            return;
        }
        
        LogicEnginerScript.Instance.GetComponent<RealNameRequest>().CallBack = realNameCallBack;
        LogicEnginerScript.Instance.GetComponent<RealNameRequest>().OnRequest(_realName, _identification);
    }

    public void realNameCallBack(string result)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("RealNameScript", "realNameCallBack"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.RealNameScript", "realNameCallBack", null, result);
            return;
        }

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