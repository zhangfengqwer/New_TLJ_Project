using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    static GameData s_instance = null;
    public static GameData getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new GameData();
        }

        return s_instance;
    }

    public string m_tag = "";
    string m_gameRoomType = "";
    public bool m_isPVP = false;

    public List<TLJCommon.PokerInfo> m_myPokerList = new List<TLJCommon.PokerInfo>();                   // 我的手牌
    public List<TLJCommon.PokerInfo> m_beforeQiangzhuPokerList = new List<TLJCommon.PokerInfo>();       // 上一个人抢主所出的牌
    public List<TLJCommon.PokerInfo> m_curRoundFirstOutPokerList = new List<TLJCommon.PokerInfo>();     // 当前出牌回合第一个人出的牌
    public List<GameObject> m_otherPlayerUIObjList = new List<GameObject>();                            // 另外3各玩家的头像GameObject
    public List<GameObject> m_myPokerObjList = new List<GameObject>();                                  // 我的手牌GameObject
    public List<List<GameObject>> m_curRoundOutPokerList = new List<List<GameObject>>();                // 当前回合每个玩家出牌列表的列表
    public List<PlayerData> m_playerDataList = new List<PlayerData>();                                  // 本桌4个玩家的信息

    public int m_outPokerTime = 5;              // 出牌时间 
    public int m_tuoGuanOutPokerTime = 1;       // 托管出牌时间 
    public int m_qiangZhuTime = 10;             // 抢主时间
    public int m_maiDiTime = 20;                // 埋底时间
    public int m_chaodiTime = 10;               // 选择是否炒底时间 

    public int m_levelPokerNum = -1;            // 级牌
    public int m_myLevelPoker = -1;             // 我方级数
    public int m_otherLevelPoker = -1;          // 对方级数
    public int m_masterPokerType = -1;          // 主牌花色
    
    public int m_isBanker;                      // 是否属于庄家一方
    public int m_getAllScore = 0;               // 庄家对家抓到的分数

    public string m_teammateUID = "";           // 我的队友uid
    public string m_curOutPokerPlayerUid = "";  // 当前出牌的人uid

    public bool m_isTuoGuan = false;
    public bool m_isFreeOutPoker = false;

    public string m_startGameJsonData = "";

    public void clear()
    {
        m_getAllScore = 0;

        m_teammateUID = "";
        m_curOutPokerPlayerUid = "";

        m_isTuoGuan = false;
        m_isFreeOutPoker = false;

        m_myPokerList.Clear();
        m_beforeQiangzhuPokerList.Clear();
        m_curRoundFirstOutPokerList.Clear();
        m_playerDataList.Clear();
        
        //s_instance = null;
    }

    public void setGameRoomType(string gameRoomType)
    {
        m_gameRoomType = gameRoomType;

        List<string> list = new List<string>();
        CommonUtil.splitStr(m_gameRoomType, list, '_');

        // 休闲场
        if (list[0].CompareTo("XiuXian") == 0)
        {
            m_isPVP = false;
        }
        // PVP
        else
        {
            m_isPVP = true;
        }
    }

    public string getGameRoomType()
    {
        return m_gameRoomType;
    }

    public PlayerData getPlayerDataByUid(string uid)
    {
        for (int i = 0; i < m_playerDataList.Count ; i++)
        {
            if (m_playerDataList[i].m_uid.CompareTo(uid) == 0)
            {
                return m_playerDataList[i];
            }
        }

        return null;
    }

    public GameObject getOtherPlayerUIByUid(string uid)
    {
        for (int i = 0; i < m_otherPlayerUIObjList.Count; i++)
        {
            if (m_otherPlayerUIObjList[i].GetComponent<OtherPlayerUIScript>().m_uid.CompareTo(uid) == 0)
            {
                return m_otherPlayerUIObjList[i];
            }
        }

        return null;
    }

    public void setOtherPlayerUI(string uid)
    {
        PlayerData playerData = getPlayerDataByUid(uid);

        getOtherPlayerUIByUid(uid).GetComponent<OtherPlayerUIScript>().m_headIcon.GetComponent<HeadIconScript>().setIcon(playerData.m_head);
        getOtherPlayerUIByUid(uid).GetComponent<OtherPlayerUIScript>().setName(playerData.m_name);
        getOtherPlayerUIByUid(uid).GetComponent<OtherPlayerUIScript>().setGoldNum(playerData.m_gold);
    }
}

public class PlayerData
{
    public PlayerData(string uid)
    {
        m_uid = uid;
    }

    public string m_uid;
    public string m_name;
    public string m_head;

    public int m_gold;
    public int m_allGameCount;
    public int m_winCount;
    public int m_runCount;
    public int m_meiliZhi;
}