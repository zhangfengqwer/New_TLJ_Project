using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResultPanelScript : MonoBehaviour {

    public GameScript m_parentScript;

    public Text m_text_result;
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
            m_text_result.text = "胜利";
            m_text_xianjia_score.text = "金币：+" + score;
        }
        else
        {
            m_text_result.text = "失败";
            m_text_xianjia_score.text = "金币：" + score;
        }

        m_text_xianjia_score.text = "闲家得分：" + score;
    }

    public void onClickJiXu()
    {
        Destroy(gameObject);
    }

    public void onClickHuanZhuo()
    {
        Destroy(gameObject);
    }

    public void onClickExit()
    {

    }
}
