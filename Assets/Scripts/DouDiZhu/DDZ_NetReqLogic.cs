using CrazyLandlords.Helper;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDZ_NetReqLogic : MonoBehaviour
{
    string m_tag = TLJCommon.Consts.Tag_DouDiZhu_Game;

    // 是否已经加入房间
    public void reqIsJoinRoom()
    {
        NetLoading.getInstance().Show();

        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_IsJoinGame;
        data["uid"] = UserData.uid;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求恢复房间
    public void reqRetryJoinGame()
    {
        NetLoading.getInstance().Close();

        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_RetryJoinGame;
        data["uid"] = UserData.uid;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求加入房间
    public void reqJoinRoom(string gameroomtype)
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_JoinGame;
        data["gameroomtype"] = gameroomtype;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求退出房间
    public void reqExitRoom()
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_ExitGame;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求抢地主
    public void reqQiangDiZhu(int fen)
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_QiangDiZhu;
        data["fen"] = fen;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求加棒
    public void reqJiaBang(int isJiaBang)
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_JiaBang;
        data["isJiaBang"] = isJiaBang;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求出牌
    public void reqOutPoker()
    {
        JsonData data = new JsonData();

        data["tag"] = DDZ_GameData.getInstance().m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_PlayerOutPoker;

        bool hasOutPoker = false;
        List<TLJCommon.PokerInfo> myOutPokerList = new List<TLJCommon.PokerInfo>();

        // 自己出的牌
        {
            JsonData jarray = new JsonData();
            for (int i = 0; i < DDZ_GameData.getInstance().m_myPokerObjList.Count; i++)
            {
                PokerScript pokerScript = DDZ_GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>();
                if (pokerScript.getIsJump())
                {
                    hasOutPoker = true;

                    JsonData jd = new JsonData();
                    jd["num"] = pokerScript.getPokerNum();
                    jd["pokerType"] = pokerScript.getPokerType();
                    jarray.Add(jd);

                    myOutPokerList.Add(new TLJCommon.PokerInfo(pokerScript.getPokerNum(),(TLJCommon.Consts.PokerType)pokerScript.getPokerType()));
                }
            }

            data["hasOutPoker"] = hasOutPoker;

            if (hasOutPoker)
            {
                data["pokerList"] = jarray;
            }
        }
        
        // 检测出牌合理性
        {
            LandlordsCardsHelper.SetWeight(myOutPokerList);
            CardsType type;
            if (LandlordsCardsHelper.GetCardsType(myOutPokerList.ToArray(), out type))
            {
                if (DDZ_GameData.getInstance().m_isFreeOutPoker)
                {
                }
                else
                {
                    List<TLJCommon.PokerInfo> m_maxPlayerOutPokerList = DDZ_GameData.getInstance().m_maxPlayerOutPokerList;
                    LandlordsCardsHelper.SetWeight(m_maxPlayerOutPokerList);
                    CardsType lastType;
                    if (LandlordsCardsHelper.GetCardsType(m_maxPlayerOutPokerList.ToArray(), out lastType))
                    {
                        if (lastType != type || LandlordsCardsHelper.GetWeight(myOutPokerList.ToArray(),type) <= LandlordsCardsHelper.GetWeight(m_maxPlayerOutPokerList.ToArray(), type))
                        {
                            ToastScript.createToast("出牌不符合规则");
                            return;
                        }
                    }
                    else
                    {
                        ToastScript.createToast("上一家出牌不符合规则");
                        return;
                    }
                }
            }
            else
            {
                if (DDZ_GameData.getInstance().m_isFreeOutPoker)
                {
                    ToastScript.createToast("出牌不符合规则");
                    return;
                }
                else
                {

                }
            }
        }

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());

        // 所有牌设为未选中
        PokerScript.setAllPokerWeiXuanZe();

        if (hasOutPoker)
        {
            
        }
        else
        {
            // 不要
        }
    }

    public void reqSetTuoGuanState(bool isTuoGuan)
    {
        NetLoading.getInstance().Show();

        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_SetTuoGuanState;
        data["isTuoGuan"] = isTuoGuan;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 发送聊天信息
    public void reqChat(int type, int content_id)
    {
        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_Chat;
        data["type"] = type;
        data["content_id"] = content_id;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }
}
