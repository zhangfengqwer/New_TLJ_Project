using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonExitPanelScript : MonoBehaviour
{
    public Button ButtonConfirm;
    public Button ButtonClose;
    public Text TextContent;

    public static GameObject create()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("CommonExitPanelScript_hotfix", "create"))
        {
            GameObject o = (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.CommonExitPanelScript_hotfix", "create", null, null);
            return o;
        }

        GameObject prefab = Resources.Load("Prefabs/UI/Panel/CommonExitPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_High").transform);
        return obj;
    }
   
}
