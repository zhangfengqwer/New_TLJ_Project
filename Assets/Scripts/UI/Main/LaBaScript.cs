using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaBaScript : MonoBehaviour {

    public Text m_text;

    public List<string> m_data = new List<string>();
    bool m_isStartMove = false;

	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        if ((m_data.Count > 0) && !m_isStartMove)
        {
            m_text.text = m_data[0];
            m_text.transform.localPosition = new Vector3(200, 0, 0);

            m_data.RemoveAt(0);

            InvokeRepeating("onTextMove", 0.05f, 0.05f);

            m_isStartMove = true;
        }
	}

    void onTextMove()
    {
        m_text.transform.localPosition -= new Vector3(2, 0, 0);

        if (m_text.transform.localPosition.x <= -800)
        {
            m_isStartMove = false;
            CancelInvoke("onTextMove");
        }
    }
}
