using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("KeFuPanelScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.KeFuPanelScript", "Start", null, null);
            return;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
