using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GameUtil
{
    // 找出一组牌中某种花色的单牌
    public static List<TLJCommon.PokerInfo> choiceSinglePoker(List<TLJCommon.PokerInfo> myPokerList, TLJCommon.Consts.PokerType pokerType)
    {
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
        // 话费
        else if (prop_id == 3)
        {
            path = "Sprites/Icon/Prop/icon_jinbi";
        }
        else
        {
            path = "Sprites/Icon/Prop/" + PropData.getInstance().getPropInfoById(prop_id).m_icon;
        }
        return path;
    }

    static public string getMasterPokerIconPath(int masterPokerType)
    {
        string path = "";
        
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
     * 用于玩家 <金币、元宝、道具> 的获得和消耗
     * 如果当前在主界面，会刷新主界面的金币和元宝数值
     */
    static public void changeData(int id , int num)
    {
        if (id == 1)
        {
            UserData.gold += num;

            if (GameObject.Find("Canvas").GetComponent<MainScript>() != null)
            {
                GameObject.Find("Canvas").GetComponent<MainScript>().refreshUI();
            }
        }
        else if (id == 2)
        {
            UserData.yuanbao += num;

            if (GameObject.Find("Canvas").GetComponent<MainScript>() != null)
            {
                GameObject.Find("Canvas").GetComponent<MainScript>().refreshUI();
            }
        }
        else
        {
            for (int i = 0; i < UserData.propData.Count; i++)
            {
                if (UserData.propData[i].prop_id == id)
                {
                    UserData.propData[i].prop_num += num;

                    if (UserData.propData[i].prop_num <= 0)
                    {
                        UserData.propData.RemoveAt(i);
                    }

                    break;
                }
            }

            GameObject.Find("Canvas").GetComponent<MainScript>().refreshUI();
        }
    }

    public static string getOneTips()
    {
        string str = "";

        int i = RandomUtil.getRandom(1, 3);
        if (i == 1)
        {
            str = "系统：欢迎来到疯狂升级 点击喇叭可进行全服喊话哦~~";
        }
        else if (i == 2)
        {
            str = "系统：关注微信公众号：“星焱娱乐”，即送丰厚大礼噢~";
        }
        else if (i == 3)
        {
            str = "系统：弹框界面点击屏幕空白处返回";
        }

        return str;
    }
}