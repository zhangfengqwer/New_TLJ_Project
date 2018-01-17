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
        OtherData.s_exitGamePanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ExitGamePanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ExitGamePanelScript_hotfix", "Start", null, null);
            return;
        }
    }

    public void OnClickQueRen()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ExitGamePanelScript_hotfix", "OnClickQueRen"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ExitGamePanelScript_hotfix", "OnClickQueRen", null, null);
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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ExitGamePanelScript_hotfix", "OnClickCancel"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ExitGamePanelScript_hotfix", "OnClickCancel", null, null);
            return;
        }

        Destroy(gameObject);
    }
}
