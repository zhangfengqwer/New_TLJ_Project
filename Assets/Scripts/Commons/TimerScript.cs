﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public enum TimerType
    {
        TimerType_QiangZhu,
        TimerType_MaiDi,
        TimerType_OutPoker,
        TimerType_ChaoDi,
        TimerType_OtherMaiDi,

        TimerType_QiangDiZhu,
        TimerType_JiaBang,
    }

    public Text m_textTime;
    public TimerType m_timerType;

    public delegate void OnTimerEvent_TimeEnd();
    public OnTimerEvent_TimeEnd m_onTimerEvent_TimeEnd = null;

    public bool m_isNeedCallBack = true;
    public bool m_isStart = false;
    public float m_time;

    public static GameObject createTimer()
    {
        GameObject prefab = Resources.Load("Prefabs/Game/Timer") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        //obj.transform.localPosition = new Vector3(450,200,0);
        obj.transform.localPosition = new Vector3(0, 30, 0);

        return obj;
    }

    private void Awake()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_isStart)
        {
            m_time -= Time.deltaTime;

            m_textTime.text = ((int)m_time).ToString();

            // 时间到
            if (m_time <= 1)
            {
                m_isStart = false;

                gameObject.transform.localScale = new Vector3(0,0,0);

                if (m_isNeedCallBack)
                {
                    if (m_onTimerEvent_TimeEnd != null)
                    {
                        m_onTimerEvent_TimeEnd();
                    }
                }
            }
        }
    }

    public void setOnTimerEvent_TimeEnd(OnTimerEvent_TimeEnd onTimerEvent_TimeEnd)
    {
        m_onTimerEvent_TimeEnd = onTimerEvent_TimeEnd;
    }

    public void start(float seconds, TimerType timerType,bool isNeedCallBack)
    {
        gameObject.transform.localScale = new Vector3(1,1,1);

        m_timerType = timerType;
        m_isNeedCallBack = isNeedCallBack;

        // 设置最后渲染
        gameObject.transform.SetAsLastSibling();

        m_time = seconds + 1;
        m_isStart = true;

        m_textTime.text = ((int)m_time).ToString();
    }

    public void stop()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        
        m_isStart = false;

        m_textTime.text = "0";
    }

    public TimerType getTimerType()
    {
        return m_timerType;
    }
}
