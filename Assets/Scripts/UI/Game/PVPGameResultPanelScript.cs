using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVPGameResultPanelScript : MonoBehaviour
{
    public GameScript m_parentScript;

    public Image m_image_result;
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
            CommonUtil.setImageSprite(m_image_result, "Sprites/GameResult/gameresult_win");
            m_image_result.SetNativeSize();

            m_button_exit.transform.localScale = new Vector3(0,0,0);
        }
        else
        {
            CommonUtil.setImageSprite(m_image_result, "Sprites/GameResult/gameresult_fail");
            m_image_result.SetNativeSize();

            m_waitMatch.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void onClickExit()
    {
        m_parentScript.onClickExitRoom();
    }
}
