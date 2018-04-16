using System;

namespace CrazyLandlords.Helper
{
    public class LandlordsCard : IEquatable<LandlordsCard>
    {
        public Weight CardWeight { get; private set; }
        public Suits CardSuits { get; private set; }

        public LandlordsCard(Weight cardWeight, Suits cardSuits)
        {
            CardWeight = cardWeight;
            CardSuits = cardSuits;
        }

        public bool Equals(LandlordsCard other)
        {
            return this.CardWeight == other.CardWeight && this.CardSuits == other.CardSuits;
        }

        /// <summary>
        /// 获取卡牌名
        /// </summary>
        /// <returns></returns>
 //       public string GetName()
 //       {
 ////           return this.CardSuits == Suits.None ? this.CardWeight.ToString() : $"{this.CardSuits.ToString()}:{this.CardWeight.ToString()}";
 //       }
    }
}