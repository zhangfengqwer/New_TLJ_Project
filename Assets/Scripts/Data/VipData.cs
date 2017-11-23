using System.Collections.Generic;
using LitJson;

public class VipData
{
    public int vipLevel;
    public int medalNum;
    public VipOnce vipOnce;
    public VipWeekly vipWeekly;


    public static void reqNet()
    {
        UnityWebReqUtil.Instance.Get(OtherData.s_webDownUrl + "VipRewardData.json", httpCallBack);
    }

    private static void httpCallBack(string tag, string data)
    {
        VipPanelScript.vipDatas = JsonMapper.ToObject<List<VipData>>(data);
        LogUtil.Log("vipDatas:" + VipPanelScript.vipDatas.Count);
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