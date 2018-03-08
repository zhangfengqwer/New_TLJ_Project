using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameUtil
{
    // 找出一组牌中某种花色的单牌
    public static List<TLJCommon.PokerInfo> choiceSinglePoker(List<TLJCommon.PokerInfo> myPokerList, TLJCommon.Consts.PokerType pokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "choiceSinglePoker"))
        {
            List<TLJCommon.PokerInfo> list = (List<TLJCommon.PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "choiceSinglePoker", null, myPokerList, pokerType);
            return list;
        }

        // 先筛选出同花色的牌
        List<TLJCommon.PokerInfo> pokerList = new List<TLJCommon.PokerInfo>();
        for (int i = myPokerList.Count - 1; i >= 0 ; i--)
        {
            if (myPokerList[i].m_pokerType == pokerType)
            {
                pokerList.Add(myPokerList[i]);
            }
        }

        List<TLJCommon.PokerInfo> singleList = new List<TLJCommon.PokerInfo>();
        List<TLJCommon.PokerInfo> doubleList = new List<TLJCommon.PokerInfo>();

        if(pokerList.Count > 1)
        {
            for (int i = pokerList.Count - 1; i >= 1; i--)
            {
                if (pokerList[i].m_num == pokerList[i - 1].m_num)
                {
                    doubleList.Add(pokerList[i]);
                    --i;

                    if (i == 1)
                    {
                        singleList.Add(pokerList[i - 1]);
                    }
                }
                else
                {
                    singleList.Add(pokerList[i]);

                    if (i == 1)
                    {
                        singleList.Add(pokerList[i - 1]);
                    }
                }
            }
        }
        else if (pokerList.Count == 1)
        {
            singleList.Add(pokerList[0]);
        }

        return singleList;
    }

    // 找出一组牌中某种花色的对子
    public static List<TLJCommon.PokerInfo> choiceDoublePoker(List<TLJCommon.PokerInfo> myPokerList, TLJCommon.Consts.PokerType pokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "choiceDoublePoker"))
        {
            List<TLJCommon.PokerInfo> list = (List<TLJCommon.PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "choiceDoublePoker", null, myPokerList, pokerType);
            return list;
        }

        // 先筛选出同花色的牌
        List<TLJCommon.PokerInfo> pokerList = new List<TLJCommon.PokerInfo>();
        for (int i = myPokerList.Count - 1; i >= 0; i--)
        {
            if (myPokerList[i].m_pokerType == pokerType)
            {
                pokerList.Add(myPokerList[i]);
            }
        }

        List<TLJCommon.PokerInfo> singleList = new List<TLJCommon.PokerInfo>();
        List<TLJCommon.PokerInfo> doubleList = new List<TLJCommon.PokerInfo>();

        if (pokerList.Count > 1)
        {
            for (int i = pokerList.Count - 1; i >= 1; i--)
            {
                if (pokerList[i].m_num == pokerList[i - 1].m_num)
                {
                    doubleList.Add(pokerList[i]);
                    --i;

                    if (i == 1)
                    {
                        singleList.Add(pokerList[i - 1]);
                    }
                }
                else
                {
                    singleList.Add(pokerList[i]);

                    if (i == 1)
                    {
                        singleList.Add(pokerList[i - 1]);
                    }
                }
            }
        }
        else if (pokerList.Count == 1)
        {
            singleList.Add(pokerList[0]);
        }

        return doubleList;
    }

    static public string getPropIconPath(int prop_id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "getPropIconPath"))
        {
            string str = (string)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "getPropIconPath", null, prop_id);
            return str;
        }

        string path = "";

        // 金币
        if (prop_id == 1)
        {
            path = "Sprites/Icon/Prop/icon_jinbi";
        }
        // 元宝
        else if (prop_id == 2)
        {
            path = "Sprites/Icon/Prop/icon_yuanbao";
        }
        // 人民币
        else if (prop_id == 3)
        {
            path = "Sprites/Icon/icon_rmb";
        }
        else
        {
            path = "Sprites/Icon/Prop/" + PropData.getInstance().getPropInfoById(prop_id).m_icon;
        }
        return path;
    }

    static public string getMasterPokerIconPath(int masterPokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "getMasterPokerIconPath"))
        {
            string str = (string)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "getMasterPokerIconPath", null, masterPokerType);
            return str;
        }

        string path = "Sprites/Game/Poker/icon_wuzhu";
        
        if (masterPokerType == (int)TLJCommon.Consts.PokerType.PokerType_FangKuai)
        {
            path = "Sprites/Game/Poker/icon_fangkuai";
        }
        else if (masterPokerType == (int)TLJCommon.Consts.PokerType.PokerType_HeiTao)
        {
            path = "Sprites/Game/Poker/icon_heitao";
        }
        else if (masterPokerType == (int)TLJCommon.Consts.PokerType.PokerType_HongTao)
        {
            path = "Sprites/Game/Poker/icon_hongtao";
        }
        else if (masterPokerType == (int)TLJCommon.Consts.PokerType.PokerType_MeiHua)
        {
            path = "Sprites/Game/Poker/icon_meihua";
        }
        else if (masterPokerType == (int)TLJCommon.Consts.PokerType.PokerType_Wang)
        {
            path = "Sprites/Game/Poker/icon_wuzhu";
        }
        else if (masterPokerType == -1)
        {
            path = "Sprites/Game/Poker/icon_wuzhu";
        }

        return path;
    }

    /*
     * 101:1000
     */
    static public void changeDataWithStr(string reward)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "changeDataWithStr"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "changeDataWithStr", null, reward);
            return;
        }

        List<string> list = new List<string>();
        CommonUtil.splitStr(reward,list,':');

        int id = int.Parse(list[0]);
        int num = int.Parse(list[1]);

        changeData(id,num);
    }

    /*
     * 用于玩家 <金币、元宝、道具> 的获得和消耗
     * 如果当前在主界面，会刷新主界面的金币和元宝数值
     */
    static public void changeData(int id , int num)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "changeData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "changeData", null, id,num);
            return;
        }

        LogUtil.Log("changeData---" + id + "  " + num);
        if (id == 1)
        {
            UserData.gold += num;

            if (UserData.gold < 0)
            {
                UserData.gold = 0;
            }

            if (OtherData.s_mainScript != null)
            {
                OtherData.s_mainScript.refreshUI();
            }
        }
        else if (id == 2)
        {
            UserData.yuanbao += num;

            if (OtherData.s_mainScript != null)
            {
                OtherData.s_mainScript.refreshUI();
            }
        }
        else if (id == (int)TLJCommon.Consts.Prop.Prop_huizhang)
        {
            UserData.medal += num;

            if (OtherData.s_mainScript != null)
            {
                OtherData.s_mainScript.refreshUI();
            }
        }
        else
        {
            bool isFind = false;
            for (int i = 0; i < UserData.propData.Count; i++)
            {
                if (UserData.propData[i].prop_id == id)
                {
                    isFind = true;

                    UserData.propData[i].prop_num += num;

                    if (UserData.propData[i].prop_num <= 0)
                    {
                        UserData.propData.RemoveAt(i);
                    }

                    break;
                }
            }

            if (!isFind)
            {
                LogUtil.Log("新增物品");
                PropInfo propInfo = PropData.getInstance().getPropInfoById(id);
                if (propInfo != null)
                {
                    UserPropData userPropData = new UserPropData();
                    userPropData.prop_id = id;
                    userPropData.prop_num = num;
                    userPropData.prop_name = propInfo.m_name;
                    userPropData.prop_icon = propInfo.m_icon;

                    UserData.propData.Add(userPropData);
                }
            }

            if (OtherData.s_mainScript != null)
            {
                OtherData.s_mainScript.refreshUI();
            }
        }
    }

    public static string getOneTips()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "getOneTips"))
        {
            string s = (string)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "getOneTips", null, null);
            return s;
        }

        string str = "";

        int i = RandomUtil.getRandom(1, 3);
        if (i == 1)
        {
            str = "欢迎来到疯狂升级 点击喇叭可进行全服喊话哦~~";
        }
        else if (i == 2)
        {
            str = "关注微信公众号：“星焱娱乐”，即送丰厚大礼噢~";
        }
        else if (i == 3)
        {
            str = "弹框界面点击屏幕空白处返回";
        }

        return str;
    }

    public static bool CheckGameObjectContainPoint(GameObject obj,Vector3 point)
    {
        point = new Vector3(point.x * (1280.0f / OtherData.s_screenSize.x), point.y * (720.0f / OtherData.s_screenSize.y));

        Vector2 objSize = obj.GetComponent<RectTransform>().sizeDelta * obj.transform.localScale.x;
        Vector3 objPos = obj.transform.localPosition;

        LogUtil.Log(OtherData.s_screenSize + "    "+objPos + "    " + point);

        if ((point.x >= (objPos.x - objSize.x / 2)) &&
            (point.x <= (objPos.x + objSize.x / 2)) &&
            (point.y >= (objPos.y - objSize.y / 2)) &&
            (point.y <= (objPos.y + objSize.y / 2)))
        {
            return true;
        }

        return false;
    }

    public static void setGameRoomTypeLogoPath(string gameroomtype,Image gameRoomLogo)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "setGameRoomTypeLogoPath"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "setGameRoomTypeLogoPath", null, gameroomtype, gameRoomLogo);
            return;
        }

        string path = "";

        if (gameroomtype.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_Common) == 0)
        {
            path = "Sprites/Game/RoomLogo/roomtype_jingdian_common";
        }
        else if (gameroomtype.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ChuJi) == 0)
        {
            path = "Sprites/Game/RoomLogo/roomtype_jingdian_xinshou";
        }
        else if (gameroomtype.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ZhongJi) == 0)
        {
            path = "Sprites/Game/RoomLogo/roomtype_jingdian_jingying";
        }
        else if (gameroomtype.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_GaoJi) == 0)
        {
            path = "Sprites/Game/RoomLogo/roomtype_jingdian_dashi";
        }
        else if (gameroomtype.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_Common) == 0)
        {
            path = "Sprites/Game/RoomLogo/roomtype_chaodi_common";
        }
        else if (gameroomtype.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_ChuJi) == 0)
        {
            path = "Sprites/Game/RoomLogo/roomtype_chaodi_xinshou";
        }
        else if (gameroomtype.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_ZhongJi) == 0)
        {
            path = "Sprites/Game/RoomLogo/roomtype_chaodi_jingying";
        }
        else if (gameroomtype.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_GaoJi) == 0)
        {
            path = "Sprites/Game/RoomLogo/roomtype_chaodi_dashi";
        }
        else
        {
            List<string> list = new List<string>();
            CommonUtil.splitStr(gameroomtype, list, '_');

            if (list.Count == 3)
            {
                if (list[0].CompareTo("PVP") == 0)
                {
                    // pvp金币场
                    if (list[1].CompareTo("JinBi") == 0)
                    {
                        path = "Sprites/Game/RoomLogo/roomlogo_jinbichang";

                        PVPGameRoomData pvpGameRoomData = PVPGameRoomDataScript.getInstance().getDataByRoomType(gameroomtype);
                        if (pvpGameRoomData != null)
                        {
                            gameRoomLogo.transform.Find("Text").GetComponent<Text>().text = pvpGameRoomData.reward_num.ToString();
                        }

                        gameRoomLogo.transform.Find("Text").GetComponent<Text>().fontSize = 23;
                        gameRoomLogo.transform.Find("Text").GetComponent<Text>().transform.localScale = new Vector3(1, 1, 1);
                        gameRoomLogo.transform.Find("Text").GetComponent<Text>().transform.localPosition = new Vector3(-42.0f, -23.2f, 0);
                    }
                    // pvp话费场
                    else if (list[1].CompareTo("HuaFei") == 0)
                    {
                        path = "Sprites/Game/RoomLogo/roomlogo_huafeichang";

                        PVPGameRoomData pvpGameRoomData = PVPGameRoomDataScript.getInstance().getDataByRoomType(gameroomtype);
                        if (pvpGameRoomData != null)
                        {
                            gameRoomLogo.transform.Find("Text").GetComponent<Text>().text = list[2];
                        }

                        gameRoomLogo.transform.Find("Text").GetComponent<Text>().fontSize = 57;
                        gameRoomLogo.transform.Find("Text").GetComponent<Text>().transform.localScale = new Vector3(1, 1, 1);
                        gameRoomLogo.transform.Find("Text").GetComponent<Text>().transform.localPosition = new Vector3(-34.4f, 53.2f, 0);
                    }
                }
            }
        }

        CommonUtil.setImageSprite(gameRoomLogo, path);
    }

    public static void showGameObject(GameObject obj)
    {
        obj.transform.localScale = new Vector3(1,1,1);
    }

    public static void hideGameObject(GameObject obj)
    {
        obj.transform.localScale = new Vector3(0,0,0);
    }

    public static string getPokerNumWithStr(int num)
    {
        if ((num >= 2) && (num <= 10))
        {
            return num.ToString();
        }
        else if(num == 11)
        {
            return "J";
        }
        else if (num == 12)
        {
            return "Q";
        }
        else if (num == 13)
        {
            return "K";
        }
        else if (num == 14)
        {
            return "A";
        }

        return "";
    }

    public static bool checkCanEnterRoom(string gameRoomType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "checkCanEnterRoom"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "checkCanEnterRoom", null, gameRoomType);
            return b;
        }

        if (gameRoomType.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ChuJi) == 0)
        {
            if (UserData.gold < 1500)
            {
                ToastScript.createToast("金币不足1500，请前去购买");
                return false;
            }
        }
        else if (gameRoomType.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ZhongJi) == 0)
        {
            if (UserData.gold < 35000)
            {
                ToastScript.createToast("金币不足35000，请前去购买");
                return false;
            }
        }
        else if (gameRoomType.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_GaoJi) == 0)
        {
            if (UserData.gold < 100000)
            {
                ToastScript.createToast("金币不足100000，请前去购买");
                return false;
            }
        }
        else if (gameRoomType.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_ChuJi) == 0)
        {
            if (UserData.gold < 1500)
            {
                ToastScript.createToast("金币不足1500，请前去购买");
                return false;
            }
        }
        else if (gameRoomType.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_ZhongJi) == 0)
        {
            if (UserData.gold < 35000)
            {
                ToastScript.createToast("金币不足35000，请前去购买");
                return false;
            }
        }
        else if (gameRoomType.CompareTo(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_GaoJi) == 0)
        {
            if (UserData.gold < 100000)
            {
                ToastScript.createToast("金币不足100000，请前去购买");
                return false;
            }
        }

        return true;
    }

    public static string getRoomName(string gameRoomType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "getRoomName"))
        {
            string s = (string)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "getRoomName", null, gameRoomType);
            return s;
        }

        string roonName = "";

        if (gameRoomType.CompareTo("XiuXian_JingDian_Common") == 0)
        {
            roonName = "经典玩法";
        }
        else if (gameRoomType.CompareTo("XiuXian_JingDian_ChuJi") == 0)
        {
            roonName = "经典玩法-新手场";
        }
        else if (gameRoomType.CompareTo("XiuXian_JingDian_ZhongJi") == 0)
        {
            roonName = "经典玩法-精英场";
        }
        else if (gameRoomType.CompareTo("XiuXian_JingDian_GaoJi") == 0)
        {
            roonName = "经典玩法-大师场";
        }
        else if (gameRoomType.CompareTo("XiuXian_ChaoDi_Common") == 0)
        {
            roonName = "抄底玩法";
        }
        else if (gameRoomType.CompareTo("XiuXian_ChaoDi_ChuJi") == 0)
        {
            roonName = "抄底玩法-新手场";
        }
        else if (gameRoomType.CompareTo("XiuXian_ChaoDi_ZhongJi") == 0)
        {
            roonName = "抄底玩法-精英场";
        }
        else if (gameRoomType.CompareTo("XiuXian_ChaoDi_GaoJi") == 0)
        {
            roonName = "抄底玩法-大师场";
        }
        else
        {
            List<string> list = new List<string>();
            CommonUtil.splitStr(gameRoomType, list, '_');

            if (list.Count == 3)
            {
                if (list[0].CompareTo("PVP") == 0)
                {
                    PVPGameRoomData pvpGameRoomData = PVPGameRoomDataScript.getInstance().getDataByRoomType(gameRoomType);

                    if (pvpGameRoomData != null)
                    {
                        roonName = pvpGameRoomData.gameroomname;
                    }
                }
            }
        }

        return roonName;
    }

    static public void setNickNameFontColor(Text text, int vipLevel)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "setNickNameFontColor"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "setNickNameFontColor", null, text, vipLevel);
            return;
        }

        if (vipLevel >= 1)
        {
            text.color = new Color(255.0f / 255.0f, 223.0f / 255.0f, 114.0f / 255.0f);
        }
        else
        {
            CommonUtil.setFontColor(text, 255, 255, 255);
        }
    }

    static public int getPropIdFromReward(string reward)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "getPropIdFromReward"))
        {
            int i = (int)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "getPropIdFromReward", null, reward);
            return i;
        }

        List<string> list1 = new List<string>();
        CommonUtil.splitStr(reward, list1, ':');
            
        int prop_id = int.Parse(list1[0]);

        return prop_id;
    }

    static public int getPropNumFromReward(string reward)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "getPropNumFromReward"))
        {
            int i = (int)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "getPropNumFromReward", null, reward);
            return i;
        }

        List<string> list1 = new List<string>();
        CommonUtil.splitStr(reward, list1, ':');
        
        int prop_num = int.Parse(list1[1]);

        return prop_num;
    }

    static public int getMyPropNumById(int id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUtil_hotfix", "getMyPropNumById"))
        {
            int i = (int)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUtil_hotfix", "getMyPropNumById", null, id);
            return i;
        }

        int num = 0;

        switch (id)
        {
            case 1:
                {
                    num = UserData.gold;
                }
                break;

            case 2:
                {
                    num = UserData.yuanbao;
                }
                break;

            case 110:
                {
                    num = UserData.medal;
                }
                break;

            default:
                {
                    for (int i = 0; i < UserData.propData.Count; i++)
                    {
                        if (UserData.propData[i].prop_id == id)
                        {
                            num = UserData.propData[i].prop_num;
                            break;
                        }
                    }
                }
                break;
        }

        return num;
    }
}