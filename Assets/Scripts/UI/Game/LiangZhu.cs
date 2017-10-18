using System.Collections;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

public class LiangZhu : MonoBehaviour
{
    public GameScript m_parentScript;

    public Button ButtonWang;
    public Button ButtonHei;
    public Button ButtonHong;
    public Button ButtonMei;
    public Button ButtonFang;
    private List<PokerInfo> liangzhuPoker;

    public static GameObject create(GameScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/Game/LiangZhu") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

        obj.GetComponent<LiangZhu>().m_parentScript = parentScript;

        return obj;
    }

    public void UpdateUi(List<PokerInfo> handerPoker, List<PokerInfo> lastLiangZhuPoker)
    {
        liangzhuPoker = PlayRuleUtil.GetLiangzhuPoker(handerPoker, lastLiangZhuPoker, GameScript.m_levelPokerNum, GameScript.m_masterPokerType);

        foreach (PokerInfo pokerInfo in liangzhuPoker)
        {
            switch (pokerInfo.m_pokerType)
            {
                case Consts.PokerType.PokerType_FangKuai:
                    if (!ButtonFang.interactable)
                    {
                        ButtonFang.interactable = true;
                    }
                    break;
                case Consts.PokerType.PokerType_MeiHua:
                    if (!ButtonMei.interactable)
                    {
                        ButtonMei.interactable = true;
                    }
                    break;
                case Consts.PokerType.PokerType_HongTao:
                    if (!ButtonHong.interactable)
                    {
                        ButtonHong.interactable = true;
                    }
                    break;
                case Consts.PokerType.PokerType_HeiTao:
                    if (!ButtonHei.interactable)
                    {
                        ButtonHei.interactable = true;
                    }
                    break;
                case Consts.PokerType.PokerType_Wang:
                    if (!ButtonWang.interactable)
                    {
                        ButtonWang.interactable = true;
                    }
                    break;
            }
        }


        // m_parentScript.onClickQiangZhu(List<TLJCommon.PokerInfo> pokerList)
    }
}