using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLJCommon;

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
    public static bool checkOutPoker(bool isFreeOutPoker, List<TLJCommon.PokerInfo> myOutPokerList,
        List<TLJCommon.PokerInfo> beforeOutPokerList, List<TLJCommon.PokerInfo> myRestPokerList,
        int mLevelPokerNum, int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("CheckOutPoker", "checkOutPoker"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.CheckOutPoker", "checkOutPoker", null, isFreeOutPoker, myOutPokerList, beforeOutPokerList, myRestPokerList);
            return b;
        }

        PlayRuleUtil.SetPokerWeight(beforeOutPokerList, mLevelPokerNum, (Consts.PokerType)masterPokerType);
        PlayRuleUtil.SetPokerWeight(myOutPokerList, mLevelPokerNum, (Consts.PokerType)masterPokerType);
        PlayRuleUtil.SetPokerWeight(myRestPokerList, mLevelPokerNum, (Consts.PokerType)masterPokerType);

        // 自由出牌
        if (isFreeOutPoker)
        {
            //判断是否是主牌
            if (PlayRuleUtil.IsAllMasterPoker(myOutPokerList,mLevelPokerNum, masterPokerType))
            {
                return true;
            }
            //判断是否为同花色副牌
            else if (PlayRuleUtil.IsAllFuPoker(myOutPokerList, mLevelPokerNum, masterPokerType))
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

            switch (CheckOutPoker.checkOutPokerType(beforeOutPokerList, mLevelPokerNum, masterPokerType))
            {
                //第一个人是单牌
                case CheckOutPoker.OutPokerType.OutPokerType_Single:
                {
                    //第一个人出的是主牌
                    if (PlayRuleUtil.IsMasterPoker(beforeOutPokerList[0], mLevelPokerNum, masterPokerType))
                    {
                        if (PlayRuleUtil.IsContainMasterPoker(myRestPokerList, mLevelPokerNum, masterPokerType))
                        {
                            if (PlayRuleUtil.IsMasterPoker(myOutPokerList[0], mLevelPokerNum, masterPokerType))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    //出的是副牌
                    else
                    {
                        if (PlayRuleUtil.IsAllMasterPoker(myRestPokerList, mLevelPokerNum, masterPokerType))
                        {
                            return true;
                        }
                     
                        //如果有该副牌花色必须出该花色
                        List<PokerInfo> typeInfo;
                        if (PlayRuleUtil.IsContainTypePoke(myRestPokerList,mLevelPokerNum, beforeOutPokerList[0].m_pokerType,out typeInfo))
                        {
                            if (myOutPokerList[0].m_pokerType == beforeOutPokerList[0].m_pokerType)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }

                }
                break;

                //第一个人是对子
                case CheckOutPoker.OutPokerType.OutPokerType_Double:
                    {
                        //第一个人出的是主牌对子
                        if (PlayRuleUtil.IsMasterPoker(beforeOutPokerList[0], mLevelPokerNum, masterPokerType))
                        {
                            List<PokerInfo> masterPoker =
                                PlayRuleUtil.GetMasterPoker(myRestPokerList, mLevelPokerNum, masterPokerType);
                            //手中有主牌
                            if (masterPoker.Count > 0)
                            {
                                if (masterPoker.Count == 1)
                                {
                                    if (PlayRuleUtil.IsContainMasterPoker(myOutPokerList, mLevelPokerNum,
                                        masterPokerType))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }

                                //出的主牌中包含对子
                                List<PokerInfo> masterDoublePoker;
                                if (PlayRuleUtil.IsContainDoublePoker(masterPoker,out masterDoublePoker))
                                {
                                    //出的牌都要是主牌
                                    if (PlayRuleUtil.IsAllMasterPoker(myOutPokerList, mLevelPokerNum, masterPokerType))
                                    {
                                        if (myOutPokerList[0].m_num == myOutPokerList[1].m_num
                                            && myOutPokerList[0].m_pokerType == myOutPokerList[1].m_pokerType)
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                //主牌中没有对子，且大于两张
                                else
                                {
                                    if (PlayRuleUtil.IsAllMasterPoker(myOutPokerList, mLevelPokerNum, masterPokerType))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                            //手中没有主牌
                            else
                            {
                                return true;
                            }

                        }
                        //出的是副牌
                        else
                        {
                            if (PlayRuleUtil.IsAllMasterPoker(myRestPokerList, mLevelPokerNum, masterPokerType))
                            {
                                return true;
                            }

                            List<PokerInfo> typeInfo;
                            if (PlayRuleUtil.IsContainTypePoke(myRestPokerList, mLevelPokerNum, beforeOutPokerList[0].m_pokerType, out typeInfo))
                            {
                                if (typeInfo.Count == 1)
                                {
                                    if (myOutPokerList[0].m_pokerType == beforeOutPokerList[0].m_pokerType ||
                                        myOutPokerList[1].m_pokerType == beforeOutPokerList[0].m_pokerType)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                //手中有该花色副牌,且有两张以上
                                else
                                {
                                    if (!PlayRuleUtil.IsAllFuPoker(myOutPokerList, mLevelPokerNum, masterPokerType))
                                    {
                                        return false;
                                    }

                                    List<PokerInfo> doublePoker = PlayRuleUtil.GetDoublePoker(typeInfo);

                                    List<PokerInfo> outdoublePoker = PlayRuleUtil.GetDoublePoker(myOutPokerList);
                                    //有对子走对子
                                    if (doublePoker.Count > 0)
                                    {
                                        return outdoublePoker.Count > 0;
                                    }
                                    else
                                    {
                                        if (myOutPokerList[0].m_pokerType == beforeOutPokerList[0].m_pokerType &&
                                            myOutPokerList[1].m_pokerType == beforeOutPokerList[0].m_pokerType)
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    break;

                //第一个人是拖拉机
                case CheckOutPoker.OutPokerType.OutPokerType_TuoLaJi:
                {
                    int count = beforeOutPokerList.Count;
                    //出的是主牌拖拉机
                    if (PlayRuleUtil.IsAllMasterPoker(beforeOutPokerList, mLevelPokerNum, masterPokerType))
                    {
                        List<PokerInfo> MyMasterPoker =  PlayRuleUtil.GetMasterPoker(myRestPokerList, mLevelPokerNum, masterPokerType);
                        List<PokerInfo> outMasterPoker =  PlayRuleUtil.GetMasterPoker(myOutPokerList, mLevelPokerNum, masterPokerType);
                         return PlayRuleUtil.IsSendByTuoLaJi(myOutPokerList, MyMasterPoker, outMasterPoker, mLevelPokerNum, masterPokerType);
                    }
                    //出的是副牌的拖拉机
                    else if (PlayRuleUtil.IsAllFuPoker(beforeOutPokerList, mLevelPokerNum, masterPokerType))
                    {
                        //得到手牌中该副牌
                        List<PokerInfo> myPokerByType = PlayRuleUtil.GetPokerByType(myRestPokerList,mLevelPokerNum, beforeOutPokerList[0].m_pokerType);
                        //得到出牌中该副牌
                        List<PokerInfo> outPokerByType = PlayRuleUtil.GetPokerByType(myOutPokerList, mLevelPokerNum, beforeOutPokerList[0].m_pokerType);
                        return PlayRuleUtil.IsSendByTuoLaJi(myOutPokerList, myPokerByType, outPokerByType, mLevelPokerNum, masterPokerType);
                    }
                }
                break;

                //第一个人是甩牌
                case CheckOutPoker.OutPokerType.OutPokerType_ShuaiPai:
                {

                    List<PokerInfo> firstDoublePoker = PlayRuleUtil.GetDoublePoker(beforeOutPokerList);
                    List<PokerInfo> firstSinglePoker = PlayRuleUtil.GetSinglePoker(beforeOutPokerList, firstDoublePoker);
                    if (PlayRuleUtil.IsAllMasterPoker(beforeOutPokerList, mLevelPokerNum, masterPokerType))
                    {
                        List<PokerInfo> myMasterPoker = PlayRuleUtil.GetMasterPoker(myRestPokerList, mLevelPokerNum, masterPokerType);
                        List<PokerInfo> myMasterDoublePoker = PlayRuleUtil.GetDoublePoker(myMasterPoker);

                        List<PokerInfo> myOutMasterPoker = PlayRuleUtil.GetMasterPoker(myOutPokerList, mLevelPokerNum, masterPokerType);
                        List<PokerInfo> myOutMasterDoublePoker = PlayRuleUtil.GetDoublePoker(myOutMasterPoker);

                        //甩的牌全是单牌
                        if (firstDoublePoker.Count == 0)
                        {
                            if (myMasterPoker.Count <= beforeOutPokerList.Count)
                                return myOutMasterPoker.Count == myMasterPoker.Count;
                            return PlayRuleUtil.IsAllMasterPoker(myOutPokerList, mLevelPokerNum, masterPokerType);
                        }
                        //甩的牌中有对子
                        else
                        {
                            //自己手中的对子不够
                            if (myMasterDoublePoker.Count <= firstDoublePoker.Count) return myOutMasterDoublePoker.Count == myMasterDoublePoker.Count;
                            //对子够
                            List<List<PokerInfo>> allTljFromFirst = PlayRuleUtil.GetAllTljFromDouble(firstDoublePoker, mLevelPokerNum, masterPokerType);
                            List<List<PokerInfo>> allTljFromMy = PlayRuleUtil.GetAllTljFromDouble(myMasterDoublePoker, mLevelPokerNum, masterPokerType);
                            List<List<PokerInfo>> allTljFromMyOut = PlayRuleUtil.GetAllTljFromDouble(myOutMasterDoublePoker, mLevelPokerNum, masterPokerType);
                            if (allTljFromFirst.Count == 0 || allTljFromMy.Count == 0)
                            {
                                return firstDoublePoker.Count == myOutMasterDoublePoker.Count;
                            }
                            if (allTljFromFirst.Count <= allTljFromMy.Count)
                            {
                                return allTljFromMyOut.Count == allTljFromFirst.Count;
                            }
                            else
                            {
                                return allTljFromMyOut.Count == allTljFromMy.Count;
                            }
                        }
                    }
                    //甩的牌是同花色的副牌
                    else
                    {
                        List<PokerInfo> myFuPoker = PlayRuleUtil.GetPokerByType(myRestPokerList,mLevelPokerNum, beforeOutPokerList[0].m_pokerType);
                        List<PokerInfo> myFuDoublePoker = PlayRuleUtil.GetDoublePoker(myFuPoker);

                        List<PokerInfo> myOutFuPoker = PlayRuleUtil.GetPokerByType(myOutPokerList, mLevelPokerNum, beforeOutPokerList[0].m_pokerType);
                        List<PokerInfo> myOutFuDoublePoker = PlayRuleUtil.GetDoublePoker(myOutFuPoker);

                        //甩的牌全是单牌
                        if (firstDoublePoker.Count == 0)
                        {
                            if (myFuPoker.Count <= beforeOutPokerList.Count)
                                return myOutFuPoker.Count == myFuPoker.Count;
                            return PlayRuleUtil.IsAllFuPoker(myOutPokerList, mLevelPokerNum, mLevelPokerNum);
                        }
                        //甩的牌中有对子
                        else
                        {
                            //自己手中的对子不够
                            if (myFuDoublePoker.Count <= firstDoublePoker.Count) return myOutFuDoublePoker.Count == myFuDoublePoker.Count;

                            //对子够
                            List<List<PokerInfo>> allTljFromFirst = PlayRuleUtil.GetAllTljFromDouble(firstDoublePoker, mLevelPokerNum, masterPokerType);
                            List<List<PokerInfo>> allTljFromMy = PlayRuleUtil.GetAllTljFromDouble(myFuDoublePoker, mLevelPokerNum, masterPokerType);
                            List<List<PokerInfo>> allTljFromMyOut = PlayRuleUtil.GetAllTljFromDouble(myOutFuDoublePoker, mLevelPokerNum, masterPokerType);
                            if (allTljFromFirst.Count == 0 || allTljFromMy.Count == 0)
                            {
                                return myOutFuDoublePoker.Count == firstDoublePoker.Count;
                            }
                            if (allTljFromFirst.Count <= allTljFromMy.Count)
                            {
                                return allTljFromMyOut.Count == allTljFromFirst.Count;
                            }
                            else
                            {
                                return allTljFromMyOut.Count == allTljFromMy.Count;
                            }
                        }
                    }
                }
                break;
            }
        }

        return true;
    }

  

    public static OutPokerType checkOutPokerType(List<TLJCommon.PokerInfo> outPokerList, int mLevelPokerNum, int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("CheckOutPoker", "checkOutPokerType"))
        {
            OutPokerType outPokerType = (OutPokerType)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.CheckOutPoker", "checkOutPokerType", null, outPokerList, mLevelPokerNum, masterPokerType);
            return outPokerType;
        }

        PlayRuleUtil.SetPokerWeight(outPokerList, mLevelPokerNum, (Consts.PokerType) masterPokerType);

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
        else if (count % 2 == 0 && count >= 4)
        {

            if (PlayRuleUtil.IsTuolaji(outPokerList, mLevelPokerNum, masterPokerType))
            {
//                TLJ_PlayService.PlayService.log.Info("出的是拖拉机");
                return OutPokerType.OutPokerType_TuoLaJi;
            }
        }
//        TLJ_PlayService.PlayService.log.Info("有人尝试甩牌");
        return OutPokerType.OutPokerType_ShuaiPai;
    }
}
