using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseHuaFeiPanelScript : MonoBehaviour {

    public InputField m_inputField_phone;

    public PropInfo m_propInfo = null;


    public static GameObject create(PropInfo propInfo)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/UseHuaFeiPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<UseHuaFeiPanelScript>().m_propInfo = propInfo;

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickQueRen()
    {
        if (!VerifyRuleUtil.CheckPhone(m_inputField_phone.text))
        {
            ToastScript.createToast("请输入正确的手机号");

            return;
        }

        LogicEnginerScript.Instance.GetComponent<UseHuaFeiRequest>().SetData(m_propInfo.m_id, m_inputField_phone.text);
        LogicEnginerScript.Instance.GetComponent<UseHuaFeiRequest>().CallBack = onReceive_UseHuaFei;
        LogicEnginerScript.Instance.GetComponent<UseHuaFeiRequest>().OnRequest();
    }

    public void onReceive_UseHuaFei(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];
        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            ToastScript.createToast("使用成功，请等待充值到账");

            GameUtil.changeData(m_propInfo.m_id, -1);

            if (BagPanelScript.Instance != null)
            {
                BagPanelScript.Instance.UpdateUI();
            }

            Destroy(gameObject);
            Destroy(GameObject.Find("PropDetailPanel(Clone)"));
        }
        else
        {
            ToastScript.createToast((string)jd["msg"]);
        }
    }
}
