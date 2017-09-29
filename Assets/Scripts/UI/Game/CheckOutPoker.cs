using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLJCommon;
using UnityEngine;

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
            //判断是否是主牌
            if (PlayRuleUtil.IsAllMasterPoker(myOutPokerList))
            {
                return true;
            }
            //判断是否为同花色副牌
            else if (PlayRuleUtil.IsAllFuPoker(myOutPokerList))
            {
                return true;
            }
            else
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
                case CheckOutPoker.OutPokerType.OutPokerType_TuoLaJi:
                    {
                        if (checkOutPokerType(myOutPokerList) == OutPokerType.OutPokerType_TuoLaJi)
                        {
                            return true;
                        }
                        else
                        {
                            List<TLJCommon.PokerInfo> firstOutPokerList_single = GameUtil.choiceSinglePoker(beforeOutPokerList, beforeOutPokerList[0].m_pokerType);
                            List<TLJCommon.PokerInfo> firstOutPokerList_double = GameUtil.choiceDoublePoker(beforeOutPokerList, beforeOutPokerList[0].m_pokerType);

                            List<TLJCommon.PokerInfo> myOutPokerList_single = GameUtil.choiceSinglePoker(myOutPokerList, beforeOutPokerList[0].m_pokerType);
                            List<TLJCommon.PokerInfo> myOutPokerList_double = GameUtil.choiceDoublePoker(myOutPokerList, beforeOutPokerList[0].m_pokerType);

                            List<TLJCommon.PokerInfo> myRestPokerList_single = GameUtil.choiceSinglePoker(myRestPokerList, beforeOutPokerList[0].m_pokerType);
                            List<TLJCommon.PokerInfo> myRestPokerList_double = GameUtil.choiceDoublePoker(myRestPokerList, beforeOutPokerList[0].m_pokerType);

                            // 先找拖拉机
                            {
                                for (int i = myRestPokerList_double.Count - 1; i >= firstOutPokerList_double.Count - 1; i--)
                                {
                                    bool find = true;
                                    for (int j = 0; j < firstOutPokerList_double.Count - 1; j++)
                                    {
                                        if ((myRestPokerList_double[i - j].m_num - myRestPokerList_double[i - j - 1].m_num) != -1)
                                        {
                                            find = false;
                                        }
                                    }

                                    if (find)
                                    {
                                        return false;
                                    }
                                }
                            }

                            // 如果没有拖拉机，则按正常流走
                            {
                                // 如果出的单牌数量比规定的少
                                if ((myOutPokerList_single.Count < firstOutPokerList_single.Count))
                                {
                                    if (myRestPokerList_single.Count > myOutPokerList_single.Count)
                                    {
                                        return false;
                                    }
                                }

                                // 如果出的对子数量比规定的少
                                if ((myOutPokerList_double.Count < firstOutPokerList_double.Count))
                                {
                                    if (myRestPokerList_double.Count > myOutPokerList_double.Count)
                                    {
                                        return false;
                                    }
                                }

                                // 如果出的单牌是由对子拆分的
                                {
                                    int temp = 0;
                                    for (int i = 0; i < myOutPokerList_single.Count; i++)
                                    {
                                        for (int j = 0; j < myRestPokerList_double.Count; j++)
                                        {
                                            if (myOutPokerList_single[i].m_num == myRestPokerList_double[j].m_num)
                                            {
                                                ++temp;
                                                break;
                                            }
                                        }
                                    }

                                    if (firstOutPokerList_single.Count <= myRestPokerList_single.Count)
                                    {
                                        if (temp > 0)
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (temp > (firstOutPokerList_single.Count - myRestPokerList_single.Count))
                                        {
                                            return false;
                                        }
                                    }
                                }

                                // 如果出的同花色的牌比比规定的少
                                {
                                    int allNum = firstOutPokerList_single.Count + firstOutPokerList_double.Count * 2;
                                    int outSampleTypeNum = myOutPokerList_single.Count + myOutPokerList_double.Count * 2;
                                    int restSampleTypeNum = myRestPokerList_single.Count + myRestPokerList_double.Count * 2;
                                    if (outSampleTypeNum < allNum)
                                    {
                                        if (restSampleTypeNum > outSampleTypeNum)
                                        {
                                            return false;
                                        }
                                    }
                                }

                                return true;
                            }
                        }
                    }
                    break;

                case CheckOutPoker.OutPokerType.OutPokerType_Single:
                case CheckOutPoker.OutPokerType.OutPokerType_Double:
                case CheckOutPoker.OutPokerType.OutPokerType_ShuaiPai:
                case CheckOutPoker.OutPokerType.OutPokerType_Error:
                    {
                        List<TLJCommon.PokerInfo> firstOutPokerList_single = GameUtil.choiceSinglePoker(beforeOutPokerList, beforeOutPokerList[0].m_pokerType);
                        List<TLJCommon.PokerInfo> firstOutPokerList_double = GameUtil.choiceDoublePoker(beforeOutPokerList, beforeOutPokerList[0].m_pokerType);

                        List<TLJCommon.PokerInfo> myOutPokerList_single = GameUtil.choiceSinglePoker(myOutPokerList, beforeOutPokerList[0].m_pokerType);
                        List<TLJCommon.PokerInfo> myOutPokerList_double = GameUtil.choiceDoublePoker(myOutPokerList, beforeOutPokerList[0].m_pokerType);

                        List<TLJCommon.PokerInfo> myRestPokerList_single = GameUtil.choiceSinglePoker(myRestPokerList, beforeOutPokerList[0].m_pokerType);
                        List<TLJCommon.PokerInfo> myRestPokerList_double = GameUtil.choiceDoublePoker(myRestPokerList, beforeOutPokerList[0].m_pokerType);

                        // 如果出的单牌数量比规定的少
                        if ((myOutPokerList_single.Count < firstOutPokerList_single.Count))
                        {
                            if (myRestPokerList_single.Count > myOutPokerList_single.Count)
                            {
                                return false;
                            }
                        }

                        // 如果出的对子数量比规定的少
                        if ((myOutPokerList_double.Count < firstOutPokerList_double.Count))
                        {
                            if (myRestPokerList_double.Count > myOutPokerList_double.Count)
                            {
                                return false;
                            }
                        }

                        // 如果出的单牌是由对子拆分的
                        {
                            int temp = 0;
                            for (int i = 0; i < myOutPokerList_single.Count; i++)
                            {
                                for (int j = 0; j < myRestPokerList_double.Count; j++)
                                {
                                    if (myOutPokerList_single[i].m_num == myRestPokerList_double[j].m_num)
                                    {
                                        ++temp;
                                        break;
                                    }
                                }
                            }

                            if (firstOutPokerList_single.Count <= myRestPokerList_single.Count)
                            {
                                if (temp > 0)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (temp > (firstOutPokerList_single.Count - myRestPokerList_single.Count))
                                {
                                    return false;
                                }
                            }
                        }

                        // 如果出的同花色的牌比比规定的少
                        {
                            int allNum = firstOutPokerList_single.Count + firstOutPokerList_double.Count * 2;
                            int outSampleTypeNum = myOutPokerList_single.Count + myOutPokerList_double.Count * 2;
                            int restSampleTypeNum = myRestPokerList_single.Count + myRestPokerList_double.Count * 2;
                            if (outSampleTypeNum < allNum)
                            {
                                if (restSampleTypeNum > outSampleTypeNum)
                                {
                                    return false;
                                }
                            }
                        }

                        return true;
                    }
                    break;
            }
        }

        return true;
    }

    public static OutPokerType checkOutPokerType(List<TLJCommon.PokerInfo> outPokerList)
    {
        int count = outPokerList.Count;
        PlayRuleUtil.SetPokerWeight(outPokerList,GameScript.m_levelPokerNum,(Consts.PokerType) GameScript.m_masterPokerType);

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
        else if (count % 2 == 0 && count >= 4)
        {
            if (PlayRuleUtil.CheckTuoLaJi(outPokerList))
            {
                ToastScript.createToast("出的是拖拉机");
				
                return OutPokerType.OutPokerType_TuoLaJi;
            }
        }
//        else if (((count % 2) == 0) && (count >= 4))
//        {
//            bool isSampleType = true;
//            
//            for (int i = 1; i < outPokerList.Count; i++)
//            {
//                if (outPokerList[i].m_pokerType != outPokerList[0].m_pokerType)
//                {
//                    isSampleType = false;
//                    break;
//                }
//            }
//
//            if (isSampleType)
//            {
//                // 排序:从大到小
//                for (int i = 0; i < outPokerList.Count - 1; i++)
//                {
//                    for (int j = (i + 1); j < outPokerList.Count; j++)
//                    {
//                        if (outPokerList[j].m_num > outPokerList[i].m_num)
//                        {
//                            TLJCommon.PokerInfo temp = outPokerList[j];
//                            outPokerList[j] = outPokerList[i];
//                            outPokerList[i] = temp;
//                        }
//                    }
//                }
//
//                bool isTuoLaJi = true;
//                int beforeNum = outPokerList[0].m_num + 1;
//                for (int i = 0; i < outPokerList.Count - 1; i += 2)
//                {
//                    if ((outPokerList[i].m_num == outPokerList[i + 1].m_num) && ((outPokerList[i].m_num - beforeNum) == -1))
//                    {
//                        beforeNum = outPokerList[i].m_num;
//                    }
//                    else
//                    {
//                        isTuoLaJi = false;
//                        break;
//                    }
//                }
//
//                if (isTuoLaJi)
//                {
//                    return OutPokerType.OutPokerType_TuoLaJi;
//                }
//            }
//        }

        return OutPokerType.OutPokerType_Error;
    }
}
