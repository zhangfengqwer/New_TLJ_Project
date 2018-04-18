using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDZ_GameResult : MonoBehaviour {

    public DDZ_GameScript m_parentScript;
    public string m_jsonData;

    public Text m_beishu;

    public static GameObject create(DDZ_GameScript parentScript,string jsonData)
    {
        GameObject prefab = Resources.Load("Prefabs/DouDiZhu/DDZ_GameResult") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<DDZ_GameResult>().m_parentScript = parentScript;
        obj.GetComponent<DDZ_GameResult>().m_jsonData = jsonData;

        return obj;
    }

    // Use this for initialization
    void Start () {
        JsonData jd = JsonMapper.ToObject(m_jsonData);

        initUI_Image();

        // 胜负图片显示
        {
            int isDiZhuWin = (int)jd["isDiZhuWin"];

            if (DDZ_GameData.getInstance().m_isDiZhu == 1)
            {
                // 地主赢
                if (isDiZhuWin == 1)
                {
                    CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_result").GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_dizhusuccess");
                    AudioScript.getAudioScript().playSound_DouDiZhu_win();
                }
                else
                {
                    CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_result").GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_dizhufail");
                    AudioScript.getAudioScript().playSound_DouDiZhu_lose();
                }
            }
            else
            {
                // 农民赢
                if (isDiZhuWin == 0)
                {
                    CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_result").GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_nongminsuccess");
                    AudioScript.getAudioScript().playSound_DouDiZhu_win();
                }
                else
                {
                    CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_result").GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_nongminfail");
                    AudioScript.getAudioScript().playSound_DouDiZhu_lose();
                }
            }
        }

        // 每个玩家的金币输赢
        {
            int hasSetOtherCount = 0;

            for (int i = 0; i < DDZ_GameData.getInstance().m_playerDataList.Count; i++)
            {
                PlayerData playerData = DDZ_GameData.getInstance().m_playerDataList[i];
                string score = jd[playerData.m_uid]["score"].ToString();
                int scire_i = int.Parse(score);
                int beishu = (int)(jd[playerData.m_uid]["beishu"]);

                if (int.Parse(score) > 0)
                {
                    score = ("+" + score);
                }

                // 我的
                if (playerData.m_uid.CompareTo(UserData.uid) == 0)
                {
                    // 倍数
                    gameObject.transform.Find("Text_beishu").GetComponent<Text>().text = beishu.ToString();
                    gameObject.transform.Find("playerScore_my/Text_name").GetComponent<Text>().text = UserData.name;
                    gameObject.transform.Find("playerScore_my/Text_score").GetComponent<Text>().text = score.ToString();

                    // 金币变化
                    {
                        GameUtil.changeData(1, scire_i);
                        m_parentScript.m_playerHead_down.transform.Find("Text_gold").GetComponent<Text>().text = UserData.gold.ToString();
                    }
                }
                else
                {
                    ++hasSetOtherCount;
                    gameObject.transform.Find("playerScore_other" + hasSetOtherCount + "/Text_name").GetComponent<Text>().text = playerData.m_name;
                    gameObject.transform.Find("playerScore_other" + hasSetOtherCount + "/Text_score").GetComponent<Text>().text = score.ToString();
                }
            }
        }

        // 倍数
        {
            string key = ("beishu_" + UserData.uid);
            for (int i = 0; i < jd[key].Count; i++)
            {
                string str = jd[key][i].ToString();
                m_beishu.text += (str + "\r\n");
            }
        }
    }

    public void initUI_Image()
    {
        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_bg").GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_gameresult_bg");
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void onClickJiXu()
    {
        if (UserData.gold < 1000)
        {
            ToastScript.createToast("金币不足1000，无法继续游戏");
            return;
        }

        m_parentScript.cleanRoom();
        m_parentScript.m_DDZ_NetReqLogic.reqJoinRoom(TLJCommon.Consts.GameRoomType_DDZ_Normal);
        Destroy(gameObject);
    }

    public void onClickOK()
    {
        m_parentScript.exitRoom();
    }
}
