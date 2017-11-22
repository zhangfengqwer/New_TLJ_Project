using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatContentScript : MonoBehaviour {

    Text m_text;

    public static GameObject createChatContent(string text,Vector2 pos, TextAnchor textAnchor)
    {
        GameObject prefab = Resources.Load("Prefabs/Game/ChatContent") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab);
        obj.transform.Find("Text").GetComponent<Text>().text = text;
        //obj.transform.Find("Text").GetComponent<Text>().alignment = textAnchor;

        obj.transform.SetParent(GameObject.Find("Canvas_Middle").transform);
        obj.transform.localScale = new Vector3(1, 1, 1);

        {
            int size = text.Length;
            int width = size * 32 + 20;
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 60);
        }

        switch (textAnchor)
        {
            case TextAnchor.MiddleCenter:
                {
                    obj.transform.localPosition = pos;
                }
                break;

            case TextAnchor.MiddleLeft:
                {
                    obj.transform.localPosition = (pos + new Vector2(obj.GetComponent<RectTransform>().sizeDelta.x / 2,0));
                }
                break;

            case TextAnchor.MiddleRight:
                {
                    obj.transform.localPosition = (pos - new Vector2(obj.GetComponent<RectTransform>().sizeDelta.x / 2, 0));
                }
                break;
        }
        
        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        m_text = gameObject.transform.Find("Text").GetComponent<Text>();

        //m_text.alignment = TextAnchor.MiddleRight;
        //m_text.alignment = TextAnchor.MiddleLeft;
        //m_text.alignment = TextAnchor.MiddleCenter;
        
        Invoke("showEnd", 3.0f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void showEnd()
    {
        Destroy(gameObject);
    }
}
