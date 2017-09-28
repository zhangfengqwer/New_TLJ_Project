using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResultScript : MonoBehaviour {

    public Text m_text;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/GameResult") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.transform.localPosition = new Vector3(0, 0, 0);

        return obj;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setText(string text)
    {
        m_text.text = text;
    }
}
