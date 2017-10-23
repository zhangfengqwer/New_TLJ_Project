using System.Collections.Generic;
public class UserData  {

    public static string uid { set; get; }
    public static string name { set; get; }
    public static string phone { set; get; }
    public static string head { set ; get; }
    public static int gold{ set; get; }
    public static int yuanbao { set; get; }
    public static UserGameData gameData { set; get; }
    public static List<UserPropData> propData = new List<UserPropData>();
    public static List<BuffData> buffData = new List<BuffData>();
}

public class UserGameData
{
    public int allGameCount { get; set; }
    public int winCount { get; set; }
    public int runCount { get; set; }
    public int meiliZhi { get; set; }
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
}