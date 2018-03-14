using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ReqLoginDataStatistics
{
    public enum StatisticsType
    {
        StatisticsType_Login = 1,
        StatisticsType_Register,
    }

    // 登录数据统计
    public static void req(int type)
    {
        JsonData data = new JsonData();

        data["tag"] = "LoginDataStatistics";
        data["type"] = type;
        data["uid"] = UserData.uid;
        data["apkVersion"] = OtherData.s_apkVersion;
        data["channelname"] = OtherData.s_channelName;
        LoginServiceSocket.s_instance.sendMessage(data.ToJson());
    }
}
