using System.Collections.Generic;
using UnityEngine;

public class DDZ_GameData
{
    public static DDZ_GameData s_instance = null;
    public static DDZ_GameData getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new DDZ_GameData();
        }

        return s_instance;
    }

    public string m_tag = TLJCommon.Consts.GameRoomType_DDZ_Normal;
    public string m_gameRoomType = "";

    public List<TLJCommon.PokerInfo> m_myPokerList = new List<TLJCommon.PokerInfo>();                   // 我的手牌
    public List<TLJCommon.PokerInfo> m_dipaiList = new List<TLJCommon.PokerInfo>();                     // 底牌列表
    public string m_maxPlayerOutPokerUID;                                                               // 上一个出牌最大的人
    public List<TLJCommon.PokerInfo> m_maxPlayerOutPokerList = new List<TLJCommon.PokerInfo>();         // 上一个出牌最大的人出的牌
    public List<GameObject> m_myPokerObjList = new List<GameObject>();                                  // 我的手牌GameObject
    public List<PlayerData> m_playerDataList = new List<PlayerData>();                                  // 本桌玩家的信息

    public int m_waitMatchTime = 30;            // 匹配队友最长时间
    public int m_outPokerTime = 15;             // 出牌时间 
    public float m_tuoGuanOutPokerTime = 2;     // 托管出牌时间
    public float m_qiangDiZhuTime = 10;          // 抢地主时间
    public float m_jiabangTime = 10;             // 加棒时间

    public int m_isDiZhu = 0;                   // 是否是地主

    public bool m_isTuoGuan = false;
    public bool m_isFreeOutPoker = false;
    public bool m_isStartGame = false;

    public void clear()
    {
        s_instance = null;
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
}