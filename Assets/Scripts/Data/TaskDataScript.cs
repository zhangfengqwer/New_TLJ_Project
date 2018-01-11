using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDataScript
{

    public static TaskDataScript s_taskData = null;

    public List<TaskData> m_taskDataList = new List<TaskData>();

    public static TaskDataScript getInstance()
    {
        if (s_taskData == null)
        {
            s_taskData = new TaskDataScript();
        }

        return s_taskData;
    }

    public void initJson(string json)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TaskDataScript", "initJson"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TaskDataScript", "initJson", null, json);
            return;
        }

        m_taskDataList.Clear();

        JsonData jsonData = JsonMapper.ToObject(json);
        m_taskDataList = JsonMapper.ToObject<List<TaskData>>(jsonData["task_list"].ToString());

        sortTask();
    }

    public List<TaskData> getTaskDataList()
    {
        return m_taskDataList;
    }

    public TaskData getTaskDataById(int task_id)
    {
        TaskData taskData = null;
        for (int i = 0; i < m_taskDataList.Count; i++)
        {
            if (m_taskDataList[i].task_id == task_id)
            {
                taskData = m_taskDataList[i];
                break;
            }
        }

        return taskData;
    }

    // 任务设为已完成
    public void setTaskIsOver(int task_id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TaskDataScript", "setTaskIsOver"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TaskDataScript", "setTaskIsOver", null, task_id);
            return;
        }

        for (int i = 0; i < m_taskDataList.Count; i++)
        {
            if (m_taskDataList[i].task_id == task_id)
            {
                m_taskDataList[i].isover = 1;
                break;
            }
        }

        sortTask();
    }

    // 可领取的置顶,已领取的放最下面
    void sortTask()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TaskDataScript", "sortTask"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TaskDataScript", "sortTask", null, null);
            return;
        }

        List<TaskData> list_kelingqu = new List<TaskData>();
        List<TaskData> list_weiwancheng = new List<TaskData>();
        List<TaskData> list_yilingqu = new List<TaskData>();

        for (int i = 0; i < m_taskDataList.Count; i++)
        {
            int progress = m_taskDataList[i].progress;
            int target = m_taskDataList[i].target;
            int isover = m_taskDataList[i].isover;

            if (isover == 1)
            {
                list_yilingqu.Add(m_taskDataList[i]);
            }
            else if (progress == target)
            {
                list_kelingqu.Add(m_taskDataList[i]);
            }
            else
            {
                list_weiwancheng.Add(m_taskDataList[i]);
            }
        }

        m_taskDataList.Clear();
        
        // 上面放可领取的
        for (int i = 0; i < list_kelingqu.Count; i++)
        {
            m_taskDataList.Add(list_kelingqu[i]);
        }

        // 中间放未完成的
        for (int i = 0; i < list_weiwancheng.Count; i++)
        {
            m_taskDataList.Add(list_weiwancheng[i]);
        }

        // 下面放已领取的
        for (int i = 0; i < list_yilingqu.Count; i++)
        {
            m_taskDataList.Add(list_yilingqu[i]);
        }
    }
}

public class TaskData
{
    public int task_id;
    public string title;
    public string content;
    public string reward;

    public int progress;
    public int target;
    public int isover;
    public int exp;
}