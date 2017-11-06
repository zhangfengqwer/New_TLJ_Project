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
        if (m_inputField_phone.text.Length != 11)
        {
            ToastScript.createToast("请输入正确的手机号");

            return;
        }

        LogicEnginerScript.Instance.GetComponent<UseLaBaRequest>().SetText(m_inputField_phone.text);
        LogicEnginerScript.Instance.GetComponent<UseLaBaRequest>().CallBack = onReceive_UseHuaFei;
        LogicEnginerScript.Instance.GetComponent<UseLaBaRequest>().OnRequest();
    }

    public void onReceive_UseHuaFei(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];
        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            ToastScript.createToast("兑换成功");

            GameUtil.changeData(106, -1);

            if (BagPanelScript.Instance != null)
            {
                BagPanelScript.Instance.UpdateUI();
            }

            Destroy(gameObject);
        }
    }
}
