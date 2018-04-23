using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Activity_51_Script : MonoBehaviour
{

    public string m_hotfix_class = "Activity_51_Script_hotfix";
    public string m_hotfix_path = "HotFix_Project.Activity_51_Script_hotfix";

    void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "Start", null, null);
            return;
        }
    }

    public void setImage(string url)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "setImage"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "setImage", null, url);
            return;
        }

        gameObject.transform.Find("Image_bg").gameObject.AddComponent<DownImageUtil>();
        gameObject.transform.Find("Image_bg").gameObject.GetComponent<DownImageUtil>().startDown(url);
    }

    public void onReceive_Data(string json)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onReceive_Data"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onReceive_Data", null, json);
            return;
        }
    }
    
    public void onClick_btn1(GameObject obj)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClick_btn1"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClick_btn1", null, obj);
            return;
        }
    }

    public void onClick_btn2(GameObject obj)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClick_btn2"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClick_btn2", null, obj);
            return;
        }
    }

    public void onClick_btn3(GameObject obj)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClick_btn3"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClick_btn3", null, obj);
            return;
        }
    }

    public void onClick_btn4(GameObject obj)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClick_btn4"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClick_btn4", null, obj);
            return;
        }
    }

    public void onClick_btn5(GameObject obj)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClick_btn5"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClick_btn5", null, obj);
            return;
        }
    }
}
