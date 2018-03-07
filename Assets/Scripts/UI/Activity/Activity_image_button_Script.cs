using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Activity_image_button_Script : MonoBehaviour {

    public Image m_image;
    public Button m_btn1;
    public Button m_btn2;
    public Button m_btn3;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/Activity/Activity_image_button") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
