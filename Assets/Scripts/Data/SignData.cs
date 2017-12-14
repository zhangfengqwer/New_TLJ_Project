using System;

public class SignData
{
    public static bool IsSign { set; get; }
    public static int SignWeekDays { set; get; }
    public static DateTime UpdateTime { set; get; }
}


public class WeChatResult
{
    public int code { get; set; }
    public string message { get; set; }
    public Data data { get; set; }
}

public class Data
{
    public int code { get; set; }
    public string name { get; set; }
    public string nick { get; set; }
    public string tId { get; set; }
    public string url { get; set; }
    public string tgId { get; set; }
    public string msg { get; set; }
    public string expand { get; set; }
}
