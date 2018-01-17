﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QueRenExitPanelScript : MonoBehaviour {

    public static GameObject s_gameobject = null;

    public GameScript m_parentScript;
    public Text m_text_tips;

    public static GameObject create(GameScript parentScript,string text)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/QueRenExitPanel") as GameObject;
        s_gameobject = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        s_gameobject.GetComponent<QueRenExitPanelScript>().m_parentScript = parentScript;
        s_gameobject.GetComponent<QueRenExitPanelScript>().m_text_tips.text = text;

        return s_gameobject;
    }

    // Use this for initialization
    void Start()
    {
        OtherData.s_queRenExitPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("QueRenExitPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.QueRenExitPanelScript_hotfix", "Start", null, null);
            return;
        }
    }

    public void OnClickQueRen()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("QueRenExitPanelScript_hotfix", "OnClickQueRen"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.QueRenExitPanelScript_hotfix", "OnClickQueRen", null, null);
            return;
        }

        m_parentScript.exitRoom();
    }

    public void OnClickCancel()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("QueRenExitPanelScript_hotfix", "OnClickCancel"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.QueRenExitPanelScript_hotfix", "OnClickCancel", null, null);
            return;
        }

        Destroy(s_gameobject);
        s_gameobject = null;
    }
}
