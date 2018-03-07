using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityManager
{
    public static GameObject s_panel = null;

    public static GameObject getActivityPanel(Activity.ActivityData activity)
    {
        if (s_panel != null)
        {
            GameObject.Destroy(s_panel);
        }
        int activity_id = activity.ActivityId;
        switch (activity_id)
        {
            // 大礼来袭
            case 1:
                {
                    {
                        GameObject prefabs = Resources.Load("Prefabs/Activity/Activity_image_button") as GameObject;
                        s_panel = GameObject.Instantiate(prefabs);
                        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.AddComponent<DownImageUtil>();
                        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.GetComponent<DownImageUtil>().startDown("http://fwdown.hy51v.com/test/file/activity/activity_1.jpg");

                        Activity_image_button_Script script = s_panel.GetComponent<Activity_image_button_Script>();

                        {
                            script.m_btn1.transform.localPosition = new Vector3(-200, 0, 0);
                            script.m_btn1.transform.Find("Text").GetComponent<Text>().text = "前往获得";
                            script.m_btn1.onClick.AddListener(() =>
                            {
                                ChoiceShareScript.Create("疯狂升级天天玩，玩就有话费奖品抱回家！", "");
                            });
                        }

                        {
                            script.m_btn2.transform.localPosition = new Vector3(0, 0, 0);
                            script.m_btn2.transform.Find("Text").GetComponent<Text>().text = "前往获得";
                            script.m_btn2.onClick.AddListener(() =>
                            {
                                GameObject.Destroy(OtherData.s_notice.gameObject);
                                OtherData.s_mainScript.onClickEnterXiuXianChang();
                            });
                        }

                        {
                            script.m_btn3.transform.localPosition = new Vector3(200, 0, 0);
                            script.m_btn3.transform.Find("Text").GetComponent<Text>().text = "前往获得";
                            script.m_btn3.onClick.AddListener(() =>
                            {
                                GameObject.Destroy(OtherData.s_notice.gameObject);
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
                        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.GetComponent<DownImageUtil>().startDown("http://fwdown.hy51v.com/test/file/activity/activity_2.jpg");

                        Activity_image_button_Script script = s_panel.GetComponent<Activity_image_button_Script>();

                        script.m_btn1.transform.Find("Text").GetComponent<Text>().text = "前往";
                        script.m_btn1.onClick.AddListener(() =>
                        {
                            GameObject.Destroy(OtherData.s_notice.gameObject);
                            PVPChoiceScript.create();
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
                        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.GetComponent<DownImageUtil>().startDown("http://fwdown.hy51v.com/test/file/activity/activity_3.jpg");

                        Activity_image_button_Script script = s_panel.GetComponent<Activity_image_button_Script>();

                        script.m_btn1.transform.Find("Text").GetComponent<Text>().text = "前往";
                        script.m_btn1.onClick.AddListener(() =>
                        {
                            GameObject.Destroy(OtherData.s_notice.gameObject);
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
                        GameObject prefabs = Resources.Load("Prefabs/Activity/Activity_image_button") as GameObject;
                        s_panel = GameObject.Instantiate(prefabs);
                        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.AddComponent<DownImageUtil>();
                        s_panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.GetComponent<DownImageUtil>().startDown("http://fwdown.hy51v.com/test/file/activity/activity_4.jpg");

                    }
                }
                break;

            default:
                {
                    // 使用热更新的代码
                    if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ActivityManager_hotfix", "getActivityPanel"))
                    {
                        s_panel = (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ActivityManager_hotfix", "getActivityPanel", null, activity_id);
                        return s_panel;
                    }
                }
                break;
        }

        return s_panel;
    }
}
