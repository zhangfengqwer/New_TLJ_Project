using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


public class OtherPlayerUIScript : MonoBehaviour {

    public Text m_textName;
    public Text m_textGoldNum;

    static public GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/Game/OtherPlayerUI") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localScale = new Vector3(1,1,1);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
    }

    public void setHead(string headPath)
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load("Sprites/Head/head_1", typeof(Sprite)) as Sprite;
    }

    public void setName(string name)
    {
        m_textName.text = name;
    }

    public void setGoldNum(int goldNum)
    {
        m_textGoldNum.text = goldNum.ToString();
    }

    public void onClickHead()
    {
        Debug.Log("点击头像");
    }
}
