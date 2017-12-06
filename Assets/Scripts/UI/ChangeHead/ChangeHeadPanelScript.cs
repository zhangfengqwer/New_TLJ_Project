using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHeadPanelScript : MonoBehaviour {

    public static ChangeHeadPanelScript s_instance = null;

    public int m_choiceHead = 0;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ChangeHeadPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        s_instance = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        s_instance = null;
    }

    public void onClickOK()
    {
        if (m_choiceHead == 0)
        {
            ToastScript.createToast("请选择头像");
            return;
        }

        NetLoading.getInstance().Show();
        
        {
            LogicEnginerScript.Instance.GetComponent<ChangeHeadRequest>().m_callBack = onReceive_ChangeHead;
            LogicEnginerScript.Instance.GetComponent<ChangeHeadRequest>().head = m_choiceHead;
            LogicEnginerScript.Instance.GetComponent<ChangeHeadRequest>().OnRequest();
        }
    }

    void onReceive_ChangeHead(string data)
    {
        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(data);

        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            UserData.head = "Sprites/Head/head_" + m_choiceHead;

            OtherData.s_mainScript.refreshUI();
            OtherData.s_userInfoScript.InitUI();
        }
        else
        {
            ToastScript.createToast("服务器出错");
        }
    }
}
