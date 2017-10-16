﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDataScript
{

    static TaskDataScript s_taskData = null;

    List<TaskData> m_taskDataList = new List<TaskData>();

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
        m_taskDataList.Clear();

        JsonData jsonData = JsonMapper.ToObject(json);
        m_taskDataList = JsonMapper.ToObject<List<TaskData>>(jsonData["task_list"].ToString());
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
        for (int i = 0; i < m_taskDataList.Count; i++)
        {
            if (m_taskDataList[i].task_id == task_id)
            {
                m_taskDataList[i].isover = 1;
                break;
            }
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
}