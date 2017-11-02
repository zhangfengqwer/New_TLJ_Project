﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetLoading : MonoBehaviour {

    static NetLoading s_instance = null;
    GameObject s_loadingPanel = null;

    public static NetLoading getInstance()
    {
        if (s_instance == null)
        {
            GameObject prefab = Resources.Load("Prefabs/Commons/NetLoadingUtil") as GameObject;
            GameObject obj = GameObject.Instantiate(prefab);

            s_instance = obj.GetComponent<NetLoading>();
        }

        return s_instance;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Show()
    {
        if (s_instance != null)
        {
            GameObject prefab = Resources.Load("Prefabs/Commons/LoadingPanel") as GameObject;
            s_loadingPanel = GameObject.Instantiate(prefab, GameObject.Find("Canvas_High").transform);
        }
    }

    public void Close()
    {
        if (s_loadingPanel != null)
        {
            Destroy(s_loadingPanel);

            s_loadingPanel = null;
        }
    }
}
