using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalExplainPanelScript : MonoBehaviour {

    public Button m_button_setSecondPsw;

    private void Awake()
    {
        OtherData.s_medalExplainPanelScript = this;

        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalExplainPanelScript", "Awake"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalExplainPanelScript", "Awake", null, null);
            return;
        }
    }

    // Use this for initialization
    void Start ()
    {
        if (UserData.isSetSecondPsw)
        {
            CommonUtil.setImageSprite(m_button_setSecondPsw.transform.Find("Image").GetComponent<Image>(), "Sprites/MedalExplain/anniu_zi_ysz");
            m_button_setSecondPsw.transform.Find("Image").GetComponent<Image>().SetNativeSize();
            Destroy(m_button_setSecondPsw.GetComponent<Button>());
        }
        else
        {
            CommonUtil.setImageSprite(m_button_setSecondPsw.transform.Find("Image").GetComponent<Image>(), "Sprites/MedalExplain/anniu_zi_szmm");
            m_button_setSecondPsw.transform.Find("Image").GetComponent<Image>().SetNativeSize();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void onClickSetPsw()
    {
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalExplainPanelScript", "onClickSetPsw"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalExplainPanelScript", "onClickSetPsw", null, null);
            return;
        }

        SetSecondPswPanelScript.create();
        Destroy(gameObject);
    }
}
