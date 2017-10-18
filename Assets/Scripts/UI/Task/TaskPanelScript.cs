using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanelScript : MonoBehaviour {

    public GameObject m_listView;
    ListViewScript m_ListViewScript;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/TaskPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

        return obj;
    }

    void Start()
    {
        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        // 拉取任务
        {
            LogicEnginerScript.Instance.GetComponent<GetTaskRequest>().CallBack = onReceive_GetTask;
            LogicEnginerScript.Instance.GetComponent<GetTaskRequest>().OnRequest();
        }
    }

    public void loadTask()
    {
        m_ListViewScript.clear();

        for (int i = 0; i < TaskDataScript.getInstance().getTaskDataList().Count; i++)
        {
            GameObject prefab = Resources.Load("Prefabs/UI/Item/Item_Task_List") as GameObject;
            GameObject obj = MonoBehaviour.Instantiate(prefab);
            obj.GetComponent<Item_Task_List_Script>().m_parentScript = this;
            obj.GetComponent<Item_Task_List_Script>().setTaskData(TaskDataScript.getInstance().getTaskDataList()[i]);

            obj.transform.name = TaskDataScript.getInstance().getTaskDataList()[i].task_id.ToString();

            m_ListViewScript.addItem(obj);
        }

        m_ListViewScript.addItemEnd();
    }

    public void setTaskOver(int task_id)
    {
        TaskDataScript.getInstance().setTaskIsOver(task_id);

        for (int i = 0; i < m_ListViewScript.getItemList().Count; i++)
        {
            if (m_ListViewScript.getItemList()[i].GetComponent<Item_Task_List_Script>().getTaskData().task_id == task_id)
            {
                m_ListViewScript.getItemList()[i].GetComponent<Item_Task_List_Script>().setTaskIsOver();
            }
        }
    }
    public void onReceive_GetTask(string data)
    {
        TaskDataScript.getInstance().initJson(data);

        loadTask();
    }
}
