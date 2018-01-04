using System.Collections.Generic;

public class ChannelHelper
{
    public static Dictionary<string, string> ChannelDic = new Dictionary<string, string>
    {
        {"huawei", "华为"},
        {"360", "360"},
        {"vivo", "vivo"},
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
        LogUtil.Log("渠道号:" + channelName);
        string channelAllName;
        if (ChannelDic.TryGetValue(channelName, out channelAllName))
        {
            return channelAllName;
        }

        return "";
    }
}