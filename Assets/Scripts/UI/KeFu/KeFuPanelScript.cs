using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeFuPanelScript : MonoBehaviour {

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/KeFuPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        OtherData.s_keFuPanelScript = this;

        initUI_Image();

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("KeFuPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.KeFuPanelScript_hotfix", "Start", null, null);
            return;
        }
    }

    public void initUI_Image()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("KeFuPanelScript_hotfix", "initUI_Image"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.KeFuPanelScript_hotfix", "initUI_Image", null, null);
            return;
        }

        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Bg/Image").GetComponent<Image>(), "kefu.unity3d", "kefu_wenzi");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
