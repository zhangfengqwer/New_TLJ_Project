using CrazyLandlords.Helper;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;

public class DDZ_NetReqLogic : MonoBehaviour
{
    public string m_hotfix_class = "DDZ_NetReqLogic_hotfix";
    public string m_hotfix_path = "HotFix_Project.DDZ_NetReqLogic_hotfix";

    string m_tag = TLJCommon.Consts.Tag_DouDiZhu_Game;

    // 是否已经加入房间
    public void reqIsJoinRoom()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "reqIsJoinRoom"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "reqIsJoinRoom", null, null);
            return;
        }

        NetLoading.getInstance().Show();

        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_IsJoinGame;
        data["uid"] = UserData.uid;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求恢复房间
    public void reqRetryJoinGame()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "reqRetryJoinGame"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "reqRetryJoinGame", null, null);
            return;
        }

        NetLoading.getInstance().Close();

        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_RetryJoinGame;
        data["uid"] = UserData.uid;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求加入房间
    public void reqJoinRoom(string gameroomtype)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "reqJoinRoom"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "reqJoinRoom", null, gameroomtype);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "reqExitRoom"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "reqExitRoom", null, null);
            return;
        }

        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_ExitGame;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }

    // 请求抢地主
    public void reqQiangDiZhu(int fen)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "reqQiangDiZhu"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "reqQiangDiZhu", null, fen);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "reqJiaBang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "reqJiaBang", null, isJiaBang);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "reqOutPoker"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "reqOutPoker", null, null);
            return;
        }

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
            List<TLJCommon.PokerInfo> m_maxPlayerOutPokerList = DDZ_GameData.getInstance().m_maxPlayerOutPokerList;
            LandlordsCardsHelper.SetWeight(myOutPokerList);
            LandlordsCardsHelper.SetWeight(m_maxPlayerOutPokerList);

            if (myOutPokerList.Count != 0)
            {
                CardsType type;
                if (LandlordsCardsHelper.GetCardsType(myOutPokerList.ToArray(), out type))
                {
                    if (!DDZ_GameData.getInstance().m_isFreeOutPoker)
                    {
                        CardsType lastType;
                        if (LandlordsCardsHelper.GetCardsType(m_maxPlayerOutPokerList.ToArray(), out lastType))
                        {
                            List<PokerInfo[]> pokerInfoses = LandlordsCardsHelper.GetPrompt(myOutPokerList, m_maxPlayerOutPokerList, lastType);
                            if (pokerInfoses.Count == 0)
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
                    ToastScript.createToast("自己出牌不符合规则");
                    return;
                }
            }
            else
            {
                if (DDZ_GameData.getInstance().m_isFreeOutPoker)
                {
                    ToastScript.createToast("请选择您出的牌");
                    return;
                }
                else
                {
                    // 不要
                }
            }
        }

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());

        // 所有牌设为未选中
        PokerScript.setAllPokerWeiXuanZe();
    }

    public void reqSetTuoGuanState(bool isTuoGuan)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "reqSetTuoGuanState"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "reqSetTuoGuanState", null, isTuoGuan);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc(m_hotfix_class, "reqChat"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke(m_hotfix_path, "reqChat", null, type, content_id);
            return;
        }

        JsonData data = new JsonData();

        data["tag"] = m_tag;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.DDZ_PlayAction.PlayAction_Chat;
        data["type"] = type;
        data["content_id"] = content_id;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }
}
