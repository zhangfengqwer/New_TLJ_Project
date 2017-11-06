using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResultPanelScript : MonoBehaviour {

    public GameScript m_parentScript;

    public Image m_image_result;
    public Text m_text_xianjia_score;
    public Text m_text_gold;

    public Text m_text_player_left1;
    public Text m_text_player_left2;
    public Text m_text_player_right1;
    public Text m_text_player_right2;

    public static GameObject create(GameScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/GameResultPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        
        obj.GetComponent<GameResultPanelScript>().m_parentScript = parentScript;

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
		
	}

    public void setData(bool isWin,int score,int gold)
    {
        if (isWin)
        {
            CommonUtil.setImageSprite(m_image_result, "Sprites/GameResult/gameresult_win");
            m_image_result.SetNativeSize();
            m_text_xianjia_score.text = "+" + score;
        }
        else
        {
            CommonUtil.setImageSprite(m_image_result, "Sprites/GameResult/gameresult_fail");
            m_image_result.SetNativeSize();
            m_text_xianjia_score.text = score.ToString();
        }

        m_text_xianjia_score.text = score.ToString();

        if (gold > 0)
        {
            m_text_gold.text = "+" + gold.ToString();
        }
        else
        {
            m_text_gold.text = gold.ToString();
        }

        m_text_player_left1.text = GameData.getInstance().m_playerDataList[0].m_name;
        m_text_player_left2.text = GameData.getInstance().m_playerDataList[2].m_name;
        m_text_player_right1.text = GameData.getInstance().m_playerDataList[1].m_name;
        m_text_player_right2.text = GameData.getInstance().m_playerDataList[3].m_name;
    }

    public void onClickJiXu()
    {
        m_parentScript.reqContinueGame();
        Destroy(gameObject);
    }

    public void onClickHuanZhuo()
    {
        m_parentScript.reqChangeRoom();
        Destroy(gameObject);
    }

    public void onClickClose()
    {
        Destroy(gameObject);
    }

    public void onClickExit()
    {
        m_parentScript.exitRoom();
    }
}
