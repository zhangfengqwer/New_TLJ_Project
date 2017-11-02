using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetErrorPanelScript : MonoBehaviour
{
    public Text m_text_content;
    public Button m_button;

    public delegate void OnClickButton();
    OnClickButton m_OnClickButton = null;

    public static NetErrorPanelScript create()
    {
        GameObject prefab = Resources.Load("Prefabs/Commons/NetErrorPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_High").transform);

        return obj.GetComponent<NetErrorPanelScript>();
    }

    private void Start()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }

    public void Show()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Close()
    {
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }
    
    public void onClickChongLian()
    {
        if (m_OnClickButton != null)
        {
            m_OnClickButton();
        }
        else
        {
            gameObject.transform.localScale = new Vector3(0, 0, 0);
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
