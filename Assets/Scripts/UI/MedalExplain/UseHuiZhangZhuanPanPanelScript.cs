using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UseHuiZhangZhuanPanPanelScript : MonoBehaviour {

    public int m_needHuiZhangNum = 3;
    public Text m_text_content;
    public TurntablePanelScript m_parentScript = null;

    public static UseHuiZhangZhuanPanPanelScript create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/UseHuiZhangZhuanPanPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj.GetComponent<UseHuiZhangZhuanPanPanelScript>();
    }

    // Use this for initialization
    void Start ()
    {
		
	}

    public void setData(TurntablePanelScript parentScript, int needHuiZhangNum)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("UseHuiZhangZhuanPanPanelScript", "setData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.UseHuiZhangZhuanPanPanelScript", "setData", null, parentScript, needHuiZhangNum);
            return;
        }

        m_parentScript = parentScript;
        m_needHuiZhangNum = needHuiZhangNum;

        m_text_content.text = " 确定使用" + needHuiZhangNum + "个徽章进行转盘抽奖？";
    }

    public void onClickOK()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("UseHuiZhangZhuanPanPanelScript", "onClickOK"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.UseHuiZhangZhuanPanPanelScript", "onClickOK", null, null);
            return;
        }

        if (UserData.medal < m_needHuiZhangNum)
        {
            ToastScript.createToast("徽章不足");

            return;
        }

        if (m_parentScript != null)
        {
            m_parentScript.reqUseZhuanPan(2);
            Destroy(gameObject);
        }
    }
}
