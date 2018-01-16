using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplainPanelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        OtherData.s_explainPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ExplainPanelScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ExplainPanelScript", "Start", null, null);
            return;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
