using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserAgreeMentScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        OtherData.s_userAgreeMentScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("UserAgreeMentScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.UserAgreeMentScript_hotfix", "Start", null, null);
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
