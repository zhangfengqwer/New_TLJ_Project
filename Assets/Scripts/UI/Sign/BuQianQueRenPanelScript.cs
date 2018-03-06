using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuQianQueRenPanelScript : MonoBehaviour {
    
    public Text m_text_buqianNum;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BuQianQueRenPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start () {
        OtherData.s_buQianQueRenPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BuQianQueRenPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BuQianQueRenPanelScript_hotfix", "Start", null, null);
            return;
        }

        m_text_buqianNum.text = "*" + getBuQianGoldHuaFei().ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int getBuQianGoldHuaFei()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BuQianQueRenPanelScript_hotfix", "getBuQianGoldHuaFei"))
        {
            int i = (int)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BuQianQueRenPanelScript_hotfix", "getBuQianGoldHuaFei", null, null);
            return i;
        }

        int num = 5000 * (Sign30RecordData.getInstance().m_curMonthBuQianCount / 3 + 1);
        return num;
    }

    public void onClickQueRenBuQian()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("BuQianQueRenPanelScript_hotfix", "onClickQueRenBuQian"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.BuQianQueRenPanelScript_hotfix", "onClickQueRenBuQian", null, null);
            return;
        }

        if (UserData.gold < getBuQianGoldHuaFei())
        {
            ToastScript.createToast("金币不足");
            return;
        }

        OtherData.s_sign30PanelScript.reqSign(OtherData.s_sign30PanelScript.m_curChoiceId);
    }
}
