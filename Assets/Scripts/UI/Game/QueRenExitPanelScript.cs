﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QueRenExitPanelScript : MonoBehaviour {

    public static GameObject s_gameobject = null;

    public GameScript m_parentScript;
    public Text m_text_tips;

    public static GameObject create(GameScript parentScript,string text)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/QueRenExitPanel") as GameObject;
        s_gameobject = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        s_gameobject.GetComponent<QueRenExitPanelScript>().m_parentScript = parentScript;
        s_gameobject.GetComponent<QueRenExitPanelScript>().m_text_tips.text = text;

        return s_gameobject;
    }

    // Use this for initialization
    void Start()
    {
        
    }

    public void OnClickQueRen()
    {
        m_parentScript.exitRoom();
    }

    public void OnClickCancel()
    {
        Destroy(s_gameobject);
        s_gameobject = null;
    }
}
