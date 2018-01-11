using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuoGuanPanelScript : MonoBehaviour {

    public GameScript m_parentScript;
    
    public static GameObject create(GameScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/Game/TuoGuanPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<TuoGuanPanelScript>().m_parentScript = parentScript;

        return obj;
    }

    public void onClickCalcel()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TuoGuanPanelScript", "onClickCalcel"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TuoGuanPanelScript", "onClickCalcel", null, null);
            return;
        }

        m_parentScript.onClickCancelTuoGuan();
        Destroy(gameObject);
    }
}
