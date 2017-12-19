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

    }

    public void setData(bool isWin)
    {
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

    void onInvokeTimeOut()
    {
        m_parentScript.onTimerEvent_TimeEnd(false);
    }

    public void onClickShare()
    {
        ChoiceShareScript.Create("我赢过许多人却没有输给过你，快来帮我赢话费！","");
    }

    public void onClickExit()
    {
        m_parentScript.exitRoom();
    }
}
