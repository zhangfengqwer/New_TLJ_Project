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

            switch (CheckOutPoker.checkOutPokerType(beforeOutPokerList))
            {
                case CheckOutPoker.OutPokerType.OutPokerType_Single:
                    {
                        if (myOutPokerList[0].m_pokerType == beforeOutPokerList[0].m_pokerType)
                        {
                            return true;
                        }

                        for (int i = 0; i < myRestPokerList.Count; i++)
                        {
                            if (myRestPokerList[i].m_pokerType == beforeOutPokerList[0].m_pokerType)
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                    break;

                case CheckOutPoker.OutPokerType.OutPokerType_Double:
                    {
                        // 跟上家一样的同花色的对子
                        if ((myOutPokerList[0].m_pokerType == myOutPokerList[1].m_pokerType) && (myOutPokerList[0].m_num == myOutPokerList[1].m_num) && (myOutPokerList[0].m_pokerType == beforeOutPokerList[0].m_pokerType))
                        {
                            return true;
                        }
                        else
                        {
                            for (int i = myRestPokerList.Count - 1; i >= 1; i--)
                            {
                                if (myRestPokerList[i].m_pokerType == beforeOutPokerList[0].m_pokerType)
                                {
                                    if (myRestPokerList[i - 1].m_pokerType == beforeOutPokerList[0].m_pokerType)
                                    {
                                        if (myRestPokerList[i].m_num == myRestPokerList[i - 1].m_num)
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                        }

                        // 跟上家一样的同花色
                        if ((myOutPokerList[0].m_pokerType == myOutPokerList[1].m_pokerType) && (myOutPokerList[0].m_pokerType == beforeOutPokerList[0].m_pokerType))
                        {
                            return true;
                        }
                        else
                        {
                            for (int i = myRestPokerList.Count - 1; i >= 1; i--)
                            {
                                if (myRestPokerList[i].m_pokerType == beforeOutPokerList[0].m_pokerType)
                                {
                                    if (myRestPokerList[i - 1].m_pokerType == beforeOutPokerList[0].m_pokerType)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }

                        return true;
                    }
                    break;

                case CheckOutPoker.OutPokerType.OutPokerType_TuoLaJi:
                    {
                        //bool m_isSampleType = true;

                        //// 检查花色
                        //for (int i = 0; i < myOutPokerList.Count; i++)
                        //{
                        //    if (myOutPokerList[i].m_pokerType != beforeOutPokerList[0].m_pokerType)
                        //    {
                        //        m_isSampleType = false;
                        //    }
                        //}

                        //if (m_isSampleType)
                        int count = beforeOutPokerList.Count;
                        {
                            // 是拖拉机
                            {
                                if (checkOutPokerType(myOutPokerList) == OutPokerType.OutPokerType_TuoLaJi)
                                {
                                    return true;
                                }
                                else
                                {
                                    // 检测是否有拖拉机而玩家没出，是的话则出牌失败
                                    {
                                        List<TLJCommon.PokerInfo> tempList = new List<TLJCommon.PokerInfo>();
                                        for (int i = myRestPokerList.Count - 1; i >= (count - 1); i--)
                                        {
                                            if (myRestPokerList[i].m_pokerType == beforeOutPokerList[0].m_pokerType)
                                            {
                                                for (int j = 0; j < count; j++)
                                                {
                                                    tempList.Add(new TLJCommon.PokerInfo(myRestPokerList[i - j].m_num, myRestPokerList[i - j].m_pokerType));
                                                }

                                                // 找到拖拉机了
                                                if (CheckOutPoker.checkOutPokerType(tempList) == CheckOutPoker.OutPokerType.OutPokerType_TuoLaJi)
                                                {
                                                    return false;
                                                }
                                                else
                                                {
                                                    tempList.Clear();
                                                }
                                            }
                                            else
                                            {
                                                tempList.Clear();
                                            }
                                        }
                                    }
                                }
                            }

                            // 不是拖拉机，检测是否都是对子
                            {
                                int doubleNum = beforeOutPokerList.Count / 2;
                                int myOutDoubleNum = 0;
                                int myRestDoubleNum = 0;
                                for (int i = 0; i < myOutPokerList.Count ; i += 2)
                                {
                                    if ((myOutPokerList[i].m_num != myOutPokerList[i + 1].m_num) &&
                                        (myOutPokerList[i].m_pokerType != myOutPokerList[i + 1].m_pokerType) &&
                                        (myOutPokerList[i].m_pokerType != beforeOutPokerList[0].m_pokerType))
                                    {
                                        ++myOutDoubleNum;
                                    }
                                }

                                // 都是对子
                                if (myOutDoubleNum == doubleNum)
                                {
                                    return true;
                                }
                                else
                                {
                                    for (int i = myRestPokerList.Count - 1; i >= 1; i--)
                                    {
                                        if ((myRestPokerList[i].m_num == myRestPokerList[i - 1].m_num) && 
                                            (myRestPokerList[i].m_pokerType == myRestPokerList[i - 1].m_pokerType) &&
                                            (myRestPokerList[i].m_pokerType == beforeOutPokerList[0].m_pokerType))
                                        {
                                            ++myRestDoubleNum;
                                        }
                                    }

                                    // 我出的不都是对子，但是剩余的牌中明明有对子，则出牌失败
                                    if (myRestDoubleNum > myOutDoubleNum)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        int restSampleTypeNum = 0;
                                        int outSampleTypeNum = 0;
                                        
                                        for (int i = 0; i < myRestPokerList.Count; i++)
                                        {
                                            if (myRestPokerList[i].m_pokerType == beforeOutPokerList[0].m_pokerType)
                                            {
                                                ++restSampleTypeNum;
                                            }
                                        }

                                        for (int i = 0; i < myOutPokerList.Count; i++)
                                        {
                                            if (myOutPokerList[i].m_pokerType == beforeOutPokerList[0].m_pokerType)
                                            {
                                                ++outSampleTypeNum;
                                            }
                                        }

                                        if (outSampleTypeNum == count)
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            if (restSampleTypeNum > outSampleTypeNum)
                                            {
                                                return false;
                                            }
                                            else
                                            {
                                                return true;
                                            }
                                        }
                                        
                                    }
                                }
                            }
                        }
                        
                    }
                    break;

                case CheckOutPoker.OutPokerType.OutPokerType_ShuaiPai:
                    {

                    }
                    break;
            }
        }

        return true;
    }

    public static OutPokerType checkOutPokerType(List<TLJCommon.PokerInfo> outPokerList)
    {
        int count = outPokerList.Count;

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
            if ((outPokerList[0].m_pokerType == outPokerList[1].m_pokerType) && (outPokerList[0].m_num == outPokerList[1].m_num))
            {
                return OutPokerType.OutPokerType_Double;
            }
        }
        // 检查是否是拖拉机
        else if (((count % 2) == 0) && (count >= 4))
        {
            bool isSampleType = true;
            
            for (int i = 1; i < outPokerList.Count; i++)
            {
                if (outPokerList[i].m_pokerType != outPokerList[0].m_pokerType)
                {
                    isSampleType = false;
                    break;
                }
            }

            if (isSampleType)
            {
                bool isTuoLaJi = true;
                int beforeNum = outPokerList[0].m_num + 1;
                for (int i = 0; i < outPokerList.Count - 1; i += 2)
                {
                    if ((outPokerList[i].m_num == outPokerList[i + 1].m_num) && ((outPokerList[i].m_num - beforeNum) == -1))
                    {
                        beforeNum = outPokerList[i].m_num;
                    }
                    else
                    {
                        isTuoLaJi = false;
                        break;
                    }
                }

                if (isTuoLaJi)
                {
                    return OutPokerType.OutPokerType_TuoLaJi;
                }
            }
        }

        return OutPokerType.OutPokerType_Error;
    }
}
