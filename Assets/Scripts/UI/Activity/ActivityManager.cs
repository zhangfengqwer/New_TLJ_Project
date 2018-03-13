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
                    {
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
                }
                break;

            // 限时话费赛
            case 2:
                {
                    {
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
                }
                break;

            // 老用户特权
            case 3:
                {
                    {
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

                        script.m_btn2.transform.localScale = new Vector3(0,0,0);
                        script.m_btn3.transform.localScale = new Vector3(0, 0, 0);
                    }
                }
                break;

            // 话费碎片
            case 4:
                {
                    {
                        GameObject prefabs = Resources.Load("Prefabs/Activity/Activity_huafeisuipian") as GameObject;
                        s_panel = GameObject.Instantiate(prefabs);
                    }
                }
                break;

            // 微信公众号
            case 5:
                {
                    {
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
                break;

            default:
                {
                    // 使用热更新的代码
                    if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ActivityManager_hotfix", "getActivityPanel_default"))
                    {
                        s_panel = (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ActivityManager_hotfix", "getActivityPanel_default", null, activity);
                        return s_panel;
                    }
                }
                break;
        }

        return s_panel;
    }
}
