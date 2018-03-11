using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVPGameResultPanelScript : MonoBehaviour
{
    public GameScript m_parentScript;

    public Image m_image_guang;
    public Image m_image_sidai;
    public Image m_image_result;
    public Button m_button_share;
    public Button m_button_exit;
    public GameObject m_waitMatch;

    public static GameObject create(GameScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/PVPGameResultPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<PVPGameResultPanelScript>().m_parentScript = parentScript;

        return obj;
    }

    // Use this for initialization
    void Start()
    {
        OtherData.s_pvpGameResultPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPGameResultPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPGameResultPanelScript_hotfix", "Start", null, null);
            return;
        }
    }

    public void setData(bool isWin)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPGameResultPanelScript_hotfix", "setData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPGameResultPanelScript_hotfix", "setData", null, isWin);
            return;
        }

        if (isWin)
        {
            CommonUtil.setImageSpriteByAssetBundle(m_image_result, "gameresult.unity3d", "fangsheguang_liang");
            CommonUtil.setImageSpriteByAssetBundle(m_image_result, "gameresult.unity3d", "sidai_hong");
            CommonUtil.setImageSpriteByAssetBundle(m_image_result, "gameresult.unity3d", "zi_jinji");
            
            m_image_result.SetNativeSize();

            m_button_share.transform.localScale = new Vector3(0, 0, 0);
            m_button_exit.transform.localScale = new Vector3(0,0,0);

            Invoke("onInvokeTimeOut", 30);
        }
        else
        {
            CommonUtil.setImageSpriteByAssetBundle(m_image_result, "gameresult.unity3d", "fangsheguang_hui");
            CommonUtil.setImageSpriteByAssetBundle(m_image_result, "gameresult.unity3d", "sidai_hui");
            CommonUtil.setImageSpriteByAssetBundle(m_image_result, "gameresult.unity3d", "zi_taotai");
            
            m_image_result.SetNativeSize();

            m_waitMatch.transform.localScale = new Vector3(0, 0, 0);
        }

        m_image_guang.SetNativeSize();
        m_image_sidai.SetNativeSize();
        m_image_result.SetNativeSize();
    }

    public void onInvokeTimeOut()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPGameResultPanelScript_hotfix", "onInvokeTimeOut"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPGameResultPanelScript_hotfix", "onInvokeTimeOut", null, null);
            return;
        }

        m_parentScript.onTimerEvent_TimeEnd(false);
    }

    public void onClickShare()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPGameResultPanelScript_hotfix", "onClickShare"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPGameResultPanelScript_hotfix", "onClickShare", null, null);
            return;
        }

        ChoiceShareScript.Create("我赢过许多人却没有输给过你，快来帮我赢话费！","");
    }

    public void onClickExit()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPGameResultPanelScript_hotfix", "onClickExit"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPGameResultPanelScript_hotfix", "onClickExit", null, null);
            return;
        }

        m_parentScript.exitRoom();
    }
}
