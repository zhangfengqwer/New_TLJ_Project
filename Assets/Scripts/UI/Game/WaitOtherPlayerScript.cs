using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitOtherPlayerScript : MonoBehaviour {

    public string m_showText;
    Text m_text;
    int m_time_index = 1;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/Game/WaitOtherPlayer") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        m_text = gameObject.GetComponent<Text>();

        InvokeRepeating("timer", 0.1f, 0.5f);
    }

    void timer()
    {
        switch (m_time_index)
        {
            case 1:
                {
                    m_text.text = m_showText;
                    m_time_index = 2;
                }
                break;

            case 2:
                {
                    m_text.text = m_showText + ".";
                    m_time_index = 3;
                }
                break;

            case 3:
                {
                    m_text.text = m_showText + "..";
                    m_time_index = 4;
                }
                break;

            case 4:
                {
                    m_text.text = m_showText + "...";
                    m_time_index = 1;
                }
                break;
        }
    }
}
