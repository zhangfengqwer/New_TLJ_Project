using System.Collections;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

public class LiangZhu : MonoBehaviour
{
    public enum UseType
    {
        UseType_liangzhu,
        UseType_chaodi,
    }
    private GameScript m_parentScript;

    public Button ButtonWang;
    public Button ButtonHei;
    public Button ButtonHong;
    public Button ButtonMei;
    public Text textLiangzhu;
    public Button ButtonFang;
    public GameObject GiveUp;
    private List<PokerInfo> liangzhuPoker;

    public UseType m_useType;

    public static GameObject create(GameScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/Game/LiangZhu") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<LiangZhu>().m_parentScript = parentScript;

        return obj;
    }

    public void setUseType(UseType useType)
    {
        m_useType = useType;
    }

    public void UpdateUi(List<PokerInfo> handerPoker, List<PokerInfo> lastLiangZhuPoker)
    {
        ButtonFang.interactable = false;
        ButtonMei.interactable = false;
        ButtonHong.interactable = false;
        ButtonHei.interactable = false;
        ButtonWang.interactable = false;

        switch (m_useType)
        {
            case UseType.UseType_chaodi:
                GiveUp.transform.localScale = new Vector3(1, 1, 1);
                textLiangzhu.text = "抄底";
                break;
            case UseType.UseType_liangzhu:
                GiveUp.transform.localScale = new Vector3(0, 0, 0);
                textLiangzhu.text = "亮主";
                break;
        }


        liangzhuPoker = PlayRuleUtil.GetLiangzhuPoker(handerPoker, lastLiangZhuPoker, GameData.getInstance().m_levelPokerNum, GameData.getInstance().m_masterPokerType);

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
    }

    public void OnClickHei()
    {
        OnClickLiangzhu(GetPokerTypePoker(Consts.PokerType.PokerType_HeiTao));
    }
    public void OnClickhong()
    {
        OnClickLiangzhu(GetPokerTypePoker(Consts.PokerType.PokerType_HongTao));
    }
    public void OnClickMei()
    {
        OnClickLiangzhu(GetPokerTypePoker(Consts.PokerType.PokerType_MeiHua));
    }
    public void OnClickFang()
    {
        OnClickLiangzhu(GetPokerTypePoker(Consts.PokerType.PokerType_FangKuai));
    }
    public void OnClickWang()
    {
        OnClickLiangzhu(GetPokerTypePoker(Consts.PokerType.PokerType_Wang));
    }

    public void OnClickGiveUp()
    {
        m_parentScript.onClickChaoDi(new List<PokerInfo>());
    }

    private void OnClickLiangzhu(List<PokerInfo> list)
    {
        switch (m_useType)
        {
            case UseType.UseType_chaodi:
                m_parentScript.onClickChaoDi(list);
                break;
            case UseType.UseType_liangzhu:
                m_parentScript.onClickQiangZhu(list);
                break;
        }
    }

    private List<PokerInfo> GetPokerTypePoker(Consts.PokerType pokerType)
    {
        List<PokerInfo> pokerInfos = new List<PokerInfo>();
        for (int i = 0; i < liangzhuPoker.Count; i++)
        {
            if (liangzhuPoker[i].m_pokerType == pokerType)
            {
                pokerInfos.Add(liangzhuPoker[i]);
            }
        }
        return pokerInfos;
    }
}