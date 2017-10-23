using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ToastScript : MonoBehaviour {

    static List<GameObject> s_toactObj = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
        float move = Time.deltaTime * m_speed / (1.0f / 60.0f);

        gameObject.transform.localPosition += new Vector3(0, move, 0);
        if(gameObject.transform.localPosition.y > 360)
        {
            s_toactObj.Remove(gameObject);
            Destroy(gameObject);
            return;
        }
	}

    public static GameObject createToast (string text)
    {
        GameObject prefab = Resources.Load("Prefabs/Commons/Toast") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab);
        m_text = obj.transform.Find("Text").GetComponent<Text>();

        obj.GetComponent<ToastScript>().setData(obj,text);

        s_toactObj.Add(obj);

        return obj;
    }

    void setData(GameObject obj, string text)
    {
        m_text.text = text;

        m_canvas = GameObject.Find("Canvas_High").GetComponent<Canvas>();
        obj.transform.SetParent(m_canvas.transform);
        obj.transform.localScale = new Vector3(1, 1, 1);

        if (s_toactObj.Count == 0)
        {
            obj.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            if (s_toactObj[s_toactObj.Count - 1] == null)
            {
                return;
            }

            float tempY = s_toactObj[s_toactObj.Count - 1].transform.localPosition.y;
            obj.transform.localPosition = new Vector3(0, tempY - 50, 0);
        }
    }

    //--------------------------------------------
    Canvas m_canvas;
    static Text m_text;
    static float m_speed = 3.0f;
}
