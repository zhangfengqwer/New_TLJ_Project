using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropDetailPanelScript : MonoBehaviour {

    public BagPanelScript m_parentScript;

    public Text m_text_name;
    public Text m_text_desc;
    public Image m_image_icon;
    public Button m_button_use;

    PropInfo m_propInfo = null;

    public static GameObject create(int prop_id, BagPanelScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/PropDetailPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

        obj.GetComponent<PropDetailPanelScript>().setPropId(prop_id);
        obj.GetComponent<PropDetailPanelScript>().m_parentScript = parentScript;

        return obj;
    }

    public void setPropId(int prop_id)
    {
        m_propInfo = PropData.getInstance().getPropInfoById(prop_id);

        if (m_propInfo != null)
        {
            m_text_name.text = m_propInfo.m_name;
            m_text_desc.text = m_propInfo.m_desc;

            if (m_propInfo.m_type != 0)
            {
                m_button_use.transform.localScale = new Vector3(0,0,0);
            }
        }
    }

    public void onClickUseProp()
    {
        //LogicEnginerScript.Instance.GetComponent<DeleteEmailRequest>().setEmailId(m_mailData.m_email_id);
        //LogicEnginerScript.Instance.GetComponent<DeleteEmailRequest>().CallBack = onReceive_DeleteMail;
        //LogicEnginerScript.Instance.GetComponent<DeleteEmailRequest>().OnRequest();
    }

    public void onReceive_UseProp(string data)
    {
        //JsonData jd = JsonMapper.ToObject(data);
        //int code = (int)jd["code"];
        //int email_id = (int)jd["email_id"];

        //if (code == (int)TLJCommon.Consts.Code.Code_OK)
        //{
        //    m_parentScript.deleteMail(email_id);
        //    Destroy(gameObject);
        //}
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
