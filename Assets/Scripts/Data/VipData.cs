using System.Collections.Generic;
using LitJson;
using System;

public class VipData
{
    public int vipLevel;
    public int medalNum;
    public VipOnce vipOnce;
    public VipWeekly vipWeekly;
    public int turnTableCount;


    public static void reqNet()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("VipData", "reqNet"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.VipData", "reqNet", null, null);
            return;
        }

        UnityWebReqUtil.Instance.Get(OtherData.getWebUrl() + "VipRewardData.json", httpCallBack);
    }

    public static void httpCallBack(string tag, string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("VipData", "httpCallBack"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.VipData", "httpCallBack", null, tag, data);
            return;
        }

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