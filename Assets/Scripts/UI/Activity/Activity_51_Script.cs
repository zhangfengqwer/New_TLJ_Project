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

        // 拉取领取状态
        {
            NetLoading.getInstance().Show();

            LogicEnginerScript.Instance.GetComponent<Activity_51_StateRequest>().CallBack = onReceive_State;
            LogicEnginerScript.Instance.GetComponent<Activity_51_StateRequest>().OnRequest();
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

    public void onReceive_State(string json)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onReceive_Data"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onReceive_Data", null, json);
            return;
        }

        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(json);
        for (int i = 0; i < jd["datalist"].Count; i++)
        {
            int id = (int)jd["datalist"][i]["id"];
            int state = (int)jd["datalist"][i]["state"];

            Button obj = gameObject.transform.Find("Button_" + id).GetComponent<Button>();

            switch (state)
            {
                // 已领取
                case 1:
                    {
                        obj.interactable = false;
                        obj.transform.Find("Text").GetComponent<Text>().text = "已领取";
                    }
                    break;

                // 当天可领
                case 2:
                    {

                    }
                    break;

                // 未到时间
                case 3:
                    {
                        obj.interactable = false;
                    }
                    break;
            }
        }
    }

    public void onReceive_GetReward(string json)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onReceive_GetReward"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onReceive_GetReward", null, json);
            return;
        }

        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(json);
        int code = (int)jd["code"];
        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            string reward = jd["reward"].ToString();
            GameUtil.changeDataWithStr(reward);
            ShowRewardPanelScript.Show(reward, false);

            // UI变化
            {
                int id = (int)jd["id"];

                Button obj = gameObject.transform.Find("Button_" + id).GetComponent<Button>();
                obj.interactable = false;
                obj.transform.Find("Text").GetComponent<Text>().text = "已领取";
            }
        }
        else
        {
            string msg = jd["msg"].ToString();
            ToastScript.createToast(msg);
        }
    }

    public void onClick_btn1()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClick_btn1"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClick_btn1", null, null);
            return;
        }

        // 领取
        {
            NetLoading.getInstance().Show();

            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().CallBack = onReceive_GetReward;
            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().m_id = 1;
            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().OnRequest();
        }
    }

    public void onClick_btn2()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClick_btn2"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClick_btn2", null, null);
            return;
        }

        // 领取
        {
            NetLoading.getInstance().Show();

            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().CallBack = onReceive_GetReward;
            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().m_id = 2;
            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().OnRequest();
        }
    }

    public void onClick_btn3()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClick_btn3"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClick_btn3", null, null);
            return;
        }

        // 领取
        {
            NetLoading.getInstance().Show();

            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().CallBack = onReceive_GetReward;
            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().m_id = 3;
            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().OnRequest();
        }
    }

    public void onClick_btn4()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClick_btn4"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClick_btn4", null, null);
            return;
        }

        // 领取
        {
            NetLoading.getInstance().Show();

            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().CallBack = onReceive_GetReward;
            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().m_id = 4;
            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().OnRequest();
        }
    }

    public void onClick_btn5()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "onClick_btn5"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "onClick_btn5", null, null);
            return;
        }

        // 领取
        {
            NetLoading.getInstance().Show();

            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().CallBack = onReceive_GetReward;
            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().m_id = 5;
            LogicEnginerScript.Instance.GetComponent<Activity_51_GetRewardRequest>().OnRequest();
        }
    }
}