using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PVPEndPanelScript : MonoBehaviour {

    public GameScript m_parentScript;

    public Text m_text_mingci;

    public static GameObject create(GameScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/PVPEndPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<PVPEndPanelScript>().m_parentScript = parentScript;

        return obj;
    }

    // Use this for initialization
    void Start()
    {

    }

    public void setData(int mingci)
    {
        m_text_mingci.text = mingci.ToString();
    }

    public void onClickExit()
    {
        //m_parentScript.onClickExitRoom();
        SceneManager.LoadScene("MainScene");
    }
}
