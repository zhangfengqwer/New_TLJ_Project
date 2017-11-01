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
        InvokeRepeating("onTextMove", 0.05f, 0.05f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_data.Count > 0)
        {
            if (isEnd)
            {
                isEnd = false;

                m_text.text = m_data[0];
                m_text.transform.localPosition = new Vector3(200, 0, 0);

                m_data.RemoveAt(0);
            }
        }
        else if(isEnd)
        {
            isEnd = false;

            if (RandomUtil.getRandom(1, 2) == 1)
            {
                m_text.text = "系统：欢迎来到疯狂升级 点击喇叭可进行全服喊话哦~~";
            }
            else
            {
                m_text.text = "系统：关注微信公众号：“星焱娱乐”，即送丰厚大礼噢~";
            }
        }
	}

    void onTextMove()
    {
        m_text.transform.localPosition -= new Vector3(2, 0, 0);

        if (m_text.transform.localPosition.x <= -800)
        {
            m_text.transform.localPosition = new Vector3(200, 0, 0);
            isEnd = true;
        }
    }

    public void addText(string text)
    {
        m_data.Add(text);
    }
}
