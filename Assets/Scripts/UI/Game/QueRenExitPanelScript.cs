using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QueRenExitPanelScript : MonoBehaviour {

    public GameScript m_parentScript;
    public Text m_text_tips;

    public static GameObject create(GameScript parentScript,string text)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/QueRenExitPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<QueRenExitPanelScript>().m_parentScript = parentScript;
        obj.GetComponent<QueRenExitPanelScript>().m_text_tips.text = text;

        return obj;
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
        Destroy(gameObject);
    }
}
