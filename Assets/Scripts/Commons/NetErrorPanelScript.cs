using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetErrorPanelScript : MonoBehaviour
{
    public static NetErrorPanelScript s_instance = null;

    static GameObject m_netErrorPanel = null;
    public Text m_text_content;
    public Button m_button;

    public delegate void OnClickButton();
    OnClickButton m_OnClickButton = null;

    public static GameObject Show()
    {
        if (m_netErrorPanel != null)
        {
            Destroy(m_netErrorPanel);
        }

        GameObject prefab = Resources.Load("Prefabs/Commons/NetErrorPanel") as GameObject;
        m_netErrorPanel = GameObject.Instantiate(prefab, GameObject.Find("Canvas_High").transform);

        return m_netErrorPanel;
    }

    public static void Close()
    {
        if (m_netErrorPanel != null)
        {
            Destroy(m_netErrorPanel);
        }
    }

    // Use this for initialization
    void Awake()
    {
        s_instance = GetComponent<NetErrorPanelScript>();
    }
    
    public void onClickChongLian()
    {
        if (m_OnClickButton != null)
        {
            m_OnClickButton();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void setOnClickButton(OnClickButton onClickButton)
    {
        m_OnClickButton = onClickButton;
    }

    public void setContentText(string str)
    {
        m_text_content.text = str;
    }
}
