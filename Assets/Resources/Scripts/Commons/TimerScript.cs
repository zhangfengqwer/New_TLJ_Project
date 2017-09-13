using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public Text m_textTime;

    public delegate void OnTimerEvent_TimeEnd();
    OnTimerEvent_TimeEnd m_onTimerEvent_TimeEnd = null;

    bool m_isStart = false;
    float m_time;

    public static GameObject createTimer()
    {
        GameObject prefab = Resources.Load("Prefabs/Game/Timer") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localScale = new Vector3(1,1,1);
        obj.transform.localPosition = new Vector3(400,200,0);

        return obj;
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

                if (m_onTimerEvent_TimeEnd != null)
                {
                    m_onTimerEvent_TimeEnd();
                }
            }
        }
    }

    public void setOnTimerEvent_TimeEnd(OnTimerEvent_TimeEnd onTimerEvent_TimeEnd)
    {
        m_onTimerEvent_TimeEnd = onTimerEvent_TimeEnd;
    }

    public void start(float seconds)
    {
        gameObject.transform.localScale = new Vector3(1,1,1);

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
}
