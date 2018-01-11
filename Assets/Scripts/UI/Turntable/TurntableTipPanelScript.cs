using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurntableTipPanelScript : MonoBehaviour {

    public Text m_text_tip;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/TurntableTipPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntableTipPanelScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntableTipPanelScript", "Start", null, null);
            return;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setTip(string tip)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntableTipPanelScript", "setTip"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntableTipPanelScript", "setTip", null, tip);
            return;
        }

        m_text_tip.text = tip;
    }
}
