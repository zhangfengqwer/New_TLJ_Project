using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutPanelScript : MonoBehaviour
{
    private void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("AboutPanelScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.AboutPanelScript", "Start", null, null);
            return;
        }
    }

    public void IsRuleToggle(bool IsClick)
    {
        if (IsClick)
        {
            
        }
    }

    public void IsAgreeToggle(bool IsClick)
    {
        if (IsClick)
        {
            
        }
    }

    public void IsMonitorToggle(bool IsClick)
    {
        if (IsClick)
        {
        }
    }
}
