using System;
using System.Collections.Generic;
using System.Linq;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

public class RememberPokerHelper : MonoBehaviour {
    public GameObject HeiTaoContent;
    public GameObject HongContent;
    public GameObject MeiContent;
    public GameObject FangContent;
    public GameObject ItemPoker;
    private Vector3 startPosition;

    private static Dictionary<Consts.PokerType, List<PokerInfo>> dicPokerData = new Dictionary<Consts.PokerType, List<PokerInfo>>();
    private static Dictionary<Consts.PokerType, List<GameObject>> dictionaryGo = new Dictionary<Consts.PokerType, List<GameObject>>();

    public GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/Game/RememberPokerCanvas") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);
        return obj;
    }

    // Use this for initialization
    void Start () {
		Init(HeiTaoContent,Consts.PokerType.PokerType_HeiTao);
		Init(HongContent, Consts.PokerType.PokerType_HongTao);
		Init(MeiContent, Consts.PokerType.PokerType_MeiHua);
		Init(FangContent, Consts.PokerType.PokerType_FangKuai);

        this.transform.localPosition = new Vector3(0, 0, 0);
        startPosition = this.transform.localPosition;
        print(startPosition);

//        List<PokerInfo> pokerInfos = new List<PokerInfo>();
//        pokerInfos.Add(new PokerInfo(2, Consts.PokerType.PokerType_FangKuai));
//        UpdateUi(pokerInfos);
    }

    private void Init(GameObject gameObject, Consts.PokerType pokerType)
    {
        //初始化数据
        List<PokerInfo> heiTaoPokers = new List<PokerInfo>();
        List<GameObject> PokerItems = new List<GameObject>();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 2; j < 15; j++)
            {
                heiTaoPokers.Add(new PokerInfo(j, pokerType));
            }
        }
        heiTaoPokers = heiTaoPokers.OrderByDescending(a => a.m_num).ToList();
        dicPokerData.Add(pokerType, heiTaoPokers);
        //初始化UI
        for (int i = 0; i < heiTaoPokers.Count; i++)
        {
            GameObject go = GameObject.Instantiate(ItemPoker, gameObject.transform);
            PokerItems.Add(go);
            string temp = null;
            switch (pokerType)
            {
                case Consts.PokerType.PokerType_FangKuai:
                    temp = "icon_fangkuai";
                    break;
                case Consts.PokerType.PokerType_HeiTao:
                    temp = "icon_heitao";
                    break;
                case Consts.PokerType.PokerType_HongTao:
                    temp = "icon_hongtao";
                    break;
                case Consts.PokerType.PokerType_MeiHua:
                    temp = "icon_meihua";
                    break;
            }
            go.transform.Find("Type").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/Poker/"+temp, typeof(Sprite)) as Sprite;
            PokerInfo poker = heiTaoPokers[i];
            int num = poker.m_num;
            if (num >= 2 && num <= 10)
            {
                go.GetComponentInChildren<Text>().text = num.ToString();
            }
            else
            {
                if (num == 11)
                {
                    go.GetComponentInChildren<Text>().text = "J";
                }
                else if (num == 12)
                {
                    go.GetComponentInChildren<Text>().text = "Q";
                }
                else if (num == 13)
                {
                    go.GetComponentInChildren<Text>().text = "K";
                }
                else if (num == 14)
                {
                    go.GetComponentInChildren<Text>().text = "A";
                }
            }
        }
        dictionaryGo.Add(pokerType, PokerItems);
    }

    public void OnClickClose()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
    }

    public void OnClickShow()
    {
        this.transform.localPosition = startPosition;
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public static void UpdateUi(List<PokerInfo> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            PokerInfo pokerInfo = list[i];
            Consts.PokerType Type = pokerInfo.m_pokerType;

            List<PokerInfo> listPoker;
            dicPokerData.TryGetValue(Type, out listPoker);
            List<GameObject> listGo;
            dictionaryGo.TryGetValue(Type, out listGo);
            int index = -1;
            for (int j = 0; j < listPoker.Count; j++)
            {
                var item = listPoker[j];
                if (pokerInfo.m_num == item.m_num && pokerInfo.m_pokerType == item.m_pokerType)
                {
                    index = listPoker.IndexOf(item);
                    listPoker.Remove(item);
                    break;
                }
            }
            if (index == -1) return;
            try
            {
                GameObject go = listGo[index];
                go.GetComponent<Image>().color = Color.gray;
                listGo.Remove(go);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    GameObject o = null;
    public void OnClickJiPaiQi()
    {
        if (o == null)
        {
            o = create();
        }
        else
        {
            o.GetComponentInChildren<RememberPokerHelper>().OnClickShow();
        }
    }
}
