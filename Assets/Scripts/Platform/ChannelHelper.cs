
public class ChannelHelper  {

    public static string[] ChannelList = { "huawei", "360", "vivo" };

    public static bool Is3RdLogin()
    {
        string channelName = PlatformHelper.GetChannelName();
        foreach (var channel in ChannelList)
        {
            if (channel.Equals(channelName))
            {
                return true;
            }
        }
        return false;
    }

    public static string GetChannelName()
    {
        return PlatformHelper.GetChannelName();
    }
}
