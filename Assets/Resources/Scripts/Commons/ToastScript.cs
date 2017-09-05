using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToastScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        float move = Time.deltaTime * m_speed / (1.0f / 60.0f);

        gameObject.transform.localPosition += new Vector3(0, move, 0);
        if(gameObject.transform.localPosition.y > 360)
        {
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

        return obj;
    }

    void setData (GameObject obj, string text)
    {
        m_text.text = text;

        m_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        obj.transform.SetParent(m_canvas.transform);
        obj.transform.localPosition = new Vector3(0,0,0);
        obj.transform.localScale = new Vector3(1,1,1);
    }

    //--------------------------------------------
    Canvas m_canvas;
    static Text m_text;
    static float m_speed = 3.0f;
}
