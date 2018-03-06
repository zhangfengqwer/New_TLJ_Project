using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShuRuTuiGuangCodePanelScript : MonoBehaviour {

    public InputField m_inputField_tuiguangcode;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ShuRuTuiGuangCodePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        OtherData.s_shuRuTuiGuangCodePanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShuRuTuiGuangCodePanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShuRuTuiGuangCodePanelScript_hotfix", "Start", null, null);
            return;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickOK()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShuRuTuiGuangCodePanelScript_hotfix", "onClickOK"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShuRuTuiGuangCodePanelScript_hotfix", "onClickOK", null, null);
            return;
        }

        string tuiguangcode = m_inputField_tuiguangcode.text;

        if (tuiguangcode.CompareTo("") == 0)
        {
            ToastScript.createToast("请输入推广码");
        }
        
        LogicEnginerScript.Instance.GetComponent<BindTuiGuangCodeRequest>().CallBack = onCallBackBindTuiGuangCode;
        LogicEnginerScript.Instance.GetComponent<BindTuiGuangCodeRequest>().tuiguangcode = tuiguangcode;
        LogicEnginerScript.Instance.GetComponent<BindTuiGuangCodeRequest>().OnRequest();
    }

    public void onCallBackBindTuiGuangCode(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShuRuTuiGuangCodePanelScript_hotfix", "onCallBackBindTuiGuangCode"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShuRuTuiGuangCodePanelScript_hotfix", "onCallBackBindTuiGuangCode", null, data);
            return;
        }

        JsonData jsonData = JsonMapper.ToObject(data);
        int code = (int)jsonData["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            string reward = (string)jsonData["reward"];

            if (reward.CompareTo("") != 0)
            {
                List<string> list = new List<string>();
                CommonUtil.splitStr(reward, list, ';');
                for (int i = 0; i < list.Count; i++)
                {
                    GameUtil.changeData(GameUtil.getPropIdFromReward(list[i]), GameUtil.getPropNumFromReward(list[i]));

                    ShowRewardPanelScript.Show(reward, false);
                }

                ToastScript.createToast("领取奖励成功");

                Destroy(gameObject);
            }
            else
            {
                ToastScript.createToast("异常：奖励为空");
            }
        }
        else
        {
            string msg = (string)jsonData["msg"];

            ToastScript.createToast(msg);
        }
    }
}
