using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityManager
{
    public static GameObject s_panel = null;

    public static GameObject getActivityPanel(Activity.ActivityData activity)
    {
        // 使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ActivityManager_hotfix", "getActivityPanel"))
        {
            s_panel = (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ActivityManager_hotfix", "getActivityPanel", null, activity);
            return s_panel;
        }

        if (s_panel != null)
        {
            GameObject.Destroy(s_panel);
        }

        int activity_id = activity.ActivityId;
        string url = activity.ImageUrl;

        switch (activity_id)
        {
            // 大礼来袭
            case 1:
                {
                    setPanel_dalilaixi(url);
                }
                break;

            // 限时话费赛
            case 2:
                {
                    setPanel_xianshihuafeisai(url);
                }
                break;

            // 老用户特权
            case 3:
                {
                    setPanel_laoyonghutequan(url);
                }
                break;

            // 话费碎片
            case 4:
                {
                    setPanel_huafeisuipian(url);
                }
                break;

            // 微信公众号
            case 5:
                {
                    setPanel_weixingongzhonghao(url);
                }
                break;

            default:
                {
                    setPanel_other(activity);
                }
                break;
        }

        return s_panel;
    }

    public static void setPanel_other(Activity.ActivityData activity)
    {
        // 使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ActivityManager_hotfix", "setPanel_other"))
        {
            s_panel = (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ActivityManager_hotfix", "setPanel_other", null, activity);
            return;
        }
    }

    // 大礼来袭
    public static void setPanel_dalilaixi(string url)
    {
        // 使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ActivityManager_hotfix", "setPanel_dalilaixi"))
        {
            s_panel = (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ActivityManager_hotfix", "setPanel_dalilaixi", null, url);
            return;
        }

        GameObject prefabs = Resources.Load("Prefabs/Activity/Activity_image_button") as GameObject;
        s_panel = GameObject.Instantiate(prefabs);
        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.AddComponent<DownImageUtil>();
        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.GetComponent<DownImageUtil>().startDown(url);

        Activity_image_button_Script script = s_panel.GetComponent<Activity_image_button_Script>();

        {
            script.m_btn1.transform.localPosition = new Vector3(269.73f, 74.3f, 0);
            script.m_btn1.transform.Find("Text").GetComponent<Text>().text = "前往获得";
            script.m_btn1.onClick.AddListener(() =>
            {
                ChoiceShareScript.Create("疯狂升级天天玩，玩就有话费奖品抱回家！", "");
            });
        }

        {
            script.m_btn2.transform.localPosition = new Vector3(269.73f, -29.52f, 0);
            script.m_btn2.transform.Find("Text").GetComponent<Text>().text = "前往获得";
            script.m_btn2.onClick.AddListener(() =>
            {
                GameObject.Destroy(OtherData.s_activity.gameObject);
                OtherData.s_mainScript.onClickEnterXiuXianChang();
            });
        }

        {
            script.m_btn3.transform.localPosition = new Vector3(269.73f, -141.35f, 0);
            script.m_btn3.transform.Find("Text").GetComponent<Text>().text = "前往获得";
            script.m_btn3.onClick.AddListener(() =>
            {
                GameObject.Destroy(OtherData.s_activity.gameObject);
                TuiGuangYouLiPanelScript.create();
            });
        }
    }

    // 限时话费赛
    public static void setPanel_xianshihuafeisai(string url)
    {
        // 使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ActivityManager_hotfix", "setPanel_xianshihuafeisai"))
        {
            s_panel = (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ActivityManager_hotfix", "setPanel_xianshihuafeisai", null, url);
            return;
        }

        GameObject prefabs = Resources.Load("Prefabs/Activity/Activity_image_button") as GameObject;
        s_panel = GameObject.Instantiate(prefabs);
        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.AddComponent<DownImageUtil>();
        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.GetComponent<DownImageUtil>().startDown(url);

        Activity_image_button_Script script = s_panel.GetComponent<Activity_image_button_Script>();

        script.m_btn2.transform.localPosition = new Vector3(-21.69f, -128.2f, 0);
        script.m_btn1.transform.Find("Text").GetComponent<Text>().text = "前往";
        script.m_btn1.onClick.AddListener(() =>
        {
            GameObject.Destroy(OtherData.s_activity.gameObject);
            PVPChoiceScript.create(true);
        });

        script.m_btn2.transform.localScale = new Vector3(0, 0, 0);
        script.m_btn3.transform.localScale = new Vector3(0, 0, 0);
    }

    // 老用户特权
    public static void setPanel_laoyonghutequan(string url)
    {
        // 使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ActivityManager_hotfix", "setPanel_laoyonghutequan"))
        {
            s_panel = (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ActivityManager_hotfix", "setPanel_laoyonghutequan", null, url);
            return;
        }

        GameObject prefabs = Resources.Load("Prefabs/Activity/Activity_image_button") as GameObject;
        s_panel = GameObject.Instantiate(prefabs);
        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.AddComponent<DownImageUtil>();
        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.GetComponent<DownImageUtil>().startDown(url);

        Activity_image_button_Script script = s_panel.GetComponent<Activity_image_button_Script>();

        script.m_btn1.transform.localPosition = new Vector3(163.5f, 14.62f, 0);
        script.m_btn1.transform.Find("Text").GetComponent<Text>().text = "前往获得";
        script.m_btn1.onClick.AddListener(() =>
        {
            GameObject.Destroy(OtherData.s_activity.gameObject);
            OldPlayerBindPanelScript.create();
        });

        script.m_btn2.transform.localScale = new Vector3(0, 0, 0);
        script.m_btn3.transform.localScale = new Vector3(0, 0, 0);
    }

    // 话费碎片
    public static void setPanel_huafeisuipian(string url)
    {
        // 使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ActivityManager_hotfix", "setPanel_huafeisuipian"))
        {
            s_panel = (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ActivityManager_hotfix", "setPanel_huafeisuipian", null, url);
            return;
        }

        GameObject prefabs = Resources.Load("Prefabs/Activity/Activity_huafeisuipian") as GameObject;
        s_panel = GameObject.Instantiate(prefabs);
    }

    // 微信公众号
    public static void setPanel_weixingongzhonghao(string url)
    {
        // 使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ActivityManager_hotfix", "setPanel_weixingongzhonghao"))
        {
            s_panel = (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ActivityManager_hotfix", "setPanel_weixingongzhonghao", null, url);
            return;
        }

        GameObject prefabs = Resources.Load("Prefabs/Activity/Activity_image_button") as GameObject;
        s_panel = GameObject.Instantiate(prefabs);
        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.AddComponent<DownImageUtil>();
        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.GetComponent<DownImageUtil>().startDown(url);

        Activity_image_button_Script script = s_panel.GetComponent<Activity_image_button_Script>();

        script.m_btn1.transform.localScale = new Vector3(0, 0, 0);
        script.m_btn2.transform.localScale = new Vector3(0, 0, 0);
        script.m_btn3.transform.localScale = new Vector3(0, 0, 0);
    }
}
