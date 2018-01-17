using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShiFangPokerScript : MonoBehaviour, IPointerDownHandler
{
    // Use this for initialization
    void Start ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShiFangPokerScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShiFangPokerScript_hotfix", "Start", null, null);
            return;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ShiFangPokerScript_hotfix", "OnPointerDown"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ShiFangPokerScript_hotfix", "OnPointerDown", null, eventData);
            return;
        }

        PokerScript.setAllPokerWeiXuanZe();
    }
}
