using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetErrorPanelScript : MonoBehaviour
{
    public delegate void OnClickButton();
    public OnClickButton m_OnClickButton = null;

    public static NetErrorPanelScript s_instance = null;
    public GameObject s_netErrorPanel = null;

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NetErrorPanelScript", "Show"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NetErrorPanelScript", "Show", null, null);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NetErrorPanelScript", "onClickChongLian"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NetErrorPanelScript", "onClickChongLian", null, null);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NetErrorPanelScript", "setContentText"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NetErrorPanelScript", "setContentText", null, str);
            return;
        }

        s_netErrorPanel.transform.Find("Image_bg").Find("Text_content").GetComponent<Text>().text = str;
    }
}
