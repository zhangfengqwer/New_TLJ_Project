using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserAgreeMentScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("UserAgreeMentScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.UserAgreeMentScript", "Start", null, null);
            return;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickClose()
    {
        Destroy(this.gameObject);
    }
}
