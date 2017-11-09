using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetErrorPanelScript : MonoBehaviour
{
    public delegate void OnClickButton();
    OnClickButton m_OnClickButton = null;

    static NetErrorPanelScript s_instance = null;
    GameObject s_netErrorPanel = null;

    public static NetErrorPanelScript getInstance()
    {
        if (s_instance == null)
        {
            GameObject prefab = Resources.Load("Prefabs/Commons/NetErrorUtil") as GameObject;
            GameObject obj = GameObject.Instantiate(prefab);

            DontDestroyOnLoad(obj);

            s_instance = obj.GetComponent<NetErrorPanelScript>();
        }

        return s_instance;
    }

    public void Show()
    {
        if (s_netErrorPanel != null)
        {
            Destroy(s_netErrorPanel);
        }

        GameObject prefab = Resources.Load("Prefabs/Commons/NetErrorPanel") as GameObject;
        s_netErrorPanel = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        s_netErrorPanel.transform.Find("Image_bg").Find("Button_chonglian").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            onClickChongLian();
        });
    }

    public void Close()
    {
        if (s_netErrorPanel != null)
        {
            Destroy(s_netErrorPanel);

            s_netErrorPanel = null;
        }
    }

    public void onClickChongLian()
    {
        if (m_OnClickButton != null)
        {
            m_OnClickButton();
        }
        else
        {
            Close();
        }
    }

    public void setOnClickButton(OnClickButton onClickButton)
    {
        m_OnClickButton = onClickButton;
    }

    public void setContentText(string str)
    {
        s_netErrorPanel.transform.Find("Image_bg").Find("Text_content").GetComponent<Text>().text = str;
    }
}
