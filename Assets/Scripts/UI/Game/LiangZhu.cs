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
    public GameScript m_parentScript;

    public Button ButtonWang;
    public Button ButtonHei;
    public Button ButtonHong;
    public Button ButtonMei;
    public Text textLiangzhu;
    public Button ButtonFang;
    public GameObject GiveUp;
    public List<PokerInfo> liangzhuPoker;
    public List<PokerInfo> handerPoker;

    public UseType m_useType;

    public static GameObject create(GameScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/Game/LiangZhu") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

        obj.GetComponent<LiangZhu>().m_parentScript = parentScript;

        return obj;
    }

    private void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LiangZhu", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LiangZhu", "Start", null, null);
            return;
        }
    }

    public void setUseType(UseType useType)
    {
        m_useType = useType;
    }

    public void UpdateUi(List<PokerInfo> handerPoker, List<PokerInfo> lastLiangZhuPoker)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LiangZhu", "UpdateUi"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LiangZhu", "UpdateUi", null, handerPoker, lastLiangZhuPoker);
            return;
        }

        this.handerPoker = handerPoker;
         
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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LiangZhu", "OnClickHei"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LiangZhu", "OnClickHei", null, null);
            return;
        }

        OnClickLiangzhu(GetPokerTypePoker(Consts.PokerType.PokerType_HeiTao));
        UpdateUi(handerPoker, GetPokerTypePoker(Consts.PokerType.PokerType_HeiTao));
    }
    public void OnClickhong()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LiangZhu", "OnClickhong"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LiangZhu", "OnClickhong", null, null);
            return;
        }

        OnClickLiangzhu(GetPokerTypePoker(Consts.PokerType.PokerType_HongTao));
        UpdateUi(handerPoker, GetPokerTypePoker(Consts.PokerType.PokerType_HongTao));
    }
    public void OnClickMei()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LiangZhu", "OnClickMei"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LiangZhu", "OnClickMei", null, null);
            return;
        }

        OnClickLiangzhu(GetPokerTypePoker(Consts.PokerType.PokerType_MeiHua));
        UpdateUi(handerPoker, GetPokerTypePoker(Consts.PokerType.PokerType_MeiHua));
    }
    public void OnClickFang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LiangZhu", "OnClickFang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LiangZhu", "OnClickFang", null, null);
            return;
        }

        OnClickLiangzhu(GetPokerTypePoker(Consts.PokerType.PokerType_FangKuai));
        UpdateUi(handerPoker, GetPokerTypePoker(Consts.PokerType.PokerType_FangKuai));
    }
    public void OnClickWang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LiangZhu", "OnClickWang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LiangZhu", "OnClickWang", null, null);
            return;
        }

        OnClickLiangzhu(GetPokerTypePoker(Consts.PokerType.PokerType_Wang));
        UpdateUi(handerPoker,GetPokerTypePoker(Consts.PokerType.PokerType_Wang));
    }

    public void OnClickGiveUp()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LiangZhu", "OnClickGiveUp"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LiangZhu", "OnClickGiveUp", null, null);
            return;
        }

        m_parentScript.onClickChaoDi(new List<PokerInfo>());
    }

    private void OnClickLiangzhu(List<PokerInfo> list)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LiangZhu", "OnClickLiangzhu"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LiangZhu", "OnClickLiangzhu", null, list);
            return;
        }
        
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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LiangZhu", "GetPokerTypePoker"))
        {
            List<PokerInfo> list = (List<PokerInfo>)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LiangZhu", "GetPokerTypePoker", null, pokerType);
            return list;
        }

        List<PokerInfo> pokerInfos = new List<PokerInfo>();
        for (int i = 0; i < liangzhuPoker.Count; i++)
        {
            if (liangzhuPoker[i].m_pokerType == pokerType)
            {
                pokerInfos.Add(liangzhuPoker[i]);
            }

            if (pokerInfos.Count == 2) break;
        }
        return pokerInfos;
    }
}