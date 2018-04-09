﻿using System.Collections.Generic;

public class ChannelHelper
{
    public static Dictionary<string, string> ChannelDic = new Dictionary<string, string>
    {
        {"huawei", "华为"},
        {"qihoo360", "360"},
        {"vivo", "vivo"},
        {"yyb", "应用宝"},
        {"baidu", "百度"},
        {"oppo", "oppo"},
        {"aiyouxi", "爱游戏"},
        {"xiaomi", "小米"},
        {"aligame", "阿里游戏"},
        {"meizu", "魅族"},
        {"zhuoyi", "卓易"},
        {"appchina", "应用汇"},
        {"leshi", "乐视"},
        {"ydmm", "移动mm"},
        {"mumayi", "木蚂蚁"},
        {"anzhi", "安智"},
    };

    public static bool Is3RdLogin()
    {
        string channelName = PlatformHelper.GetChannelName();
        foreach (var channel in ChannelDic.Keys)
        {
            if (channel.Equals(channelName))
            {
                return true;
            }
        }
        return false;
    }

    public static string GetChannelAllName()
    {
        string channelName = PlatformHelper.GetChannelName();
        string channelAllName;
        if (ChannelDic.TryGetValue(channelName, out channelAllName))
        {
            return channelAllName;
        }

        return "";
    }
}