using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetErrorPanelScript : MonoBehaviour
{
    static NetErrorPanelScript s_instance = null;

    static GameObject m_netErrorPanel = null;
    public Text m_text_content;
    public Button m_button;

    public delegate void OnClickButton();
    OnClickButton m_OnClickButton = null;

    public static NetErrorPanelScript getInstance()
    {
        if (s_instance == null)
        {
            GameObject prefab = Resources.Load("Prefabs/Commons/NetErrorPanel") as GameObject;
            m_netErrorPanel = GameObject.Instantiate(prefab, GameObject.Find("Canvas_High").transform);
            m_netErrorPanel.transform.localPosition = new Vector3(0,0,0);

            s_instance = m_netErrorPanel.GetComponent<NetErrorPanelScript>();
        }
        
        return s_instance;
    }

    public void Show()
    {
        m_netErrorPanel.transform.localPosition = new Vector3(1, 1, 1);
    }

    public void Close()
    {
        Destroy(gameObject);
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
