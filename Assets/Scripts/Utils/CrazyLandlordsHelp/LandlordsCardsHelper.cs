using System;
using System.Collections.Generic;
using System.Linq;
using TLJCommon;

namespace CrazyLandlords.Helper
{
    public static class LandlordsCardsHelper
    {
        /// <summary>
        /// 获取牌组权重
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static int GetWeight(PokerInfo[] cards, CardsType rule)
        {
            int totalWeight_DDZ = 0;
            if (rule == CardsType.JokerBoom)
            {
                totalWeight_DDZ = int.MaxValue;
            }
            else if (rule == CardsType.Boom)
            {
                totalWeight_DDZ = (int)cards[0].m_weight_DDZ * (int)cards[1].m_weight_DDZ * (int)cards[2].m_weight_DDZ * (int)cards[3].m_weight_DDZ + (int.MaxValue / 2);
            }
            else if (rule == CardsType.BoomAndTwo || rule == CardsType.BoomAndOne)
            {
                for (int i = 0; i < cards.Length - 3; i++)
                {
                    if (cards[i].m_weight_DDZ == cards[i + 1].m_weight_DDZ &&
                        cards[i].m_weight_DDZ == cards[i + 2].m_weight_DDZ &&
                        cards[i].m_weight_DDZ == cards[i + 3].m_weight_DDZ)
                    {
                        totalWeight_DDZ += (int)cards[i].m_weight_DDZ;
                        totalWeight_DDZ *= 4;
                        break;
                    }
                }
            }
            else if (rule == CardsType.ThreeAndOne || rule == CardsType.ThreeAndTwo)
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    if (i < cards.Length - 2)
                    {
                        if (cards[i].m_weight_DDZ == cards[i + 1].m_weight_DDZ &&
                            cards[i].m_weight_DDZ == cards[i + 2].m_weight_DDZ)
                        {
                            totalWeight_DDZ += (int)cards[i].m_weight_DDZ;
                            totalWeight_DDZ *= 3;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    totalWeight_DDZ += (int)cards[i].m_weight_DDZ;
                }
            }

            return totalWeight_DDZ;
        }

        ///// <summary>
        ///// 给所有玩家设权重并排序
        ///// </summary>
        ///// <param name="room"></param>
        //public static void SetWeight(DDZ_RoomData room)
        //{
        //    foreach (var PlayerData in room.getPlayerDataList())
        //    {
        //        SetWeight(PlayerData.getPokerList());
        //        SortCards(PlayerData.getPokerList());
        //    }
        //}

        /// <summary>
        /// 权重复制
        /// </summary>
        /// <param name="cards"></param>
        public static void SetWeight(List<PokerInfo> cards)
        {
            foreach (var card in cards)
            {
                if (card.m_num == 2)
                {
                    card.m_weight_DDZ = Weight_DDZ.Two;
                }else if (card.m_num == 14)
                {
                    card.m_weight_DDZ = Weight_DDZ.One;
                }else if (card.m_num == 15)
                {
                    card.m_weight_DDZ = Weight_DDZ.SJoker;
                }
                else if (card.m_num == 16)
                {
                    card.m_weight_DDZ = Weight_DDZ.LJoker;
                }
                else
                {
                    card.m_weight_DDZ = (Weight_DDZ)card.m_num;
                }
            }
            SortCards(cards);
        }

        /// <summary>
        /// 根据Weight_DDZ在卡组中找到所有重复的卡牌(两个及两个以上)
        /// </summary>
        /// <param name="cards"></param>
        /// <returns>Weight_DDZ:类型，int:重复的个数</returns>
        public static Dictionary<Weight_DDZ, int> FindSameCards(PokerInfo[] cards)
        {
            return cards.GroupBy(x => x.m_weight_DDZ).Where(x => x.Count() > 1).ToDictionary(x => x.Key, y => y.Count());
        }

        /// <summary>
        /// 根据Weight_DDZ在卡组中找到所有重复的卡牌(两个以上)
        /// </summary>
        /// <param name="cards"></param>
        /// <returns>Weight_DDZ:类型，int:重复的个数</returns>
        private static Dictionary<Weight_DDZ, int> FindTripleCards(PokerInfo[] cards)
        {
            return cards.GroupBy(x => x.m_weight_DDZ).Where(x => x.Count() > 2).ToDictionary(x => x.Key, y => y.Count());
        }

        /// <summary>f
        /// 卡组排序
        /// </summary>
        /// <param name="cards"></param>
        public static void SortCards(List<PokerInfo> cards)
        {
            cards.Sort(
                (PokerInfo a, PokerInfo b) =>
                {
                    //先按照权重降序，再按花色升序
                    return -a.m_weight_DDZ.CompareTo(b.m_weight_DDZ) * 2 + a.m_pokerType.CompareTo(b.m_pokerType);
                }
            );
        }

        /// <summary>
        /// 是否是单
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsSingle(PokerInfo[] cards)
        {
            if (cards.Length == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否是对子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsDouble(PokerInfo[] cards)
        {
            if (cards.Length == 2)
            {
                if (cards[0].m_weight_DDZ == cards[1].m_weight_DDZ)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 是否是4带2个对
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsBoomAndTwo(PokerInfo[] cards)
        {
            //4444 + 55 + 77
            if (cards.Length == 8)
            {
                Dictionary<Weight_DDZ, int> findSameCards = FindSameCards(cards);
                int totalNum = 0;
                foreach (var item in findSameCards.Values)
                {
                    totalNum += item;
                }
                if (findSameCards.Values.Contains(4) && findSameCards.Values.Contains(2) && totalNum == 8)
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// 是否是4带2个单
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsBoomAndOne(PokerInfo[] cards)
        {
            //5555 + 3 + 8
            if (cards.Length == 6)
            {
                Dictionary<Weight_DDZ, int> findSameCards = FindSameCards(cards);
                foreach (var item in findSameCards.Values)
                {
                    //有4个重复的元素
                    if (item == 4)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 是否是顺子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsStraight(PokerInfo[] cards)
        {
            if (cards.Length < 5 || cards.Length > 12)
                return false;
            for (int i = 0; i < cards.Length - 1; i++)
            {
                Weight_DDZ w = cards[i].m_weight_DDZ;
                if (w - cards[i + 1].m_weight_DDZ != 1)
                    return false;

                //不能超过A
                if (w > Weight_DDZ.One || cards[i + 1].m_weight_DDZ > Weight_DDZ.One)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 是否是双顺子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsDoubleStraight(PokerInfo[] cards)
        {
            if (cards.Length < 6 || cards.Length % 2 != 0)
                return false;

            for (int i = 0; i < cards.Length; i += 2)
            {
                if (cards[i + 1].m_weight_DDZ != cards[i].m_weight_DDZ)
                    return false;

                if (i < cards.Length - 2)
                {
                    if (cards[i].m_weight_DDZ - cards[i + 2].m_weight_DDZ != 1)
                        return false;

                    //不能超过A
                    if (cards[i].m_weight_DDZ > Weight_DDZ.One || cards[i + 2].m_weight_DDZ > Weight_DDZ.One)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 飞机不带
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsTripleStraight(PokerInfo[] cards)
        {
            if (cards.Length < 6 || cards.Length % 3 != 0)
                return false;

            for (int i = 0; i < cards.Length; i += 3)
            {
                if (cards[i + 1].m_weight_DDZ != cards[i].m_weight_DDZ)
                    return false;
                if (cards[i + 2].m_weight_DDZ != cards[i].m_weight_DDZ)
                    return false;
                if (cards[i + 1].m_weight_DDZ != cards[i + 2].m_weight_DDZ)
                    return false;

                if (i < cards.Length - 3)
                {
                    if (cards[i].m_weight_DDZ - cards[i + 3].m_weight_DDZ != 1)
                        return false;

                    //不能超过A
                    if (cards[i].m_weight_DDZ > Weight_DDZ.One || cards[i + 3].m_weight_DDZ > Weight_DDZ.One)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 三顺 带单
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsTripleStraightAndOne(PokerInfo[] cards)
        {
            if (cards.Length % 4 != 0 || cards.Length < 8)
                return false;
            //找出3张以上的牌
            Dictionary<Weight_DDZ, int> findSameCards = FindTripleCards(cards);
            List<Weight_DDZ> weights = new List<Weight_DDZ>(findSameCards.Keys);
            //两个3张以上
            if (weights.Count < 2)
                return false;
            //是否是连续
            for (int i = 0; i < weights.Count - 1; i++)
            {
                if (weights[i] - weights[i + 1] != 1)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 三顺 带双
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsTripleStraightAndTwo(PokerInfo[] cards)
        {
            if (cards.Length % 5 != 0 || cards.Length < 10)
                return false;
            Dictionary<Weight_DDZ, int> doubleCards = cards.GroupBy(x => x.m_weight_DDZ).Where(x => x.Count() == 2).ToDictionary(x => x.Key, y => y.Count());
            Dictionary<Weight_DDZ, int> tripleCards = cards.GroupBy(x => x.m_weight_DDZ).Where(x => x.Count() == 3).ToDictionary(x => x.Key, y => y.Count());
            List<Weight_DDZ> doubleweights = new List<Weight_DDZ>(doubleCards.Keys);
            List<Weight_DDZ> tripleweights = new List<Weight_DDZ>(tripleCards.Keys);

            if (tripleweights.Count < 2 || tripleweights.Count != doubleweights.Count)
                return false;
            //是否是连续
            for (int i = 0; i < tripleweights.Count - 1; i++)
            {
                if (tripleweights[i] - tripleweights[i + 1] != 1)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 三不带
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsOnlyThree(PokerInfo[] cards)
        {
            if (cards.Length != 3)
                return false;
            if (cards[0].m_weight_DDZ != cards[1].m_weight_DDZ)
                return false;
            if (cards[1].m_weight_DDZ != cards[2].m_weight_DDZ)
                return false;
            if (cards[0].m_weight_DDZ != cards[2].m_weight_DDZ)
                return false;
            return true;
        }

        /// <summary>
        /// 三带一
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThreeAndOne(PokerInfo[] cards)
        {
            if (cards.Length != 4)
                return false;
            if (cards[0].m_weight_DDZ == cards[1].m_weight_DDZ &&
                cards[1].m_weight_DDZ == cards[2].m_weight_DDZ)
                return true;
            else if (cards[1].m_weight_DDZ == cards[2].m_weight_DDZ &&
                cards[2].m_weight_DDZ == cards[3].m_weight_DDZ)
                return true;
            return false;
        }

        /// <summary>
        /// 三代二
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThreeAndTwo(PokerInfo[] cards)
        {
            if (cards.Length != 5)
                return false;

            if (cards[0].m_weight_DDZ == cards[1].m_weight_DDZ &&
                cards[1].m_weight_DDZ == cards[2].m_weight_DDZ)
            {
                if (cards[3].m_weight_DDZ == cards[4].m_weight_DDZ)
                    return true;
            }

            else if (cards[2].m_weight_DDZ == cards[3].m_weight_DDZ && 
                     cards[3].m_weight_DDZ == cards[4].m_weight_DDZ)
            {
                if (cards[0].m_weight_DDZ == cards[1].m_weight_DDZ)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 炸弹
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsBoom(PokerInfo[] cards)
        {
            if (cards.Length != 4)
                return false;
            if (cards[0].m_weight_DDZ != cards[1].m_weight_DDZ)
                return false;
            if (cards[1].m_weight_DDZ != cards[2].m_weight_DDZ)
                return false;
            if (cards[2].m_weight_DDZ != cards[3].m_weight_DDZ)
                return false;
            return true;
        }

        /// <summary>
        /// 王炸
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsJokerBoom(PokerInfo[] cards)
        {
            if (cards.Length != 2)
                return false;
            if (cards[0].m_weight_DDZ == Weight_DDZ.SJoker)
            {
                if (cards[1].m_weight_DDZ == Weight_DDZ.LJoker)
                    return true;
                return false;
            }
            else if (cards[0].m_weight_DDZ == Weight_DDZ.LJoker)
            {
                if (cards[1].m_weight_DDZ == Weight_DDZ.SJoker)
                    return true;
                return false;
            }

            return false;
        }

        /// <summary>
        /// 获取出牌的牌型
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool GetCardsType(PokerInfo[] cards, out CardsType type)
        {
            type = CardsType.None;
            bool isRule = false;
            switch (cards.Length)
            {
                case 1:
                    isRule = true;
                    type = CardsType.Single;
                    break;
                case 2:
                    if (IsDouble(cards))
                    {
                        isRule = true;
                        type = CardsType.Double;
                    }
                    else if (IsJokerBoom(cards))
                    {
                        isRule = true;
                        type = CardsType.JokerBoom;
                    }
                    break;
                case 3:
                    if (IsOnlyThree(cards))
                    {
                        isRule = true;
                        type = CardsType.OnlyThree;
                    }
                    break;
                case 4:
                    if (IsBoom(cards))
                    {
                        isRule = true;
                        type = CardsType.Boom;
                    }
                    else if (IsThreeAndOne(cards))
                    {
                        isRule = true;
                        type = CardsType.ThreeAndOne;
                    }
                    break;
                case 5:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.Straight;
                    }
                    else if (IsThreeAndTwo(cards))
                    {
                        isRule = true;
                        type = CardsType.ThreeAndTwo;
                    }
                    break;
                case 6:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.Straight;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.TripleStraight;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.DoubleStraight;
                    }
                    else if (IsBoomAndOne(cards))
                    {
                        isRule = true;
                        type = CardsType.BoomAndOne;
                    }
                    break;
                case 7:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.Straight;
                    }
                    break;
                case 8:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.Straight;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.DoubleStraight;
                    }
                    else if (IsBoomAndTwo(cards))
                    {
                        isRule = true;
                        type = CardsType.BoomAndTwo;
                    }
                    else if (IsTripleStraightAndOne(cards))
                    {
                        isRule = true;
                        type = CardsType.TripleStraightAndOne;
                    }
                    break;
                case 9:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.Straight;
                    }
                    else if (IsOnlyThree(cards))
                    {
                        isRule = true;
                        type = CardsType.OnlyThree;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.TripleStraight;
                    }
                    break;
                case 10:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.Straight;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.DoubleStraight;
                    }
                    else if (IsTripleStraightAndTwo(cards))
                    {
                        isRule = true;
                        type = CardsType.TripleStraightAndTwo;
                    }
                    break;

                case 11:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.Straight;
                    }
                    break;
                case 12:
                    if (IsStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.Straight;
                    }
                    else if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.DoubleStraight;
                    }
                    else if (IsOnlyThree(cards))
                    {
                        isRule = true;
                        type = CardsType.OnlyThree;
                    }
                    else if (IsTripleStraightAndOne(cards))
                    {
                        isRule = true;
                        type = CardsType.TripleStraightAndOne;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.TripleStraight;
                    }
                    break;
                case 13:
                    break;
                case 14:
                    if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.DoubleStraight;
                    }
                    break;
                case 15:
                    if (IsOnlyThree(cards))
                    {
                        isRule = true;
                        type = CardsType.OnlyThree;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.TripleStraight;
                    }
                    else if (IsTripleStraightAndTwo(cards))
                    {
                        isRule = true;
                        type = CardsType.TripleStraightAndTwo;
                    }
                    break;
                case 16:
                    if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.DoubleStraight;
                    }
                    else if (IsTripleStraightAndOne(cards))
                    {
                        isRule = true;
                        type = CardsType.TripleStraightAndOne;
                    }
                    break;
                case 17:
                    break;
                case 18:
                    if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.DoubleStraight;
                    }
                    else if (IsTripleStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.TripleStraight;
                    }
                    else if (IsOnlyThree(cards))
                    {
                        isRule = true;
                        type = CardsType.OnlyThree;
                    }
                    break;
                case 19:
                    break;

                case 20:
                    if (IsDoubleStraight(cards))
                    {
                        isRule = true;
                        type = CardsType.DoubleStraight;
                    }
                    else if (IsTripleStraightAndOne(cards))
                    {
                        isRule = true;
                        type = CardsType.TripleStraightAndOne;
                    }
                    else if (IsTripleStraightAndTwo(cards))
                    {
                        isRule = true;
                        type = CardsType.TripleStraightAndTwo;
                    }
                    break;
                default:
                    break;
            }
            return isRule;
        }

        public static List<PokerInfo> GetActiveCard(List<PokerInfo> cards)
        {
            List<PokerInfo> temp = new List<PokerInfo>();

            List<PokerInfo> jokerBoomCards = new List<PokerInfo>();
            List<PokerInfo> boomCards = new List<PokerInfo>();

            List<PokerInfo> tripleCards = new List<PokerInfo>();
            List<List<PokerInfo>> tripleStraghtCards;

//            List<PokerInfo> tripleStraghtCards = new List<PokerInfo>();

            //拷贝一份
            List<PokerInfo> copyCards = new List<PokerInfo>(cards);

            //检索王炸
            if (copyCards.Count >= 2)
            {
                PokerInfo[] groupCards = new PokerInfo[2];
                groupCards[0] = copyCards[0];
                groupCards[1] = copyCards[1];

                if (IsJokerBoom(groupCards))
                {
                    jokerBoomCards.AddRange(groupCards);
                }
            }

            //检索炸弹
            for (int i = copyCards.Count - 1; i >= 3; i--)
            {
                PokerInfo[] groupCards = new PokerInfo[4];
                groupCards[0] = copyCards[i - 3];
                groupCards[1] = copyCards[i - 2];
                groupCards[2] = copyCards[i - 1];
                groupCards[3] = copyCards[i];

                if (IsBoom(groupCards))
                {
                    boomCards.AddRange(groupCards);
                }
            }

            //检索3张
            IGrouping<Weight_DDZ, PokerInfo>[] tempTripleCards = copyCards.GroupBy(x => x.m_weight_DDZ).Where(x => x.Count() == 3).ToArray();
            foreach (var item in tempTripleCards)
            {
                tripleCards.AddRange(item);
            }
            //在3张中检索3顺
            tripleStraghtCards = GetAllTripleStraght(tripleCards);
            foreach (var tripleStraghtCard in tripleStraghtCards)
            {
                foreach (var card in tripleStraghtCard)
                {
                    tripleCards.Remove(card);
                }
            }
        
            //排除3顺，炸弹的情况下，用剩下来的牌组成单顺
            copyCards.RemoveList(jokerBoomCards);
            copyCards.RemoveList(boomCards);
            copyCards.RemoveList(tripleStraghtCards);

            List<List<PokerInfo>> allFiveStraght = FindAllFiveStraght(copyCards);
            copyCards.RemoveList(allFiveStraght);

            foreach (var fiveStraght in allFiveStraght)
            {
                foreach (var card in copyCards)
                {
                    if (card.m_weight_DDZ - fiveStraght[fiveStraght.Count - 1].m_weight_DDZ == 1)
                    {

                    }
                }
            }


            return null;
        }

        /// <summary>
        /// 从3张中获得3顺
        /// </summary>
        /// <param name="tripleCards"></param>
        /// <returns></returns>
        private static List<PokerInfo> GetTripleStraght(List<PokerInfo> tripleCards)
        {
            List<PokerInfo> tempCards = new List<PokerInfo>();

            for (int i = 0; i < tripleCards.Count - 3; i += 3)
            {
                int k = 1;
                for (int j = i; j < tripleCards.Count - 3; j += 3)
                {
                    if (tripleCards[i].m_weight_DDZ - tripleCards[j + 3].m_weight_DDZ == k)
                    {
                        tempCards.Add(tripleCards[j + 3]);
                        tempCards.Add(tripleCards[j + 4]);
                        tempCards.Add(tripleCards[j + 5]);
                    }

                    k++;
                }
                if (tempCards.Count > 0)
                {
                    tempCards.Add(tripleCards[i]);
                    tempCards.Add(tripleCards[i + 1]);
                    tempCards.Add(tripleCards[i + 2]);
                    break;
                }
            }

            SortCards(tempCards);
            return tempCards;
        }

        public static List<List<PokerInfo>> GetAllTripleStraght(List<PokerInfo> tripleCards)
        {
            List<List<PokerInfo>> result = new List<List<PokerInfo>>();
            List<PokerInfo> copyCards = new List<PokerInfo>(tripleCards);
            while (copyCards.Count >= 6)
            {
                List<PokerInfo> landlordsCards = GetTripleStraght(copyCards);
                if (landlordsCards.Count == 0) break;
                foreach (var VARIABLEs in landlordsCards)
                {
                    copyCards.Remove(VARIABLEs);
                }
                result.Add(landlordsCards);
            }
            return result;
        }

        /// <summary>
        /// 提示出牌
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="lastCards"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<PokerInfo[]> GetPrompt(List<PokerInfo> cards, List<PokerInfo> lastCards, CardsType type)
        {
            List<PokerInfo[]> result = new List<PokerInfo[]>();
            List<PokerInfo> copyCards = new List<PokerInfo>(cards);

            IGrouping<Weight_DDZ, PokerInfo>[] boomCards = copyCards.GroupBy(x => x.m_weight_DDZ).Where(x => x.Count() == 4).ToArray();
            IGrouping<Weight_DDZ, PokerInfo>[] tripleCards = copyCards.GroupBy(x => x.m_weight_DDZ).Where(x => x.Count() == 3).ToArray();
            IGrouping<Weight_DDZ, PokerInfo>[] doubleCards = copyCards.GroupBy(x => x.m_weight_DDZ).Where(x => x.Count() == 2).ToArray();
            IGrouping<Weight_DDZ, PokerInfo>[] singleCards = copyCards.GroupBy(x => x.m_weight_DDZ).Where(x => x.Count() == 1).ToArray();

            //TODO 缺少牌桌上的牌
            PokerInfo[] deskCards = lastCards.ToArray();
            IGrouping<Weight_DDZ, PokerInfo>[] lastTripleCards = deskCards.GroupBy(x => x.m_weight_DDZ).Where(x => x.Count() >= 3).ToArray();
            int weight = GetWeight(deskCards, type);

            if (type == CardsType.JokerBoom)
            {
                return result;
            }

            //检索王炸
            if (copyCards.Count >= 2)
            {
                PokerInfo[] groupCards = new PokerInfo[2];
                groupCards[0] = copyCards[0];
                groupCards[1] = copyCards[1];

                if (IsJokerBoom(groupCards))
                {
                    result.Add(groupCards);
                }
            }
            //检索炸弹
            for (int i = copyCards.Count - 1; i >= 3; i--)
            {
                PokerInfo[] groupCards = new PokerInfo[4];
                groupCards[0] = copyCards[i - 3];
                groupCards[1] = copyCards[i - 2];
                groupCards[2] = copyCards[i - 1];
                groupCards[3] = copyCards[i];

                if (IsBoom(groupCards) && GetWeight(groupCards, CardsType.Boom) > weight)
                {
                    result.Add(groupCards);
                }
            }

            //删除炸弹
//            foreach (var resultCards in result)
//            {
//                foreach (var resultCard in resultCards)
//                {
//                    copyCards.Remove(resultCard);
//                }
//            }

            switch (type)
            {
                //TODO 
                case CardsType.BoomAndOne:

                    for (int i = copyCards.Count - 1; i >= 3; i--)
                    {
                        PokerInfo[] groupCards = new PokerInfo[6];
                        groupCards[0] = copyCards[i - 3];
                        groupCards[1] = copyCards[i - 2];
                        groupCards[2] = copyCards[i - 1];
                        groupCards[3] = copyCards[i];

                        if (GetWeight(groupCards, CardsType.Boom) > weight)
                        {
                            copyCards.RemoveRange(i - 3, 3);
                            int i1 = RandomHelper.RandomNumber(0, copyCards.Count);
                            groupCards[4] = copyCards[i1];
                            copyCards.RemoveAt(i1);

                            int i2 = RandomHelper.RandomNumber(0, copyCards.Count);
                            groupCards[5] = copyCards[i2];
                            copyCards.RemoveAt(i2);
                            result.Add(groupCards);
                        }
                    }

                    break;
                //TODO 
                case CardsType.BoomAndTwo:
                    //没有炸弹
                    if (boomCards.Length == 0 || doubleCards.Length < 2) break;
                    foreach (var item in boomCards)
                    {
                        //4带2没有对方大
                        if (((int) item.Key * 4) <= weight)
                            continue;

                        List<PokerInfo> temp = new List<PokerInfo>(item);
                        temp.AddRange(doubleCards[0]);
                        temp.AddRange(doubleCards[1]);
                        result.Add(temp.ToArray());
                    }
                    break;
                //TODO 
                case CardsType.TripleStraightAndOne:
                    //找出3张以上的牌
                 
                    if (tripleCards.Length < lastTripleCards.Length)
                        break;
                    
                    for (int i = 0; i < tripleCards.Length - 1; i++)
                    {
                        if (tripleCards[i].Key - tripleCards[i + 1].Key == 1)
                        {
                            List<PokerInfo> temp = new List<PokerInfo>();

                            temp.AddRange(tripleCards[i]);
                            temp.AddRange(tripleCards[i + 1]);
                            if (singleCards.Length >= 2)
                            {
                                temp.AddRange(singleCards[0]);
                                temp.AddRange(singleCards[1]);
                            }
                            else
                            {
                                if (doubleCards.Length > 0)
                                {
                                    temp.AddRange(doubleCards[0]);
                                }
                            }
                            result.Add(temp.ToArray());
                        }
                    }
                    break;
                //TODO 
                case CardsType.TripleStraightAndTwo:
                    if (tripleCards.Length < lastTripleCards.Length)
                        break;

                    for (int i = 0; i < tripleCards.Length - 1; i++)
                    {
                        if (tripleCards[i].Key - tripleCards[i + 1].Key == 1)
                        {
                            List<PokerInfo> temp = new List<PokerInfo>();

                            temp.AddRange(tripleCards[i]);
                            temp.AddRange(tripleCards[i + 1]);
                            if (doubleCards.Length >= 2)
                            {
                                temp.AddRange(doubleCards[0]);
                                temp.AddRange(doubleCards[1]);
                            }
                            else if (doubleCards.Length == 1)
                            {
                                if (tripleCards.Length > 0)
                                {
                                    temp.AddRange(doubleCards[0]);
                                    List<PokerInfo> tempCards = new List<PokerInfo>();
                                    foreach (var items in tripleCards)
                                    {
                                        if (items.Key == tripleCards[i].Key || items.Key == tripleCards[i + 1].Key)
                                            continue;
                                        foreach (var item in items)
                                        {
                                            tempCards.Add(item);
                                        }
                                    }

                                    for (int j = 0; j < 2; j++)
                                    {
                                        temp.Add(tempCards[j]);
                                    }
                                }
                            }
                            if(temp.Count == lastCards.Count)
                                result.Add(temp.ToArray());
                        }
                    }

                    break;
                case CardsType.OnlyThree:
                    for (int i = copyCards.Count - 1; i >= 2; i--)
                    {
                        if (copyCards[i].m_weight_DDZ <= deskCards[deskCards.Length - 1].m_weight_DDZ)
                        {
                            continue;
                        }

                        PokerInfo[] groupCards = new PokerInfo[3];
                        groupCards[0] = copyCards[i - 2];
                        groupCards[1] = copyCards[i - 1];
                        groupCards[2] = copyCards[i];

                        if (IsOnlyThree(groupCards) && GetWeight(groupCards, type) > weight)
                        {
                            result.Add(groupCards);
                        }
                    }
                    break;
                case CardsType.ThreeAndOne:
                    if (copyCards.Count >= 4)
                    {
                        for (int i = copyCards.Count - 1; i >= 2; i--)
                        {
                            if (copyCards[i].m_weight_DDZ <= deskCards[deskCards.Length - 1].m_weight_DDZ)
                            {
                                continue;
                            }

                            List<PokerInfo> other = new List<PokerInfo>(copyCards);
                            other.RemoveRange(i - 2, 3);

                            PokerInfo[] groupCards = new PokerInfo[4];
                            groupCards[0] = copyCards[i - 2];
                            groupCards[1] = copyCards[i - 1];
                            groupCards[2] = copyCards[i];
                            groupCards[3] = other[RandomHelper.RandomNumber(0, other.Count)];

                            if (IsThreeAndOne(groupCards) && GetWeight(groupCards, type) > weight)
                            {
                                result.Add(groupCards);
                            }
                        }
                    }
                    break;
                case CardsType.ThreeAndTwo:
                    if (copyCards.Count >= 5)
                    {
                        for (int i = copyCards.Count - 1; i >= 2; i--)
                        {
                            if (copyCards[i].m_weight_DDZ <= deskCards[deskCards.Length - 1].m_weight_DDZ)
                            {
                                continue;
                            }

                            List<PokerInfo> other = new List<PokerInfo>(copyCards);
                            other.RemoveRange(i - 2, 3);

                            List<PokerInfo[]> otherDouble = GetPrompt(other, lastCards, CardsType.Double);
                            if (otherDouble.Count > 0)
                            {
                                PokerInfo[] randomDouble = otherDouble[RandomHelper.RandomNumber(0, otherDouble.Count)];
                                PokerInfo[] groupCards = new PokerInfo[5];
                                groupCards[0] = copyCards[i - 2];
                                groupCards[1] = copyCards[i - 1];
                                groupCards[2] = copyCards[i];
                                groupCards[3] = randomDouble[0];
                                groupCards[4] = randomDouble[1];

                                if (IsThreeAndTwo(groupCards) && GetWeight(groupCards, type) > weight)
                                {
                                    result.Add(groupCards);
                                }
                            }
                        }
                    }
                    break;
                case CardsType.Straight:
                    /*
                     * 7 6 5 4 3
                     * 8 7 6 5 4
                     * 
                     * */
                    if (copyCards.Count >= deskCards.Length)
                    {
                        for (int i = copyCards.Count - 1; i >= deskCards.Length - 1; i--)
                        {
                            if (copyCards[i].m_weight_DDZ <= deskCards[deskCards.Length - 1].m_weight_DDZ)
                            {
                                continue;
                            }

                            //是否全部搜索完成
                            bool isTrue = true;
                            PokerInfo[] groupCards = new PokerInfo[deskCards.Length];
                            for (int j = 0; j < deskCards.Length; j++)
                            {
                                //搜索连续权重牌
                                PokerInfo findCard = copyCards.FirstOrDefault(card => (int)card.m_weight_DDZ == (int)copyCards[i].m_weight_DDZ + j);
                                if (findCard == null)
                                {
                                    isTrue = false;
                                    break;
                                }
                                groupCards[deskCards.Length - 1 - j] = findCard;
                            }

                            if (isTrue && IsStraight(groupCards) && GetWeight(groupCards, type) > weight)
                            {
                                result.Add(groupCards);
                            }
                        }
                    }
                    break;
                case CardsType.DoubleStraight:
                    /*
                     * 5 5 4 4 3 3
                     * 6 6 5 5 4 4
                     * 
                     * */
                    if (copyCards.Count >= deskCards.Length)
                    {
                        for (int i = copyCards.Count - 1; i >= deskCards.Length - 1; i--)
                        {
                            if (copyCards[i].m_weight_DDZ <= deskCards[deskCards.Length - 1].m_weight_DDZ)
                            {
                                continue;
                            }

                            //是否全部搜索完成
                            bool isTrue = true;
                            PokerInfo[] groupCards = new PokerInfo[deskCards.Length];
                            for (int j = 0; j < deskCards.Length; j += 2)
                            {
                                //搜索连续权重牌
                                PokerInfo[] findCards = copyCards.Where(card => (int)card.m_weight_DDZ == (int)copyCards[i].m_weight_DDZ + (j / 2)).ToArray();
                                if (findCards.Length < 2)
                                {
                                    isTrue = false;
                                    break;
                                }
                                groupCards[deskCards.Length - 2 - j] = findCards[0];
                                groupCards[deskCards.Length - 1 - j] = findCards[1];
                            }

                            if (isTrue && IsDoubleStraight(groupCards) && GetWeight(groupCards, type) > weight)
                            {
                                result.Add(groupCards);
                            }
                        }
                    }
                    break;
                case CardsType.TripleStraight:
                    if (copyCards.Count >= deskCards.Length)
                    {
                        for (int i = copyCards.Count - 1; i >= deskCards.Length - 1; i--)
                        {
                            if (copyCards[i].m_weight_DDZ <= deskCards[deskCards.Length - 1].m_weight_DDZ)
                            {
                                continue;
                            }

                            //是否全部搜索完成
                            bool isTrue = true;
                            PokerInfo[] groupCards = new PokerInfo[deskCards.Length];
                            for (int j = 0; j < deskCards.Length; j += 3)
                            {
                                //搜索连续权重牌
                                PokerInfo[] findCards = copyCards.Where(card => (int)card.m_weight_DDZ == (int)copyCards[i].m_weight_DDZ + (j / 3)).ToArray();
                                if (findCards.Length < 3)
                                {
                                    isTrue = false;
                                    break;
                                }
                                groupCards[deskCards.Length - 3 - j] = findCards[0];
                                groupCards[deskCards.Length - 2 - j] = findCards[1];
                                groupCards[deskCards.Length - 1 - j] = findCards[2];
                            }

                            if (isTrue && IsTripleStraight(groupCards) && GetWeight(groupCards, type) > weight)
                            {
                                result.Add(groupCards);
                            }
                        }
                    }
                    break;
                case CardsType.Double:
                    if (copyCards.Count >= 2)
                    {
                        for (int i = copyCards.Count - 1; i >= 1; i--)
                        {
                            PokerInfo[] groupCards = new PokerInfo[2];
                            groupCards[0] = copyCards[i - 1];
                            groupCards[1] = copyCards[i];

                            if (IsDouble(groupCards) && GetWeight(groupCards, type) > weight)
                            {
                                result.Add(groupCards);
                            }
                        }
                    }
                    break;
                case CardsType.Single:
                    if (copyCards.Count >= 1)
                    {
                        for (int i = copyCards.Count - 1; i >= 0; i--)
                        {
                            if (copyCards[i].m_weight_DDZ <= deskCards[deskCards.Length - 1].m_weight_DDZ)
                            {
                                continue;
                            }
                            PokerInfo[] groupCards = new PokerInfo[1];
                            groupCards[0] = copyCards[i];

                            if (IsSingle(groupCards) && GetWeight(groupCards, type) > weight)
                            {
                                result.Add(groupCards);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// 找到5张单顺
        /// </summary>
        /// <param name="landlordsCards"></param>
        /// <returns></returns>
        public static List<PokerInfo> FindFiveStraght(List<PokerInfo> landlordsCards)
        {
            HashSet<Weight_DDZ> cardsWeight_DDZ = new HashSet<Weight_DDZ>();
            List<PokerInfo> landlordsCard = new List<PokerInfo>();

            for (int i = landlordsCards.Count - 1; i >= 1; i--)
            {
                if (landlordsCards[i].m_weight_DDZ + 1 == landlordsCards[i - 1].m_weight_DDZ ||
                    landlordsCards[i].m_weight_DDZ == landlordsCards[i - 1].m_weight_DDZ)
                {
                    if (cardsWeight_DDZ.Add(landlordsCards[i].m_weight_DDZ))
                    {
                        landlordsCard.Add(landlordsCards[i]);
                    }

                    if (cardsWeight_DDZ.Add(landlordsCards[i - 1].m_weight_DDZ))
                    {
                        landlordsCard.Add(landlordsCards[i - 1]);
                    }
                    if (landlordsCard.Count == 5)
                        break;
                }
            }

            if (landlordsCard.Count != 5)
            {
                return new List<PokerInfo>();
            }
            return landlordsCard;
        }

        /// <summary>
        /// 找到所有的5张单顺
        /// </summary>
        /// <param name="landlordsCards"></param>
        /// <returns></returns>
        public static List<List<PokerInfo>> FindAllFiveStraght(List<PokerInfo> landlordsCards)
        {
            List<PokerInfo> copyCards = new List<PokerInfo>(landlordsCards);
            List<List<PokerInfo>> result = new List<List<PokerInfo>>();
            while (copyCards.Count >= 5)
            {
                List<PokerInfo> findFiveStraght = FindFiveStraght(copyCards);
                if (findFiveStraght.Count == 0)
                    break;
                result.Add(findFiveStraght);
                copyCards.RemoveList(findFiveStraght);
            }
            return result;
        }

        /// <summary>
        /// 游戏调用托管出牌
        /// </summary>
        /// <param name="room"></param>
        /// <param name="playerData"></param>
        /// <param name="listPoker"></param>
        /// <returns></returns>
        //public static List<PokerInfo> GetTrusteeshipPoker(DDZ_RoomData room, DDZ_PlayerData playerData)
        //{
        //    List<TLJCommon.PokerInfo> listPoker = new List<TLJCommon.PokerInfo>();
        //    LandlordsCardsHelper.SetWeight(room);

        //    List<PokerInfo> handPoker = playerData.getPokerList();

        //    if (!playerData.m_isAI)
        //    {
        //        PlayService.log.Warn($"当前最大的玩家:{room?.biggestPlayerData.m_uid}");
        //        主动出牌

        //        if (room.biggestPlayerData == null || room.biggestPlayerData == playerData)
        //        {
        //            listPoker = handPoker.Where(card => card.m_weight_DDZ == handPoker[handPoker.Count - 1].m_weight_DDZ).ToList();
        //        }
        //        else
        //        {
        //            DDZ_PlayerData beforePlayerData = room.getBeforePlayerData(playerData.m_uid);

        //            给上一乱手牌设权重
        //            LandlordsCardsHelper.SetWeight(beforePlayerData.getPokerList());
        //            LandlordsCardsHelper.SetWeight(beforePlayerData.m_curOutPokerList);

        //            if (beforePlayerData.m_curOutPokerList.Count != 0)
        //            {
        //                if (LandlordsCardsHelper.GetCardsType(beforePlayerData.m_curOutPokerList.ToArray(), out var type))
        //                {
        //                    PlayService.log.Warn($"上一轮玩家{beforePlayerData.m_uid}出牌类型：{type}\n{Newtonsoft.Json.JsonConvert.SerializeObject(beforePlayerData.m_curOutPokerList.ToArray())}");

        //                    List<PokerInfo[]> result = LandlordsCardsHelper.GetPrompt(handPoker, beforePlayerData.m_curOutPokerList, type);

        //                    if (result.Count > 0)
        //                    {
        //                        listPoker = result[RandomHelper.RandomNumber(0, result.Count)].ToList();
        //                        room.biggestPlayerData = playerData;

        //                        PlayService.log.Warn($"当前玩家{playerData.m_uid}可以出牌类型：{type}\n{Newtonsoft.Json.JsonConvert.SerializeObject(result.ToArray())}");
        //                        PlayService.log.Warn($"当前玩家{playerData.m_uid}出牌:{Newtonsoft.Json.JsonConvert.SerializeObject(listPoker.ToArray())}");
        //                    }
        //                }
        //                else
        //                {
        //                    PlayService.log.Error($"未知的类型：{type}");
        //                }
        //            }
        //            else
        //            {
        //            }
        //        }

        //    }
        //    return listPoker;
        //}
    }
}
