using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckSecondPSWPanelScript : MonoBehaviour {

    public InputField m_inputField_psw;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/CheckSecondPSWPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        OtherData.s_checkSecondPSWPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("CheckSecondPSWPanelScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.CheckSecondPSWPanelScript", "Start", null, null);
            return;
        }

        LogicEnginerScript.Instance.GetComponent<CheckSecondPSWRequest>().m_callBack = onReceive_CheckSecondPSW;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void onClickOK()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("CheckSecondPSWPanelScript", "onClickOK"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.CheckSecondPSWPanelScript", "onClickOK", null, null);
            return;
        }

        // 检测密码是否合格
        {
            if (m_inputField_psw.text.CompareTo("") == 0)
            {
                ToastScript.createToast("请输入密码");
                return;
            }

            for (int i = 0; i < m_inputField_psw.text.Length; i++)
            {
                string str = m_inputField_psw.text[i].ToString();
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

            if (m_inputField_psw.text.Length < 6)
            {
                ToastScript.createToast("密码至少6位");
                return;
            }
        }

        NetLoading.getInstance().Show();
        LogicEnginerScript.Instance.GetComponent<CheckSecondPSWRequest>().setData(m_inputField_psw.text);
        LogicEnginerScript.Instance.GetComponent<CheckSecondPSWRequest>().OnRequest();
    }

    public void onReceive_CheckSecondPSW(string result)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("CheckSecondPSWPanelScript", "onReceive_CheckSecondPSW"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.CheckSecondPSWPanelScript", "onReceive_CheckSecondPSW", null, result);
            return;
        }

        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(result);
        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            OtherData.s_hasCheckSecondPSW = true;
            Destroy(gameObject);
        }
        else if (code == (int)TLJCommon.Consts.Code.Code_PasswordError)
        {
            ToastScript.createToast("徽章密码错误");
        }
        else if (code == (int)TLJCommon.Consts.Code.Code_CommonFail)
        {
            ToastScript.createToast("徽章密码错误");
        }
        else
        {
            ToastScript.createToast("服务器内部错误");
        }
    }
}
