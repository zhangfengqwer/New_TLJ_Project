using System.Collections.Generic;
using LitJson;
using System;

public class VipData
{
    public int vipLevel;
    public int medalNum;
    public int turnTableCount;
    public VipOnce vipOnce;
    public VipWeekly vipWeekly;


    public static void reqNet()
    {
        UnityWebReqUtil.Instance.Get(OtherData.s_webStorageUrl + "VipRewardData.json", httpCallBack);
    }

    private static void httpCallBack(string tag, string data)
    {
        try
        {
            VipPanelScript.vipDatas = JsonMapper.ToObject<List<VipData>>(data);

            OtherData.s_getNetEntityFile.GetFileSuccess("VipRewardData.json");
        }
        catch (Exception ex)
        {
            LogUtil.Log("获取VIP数据出错：" + ex.Message);
            OtherData.s_getNetEntityFile.GetFileFail("VipRewardData.json");

            //throw ex;
        }
    }
}

public class VipWeekly
{
    public int goldNum;
    public int diamondNum;
}

public class VipOnce
{
    public int goldNum;
    public string prop;
}