using System;
using System.Collections.Generic;
using System.Linq;
using TLJCommon;


public class PlayRuleUtil
{
    /// <summary>
    /// 只能判断是否是一个连续的拖拉机
    /// </summary>
    /// <param name="playerOutPokerList"></param>
    /// <param name="mLevelPokerNum"></param>g
    /// <param name="masterPokerType"></param>
    /// <returns></returns>
    public static bool IsTuolaji(List<PokerInfo> playerOutPokerList, int mLevelPokerNum, int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "IsTuolaji"))
        {
            bool b= (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "IsTuolaji", null, playerOutPokerList, mLevelPokerNum, masterPokerType);
            return b;
        }

        if (playerOutPokerList.Count % 2 == 0 && playerOutPokerList.Count >= 4)
        {
            //都是主牌或者都是同一花色的副牌
            if (IsAllMasterPoker(playerOutPokerList, mLevelPokerNum, masterPokerType) ||
                IsAllFuPoker(playerOutPokerList, mLevelPokerNum, masterPokerType))
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
                    if (Math.Abs(playerOutPokerList[i + 2].m_weight - playerOutPokerList[i].m_weight) != 1)
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
    public static bool IsMasterPoker(PokerInfo pokerInfo, int mLevelPokerNum, int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "IsMasterPoker"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "IsMasterPoker", null, pokerInfo, mLevelPokerNum, masterPokerType);
            return b;
        }

        if (pokerInfo.m_num == mLevelPokerNum || pokerInfo.m_pokerType == (Consts.PokerType) masterPokerType
            || pokerInfo.m_pokerType == Consts.PokerType.PokerType_Wang)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 是否都是主牌
    /// </summary>
    /// <param name="list"></param>
    /// <param name="mLevelPokerNum"></param>
    /// <param name="masterPokerType"></param>
    /// <returns></returns>
    public static bool IsAllMasterPoker(List<PokerInfo> list, int mLevelPokerNum, int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "IsAllMasterPoker"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "IsAllMasterPoker", null, list, mLevelPokerNum, masterPokerType);
            return b;
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (!IsMasterPoker(list[i], mLevelPokerNum, masterPokerType))
            {
                return false;
            }
        }
        return true;
    }

    //    public static List<PokerInfo> Get
    /// <summary>
    /// 是否都是同一花色的副牌
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool IsAllFuPoker(List<PokerInfo> list, int mLevelPokerNum, int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "IsAllFuPoker"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "IsAllFuPoker", null, list, mLevelPokerNum, masterPokerType);
            return b;
        }

        for (int i = 0; i < list.Count - 1; i++)
        {
            if (list[i].m_num == mLevelPokerNum || list[i].m_pokerType == (Consts.PokerType) masterPokerType)
            {
                return false;
            }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "SetPokerWeight"))
        {
            List<PokerInfo> list_temp = (List<PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "SetPokerWeight", null, list, levelPokerNum, masterPokerType);
            return list_temp;
        }

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
                pokerInfo.m_weight = (int) masterPokerType != (-1) ? 17 : 16;
            }
            //小王
            else if (pokerInfo.m_num == 15)
            {
                pokerInfo.m_weight = (int) masterPokerType != (-1) ? 16 : 15;
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

    //比较甩牌中的对子
    public static List<PokerInfo> CompareDoublePoker(List<PokerInfo> firestDoubleList,
        List<PokerInfo> OtherDoubleleList,
        int mLevelPokerNum, int mMasterPokerType, out List<List<PokerInfo>> OtherTlj)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "CompareDoublePoker"))
        {
            OtherTlj = new List<List<PokerInfo>>();
            List<PokerInfo> list_temp = (List<PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "CompareDoublePoker", null, firestDoubleList, OtherDoubleleList, mLevelPokerNum, mMasterPokerType, OtherTlj);
            return list_temp;
        }

        List<PokerInfo> list = new List<PokerInfo>();
        OtherTlj = new List<List<PokerInfo>>();
        //继续比较对子
        if (firestDoubleList.Count > 0)
        {
            if (OtherDoubleleList.Count > 0)
            {
                List<List<PokerInfo>> firstAllTlj =
                    GetAllTljFromDouble(firestDoubleList, mLevelPokerNum, mMasterPokerType);
                List<List<PokerInfo>> OtherAllTlj =
                    GetAllTljFromDouble(OtherDoubleleList, mLevelPokerNum, mMasterPokerType);
                //如果甩牌的对子中包括拖拉机,则和其他玩家手中的拖拉机比较
                if (firstAllTlj.Count > 0)
                {
                    if (OtherAllTlj.Count > 0)
                    {
                        OtherTlj = OtherAllTlj;
                        for (int j = 0; j < firstAllTlj.Count; j++)
                        {
                            List<PokerInfo> TljPoker = firstAllTlj[j];
                            for (int k = 0; k < OtherAllTlj.Count; k++)
                            {
                                var otherTljPoker = OtherAllTlj[k];
                                //第一个拖拉机的对子多于其他玩家手中的拖拉机
                                if (TljPoker.Count > otherTljPoker.Count)
                                {
                                }
                                else
                                {
                                    //第一个拖拉机最小对子大于其他玩家手中的拖拉机最大的
                                    if (TljPoker[0].m_weight >=
                                        otherTljPoker[otherTljPoker.Count - 1].m_weight)
                                    {
                                        //TLJ_PlayService.PlayService.log.Info("拖拉机大");
                                    }
                                    else
                                    {
                                        //TLJ_PlayService.PlayService.log.Info("拖拉机比别人小");
                                        list = TljPoker;
                                        return list;
                                    }
                                }
                            }
                        }
                    }
                    //其他玩家手中没有拖拉机
                    else
                    {
                        //TLJ_PlayService.PlayService.log.Info("没有拖拉机");
                        //该玩家牌大，不作处理 
                    }
                }
                //甩牌为多个不连续对子
                else
                {
                    if (OtherDoubleleList.Count > 0)
                    {
                        if (firestDoubleList[0].m_weight >= OtherDoubleleList[OtherDoubleleList.Count - 1].m_weight)
                        {
                            //该玩家牌大，不作处理
                        }
                        else
                        {
                            //甩牌失败,单牌大，但是对子比别人小
                            //TLJ_PlayService.PlayService.log.Info("甩牌失败,单牌大，但是对子比别人小");
                            list.Add(firestDoubleList[0]);
                            list.Add(firestDoubleList[1]);
                            return list;
                        }
                    }
                    //没有对子
                    else
                    {
                    }
                }
            }
            //其他玩家没有对子，甩牌成功
            else
            {
                //该玩家牌大，不作处理 
            }
        }
        return list;
    }

    /// <summary>
    /// 得到指定花色的副牌,并排序
    /// </summary>
    /// <param name="pokerList"></param>
    /// <param name="mPokerType"></param>
    public static List<PokerInfo> GetPokerByType(List<PokerInfo> pokerList, int mLevelPokerNum,
        Consts.PokerType mPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetPokerByType"))
        {
            List<PokerInfo> list_temp = (List<PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetPokerByType", null, pokerList, mLevelPokerNum, mPokerType);
            return list_temp;
        }

        List<PokerInfo> list = new List<PokerInfo>();
        for (int i = 0; i < pokerList.Count; i++)
        {
            var poker = pokerList[i];
            if (poker.m_pokerType == mPokerType && poker.m_num != mLevelPokerNum)
            {
                list.Add(poker);
            }
        }
        return list.OrderBy(a => a.m_weight).ToList();
    }

    /// <summary>
    /// 通过去除对子来得到单牌,并通过权重排序
    /// </summary>
    /// <param name="outPokerList"></param>
    /// <param name="firestDoubleList"></param>
    /// <returns></returns>
    public static List<PokerInfo> GetSinglePoker(List<PokerInfo> PokerList, List<PokerInfo> DoubleList)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetSinglePoker"))
        {
            List<PokerInfo> list_temp = (List<PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetSinglePoker", null, PokerList, DoubleList);
            return list_temp;
        }

        List<PokerInfo> firestSingleList = new List<PokerInfo>();
        firestSingleList = firestSingleList.Union(PokerList.Except(DoubleList).ToList()).ToList();
        return firestSingleList.OrderBy(a => a.m_weight).ToList();
    }

    /// <summary>
    /// 得到牌中对子，并通过权重排序
    /// 如果是3副以上的牌，此算法有问题
    /// </summary>
    /// <param name="PokerList"></param>
    /// <returns></returns>
    public static List<PokerInfo> GetDoublePoker(List<PokerInfo> PokerList)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetDoublePoker"))
        {
            List<PokerInfo> list_temp = (List<PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetDoublePoker", null, PokerList);
            return list_temp;
        }

        List<PokerInfo> firestDoubleList = new List<PokerInfo>();
        for (int i = 0; i < PokerList.Count - 1; i++)
        {
            PokerInfo pokerInfo = PokerList[i];
            for (int j = i + 1; j < PokerList.Count; j++)
            {
                if (pokerInfo.m_num == PokerList[j].m_num && pokerInfo.m_pokerType == PokerList[j].m_pokerType)
                {
                    firestDoubleList.Add(PokerList[i]);
                    firestDoubleList.Add(PokerList[j]);
                }
            }
        }
        return firestDoubleList.OrderBy(a => a.m_weight).ToList();
    }

    /// <summary>
    /// 从对子中得到所有的拖拉机
    /// </summary>
    /// <param name="doubleList"></param>
    /// <param name="mLevelPokerNum"></param>
    /// <param name="masterPokerType"></param>
    /// <returns></returns>
    public static List<List<PokerInfo>> GetAllTljFromDouble(List<PokerInfo> doubleList, int mLevelPokerNum,
        int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetAllTljFromDouble"))
        {
            List<List<PokerInfo>> list_temp = (List<List<PokerInfo>>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetAllTljFromDouble", null, doubleList, mLevelPokerNum, masterPokerType);
            return list_temp;
        }

        List<List<PokerInfo>> list = new List<List<PokerInfo>>();

        List<PokerInfo> tempDoub = new List<PokerInfo>();
        foreach (var VARIABLE in doubleList)
        {
            tempDoub.Add(VARIABLE);
        }
        while (tempDoub.Count >= 4)
        {
            List<PokerInfo> tuoLaJi = GetTuoLaJi(tempDoub, mLevelPokerNum, masterPokerType);
            if (tuoLaJi.Count == 0) break;
            foreach (var VARIABLEs in tuoLaJi)
            {
                tempDoub.Remove(VARIABLEs);
            }
            list.Add(tuoLaJi);
        }
        return list;
    }

    /// <summary>
    /// 得到拖拉机
    /// </summary>
    /// <param name="playerOutPokerList"></param>
    /// <param name="mLevelPokerNum"></param>
    /// <param name="masterPokerType"></param>
    /// <returns></returns>
    public static List<PokerInfo> GetTuoLaJi(List<PokerInfo> playerOutPokerList, int mLevelPokerNum,
        int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetTuoLaJi"))
        {
            List<PokerInfo> list_temp = (List<PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetTuoLaJi", null, playerOutPokerList, mLevelPokerNum, masterPokerType);
            return list_temp;
        }

        List<PokerInfo> pokerInfos = new List<PokerInfo>();
        if (playerOutPokerList.Count % 2 == 0 && playerOutPokerList.Count >= 4)
        {
            //都是主牌或者都是同一花色的副牌
            if (IsAllMasterPoker(playerOutPokerList, mLevelPokerNum, masterPokerType) ||
                IsAllFuPoker(playerOutPokerList, mLevelPokerNum, masterPokerType))
            {
                //判断权重
                for (int j = 0; j < playerOutPokerList.Count - 2; j += 2)
                {
                    int temp = 1;
                    for (int i = j; i < playerOutPokerList.Count - 2; i += 2)
                    {
                        if (Math.Abs(playerOutPokerList[i + 2].m_weight - playerOutPokerList[j].m_weight) == temp)
                        {
                            pokerInfos.Add(playerOutPokerList[i + 2]);
                            pokerInfos.Add(playerOutPokerList[i + 3]);
                        }
                        temp++;

                        if (pokerInfos.Count > 0 && !pokerInfos.Contains(playerOutPokerList[j]))
                        {
                            pokerInfos.Add(playerOutPokerList[j]);
                            pokerInfos.Add(playerOutPokerList[j + 1]);
                        }
                    }

                    if (pokerInfos.Count > 0) break;
                }
            }
        }
        return pokerInfos.OrderBy(a => a.m_weight).ToList();
    }

    /// <summary>
    /// 得到手牌中的所有主牌
    /// </summary>
    /// <param name="pokerInfos"></param>
    /// <param name="mLevelPokerNum"></param>
    /// <param name="masterPokerType"></param>
    /// <returns></returns>
    public static List<PokerInfo> GetMasterPoker(List<PokerInfo> pokerInfos, int mLevelPokerNum, int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetMasterPoker"))
        {
            List<PokerInfo> list_temp = (List<PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetMasterPoker", null, pokerInfos, mLevelPokerNum, masterPokerType);
            return list_temp;
        }

        List<PokerInfo> pokers = new List<PokerInfo>();
        for (int i = 0; i < pokerInfos.Count; i++)
        {
            PokerInfo pokerInfo = pokerInfos[i];
            if (IsMasterPoker(pokerInfo, mLevelPokerNum, masterPokerType))
            {
                pokers.Add(pokerInfo);
            }
        }
        return pokers.OrderBy(a => a.m_weight).ToList();
    }

    /// <summary>
    /// 判断手牌中是否有主牌
    /// </summary>
    /// <param name="myRestPokerList"></param>
    /// <param name="mLevelPokerNum"></param>
    /// <param name="masterPokerType"></param>
    /// <returns></returns>
    public static bool IsContainMasterPoker(List<PokerInfo> myRestPokerList, int mLevelPokerNum, int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "IsContainMasterPoker"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "IsContainMasterPoker", null, myRestPokerList, mLevelPokerNum, masterPokerType);
            return b;
        }

        for (int i = 0; i < myRestPokerList.Count; i++)
        {
            PokerInfo myRestPoker = myRestPokerList[i];
            if (IsMasterPoker(myRestPoker, mLevelPokerNum, masterPokerType))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 判断手牌中是否有对子
    /// </summary>
    /// <param name="myRestPokerList"></param>
    /// <param name="mLevelPokerNum"></param>
    /// <param name="masterPokerType"></param>
    /// <returns></returns>
    public static bool IsContainDoublePoker(List<PokerInfo> myRestPokerList, out List<PokerInfo> doublePoker)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "IsContainDoublePoker"))
        {
            doublePoker = new List<PokerInfo>();
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "IsContainDoublePoker", null, myRestPokerList, doublePoker);
            return b;
        }

        doublePoker = new List<PokerInfo>();
        if (myRestPokerList.Count < 2) return false;
        for (int i = 0; i < myRestPokerList.Count - 1; i++)
        {
            if (myRestPokerList[i].m_num == myRestPokerList[i + 1].m_num
                && myRestPokerList[i].m_pokerType == myRestPokerList[i + 1].m_pokerType)
            {
                doublePoker.Add(myRestPokerList[i]);
                doublePoker.Add(myRestPokerList[i + 1]);
            }
        }
        doublePoker = doublePoker.OrderBy(a => a.m_weight).ToList();
        if (doublePoker.Count > 0) return true;
        return false;
    }


    /// <summary>
    /// 判断手牌中是否有该花色的副牌
    /// </summary>
    /// <param name="mPokerType"></param>
    /// <returns></returns>
    public static bool IsContainTypePoke(List<PokerInfo> myRestPokerList, int mLevelPokerNum,
        Consts.PokerType mPokerType,
        out List<PokerInfo> typeList)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "IsContainTypePoke"))
        {
            typeList = new List<PokerInfo>();
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "IsContainTypePoke", null, myRestPokerList, mLevelPokerNum, mPokerType, typeList);
            return b;
        }

        typeList = new List<PokerInfo>();
        for (int i = 0; i < myRestPokerList.Count; i++)
        {
            PokerInfo myRestPoker = myRestPokerList[i];
            if (myRestPoker.m_pokerType == mPokerType && myRestPoker.m_num != mLevelPokerNum)
            {
                typeList.Add(myRestPokerList[i]);
            }
        }

        if (typeList.Count > 0)
        {
            typeList = typeList.OrderBy(a => a.m_weight).ToList();
            return true;
        }

        return false;
    }

    /// <summary>
    /// 第一家出的拖拉机，判断其他玩家出牌是否符合规则
    /// </summary>
    /// <param name="myOutPokerList">玩家打算出的牌</param>
    /// <param name="myPokerByType">玩家手牌中该类型的所有牌</param>
    /// <param name="OutPokerType">玩家打算出的牌中的该类型的所有牌</param>
    /// <param name="mLevelPokerNum"></param>
    /// <param name="masterPokerType"></param>
    /// <returns></returns>
    public static bool IsSendByTuoLaJi(List<PokerInfo> myOutPokerList, List<PokerInfo> myPokerByType,
        List<PokerInfo> OutPokerType, int mLevelPokerNum, int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "IsSendByTuoLaJi"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "IsSendByTuoLaJi", null, myOutPokerList, myPokerByType, OutPokerType, mLevelPokerNum, masterPokerType);
            return b;
        }

        int count = myOutPokerList.Count;
        if (myPokerByType.Count <= count) return OutPokerType.Count == myPokerByType.Count;

        List<PokerInfo> myDoublePoker = PlayRuleUtil.GetDoublePoker(myPokerByType);
        List<PokerInfo> outDoublePoker = PlayRuleUtil.GetDoublePoker(myOutPokerList);
        if (myDoublePoker.Count <= count) return outDoublePoker.Count == myDoublePoker.Count;

        //很多对子+
        List<List<PokerInfo>> allTljFromDouble =
            PlayRuleUtil.GetAllTljFromDouble(myDoublePoker, mLevelPokerNum, masterPokerType);
        List<List<PokerInfo>> allTljFromoutDouble =
            PlayRuleUtil.GetAllTljFromDouble(outDoublePoker, mLevelPokerNum, masterPokerType);
        if (allTljFromDouble.Count > 0)
        {
            return allTljFromoutDouble.Count > 0;
        }
        else
        {
            return outDoublePoker.Count == count;
        }
    }

    /// <summary>
    /// 托管的时候自动获取跟牌时要出的牌
    /// </summary>
    /// <param name="firstPokerList"></param>
    /// <param name="myPokerList"></param>
    /// <param name="mLevelPokerNum"></param>
    /// <param name="masterPokerType"></param>
    /// <returns></returns>
    public static List<PokerInfo> GetPokerWhenTuoGuan(List<PokerInfo> firstPokerList, List<PokerInfo> myPokerList,
        int mLevelPokerNum, int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetPokerWhenTuoGuan"))
        {
            List<PokerInfo> list_temp = (List<PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetPokerWhenTuoGuan", null, firstPokerList, myPokerList, mLevelPokerNum, masterPokerType);
            return list_temp;
        }

        List<PokerInfo> tempAll = new List<PokerInfo>();
        foreach (var poker in myPokerList)
        {
            tempAll.Add(poker);
        }
        //设置权重
        SetPokerWeight(firstPokerList, mLevelPokerNum, (Consts.PokerType) masterPokerType);
        SetPokerWeight(myPokerList, mLevelPokerNum, (Consts.PokerType) masterPokerType);

        List<PokerInfo> resultList = new List<PokerInfo>();
        //第一个人出牌的牌数
        int count = firstPokerList.Count;

        switch (CheckOutPoker.checkOutPokerType(firstPokerList, mLevelPokerNum,
            masterPokerType))
        {
            //第一个人是单牌
            case CheckOutPoker.OutPokerType.OutPokerType_Single:
                //第一个人出的是主牌
                if (PlayRuleUtil.IsMasterPoker(firstPokerList[0], mLevelPokerNum, masterPokerType))
                {
                    List<PokerInfo> masterPoker =
                        PlayRuleUtil.GetMasterPoker(tempAll, mLevelPokerNum, masterPokerType);
                    GetPokerWhenSingle(tempAll, masterPoker, resultList);
                }
                //出的是副牌
                else
                {
                    //如果有该副牌花色必须出该花色
                    List<PokerInfo> typeInfo;
                    PlayRuleUtil.IsContainTypePoke(tempAll, mLevelPokerNum, firstPokerList[0].m_pokerType,
                        out typeInfo);
                    GetPokerWhenSingle(tempAll, typeInfo, resultList);
                }
                break;

            //第一个人是对子
            case CheckOutPoker.OutPokerType.OutPokerType_Double:
                //第一个人出的是主牌
                if (PlayRuleUtil.IsMasterPoker(firstPokerList[0], mLevelPokerNum, masterPokerType))
                {
                    List<PokerInfo> masterPoker =
                        PlayRuleUtil.GetMasterPoker(tempAll, mLevelPokerNum, masterPokerType);
                    GetPokerWhenDouble(tempAll, masterPoker, resultList);
                }
                //出的是副牌
                else
                {
                    //如果有该副牌花色必须出该花色
                    List<PokerInfo> typeInfo;
                    PlayRuleUtil.IsContainTypePoke(tempAll, mLevelPokerNum, firstPokerList[0].m_pokerType,
                        out typeInfo);
                    GetPokerWhenDouble(tempAll, typeInfo, resultList);
                }
                break;

            //第一个人是拖拉机
            case CheckOutPoker.OutPokerType.OutPokerType_TuoLaJi:
                //第一个人出的是主牌
                if (PlayRuleUtil.IsMasterPoker(firstPokerList[0], mLevelPokerNum, masterPokerType))
                {
                    List<PokerInfo> masterPoker =
                        PlayRuleUtil.GetMasterPoker(tempAll, mLevelPokerNum, masterPokerType);
                    GetPokerWhenTlj(tempAll, masterPoker, resultList, count, mLevelPokerNum, masterPokerType);
                }
                //出的是副牌
                else
                {
                    //如果有该副牌花色必须出该花色
                    List<PokerInfo> typeInfo;
                    PlayRuleUtil.IsContainTypePoke(tempAll, mLevelPokerNum, firstPokerList[0].m_pokerType,
                        out typeInfo);
                    GetPokerWhenTlj(tempAll, typeInfo, resultList, count, mLevelPokerNum, masterPokerType);
                }
                break;

            case CheckOutPoker.OutPokerType.OutPokerType_ShuaiPai:
                if (PlayRuleUtil.IsMasterPoker(firstPokerList[0], mLevelPokerNum, masterPokerType))
                {
                    //第一个人出的是主牌
                    List<PokerInfo> masterPoker = GetMasterPoker(tempAll, mLevelPokerNum, masterPokerType);
                    GetPokerWhenShuaiP(firstPokerList, masterPoker, tempAll, count, resultList);
                }
                //甩的是副牌
                else
                {
                    //如果有该副牌花色必须出该花色
                    List<PokerInfo> typeInfo;
                    PlayRuleUtil.IsContainTypePoke(tempAll, mLevelPokerNum, firstPokerList[0].m_pokerType,
                        out typeInfo);
                    GetPokerWhenShuaiP(firstPokerList, typeInfo, tempAll, count, resultList);
                }
                break;
        }

        if (firstPokerList.Count != resultList.Count)
        {
//            TLJ_PlayService.PlayService.log.Warn($"托管的出的牌的牌数不一致,托管出了{resultList.Count}张,第一个人出了{firstPokerList.Count}张");
        }
        return resultList;
    }

    public static void GetPokerWhenShuaiP(List<PokerInfo> firstPokerList, List<PokerInfo> masterPoker,
        List<PokerInfo> tempAll, int count, List<PokerInfo> tempList)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetPokerWhenShuaiP"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetPokerWhenShuaiP", null, firstPokerList, masterPoker, tempAll, count, tempList);
            return;
        }

        List<PokerInfo> firstDoublePoker = GetDoublePoker(firstPokerList);
        List<PokerInfo> firstSinglePoker = GetSinglePoker(firstPokerList, firstDoublePoker);
        //从手牌中去除该类型的牌
        foreach (var poker in masterPoker)
        {
            tempAll.Remove(poker);
        }

        if (masterPoker.Count <= count)
        {
            tempList.AddRange(masterPoker);
            //主牌不足会补副牌
            for (int j = 0; j < count - masterPoker.Count; j++)
            {
                tempList.Add(tempAll[tempAll.Count - 1 - j]);
            }
        }
        //主牌大于出牌的张数
        else
        {
            //得到主牌中的对子
            List<PokerInfo> myDoublePoker = GetDoublePoker(masterPoker);
            List<PokerInfo> mySinglePoker = GetSinglePoker(masterPoker, myDoublePoker);
            if (myDoublePoker.Count <= firstDoublePoker.Count)
            {
                tempList.AddRange(myDoublePoker);
                //对子不足会补牌
                for (int j = 0; j < count - myDoublePoker.Count; j++)
                {
                    tempList.Add(mySinglePoker[j]);
                }
            }
            //手牌中的对子多于出牌的对子
            else
            {
                for (int i = 0; i < firstDoublePoker.Count; i++)
                {
                    tempList.Add(myDoublePoker[i]);
                    //                    myDoublePoker.Remove(myDoublePoker[i]);
                }

                foreach (var Poker in tempList)
                {
                    myDoublePoker.Remove(Poker);
                }

                for (int i = 0; i < firstSinglePoker.Count; i++)
                {
                    if (mySinglePoker.Count == 0)
                    {
                        tempList.Add(myDoublePoker[i]);
                    }
                    else
                    {
                        tempList.Add(mySinglePoker[i]);
                    }
                }
            }
        }
    }

    public static void GetPokerWhenTlj(List<PokerInfo> myPokerList, List<PokerInfo> masterPoker,
        List<PokerInfo> tempList, int count, int mLevelPokerNum, int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetPokerWhenTlj"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetPokerWhenTlj", null, myPokerList, masterPoker, tempList, count, mLevelPokerNum,masterPokerType);
            return;
        }

        //从手牌中去除该类型的牌
        foreach (var poker in masterPoker)
        {
            myPokerList.Remove(poker);
        }

        if (masterPoker.Count <= count)
        {
            tempList.AddRange(masterPoker);
            //主牌不足会补副牌
            for (int j = 0; j < count - masterPoker.Count; j++)
            {
                tempList.Add(myPokerList[myPokerList.Count - 1 - j]);
            }
        }
        //主牌大于出牌的张数
        else
        {
            List<PokerInfo> doublePoker = GetDoublePoker(masterPoker);
            List<PokerInfo> singlePoker = GetSinglePoker(masterPoker, doublePoker);
            if (doublePoker.Count <= count)
            {
                tempList.AddRange(doublePoker);
                //对子不足会补牌
                for (int j = 0; j < count - doublePoker.Count; j++)
                {
                    tempList.Add(singlePoker[j]);
                }
            }
            //手牌中的对子多于出牌的对子
            else
            {
                List<List<PokerInfo>> allTljFromDouble =
                    GetAllTljFromDouble(doublePoker, mLevelPokerNum, masterPokerType);
                if (allTljFromDouble.Count == 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        tempList.Add(doublePoker[doublePoker.Count - 1 - i]);
                    }
                }
                else
                {
                    foreach (var tljList in allTljFromDouble)
                    {
                        if (tljList.Count >= count)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                tempList.Add(tljList[i]);
                            }
                            return;
                        }
                    }

                    //拖拉机没有第一家的长
                    tempList.AddRange(allTljFromDouble[0]);
                    foreach (var poker in allTljFromDouble[0])
                    {
                        doublePoker.Remove(poker);
                    }
                    for (int j = 0; j < count - allTljFromDouble[0].Count; j++)
                    {
                        tempList.Add(doublePoker[j]);
                    }
                }
            }
        }
    }

    public static void GetPokerWhenDouble(List<PokerInfo> myPokerList, List<PokerInfo> masterPoker,
        List<PokerInfo> tempList)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetPokerWhenDouble"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetPokerWhenDouble", null, myPokerList, masterPoker, tempList);
            return;
        }

        //从手牌中去除该类型的牌
        foreach (var poker in masterPoker)
        {
            myPokerList.Remove(poker);
        }

        if (masterPoker.Count == 0)
        {
            tempList.Add(myPokerList[myPokerList.Count - 1]);
            tempList.Add(myPokerList[myPokerList.Count - 2]);
        }
        else if (masterPoker.Count == 1)
        {
            tempList.Add(masterPoker[0]);
            tempList.Add(myPokerList[myPokerList.Count - 1]);
        }
        //主牌有两张以上
        else
        {
            List<PokerInfo> doublePoker = GetDoublePoker(masterPoker);
            //有对子
            if (doublePoker.Count > 0)
            {
                tempList.Add(doublePoker[0]);
                tempList.Add(doublePoker[1]);
            }
            //没有对子
            else
            {
                tempList.Add(masterPoker[0]);
                tempList.Add(masterPoker[1]);
            }
        }
    }

    public static void GetPokerWhenSingle(List<PokerInfo> myPokerList, List<PokerInfo> masterPoker,
        List<PokerInfo> tempList)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetPokerWhenSingle"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetPokerWhenSingle", null, myPokerList, masterPoker, tempList);
            return;
        }

        //从手牌中去除该类型的牌
        foreach (var poker in masterPoker)
        {
            myPokerList.Remove(poker);
        }

        if (masterPoker.Count == 0)
        {
            int random = new Random().Next(0, myPokerList.Count - 1);
            tempList.Add(myPokerList[random]);
        }
        else
        {
            int random = new Random().Next(0, masterPoker.Count - 1);
            tempList.Add(masterPoker[random]);
        }
    }

    /// <summary>
    /// 得到主牌中的级牌和大小王
    /// </summary>
    /// <param name="masterPoker"></param>
    /// <param name="mLevelPokerNum"></param>
    /// <param name="masterPokerType"></param>
    /// <returns></returns>
    public static List<PokerInfo> GetJiPaiAndWang(List<PokerInfo> masterPoker, int mLevelPokerNum)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetJiPaiAndWang"))
        {
            List<PokerInfo> list_temp = (List<PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetJiPaiAndWang", null, masterPoker, mLevelPokerNum);
            return list_temp;
        }

        List<PokerInfo> pokerInfos = new List<PokerInfo>();
        for (int i = 0; i < masterPoker.Count; i++)
        {
            PokerInfo pokerInfo = masterPoker[i];
            if (pokerInfo.m_num == mLevelPokerNum || pokerInfo.m_pokerType == Consts.PokerType.PokerType_Wang)
            {
                pokerInfos.Add(pokerInfo);
            }
        }
        return pokerInfos;
    }

    /// <summary>
    /// 只在客户端调用,得到能够亮主的牌
    /// </summary>
    /// <param name="handerPoker"></param>
    /// <param name="liangZhuPoker"></param>
    /// <param name="mLevelPokerNum"></param>
    /// <param name="masterPokerType"></param>
    /// <returns></returns>
    public static List<PokerInfo> GetLiangzhuPoker(List<PokerInfo> handerPoker, List<PokerInfo> liangZhuPoker,
        int mLevelPokerNum,
        int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetLiangzhuPoker"))
        {
            List<PokerInfo> list_temp = (List<PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetLiangzhuPoker", null, handerPoker, liangZhuPoker, mLevelPokerNum, masterPokerType);
            return list_temp;
        }

        List<PokerInfo> pokerInfos = new List<PokerInfo>();
        List<PokerInfo> jiPaiAndWang = PlayRuleUtil.GetJiPaiAndWang(handerPoker, mLevelPokerNum);
        if (jiPaiAndWang.Count == 0) return pokerInfos;

        //第一次亮主
        if (liangZhuPoker == null || liangZhuPoker.Count == 0)
        {
            for (int i = 0; i < handerPoker.Count; i++)
            {
                if (handerPoker[i].m_num == mLevelPokerNum )
                {
                    pokerInfos.Add(handerPoker[i]);
                }
            }
            List<PokerInfo> doublePoker = PlayRuleUtil.GetDoublePoker(jiPaiAndWang);
            //大小王
            foreach (var poker in doublePoker)
            {
                if (poker.m_pokerType == Consts.PokerType.PokerType_Wang)
                {
                    pokerInfos.Add(poker);
                }
            }
            return pokerInfos;
        }else if (liangZhuPoker.Count == 1)
        {
            List<PokerInfo> doublePoker = PlayRuleUtil.GetDoublePoker(jiPaiAndWang);
            return doublePoker;
        }
        else if (liangZhuPoker.Count == 2)
        {
            Consts.PokerType mPokerType = liangZhuPoker[0].m_pokerType;
            List<PokerInfo> doublePoker = PlayRuleUtil.GetDoublePoker(jiPaiAndWang);
            for (int i = 0; i < doublePoker.Count - 1; i += 2)
            {
                if (doublePoker[i].m_pokerType > mPokerType)
                {
                    pokerInfos.Add(doublePoker[i]);
                    pokerInfos.Add(doublePoker[i + 1]);
                }

                if (liangZhuPoker[0].m_num == 15 && doublePoker[i].m_num == 16)
                {
                    pokerInfos.Add(doublePoker[i]);
                    pokerInfos.Add(doublePoker[i + 1]);
//                    ToastScript.createToast("dawang");
                }
            }
            return pokerInfos;
        }
        else
        {
            return pokerInfos;
        }
    }

    public static List<PokerInfo> GetPokerWhenFirst(List<PokerInfo> handerPoker, int mLevelPokerNum,
        int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetPokerWhenFirst"))
        {
            List<PokerInfo> list_temp = (List<PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetPokerWhenFirst", null, handerPoker, mLevelPokerNum, masterPokerType);
            return list_temp;
        }

        SetPokerWeight(handerPoker, mLevelPokerNum, (Consts.PokerType) masterPokerType);

        List<PokerInfo> result = new List<PokerInfo>();
        List<PokerInfo> tempAll = new List<PokerInfo>();
        foreach (var poker in handerPoker)
        {
            tempAll.Add(poker);
        }

        List<PokerInfo> masterPoker = GetMasterPoker(tempAll, mLevelPokerNum, masterPokerType);
        foreach (var poker in masterPoker)
        {
            tempAll.Remove(poker);
        }

        //得到副牌中的对子
        List<PokerInfo> doublePoker = GetDoublePoker(tempAll);
        foreach (var poker in doublePoker)
        {
            tempAll.Remove(poker);
        }

        foreach (var poker in tempAll)
        {
            if (poker.m_num == 14)
            {
                result.Add(poker);
                return result;
            }
        }
        if (doublePoker.Count > 0)
        {
            result.Add(doublePoker[doublePoker.Count - 1]);
            result.Add(doublePoker[doublePoker.Count - 2]);
        }
        else
        {
            if (tempAll.Count > 0)
            {
                result.Add(tempAll[new Random().Next(0, tempAll.Count - 1)]);
            }
            else
            {
                List<PokerInfo> masterDoublePoker = GetDoublePoker(masterPoker);
                if (masterDoublePoker.Count > 0)
                {
                    result.Add(masterDoublePoker[0]);
                    result.Add(masterDoublePoker[1]);
                }
                else
                {
                    result.Add(masterPoker[0]);
                }
            }
        }

        return result;
    }

    public static int GetDiPaiBeiLv(List<PokerInfo> pokers, int mLevelPokerNum, int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayRuleUtil", "GetDiPaiBeiLv"))
        {
            int i = (int)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayRuleUtil", "GetDiPaiBeiLv", null, pokers, mLevelPokerNum, masterPokerType);
            return i;
        }

        SetPokerWeight(pokers, mLevelPokerNum, (Consts.PokerType) masterPokerType);
        var tuoLaJi = GetTuoLaJi(pokers, mLevelPokerNum, masterPokerType);
        if (tuoLaJi.Count > 0) return 8;
        List<PokerInfo> doublePoker;
        if (IsContainDoublePoker(pokers, out doublePoker)) return 4;
        return 2;
    }
}