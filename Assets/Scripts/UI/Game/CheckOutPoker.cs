using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CheckOutPoker
{
    public enum OutPokerType
    {
        OutPokerType_Error,
        OutPokerType_Single,
        OutPokerType_Double,
        OutPokerType_ShuaiPai,
        OutPokerType_TuoLaJi,
    }

    // 检测出牌合理性
    public static bool checkOutPoker(bool isFreeOutPoker, List<TLJCommon.PokerInfo> myOutPokerList, List<TLJCommon.PokerInfo> beforeOutPokerList, List<TLJCommon.PokerInfo> myRestPokerList)
    {
        // 自由出牌
        if (isFreeOutPoker)
        {
            // 先检查花色是否一致
            {
                for (int i = 1; i < myOutPokerList.Count; i++)
                {
                    if (myOutPokerList[i].m_pokerType != myOutPokerList[0].m_pokerType)
                    {
                        return false;
                    }
                }
            }

            // 检查出牌的类型是否正确：单排、对子、拖拉机、甩牌
            if (checkOutPokerType(myOutPokerList) == OutPokerType.OutPokerType_Error)
            {
                return false;
            }
        }
        // 跟牌
        else
        {
            // 检查张数是否一致
            {
                if (myOutPokerList.Count != beforeOutPokerList.Count)
                {
                    return false;
                }
            }

            // 检查花色是否一致
            {
                if (myOutPokerList[0].m_pokerType != beforeOutPokerList[0].m_pokerType)
                {
                    int num = 0;
                    for (int i = 0; i < myRestPokerList.Count; i++)
                    {
                        if (beforeOutPokerList[0].m_pokerType == myRestPokerList[i].m_pokerType)
                        {
                            ++num;
                        }
                    }

                    if (num >= beforeOutPokerList.Count)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public static OutPokerType checkOutPokerType(List<TLJCommon.PokerInfo> outPokerList)
    {
        int count = outPokerList.Count;

        // 单牌
        if (count == 0)
        {
            return OutPokerType.OutPokerType_Error;
        }
        // 单牌
        else if (count == 1)
        {
            return OutPokerType.OutPokerType_Single;
        }
        // 检查是否是对子
        else if (count == 2)
        {
            if (outPokerList[0].m_pokerType == outPokerList[1].m_pokerType)
            {
                return OutPokerType.OutPokerType_Double;
            }
        }
        // 检查是否是拖拉机
        else if ((count % 2) == 0)
        {
            return OutPokerType.OutPokerType_TuoLaJi;
        }

        return OutPokerType.OutPokerType_Error;
    }
}
