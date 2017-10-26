using System.Collections.Generic;

public class RankData {
    public static List<GoldRankItemData> goldRankDataList;
    public static List<MedalRankItemData> medalRankDataList;
}

public class GoldRankItemData
{
    public string name { set; get; }
    public int head { set; get; }
    public int gold { set; get; }
}

public class MedalRankItemData
{
    public string name { set; get; }
    public int head { set; get; }
    public int medal { set; get; }
}