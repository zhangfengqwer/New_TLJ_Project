using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerShowTuiGuangPanelScript : MonoBehaviour {

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/NewPlayerShowTuiGuangPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start () {
        OtherData.s_newPlayerShowTuiGuangPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NewPlayerShowTuiGuangPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NewPlayerShowTuiGuangPanelScript_hotfix", "Start", null, null);
            return;
        }

        initUI_Image();
    }

    public void initUI_Image()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NewPlayerShowTuiGuangPanelScript_hotfix", "initUI_Image"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NewPlayerShowTuiGuangPanelScript_hotfix", "initUI_Image", null, null);
            return;
        }

        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Bg/Image").GetComponent<Image>(), "tuiguang.unity3d", "tuiguang_bg");
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void onClickShuRuTuiGuangCode()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NewPlayerShowTuiGuangPanelScript_hotfix", "onClickShuRuTuiGuangCode"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NewPlayerShowTuiGuangPanelScript_hotfix", "onClickShuRuTuiGuangCode", null, null);
            return;
        }

        ShuRuTuiGuangCodePanelScript.create();
    }

    public void onClickMyTuiGuang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NewPlayerShowTuiGuangPanelScript_hotfix", "onClickMyTuiGuang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NewPlayerShowTuiGuangPanelScript_hotfix", "onClickMyTuiGuang", null, null);
            return;
        }

        Destroy(gameObject);
        TuiGuangYouLiPanelScript.create();
    }

    public void OnClickClose()
    {
        Destroy(gameObject);

        AudioScript.getAudioScript().playSound_LayerClose();

        EnterMainPanelShowManager.getInstance().showNextPanel();
    }
}
