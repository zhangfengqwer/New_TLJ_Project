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
        obj.transform.Find("Text").GetComponent<Text>().alignment = textAnchor;

        obj.transform.SetParent(GameObject.Find("Canvas_Middle").transform);
        obj.transform.localScale = new Vector3(1, 1, 1);
        
        obj.transform.localPosition = pos;

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
