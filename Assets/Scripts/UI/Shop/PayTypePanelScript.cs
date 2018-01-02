using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class PayTypePanelScript : MonoBehaviour
{
    private ShopData _shopData;

    public static GameObject create(ShopData shopData)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/PayTypePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        obj.GetComponent<PayTypePanelScript>().SetShopData(shopData);
        return obj;
    }

    public void SetShopData(ShopData shopData)
    {
        _shopData = shopData;
    }

    private JsonData SetRequest()
    {
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
        var data = SetRequest();
        PlatformHelper.pay(Constants.PAY_TYPE_ALIPAY, "AndroidCallBack", "GetPayResult", data.ToJson());
    }

    public void OnClickWeChatPay()
    {
        var data = SetRequest();

        PlatformHelper.pay(Constants.PAY_TYPE_WX, "AndroidCallBack", "GetPayResult", data.ToJson());
    }
}
