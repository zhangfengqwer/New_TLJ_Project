using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLJCommon;


public class PlayRuleUtil
{
    //检查是否是拖拉机
    public static bool CheckTuoLaJi(List<PokerInfo> playerOutPokerList)
    {
        if (playerOutPokerList.Count % 2 == 0 && playerOutPokerList.Count >= 4)
        {
            //都是主牌或者都是同一花色的副牌
            if (IsAllMasterPoker(playerOutPokerList) || IsAllFuPoker(playerOutPokerList))
            {
                //先判断是否为对子
                for (int i = 0; i < playerOutPokerList.Count; i += 2)
                {
                    if (playerOutPokerList[i].m_num != playerOutPokerList[i + 1].m_num
                        || playerOutPokerList[i].m_pokerType != playerOutPokerList[i + 1].m_pokerType)
                    {
                        return false;
                    }
                }
                //判断权重
                for (int i = 0; i < playerOutPokerList.Count - 2; i += 2)
                {
                    if (playerOutPokerList[i + 2].m_weight - playerOutPokerList[i].m_weight != 1)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        return false;
    }

    //单牌是否为主牌
    static bool IsMasterPoker(PokerInfo pokerInfo)
    {
        if (pokerInfo.m_num ==RoomData.m_levelPokerNum || (int)pokerInfo.m_pokerType == RoomData.m_masterPokerType
            || pokerInfo.m_pokerType == Consts.PokerType.PokerType_Wang)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //是否都是主牌
    public static bool IsAllMasterPoker(List<PokerInfo> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!IsMasterPoker(list[i]))
            {
                return false;
            }
        }
        return true;
    }

    //是否都是同一花色
    public static bool IsAllFuPoker(List<PokerInfo> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            if (list[i].m_pokerType != list[i + 1].m_pokerType)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    ///  给weight重新赋值，从2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17
    ///  17为大王，16为小王，15为主级牌,14为副级牌
    ///  无主情况下,16为大王,15为小王，14为级牌
    /// </summary>
    /// <param name="list">牌</param>
    /// <param name="levelPokerNum">级牌</param>
    /// <param name="masterPokerType">主牌花色</param>
    /// <returns></returns>
    public static List<PokerInfo> SetPokerWeight(List<PokerInfo> list, int levelPokerNum,
        Consts.PokerType masterPokerType)
    {
        for (int i = 0; i < list.Count; i++)
        {
            PokerInfo pokerInfo = list[i];
            //是级牌
            if (pokerInfo.m_num == levelPokerNum)
            {
                if (pokerInfo.m_pokerType == masterPokerType)
                {
                    pokerInfo.m_weight = 15;
                }
                else
                {
                    pokerInfo.m_weight = 14;
                }
            }
            //大王
            else if (pokerInfo.m_num == 16)
            {
                pokerInfo.m_weight = (int)masterPokerType != (-1) ? 17 : 16;
            }
            //小王
            else if (pokerInfo.m_num == 15)
            {
                pokerInfo.m_weight = (int)masterPokerType != (-1) ? 16 : 15;
            }
            else if (pokerInfo.m_num < levelPokerNum)
            {
                pokerInfo.m_weight = pokerInfo.m_num;
            }
            else
            {
                pokerInfo.m_weight = pokerInfo.m_num - 1;
            }
        }
        return list;
    }
}