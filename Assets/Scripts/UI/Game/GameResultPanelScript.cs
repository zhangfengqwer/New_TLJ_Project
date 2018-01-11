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

    public string m_gameRoomType;
    public bool m_isWin = true;

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

    public void setData(bool isWin,int score,int gold,string gameRoomType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameResultPanelScript", "setData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameResultPanelScript", "setData", null, isWin, score, gold, gameRoomType);
            return;
        }

        m_isWin = isWin;
        m_gameRoomType = gameRoomType;

        if (isWin)
        {
            AudioScript.getAudioScript().playSound_Win();

            CommonUtil.setImageSprite(m_image_result, "Sprites/GameResult/gameresult_win");
            m_image_result.SetNativeSize();
            m_text_xianjia_score.text = "+" + score;
        }
        else
        {
            AudioScript.getAudioScript().playSound_Fail();

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameResultPanelScript", "onClickJiXu"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameResultPanelScript", "onClickJiXu", null, null);
            return;
        }

        if (!GameUtil.checkCanEnterRoom(m_gameRoomType))
        {
            onClickExit();
            return;
        }

        m_parentScript.reqContinueGame();
        Destroy(gameObject);
    }

    public void onClickHuanZhuo()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameResultPanelScript", "onClickHuanZhuo"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameResultPanelScript", "onClickHuanZhuo", null, null);
            return;
        }

        if (!GameUtil.checkCanEnterRoom(m_gameRoomType))
        {
            onClickExit();
            return;
        }

        m_parentScript.reqChangeRoom();
        Destroy(gameObject);
    }

    public void onClickClose()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameResultPanelScript", "onClickClose"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameResultPanelScript", "onClickClose", null, null);
            return;
        }

        Destroy(gameObject);
    }

    public void onClickExit()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameResultPanelScript", "onClickExit"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameResultPanelScript", "onClickExit", null, null);
            return;
        }

        m_parentScript.exitRoom();
    }

    public void onClickShare()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameResultPanelScript", "onClickShare"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameResultPanelScript", "onClickShare", null, null);
            return;
        }

        if (m_isWin)
        {
            ChoiceShareScript.Create("我在疯狂升级普通场赢得了胜利，话费、徽章等你来拿！", "");
        }
        else
        {
            ChoiceShareScript.Create("我赢过许多人却没有输给过你，下载游戏和我一起玩吧！", "");
        }
    }
}
