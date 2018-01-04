using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaBaScript : MonoBehaviour {

    public Text m_text;

    List<string> m_data = new List<string>();

    bool isEnd = true;

	// Use this for initialization
	void Start ()
    {
        m_data.Add("系统：" + GameUtil.getOneTips());
        m_text.text = m_data[0];
        m_text.GetComponent<RectTransform>().sizeDelta = new Vector2(m_text.text.Length * 25f, 40);

        InvokeRepeating("onTextMove", 0.05f, 0.05f);
    }

    void onTextMove()
    {
        m_text.transform.localPosition -= new Vector3(2, 0, 0);

        if (m_text.transform.localPosition.x <= (-200 - m_text.GetComponent<RectTransform>().sizeDelta.x))
        {
            m_data.RemoveAt(0);

            m_text.transform.localPosition = new Vector3(200, 0, 0);

            if (m_data.Count > 0)
            {
                m_text.text = m_data[0];
            }
            else
            {
                m_data.Add("系统：" + GameUtil.getOneTips());
                m_text.text = m_data[0];
            }

            m_text.GetComponent<RectTransform>().sizeDelta = new Vector2(m_text.text.Length * 25f, 40);
        }
    }

    public void addText(string text)
    {
        if (m_text.text.Substring(0,3).CompareTo("系统：") == 0)
        {
            m_data.Clear();
            m_data.Add(text);

            m_text.text = m_data[0];
            m_text.GetComponent<RectTransform>().sizeDelta = new Vector2(m_text.text.Length * 25f, 40);
            m_text.transform.localPosition = new Vector3(200, 0, 0);
        }
        else
        {
            m_data.Add(text);
        }        
    }
}
