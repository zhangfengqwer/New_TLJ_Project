﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShouChongPanelScript : MonoBehaviour {

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ShouChongPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start () {
        OtherData.s_shouChongPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShouChongPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShouChongPanelScript_hotfix", "Start", null, null);
            return;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickChongZhi()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShouChongPanelScript_hotfix", "onClickChongZhi"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShouChongPanelScript_hotfix", "onClickChongZhi", null, null);
            return;
        }

        ShopPanelScript.create(2);
        Destroy(gameObject);
    }
}
