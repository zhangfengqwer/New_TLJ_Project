using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ToastScript : MonoBehaviour {

    public Canvas m_canvas;
    public static Text m_text;

    public static List<GameObject> s_toactObj = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        Invoke("onInvoke",2);
    }

    void onInvoke()
    {
        s_toactObj.Remove(gameObject);
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update ()
    {
	}

    public static GameObject createToast (string text)
    {
        GameObject prefab = Resources.Load("Prefabs/Commons/Toast") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab);
        m_text = obj.transform.Find("Text").GetComponent<Text>();

        obj.GetComponent<ToastScript>().setData(obj,text);
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(text.Length * 30, 50);

        return obj;
    }

    public static void clear()
    {
        s_toactObj.Clear();
    }

    void setData(GameObject obj, string text)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ToastScript", "setData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ToastScript", "setData", null, obj, text);
            return;
        }

        m_text.text = text;

        m_canvas = GameObject.Find("Canvas_High").GetComponent<Canvas>();
        obj.transform.SetParent(m_canvas.transform);
        obj.transform.localScale = new Vector3(1, 1, 1);

        if (s_toactObj.Count < 4)
        {
            s_toactObj.Add(obj);
        }
        else
        {
            Destroy(s_toactObj[0]);
            s_toactObj.RemoveAt(0);

            s_toactObj.Add(obj);
        }

        for (int i = s_toactObj.Count - 1; i >= 0; i--)
        {
            s_toactObj[i].transform.localPosition = new Vector3(0, -230 + (s_toactObj.Count - i) * 60, 0);
        }
    }
}
