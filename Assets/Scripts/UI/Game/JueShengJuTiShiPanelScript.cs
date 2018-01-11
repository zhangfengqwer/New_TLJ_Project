using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JueShengJuTiShiPanelScript : MonoBehaviour {

    public static GameObject s_instance = null;

    public static GameObject show()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/JueShengJuTiShiPanel") as GameObject;
        s_instance = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        
        return s_instance;
    }

    public static void checkClose()
    {
        if (s_instance != null)
        {
            Destroy(s_instance);
        }
    }
}
