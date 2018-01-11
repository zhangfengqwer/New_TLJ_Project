﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadIconScript : MonoBehaviour {

    public Image m_icon;

	// Use this for initialization
	void Start ()
    {
		
	}

    public void setIcon(string path)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HeadIconScript", "setIcon"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HeadIconScript", "setIcon", null, path);
            return;
        }

        m_icon.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
    }
}
