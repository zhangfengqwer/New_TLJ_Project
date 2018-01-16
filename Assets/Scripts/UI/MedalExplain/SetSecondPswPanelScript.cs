using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSecondPswPanelScript : MonoBehaviour {

    public InputField m_inputField_mima;
    public InputField m_inputField_querenmima;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/SetSecondPswPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        OtherData.s_setSecondPswPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SetSecondPswPanelScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SetSecondPswPanelScript", "Start", null, null);
            return;
        }

        LogicEnginerScript.Instance.GetComponent<SetSecondPswRequest>().CallBack = onReceive_SetSecondPsw;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickOK()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SetSecondPswPanelScript", "onClickOK"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SetSecondPswPanelScript", "onClickOK", null, null);
            return;
        }

        if ((m_inputField_mima.text.CompareTo("") == 0) ||
            (m_inputField_querenmima.text.CompareTo("") == 0))
        {
            ToastScript.createToast("请输入密码");
            return;
        }

        if (m_inputField_mima.text.CompareTo(m_inputField_querenmima.text) != 0)
        {
            ToastScript.createToast("密码不一致");
            return;
        }

        // 检测密码是否合格
        {
            for (int i = 0; i < m_inputField_mima.text.Length; i++)
            {
                string str = m_inputField_mima.text[i].ToString();
                if (((CommonUtil.charToAsc(str) >= 48) && (CommonUtil.charToAsc(str) <= 57) ||
                     ((CommonUtil.charToAsc(str) >= 65) && (CommonUtil.charToAsc(str) <= 90) ||
                      ((CommonUtil.charToAsc(str) >= 97) && (CommonUtil.charToAsc(str) <= 122)))))
                {
                }
                else
                {
                    ToastScript.createToast("密码格式不对");

                    return;
                }
            }

            if (m_inputField_mima.text.Length < 6)
            {
                ToastScript.createToast("密码至少6位");
                return;
            }
        }

        LogicEnginerScript.Instance.GetComponent<SetSecondPswRequest>().SetData(m_inputField_mima.text);
        LogicEnginerScript.Instance.GetComponent<SetSecondPswRequest>().OnRequest();
    }

    public void onReceive_SetSecondPsw(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SetSecondPswPanelScript", "onReceive_SetSecondPsw"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SetSecondPswPanelScript", "onReceive_SetSecondPsw", null, data);
            return;
        }

        JsonData jd = JsonMapper.ToObject(data);

        int code = (int)jd["code"];
        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            ToastScript.createToast("设置成功");
            UserData.isSetSecondPsw = true;
            OtherData.s_hasCheckSecondPSW = true;

            Destroy(gameObject);
        }
        else
        {
            ToastScript.createToast("设置失败");
        }
    }
}
