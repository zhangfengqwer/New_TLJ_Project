
public class VipUtil
{
	private static int vip1 = 6;
	private static int vip2 = 30;
	private static int vip3 = 98;
	private static int vip4 = 320;
	private static int vip5 = 888;
	private static int vip6 = 1600;
	private static int vip7 = 5000;
	private static int vip8 = 10000;
	private static int vip9 = 18000;
	private static int vip10 = 30000;
	
	
	
	public static int GetVipLevel(int recharge)
	{
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("VipUtil", "GetVipLevel"))
        {
            int i = (int)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.VipUtil", "GetVipLevel", null, recharge);
            return i;
        }

        if (recharge > 0 && recharge < vip1) return 0;
		if (recharge >= vip1 && recharge < vip2) return 1;
		if (recharge >= vip2 && recharge < vip3) return 2;
		if (recharge >= vip3 && recharge < vip4) return 3;
		if (recharge >= vip4 && recharge < vip5) return 4;
		if (recharge >= vip5 && recharge < vip6) return 5;
		if (recharge >= vip6 && recharge < vip7) return 6;
		if (recharge >= vip7 && recharge < vip8) return 7;
		if (recharge >= vip8 && recharge < vip9) return 8;
		if (recharge >= vip9 && recharge < vip10) return 9;
		if (recharge >= vip10 ) return 10;

		return 0;
	}
    
	public static int GetCurrentVipTotal(int vipLevel)
	{
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("VipUtil", "GetCurrentVipTotal"))
        {
            int i = (int)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.VipUtil", "GetCurrentVipTotal", null, vipLevel);
            return i;
        }

        int vipTotal = 0;
		switch (vipLevel)
		{
			case 0:
				vipTotal = vip1;
				break;
			case 1:
				vipTotal = vip2;
				break;
			case 2:
				vipTotal = vip3;
				break;
			case 3:
				vipTotal = vip4;
				break;
			case 4:
				vipTotal = vip5;
				break;
			case 5:
				vipTotal = vip6;
				break;
			case 6:
				vipTotal = vip7;
				break;
			case 7:
				vipTotal = vip8;
				break;
			case 8:
				vipTotal = vip9;
				break;
			case 9:
				vipTotal = vip10;
				break;
			default:
				vipTotal = 30000;
				break;
		}

		return vipTotal;
	}
}
