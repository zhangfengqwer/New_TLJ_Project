namespace CrazyLandlords.Helper
{
    /// <summary>
    /// 花色
    /// </summary>
    public enum Suits : byte
    {
        Club,    //梅花
        Diamond, //方块
        Heart,   //红心
        Spade,   //黑桃
        None
    }

    /// <summary>
    /// 出牌类型
    /// </summary>
    public enum CardsType : byte
    {
        None,
        JokerBoom,              //王炸
        Boom,                   //炸弹
        BoomAndOne,             //四带二个单
        BoomAndTwo,             //四带二个对
        OnlyThree,              //三张
        ThreeAndOne,            //三带一
        ThreeAndTwo,            //三带二
        Straight,               //顺子 五张或更多的连续单牌
        DoubleStraight,         //双顺 三对或更多的连续对牌
        TripleStraight,         //三顺 二个或更多的连续三张牌
        TripleStraightAndOne,   //三顺带同数量的单
        TripleStraightAndTwo,   //三顺带同数量的对
        Double,                 //对子
        Single                  //单牌
    }

    /// <summary>
    /// 身份
    /// </summary>
    public enum Identity : byte
    {
        None,
        Farmer,     //平民
        Landlord    //地主
    }

    /// <summary>
    /// 卡牌权值
    /// </summary>
    public enum Weight : byte
    {
        Three,      //3
        Four,       //4
        Five,       //5
        Six,        //6
        Seven,      //7
        Eight,      //8
        Nine,       //9
        Ten,        //10
        Jack,       //J
        Queen,      //Q
        King,       //K
        One,        //A
        Two,        //2
        SJoker,     //小王
        LJoker,     //大王
    }
}