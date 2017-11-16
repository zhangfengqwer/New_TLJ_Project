using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurntablePanelScript : MonoBehaviour
{
    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/TurntablePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    void Start()
    {
        // 获取转盘数据
        {
            LogicEnginerScript.Instance.GetComponent<GetTurntableRequest>().CallBack = onReceive_GetTurntable;
            LogicEnginerScript.Instance.GetComponent<GetTurntableRequest>().OnRequest();
        }

        // 使用转盘
        {
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().CallBack = onReceive_UseTurntable;
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().type = 0;
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().OnRequest();
        }
    }

    // 获取转盘数据
    void onReceive_GetTurntable(string data)
    {
        GetTurntableDataScript.getInstance().initJson(data);
    }

    // 使用转盘
    void onReceive_UseTurntable(string data)
    {
        GetTurntableDataScript.getInstance().initJson(data);
    }

    private void OnDestroy()
    {
        LogicEnginerScript.Instance.GetComponent<GetTurntableRequest>().CallBack = null;
    }
}
