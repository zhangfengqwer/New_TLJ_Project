using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class PayTypePanelScript : MonoBehaviour
{
    public ShopData _shopData;

    public static GameObject create(ShopData shopData)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/PayTypePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        obj.GetComponent<PayTypePanelScript>().SetShopData(shopData);
        return obj;
    }

    private void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PayTypePanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PayTypePanelScript_hotfix", "Start", null, null);
            return;
        }
    }

    public void SetShopData(ShopData shopData)
    {
        _shopData = shopData;
    }

    public JsonData SetRequest()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PayTypePanelScript_hotfix", "SetRequest"))
        {
            JsonData jd = (JsonData)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PayTypePanelScript_hotfix", "SetRequest", null, null);
            return jd;
        }

        JsonData data = new JsonData();
        data["uid"] = UserData.uid;
        data["goods_id"] = _shopData.goods_id;
        data["goods_num"] = 1;
        data["goods_name"] = _shopData.goods_name;
        data["price"] = _shopData.price;
        return data;
    }

    public void OnClickAliPay()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PayTypePanelScript_hotfix", "OnClickAliPay"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PayTypePanelScript_hotfix", "OnClickAliPay", null, null);
            return;
        }

        var data = SetRequest();
        PlatformHelper.pay(Constants.PAY_TYPE_ALIPAY, "AndroidCallBack", "GetPayResult", data.ToJson());
    }

    public void OnClickWeChatPay()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PayTypePanelScript_hotfix", "OnClickWeChatPay"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PayTypePanelScript_hotfix", "OnClickWeChatPay", null, null);
            return;
        }

        var data = SetRequest();

        PlatformHelper.pay(Constants.PAY_TYPE_WX, "AndroidCallBack", "GetPayResult", data.ToJson());
    }
}
