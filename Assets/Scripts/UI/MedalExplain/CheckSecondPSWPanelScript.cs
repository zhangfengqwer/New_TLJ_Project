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
        LogicEnginerScript.Instance.GetComponent<CheckSecondPSWRequest>().m_callBack = onReceive_CheckSecondPSW;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void onClickOK()
    {
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

    void onReceive_CheckSecondPSW(string result)
    {
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
