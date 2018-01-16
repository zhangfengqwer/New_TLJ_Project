using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanelScript : MonoBehaviour {

    public GameObject m_listView;
    public ListViewScript m_ListViewScript;

    public bool m_isScaleEnd = false;
    public bool m_hasGetData = false;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/TaskPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TaskPanelScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TaskPanelScript", "Start", null, null);
            return;
        }

        NetLoading.getInstance().Show();

        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        gameObject.GetComponent<ScaleUtil>().m_callBack = scaleCallBack;

        // 拉取任务
        {
            LogicEnginerScript.Instance.GetComponent<GetTaskRequest>().CallBack = onReceive_GetTask;
            LogicEnginerScript.Instance.GetComponent<GetTaskRequest>().OnRequest();
        }
    }

    public void scaleCallBack()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TaskPanelScript", "scaleCallBack"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TaskPanelScript", "scaleCallBack", null, null);
            return;
        }

        m_isScaleEnd = true;

        if (m_hasGetData)
        {
            loadTask();

            NetLoading.getInstance().Close();
        }
    }

    public void loadTask()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TaskPanelScript", "loadTask"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TaskPanelScript", "loadTask", null, null);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TaskPanelScript", "setTaskOver"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TaskPanelScript", "setTaskOver", null, task_id);
            return;
        }

        TaskDataScript.getInstance().setTaskIsOver(task_id);

        for (int i = 0; i < m_ListViewScript.getItemList().Count; i++)
        {
            if (m_ListViewScript.getItemList()[i].GetComponent<Item_Task_List_Script>().getTaskData().task_id == task_id)
            {
                m_ListViewScript.getItemList()[i].GetComponent<Item_Task_List_Script>().setTaskIsOver();
                break;
            }
        }

        loadTask();
    }

    public void onReceive_GetTask(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TaskPanelScript", "onReceive_GetTask"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TaskPanelScript", "onReceive_GetTask", null, data);
            return;
        }

        TaskDataScript.getInstance().initJson(data);

        //loadTask();
        m_hasGetData = true;

        if (m_isScaleEnd)
        {
            loadTask();

            NetLoading.getInstance().Close();
        }
    }
}
