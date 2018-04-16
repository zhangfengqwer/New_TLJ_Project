using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DDZ_GameResult : MonoBehaviour {

    public DDZ_GameScript m_parentScript;
    public string m_jsonData;

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

        // 胜负图片显示
        {
            int isDiZhuWin = (int)jd["isDiZhuWin"];

            if (DDZ_GameData.getInstance().m_isDiZhu == 1)
            {
                // 地主赢
                if (isDiZhuWin == 1)
                {
                    CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_result").GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_dizhusuccess");
                }
                else
                {
                    CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_result").GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_dizhufail");
                }
            }
            else
            {
                // 农民赢
                if (isDiZhuWin == 0)
                {
                    CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_result").GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_nongminsuccess");
                }
                else
                {
                    CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_result").GetComponent<Image>(), "doudizhu.unity3d", "doudizhu_nongminfail");
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
                }
                else
                {
                    ++hasSetOtherCount;
                    gameObject.transform.Find("playerScore_other" + hasSetOtherCount + "/Text_name").GetComponent<Text>().text = playerData.m_name;
                    gameObject.transform.Find("playerScore_other" + hasSetOtherCount + "/Text_score").GetComponent<Text>().text = score.ToString();
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickJiXu()
    {
        m_parentScript.cleanRoom();
        m_parentScript.m_DDZ_NetReqLogic.reqJoinRoom(TLJCommon.Consts.GameRoomType_DDZ_Normal);
        Destroy(gameObject);
    }
}
