using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityManager
{
    public static GameObject getActivityPanel(int activity_id)
    {
        GameObject panel = null;

        switch (activity_id)
        {
            // 大礼来袭
            case 1:
                {
                    {
                        GameObject prefabs = Resources.Load("Prefabs/Activity/Activity_image_button") as GameObject;
                        panel = GameObject.Instantiate(prefabs);
                        panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.AddComponent<DownImageUtil>();
                        panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.GetComponent<DownImageUtil>().startDown("http://fwdown.hy51v.com/test/file/share-5.jpg");

                        Activity_image_button_Script script = panel.GetComponent<Activity_image_button_Script>();

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
                                GameObject.Destroy(panel);
                                OtherData.s_mainScript.onClickEnterXiuXianChang();
                            });
                        }

                        {
                            script.m_btn3.transform.localPosition = new Vector3(200, 0, 0);
                            script.m_btn3.transform.Find("Text").GetComponent<Text>().text = "前往获得";
                            script.m_btn3.onClick.AddListener(() =>
                            {
                                GameObject.Destroy(panel);
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
                        panel = GameObject.Instantiate(prefabs);
                        panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.AddComponent<DownImageUtil>();
                        panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.GetComponent<DownImageUtil>().startDown("http://fwdown.hy51v.com/test/file/share-5.jpg");

                        Activity_image_button_Script script = panel.GetComponent<Activity_image_button_Script>();

                        script.m_btn1.transform.Find("Text").GetComponent<Text>().text = "前往";
                        script.m_btn1.onClick.AddListener(() =>
                        {
                            GameObject.Destroy(panel);
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
                        panel = GameObject.Instantiate(prefabs);
                        panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.AddComponent<DownImageUtil>();
                        panel.GetComponent<Activity_image_button_Script>().m_image.gameObject.GetComponent<DownImageUtil>().startDown("http://fwdown.hy51v.com/test/file/share-5.jpg");

                        Activity_image_button_Script script = panel.GetComponent<Activity_image_button_Script>();

                        script.m_btn1.transform.Find("Text").GetComponent<Text>().text = "前往";
                        script.m_btn1.onClick.AddListener(() =>
                        {
                            GameObject.Destroy(panel);
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
                        panel = GameObject.Instantiate(prefabs);
                    }
                }
                break;

            default:
                {
                    // 使用热更新的代码
                    if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ActivityManager_hotfix", "getActivityPanel"))
                    {
                        panel = (GameObject)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ActivityManager_hotfix", "getActivityPanel", null, activity_id);
                        return panel;
                    }
                }
                break;
        }

        return panel;
    }
}
