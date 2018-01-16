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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPGameResultPanelScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPGameResultPanelScript", "Start", null, null);
            return;
        }
    }

    public void setData(bool isWin)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPGameResultPanelScript", "setData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPGameResultPanelScript", "setData", null, isWin);
            return;
        }

        if (isWin)
        {
            CommonUtil.setImageSprite(m_image_guang, "Sprites/GameResult/fangsheguang_liang");
            CommonUtil.setImageSprite(m_image_sidai, "Sprites/GameResult/sidai_hong");
            CommonUtil.setImageSprite(m_image_result, "Sprites/GameResult/zi_jinji");
            
            m_image_result.SetNativeSize();

            m_button_share.transform.localScale = new Vector3(0, 0, 0);
            m_button_exit.transform.localScale = new Vector3(0,0,0);

            Invoke("onInvokeTimeOut", 10);
        }
        else
        {
            CommonUtil.setImageSprite(m_image_guang, "Sprites/GameResult/fangsheguang_hui");
            CommonUtil.setImageSprite(m_image_sidai, "Sprites/GameResult/sidai_hui");
            CommonUtil.setImageSprite(m_image_result, "Sprites/GameResult/zi_taotai");
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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPGameResultPanelScript", "onInvokeTimeOut"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPGameResultPanelScript", "onInvokeTimeOut", null, null);
            return;
        }

        m_parentScript.onTimerEvent_TimeEnd(false);
    }

    public void onClickShare()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPGameResultPanelScript", "onClickShare"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPGameResultPanelScript", "onClickShare", null, null);
            return;
        }

        ChoiceShareScript.Create("我赢过许多人却没有输给过你，快来帮我赢话费！","");
    }

    public void onClickExit()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPGameResultPanelScript", "onClickExit"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPGameResultPanelScript", "onClickExit", null, null);
            return;
        }

        m_parentScript.exitRoom();
    }
}
