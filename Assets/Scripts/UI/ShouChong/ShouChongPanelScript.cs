using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        initUI_Image();
    }

    public void initUI_Image()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShouChongPanelScript_hotfix", "initUI_Image"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShouChongPanelScript_hotfix", "initUI_Image", null, null);
            return;
        }

        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_bg").GetComponent<Image>(), "shouchong.unity3d", "shouchong_bg");
        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_bg/Image_text").GetComponent<Image>(), "shouchong.unity3d", "shouchong_9");
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
