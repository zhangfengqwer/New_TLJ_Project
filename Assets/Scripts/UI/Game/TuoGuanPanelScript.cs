using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuoGuanPanelScript : MonoBehaviour {

    public GameScript m_parentScript;
    
    public static GameObject create(GameScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/TuoGuanPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    public void onClickCalcel()
    {
        m_parentScript.onClickCancelTuoGuan();
        Destroy(gameObject);
    }
}
