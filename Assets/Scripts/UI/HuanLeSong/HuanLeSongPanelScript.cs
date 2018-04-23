using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HuanLeSongPanelScript : MonoBehaviour {

    public Text m_text_day;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/HuanLeSongPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HuanLeSongPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HuanLeSongPanelScript_hotfix", "Start", null, null);
            return;
        }

        initUI_Image();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void initUI_Image()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HuanLeSongPanelScript_hotfix", "initUI_Image"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HuanLeSongPanelScript_hotfix", "initUI_Image", null, null);
            return;
        }

        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_bg").GetComponent<Image>(), "huanlesong.unity3d", "huanlesong_bg");
    }

    public void onClickLingQu()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HuanLeSongPanelScript_hotfix", "onClickLingQu"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HuanLeSongPanelScript_hotfix", "onClickLingQu", null, null);
            return;
        }

        ToastScript.createToast("暂未开放");
    }
}
