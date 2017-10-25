using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuoGuanPanelScript : MonoBehaviour {

    public GameScript m_parentScript;
    
    public static GameObject create(GameScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/Game/TuoGuanPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<TuoGuanPanelScript>().m_parentScript = parentScript;

        return obj;
    }

    public void onClickCalcel()
    {
        m_parentScript.onClickCancelTuoGuan();
        Destroy(gameObject);
    }
}
