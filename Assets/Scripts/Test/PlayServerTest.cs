using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayServerTest : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    static public void joinRoom(int maxCount)
    {
        for (int i = 1; i <= maxCount; i++)
        {
            JsonData data = new JsonData();

            data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
            data["uid"] = (10000 + i).ToString();
            data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_JoinGame;
            data["gameroomtype"] = TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ChuJi;

            PlayServiceSocket.s_instance.sendMessage(data.ToJson());
        }
    }
}
