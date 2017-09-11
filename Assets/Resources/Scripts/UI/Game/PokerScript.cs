using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerScript : MonoBehaviour {

    int m_num;
    int m_pokerType;

    public static GameObject createPoker()
    {
        GameObject prefabs = Resources.Load("Prefabs/Game/Poker") as GameObject;
        GameObject obj = Instantiate(prefabs);
        return obj;
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void initPoker(int num,int pokerType)
    {
        m_num = num;
        m_pokerType = pokerType;

        if (num >= 2 && num <= 10)
        {
            gameObject.transform.Find("Text").GetComponent<Text>().text = num.ToString();
        }
        else
        {
            if (num == 11)
            {
                gameObject.transform.Find("Text").GetComponent<Text>().text = "J";
            }
            else if (num == 12)
            {
                gameObject.transform.Find("Text").GetComponent<Text>().text = "Q";
            }
            else if (num == 13)
            {
                gameObject.transform.Find("Text").GetComponent<Text>().text = "K";
            }
            else if (num == 14)
            {
                gameObject.transform.Find("Text").GetComponent<Text>().text = "A";
            }
            else if (num == 15)
            {
                gameObject.transform.Find("Text").GetComponent<Text>().text = "小王";
            }
            else if (num == 16)
            {
                gameObject.transform.Find("Text").GetComponent<Text>().text = "大王";
            }
        }
    }
}
