using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Task_List_Script : MonoBehaviour {

    public TaskPanelScript m_parentScript;

    public Text m_text_title;
    public Text m_text_content;
    public Text m_text_progress;

    public Button m_button_wancheng;

    TaskData m_taskData;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setTaskData(TaskData taskData)
    {
        m_taskData = taskData;

        {
            m_text_title.text = m_taskData.title;
            m_text_content.text = m_taskData.content;
            m_text_progress.text = "进度: " + m_taskData.progress +"/" + m_taskData.target;

            // 已完成
            if (m_taskData.isover == 1)
            {
                m_button_wancheng.interactable = false;
                CommonUtil.setImageSprite(m_button_wancheng.GetComponent<Image>(), "Sprites/Task/anniu_yilingque");
            }
            else
            {
                if (m_taskData.progress == m_taskData.target)
                {
                    m_button_wancheng.interactable = true;
                    CommonUtil.setImageSprite(m_button_wancheng.GetComponent<Image>(), "Sprites/Task/anniu_yiwancheng");
                }
                else
                {
                    m_button_wancheng.interactable = false;
                    CommonUtil.setImageSprite(m_button_wancheng.GetComponent<Image>(), "Sprites/Task/anniu_weiwancheng");
                }
            }
        }
    }

    public TaskData getTaskData()
    {
        return m_taskData;
    }

    public void setTaskIsOver()
    {
        m_button_wancheng.interactable = false;
        CommonUtil.setImageSprite(m_button_wancheng.GetComponent<Image>(), "Sprites/Task/anniu_yilingque");
    }

    public void onClickWanCheng()
    {
        LogicEnginerScript.Instance.GetComponent<CompleteTaskRequest>().setTaskId(int.Parse(gameObject.transform.name));
        LogicEnginerScript.Instance.GetComponent<CompleteTaskRequest>().CallBack = onReceive_CompleteTask;
        LogicEnginerScript.Instance.GetComponent<CompleteTaskRequest>().OnRequest();
    }

    public void onReceive_CompleteTask(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];
        int task_id = (int)jd["task_id"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            m_parentScript.setTaskOver(task_id);

            // 增加奖励
            {
                string reward = TaskDataScript.getInstance().getTaskDataById(task_id).reward;
                List<string> tempList = new List<string>();
                CommonUtil.splitStr(reward, tempList, ':');

                GameUtil.changeData(int.Parse(tempList[0]), int.Parse(tempList[1]));
            }

            ShowRewardPanelScript.create().GetComponent<ShowRewardPanelScript>().setData(TaskDataScript.getInstance().getTaskDataById(task_id).reward);
        }
    }
}
