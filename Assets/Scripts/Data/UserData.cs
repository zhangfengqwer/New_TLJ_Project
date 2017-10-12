
public class UserData  {

    public static string uid { set; get; }
    public static string name { set; get; }
    public static string phone { set; get; }
    public static int gold{ set; get; }
    public static int yuanbao { set; get; }
    public static UserGameData gameData { set; get; }
}

public class UserGameData
{
    public int allGameCount { get; set; }
    public int winCount { get; set; }
    public int runCount { get; set; }
    public int meiliZhi { get; set; }
}