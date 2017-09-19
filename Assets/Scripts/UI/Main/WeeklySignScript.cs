using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeeklySignScript : MonoBehaviour {
    private List<string> signData = new List<string>();
    private List<GameObject> signObjects = new List<GameObject>();
    public GameObject content;

    private int totalSignDays = 2;
	// Use this for initialization
	void Start () {
		InitData();
	    if (signData.Count != signObjects.Count)
	    {
	        print("数据初始化错误");
	        return;
	    }
	    InitUi();


	}

    private void InitData()
    {
        signData.Add("第一天");
        signData.Add("第二天");
        signData.Add("第三天");
        signData.Add("第四天");
        signData.Add("第五天");
        signData.Add("第六天");
        signData.Add("第七天");

        for (int i = 0; i < 7; i++)
        {
            signObjects.Add(content.transform.GetChild(i).gameObject);
        }
    }
    private void InitUi()
    {
        for (int i = 0; i < signObjects.Count; i++)
        {
            GameObject Object = signObjects[i];
            string s = signData[i];
            
         
            //            Transform[] transforms = Object.GetComponentsInChildren<Transform>();
            var child = Object.transform.GetChild(0);
            Text text = child.GetComponent<Text>();
            text.text = s;
            if (totalSignDays > i)
            {
                Color color = Object.GetComponent<Image>().color;
                color.a = 0.5f;
                Object.GetComponent<Image>().color = color;
            }
            if (totalSignDays == i)
            {
                Object.GetComponent<Image>().color = Color.blue;
            }
            //            foreach (var VARIABLE in transforms)
            //            {
            //                Text text = VARIABLE.GetComponent<Text>();
            //                text.text = s;
            //            }
        }
    }

    public void OnSignClick()
    {
        GameObject signObject = signObjects[totalSignDays];

        Color color = signObject.GetComponent<Image>().color;
        color.a = 0.5f;
        signObject.GetComponent<Image>().color = color;
    }

   
}
