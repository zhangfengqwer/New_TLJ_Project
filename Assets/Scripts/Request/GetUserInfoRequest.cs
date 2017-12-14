using System;
using LitJson;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;

public class GetUserInfoRequest : Request
{
    private bool flag = false;
    private string result;

    private void Awake()
    {
        Tag = Consts.Tag_UserInfo;
    }

    void Update()
    {
        if (flag)
        {
            if (OtherData.s_mainScript != null)
            {
                OtherData.s_mainScript.refreshUI();
            }

            LogicEnginerScript.Instance.GetComponent<GetTaskRequest>().OnRequest();
            LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().OnRequest();
            if (ShopPanelScript.Instance != null)
            {
                ShopPanelScript.Instance.InitUserInfo();
            }
            flag = false;
        }
    }

    public override void OnRequest()
    {
        JsonData jsonData = new JsonData();
        jsonData["tag"] = Tag;
        jsonData["uid"] = UserData.uid;
        string requestData = jsonData.ToJson();
        LogicEnginerScript.Instance.SendMyMessage(requestData);
    }

    public override void OnResponse(string data)
    {
        JsonData jsonData = JsonMapper.ToObject(data);
        var code = (int) jsonData["code"];
        if (code == (int) Consts.Code.Code_OK)
        {
            try
            {
                UserData.name = (string) jsonData["name"];
                UserData.phone = (string) jsonData["phone"];
                UserData.head = "Sprites/Head/head_" + jsonData["head"];
                UserData.gold = (int) jsonData["gold"];
                UserData.yuanbao = (int) jsonData["yuanbao"];
                UserData.medal = (int) jsonData["medal"];
                UserData.IsRealName = (bool) jsonData["isRealName"];
                UserData.isSetSecondPsw = (bool) jsonData["isSetSecondPsw"];
                UserData.rechargeVip = (int) jsonData["recharge_vip"];
                UserData.gameData = JsonMapper.ToObject<UserGameData>(jsonData["gameData"].ToString());
                UserData.buffData = JsonMapper.ToObject<List<BuffData>>(jsonData["BuffData"].ToString());
                UserData.userRecharge = JsonMapper.ToObject<List<UserRecharge>>(jsonData["userRecharge"].ToString());
                UserData.myTurntableData = JsonMapper.ToObject<MyTurntableData>(jsonData["turntableData"].ToString());
            }
            catch (Exception e)
            {
                LogUtil.Log("解析用户信息json失败:" + e);
            }
            result = data;
            flag = true;
        }
        else
        {
            LogUtil.Log("用户信息数据错误：" + code);
        }
    }
}