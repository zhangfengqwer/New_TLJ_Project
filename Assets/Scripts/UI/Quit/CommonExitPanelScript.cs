﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonExitPanelScript : MonoBehaviour
{
    public Button ButtonConfirm;
    public Text TextContent;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/CommonExitPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        return obj;
    }
}