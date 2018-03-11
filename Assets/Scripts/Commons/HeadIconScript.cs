using System.Collections;
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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HeadIconScript_hotfix", "setIcon"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HeadIconScript_hotfix", "setIcon", null, path);
            return;
        }

        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        
        CommonUtil.setImageSpriteByAssetBundle(m_icon,"head.unity3d", path);
    }
}
