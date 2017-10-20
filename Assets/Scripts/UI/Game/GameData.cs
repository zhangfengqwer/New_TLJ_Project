﻿using System.Collections.Generic;
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
    public int m_masterPokerType = -1;          // 主牌花色

    public int m_isBanker;                      // 是否属于庄家一方
    public int m_getAllScore = 0;               // 庄家对家抓到的分数

    public string m_teammateUID = "";           // 我的队友uid
    public string m_curOutPokerPlayerUid = "";  // 当前出牌的人uid

    public bool m_isTuoGuan = false;
    public bool m_isFreeOutPoker = false;
    
    public void clear()
    {
        m_myPokerList.Clear();
        m_beforeQiangzhuPokerList.Clear();
        m_curRoundFirstOutPokerList.Clear();
        m_playerDataList.Clear();

        //{
        //    for (int i = 0; i < m_otherPlayerUIObjList.Count; i++)
        //    {
        //        Destroy(m_otherPlayerUIObjList[i]);
        //    }
        //    m_otherPlayerUIObjList.Clear();
        //}

        //{
        //    for (int i = 0; i < m_myPokerObjList.Count; i++)
        //    {
        //        Destroy(m_myPokerObjList[i]);
        //    }
        //    m_myPokerObjList.Clear();
        //}

        //{
        //    for (int i = 0; i < m_curRoundOutPokerList.Count; i++)
        //    {
        //        for (int j = 0; j < m_curRoundOutPokerList.Count; j++)
        //        {
        //            Destroy(m_curRoundOutPokerList[i][j]);
        //        }
        //    }
        //    m_curRoundOutPokerList.Clear();
        //}
    }
}

public class PlayerData
{
    public string m_uid;
    public string m_teammateUID;    // 队友uid
    public int m_isBanker = 0;      // 是否是庄家

    List<TLJCommon.PokerInfo> m_pokerList = new List<TLJCommon.PokerInfo>();
    public List<TLJCommon.PokerInfo> m_curOutPokerList = new List<TLJCommon.PokerInfo>();
}