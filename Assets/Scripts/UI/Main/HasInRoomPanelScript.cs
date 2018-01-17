using LitJson;
using System.Collections;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

public class HasInRoomPanelScript : MonoBehaviour {

    public delegate void OnClickButton();
    public OnClickButton m_OnClickButton = null;
    public Text m_content;

    public static GameObject create(string text, OnClickButton onClickButton)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/HasInRoomPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<HasInRoomPanelScript>().m_content.text = text;
        obj.GetComponent<HasInRoomPanelScript>().m_OnClickButton = onClickButton;

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        OtherData.s_hasInRoomPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HasInRoomPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HasInRoomPanelScript_hotfix", "Start", null, null);
            return;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickOK()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HasInRoomPanelScript_hotfix", "onClickOK"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HasInRoomPanelScript_hotfix", "onClickOK", null, null);
            return;
        }

        if (m_OnClickButton != null)
        {
            m_OnClickButton();
        }
    }
}
