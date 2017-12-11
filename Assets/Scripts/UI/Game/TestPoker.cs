﻿using System;
using System.Collections.Generic;
using System.Linq;
using TLJCommon;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestPoker : MonoBehaviour {
    public GameObject HeiTaoContent;
    public GameObject HongContent;
    public GameObject MeiContent;
    public GameObject FangContent;
    public GameObject ItemPoker;
    public Image xiaoWang1;
    public Image xiaoWang2;
    public Image daWang1;
    public Image daWang2;
    public Text text;

    private Vector3 startPosition;
    private  List<Image> xiaoWangList = new List<Image>();
    private  List<Image> daWangList = new List<Image>();
    private  Dictionary<Consts.PokerType, List<PokerInfo>> dicPokerData = new Dictionary<Consts.PokerType, List<PokerInfo>>();
    private  Dictionary<Consts.PokerType, List<GameObject>> dictionaryGo = new Dictionary<Consts.PokerType, List<GameObject>>();
    private List<PokerInfo> selectedPokers = new List<PokerInfo>();


    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/Game/TestPoker") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        return obj;
    }

    // Use this for initialization
    public void Awake ()
    {
        InitFirst();
    }

    private void InitFirst()
    {
        Init(HeiTaoContent, Consts.PokerType.PokerType_HeiTao);
        Init(HongContent, Consts.PokerType.PokerType_HongTao);
        Init(MeiContent, Consts.PokerType.PokerType_MeiHua);
        Init(FangContent, Consts.PokerType.PokerType_FangKuai);

        xiaoWangList.Add(xiaoWang1);
        xiaoWangList.Add(xiaoWang2);
        daWangList.Add(daWang1);
        daWangList.Add(daWang2);
        var xiaopokerInfo = new PokerInfo(15, Consts.PokerType.PokerType_Wang);
        var dapokerInfo = new PokerInfo(16, Consts.PokerType.PokerType_Wang);

        for (int i = 0; i < xiaoWangList.Count; i++)
        {
            var xiaoWang = xiaoWangList[i];
            var dawang = daWangList[i];
            var btn = xiaoWang.gameObject.AddComponent<Button>();
            var image = xiaoWang.GetComponent<Image>();
            bool isClick = false;
            btn.onClick.AddListener(() =>
            {
                if (!isClick)
                {
                    image.color = Color.gray;
                    selectedPokers.Add(xiaopokerInfo);
                    isClick = true;
                    text.text = "当前选择:" + selectedPokers.Count + "张";
                }
                else
                {
                    image.color = Color.white;
                    selectedPokers.Remove(xiaopokerInfo);
                    isClick = false;
                    text.text = "当前选择:" + selectedPokers.Count + "张";
                }
            });

            var btn1 = dawang.gameObject.AddComponent<Button>();
            var image1 = dawang.GetComponent<Image>();
            bool isClick1 = false;
            btn1.onClick.AddListener(() =>
            {
                if (!isClick)
                {
                    image1.color = Color.gray;
                    selectedPokers.Add(dapokerInfo);
                    isClick1 = true;
                    text.text = "当前选择:" + selectedPokers.Count + "张";
                }
                else
                {
                    image.color = Color.white;
                    selectedPokers.Remove(dapokerInfo);
                    isClick1 = false;
                    text.text = "当前选择:" + selectedPokers.Count + "张";
                }
            });


        }

        this.transform.localPosition = new Vector3(0, 0, 0);
        startPosition = this.transform.localPosition;
    }

    public void Init(GameObject gameObject, Consts.PokerType pokerType)
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
        if (!dicPokerData.ContainsKey(pokerType))
        {
            dicPokerData.Add(pokerType, heiTaoPokers);
        }
       
        //初始化UI
        for (int i = 0; i < heiTaoPokers.Count; i++)
        {
            PokerInfo poker = heiTaoPokers[i];

            GameObject go = GameObject.Instantiate(ItemPoker, gameObject.transform);
            var btn = go.AddComponent<Button>();
            var image = go.GetComponent<Image>();
            bool isClick = false;
            btn.onClick.AddListener(() =>
            {
                if (!isClick)
                {
                    image.color = Color.gray;
                    selectedPokers.Add(poker);
                    isClick = true;
                    text.text = "当前选择:" + selectedPokers.Count + "张";
                }
                else
                {
                    image.color = Color.white;
                    selectedPokers.Remove(poker);
                    isClick = false;
                    text.text = "当前选择:" + selectedPokers.Count + "张";
                }
            });

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
        if (!dictionaryGo.ContainsKey(pokerType))
        {
            dictionaryGo.Add(pokerType, PokerItems);
        }
       
    }

    public void OnClickClose()
    {
        Destroy(this.gameObject);
    }

    public void OnClickConfirm()
    {
        if (selectedPokers.Count == 25)
        {
            foreach (var poker in selectedPokers)
            {
                LogUtil.Log(poker.m_pokerType + ":" + poker.m_num);
            }
        }
        else
        {
            ToastScript.createToast("牌不是25张");
        }
    }

    public void UpdateUi(List<PokerInfo> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            PokerInfo pokerInfo = list[i];
            //大小王
            if (pokerInfo.m_num == 15)
            {
                if (xiaoWangList.Count == 2 || xiaoWangList.Count == 1)
                {
                    xiaoWangList[0].color = Color.gray;
                    xiaoWangList.Remove(xiaoWangList[0]);
                }
            }else if (pokerInfo.m_num == 16)
            {
                if (daWangList.Count == 2 || daWangList.Count == 1)
                {
                    daWangList[0].color = Color.gray;
                    daWangList.Remove(daWangList[0]);
                }
            }
            else if (pokerInfo.m_num < 15 && pokerInfo.m_num >= 2)
            {
                Consts.PokerType Type = pokerInfo.m_pokerType;

                List<PokerInfo> listPoker;
                dicPokerData.TryGetValue(Type, out listPoker);
               
                List<GameObject> listGo;
                dictionaryGo.TryGetValue(Type, out listGo);

                int index = -1;
                if (listPoker != null)
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
                    if (listGo != null)
                    {
                        GameObject go = listGo[index];
                        go.GetComponent<Image>().color = Color.gray;
                        listGo.Remove(go);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                throw new Exception("牌的num异常");
            }
        }
    }
}
