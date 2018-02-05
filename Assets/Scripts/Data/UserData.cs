using System.Collections.Generic;
using LitJson;
public class UserData  {
    
    public static string uid { set; get; }
    public static string name { set; get; }
    public static string phone { set; get; }
    public static string head { set ; get; }
    public static int gold{ set; get; }
    public static int medal{ set; get; }
    public static int yuanbao { set; get; }
    public static int rechargeVip { set; get; }
    public static int vipLevel { set; get; }
    public static bool IsRealName { set; get; }
    public static bool isSetSecondPsw { set; get; }
    public static bool isOldPlayerBind { set; get; }
    public static UserGameData gameData { set; get; }
    public static List<UserPropData> propData = new List<UserPropData>();
    public static List<BuffData> buffData = new List<BuffData>();
    public static List<UserRecharge> userRecharge;
    public static MyTurntableData myTurntableData = new MyTurntableData();
}

public class UserRecharge
{
    public virtual int goods_id { get; set; }
    public virtual int recharge_count { get; set; }
}

public class UserGameData
{
    public int allGameCount { get; set; }
    public int winCount { get; set; }
    public int runCount { get; set; }
    public int meiliZhi { get; set; }
    public int xianxianJDPrimary { get; set; }
    public int xianxianJDMiddle { get; set; }
    public int xianxianJDHigh { get; set; }
    public int xianxianCDPrimary { get; set; }
    public int xianxianCDMiddle { get; set; }
    public int xianxianCDHigh { get; set; }
}

public class UserPropData
{
    public int prop_id;
    public int prop_num;
    public string prop_icon;
    public string prop_name;
}

public class UserNoticeJsonObj
{
    public int notice_id;
    public string title;
    public string content;
    public int type;
    public int state;
    public string start_time;
    public string end_time;
}

public class BuffData
{
    public int prop_id;
    public int buff_num;

    public BuffData()
    {

    }

    public BuffData(int id,int num)
    {
        prop_id = id;
        buff_num = num;
    }
}

public class MyTurntableData
{
    public int freeCount;
    public int huizhangCount;
    public int luckyValue;
}