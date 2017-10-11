using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankListJifenScript : MonoBehaviour
{
    private UIWarpContent uiWarpContent;
    private List<string> _list;
    public GameObject Content;
    public GameObject Item;
    public Text JifenRank;
    public Text JifenCount;
    void Start()
    {
        InitData();
        InitUI();

        JifenRank.text = "我的排名:" + 1094;
        JifenCount.text = "我的积分:" +  10000;
    }

    private void InitData()
    {
        _list = new List<string>();
        for (int i = 0; i < 30; i++)
        {
            _list.Add(i + "");
        }
    }

    private void InitUI()
    {
        RectTransform ContentRect = Content.GetComponent<RectTransform>();
        RectTransform ItemRect = Item.GetComponent<RectTransform>();
        Vector2 itemRectSizeDelta = ItemRect.sizeDelta;
        ContentRect.sizeDelta = new Vector2(0, itemRectSizeDelta.y * _list.Count);
        for (int i = 0; i < _list.Count; i++)
        {
            GameObject goChild = GameObject.Instantiate(Item, Content.transform);
            goChild.layer = Content.layer;
            var Text_Ranking = goChild.transform.Find("Text_Ranking");
            Text RankText = Text_Ranking.GetComponent<Text>();
            var Image_Head = goChild.transform.Find("Image_Head");
            var Name = goChild.transform.Find("name");
            var Count = goChild.transform.Find("Count");
            var Ranking = goChild.transform.Find("Ranking");
            Image rankImage = Ranking.GetComponent<Image>();

            Count.GetComponent<Text>().text = "积分：" + i;
            Image_Head.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Head/head_1");
            if (i < 3)
            {
                Text_Ranking.gameObject.SetActive(false);
                Ranking.gameObject.SetActive(true);
                if (i == 1)
                {
                    rankImage.sprite = Resources.Load<Sprite>("Sprites/Main/award_2");
                }
                else if (i == 2)
                {
                    rankImage.sprite = Resources.Load<Sprite>("Sprites/Main/award_3");
                }
            }
            else
            {
                Text_Ranking.gameObject.SetActive(true);
                Ranking.gameObject.SetActive(false);
                RankText.text = i + 1 + "";
            }
        }
    }
}