using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGamePanelScript : MonoBehaviour {

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ExitGamePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_High").transform);

        return obj;
    }

    private void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ExitGamePanelScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ExitGamePanelScript", "Start", null, null);
            return;
        }
    }

    public void OnClickQueRen()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ExitGamePanelScript", "OnClickQueRen"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ExitGamePanelScript", "OnClickQueRen", null, null);
            return;
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickCancel()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ExitGamePanelScript", "OnClickCancel"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ExitGamePanelScript", "OnClickCancel", null, null);
            return;
        }

        Destroy(gameObject);
    }
}
