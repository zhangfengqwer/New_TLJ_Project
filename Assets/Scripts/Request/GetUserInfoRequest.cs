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
            GameObject.Find("Canvas").GetComponent<MainScript>().refreshUI();

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
            UserData.name = (string) jsonData["name"];
            UserData.phone = (string) jsonData["phone"];
            UserData.head = "Sprites/Head/head_" + jsonData["head"];
            UserData.gold = (int) jsonData["gold"];
            UserData.yuanbao = (int) jsonData["yuanbao"];
            UserData.IsRealName = (bool) jsonData["isRealName"];
            UserData.gameData = JsonMapper.ToObject<UserGameData>(jsonData["gameData"].ToString());
            UserData.buffData = JsonMapper.ToObject<List<BuffData>>(jsonData["BuffData"].ToString());
           

            result = data;
            flag = true;
        }
        else
        {
            Debug.Log("用户信息数据错误：" + code);
        }
    }
}