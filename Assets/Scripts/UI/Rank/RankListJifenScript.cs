using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankListJifenScript : MonoBehaviour
{
    private UIWarpContent uiWarpContent;
    private List<GoldRankItemData> _GoldRankList;
    public GameObject Content;
    public GameObject Item;
    public Text JifenRank;
    public Text JifenCount;
    public static RankListJifenScript Instance;
    private static string myGoldRank;

    void Start()
    {
        Instance = this;
    }

    public void InitData()
    {
        _GoldRankList = RankData.goldRankDataList;
    }

    public void InitUI()
    {
        RectTransform ContentRect = Content.GetComponent<RectTransform>();
        RectTransform ItemRect = Item.GetComponent<RectTransform>();
        Vector2 itemRectSizeDelta = ItemRect.sizeDelta;
        ContentRect.sizeDelta = new Vector2(0, itemRectSizeDelta.y * _GoldRankList.Count);
        for (int i = 0; i < _GoldRankList.Count; i++)
        {
            GameObject goChild = GameObject.Instantiate(Item, Content.transform);
            GoldRankItemData goldRankItemData = _GoldRankList[i];

            if (UserData.name.Equals(goldRankItemData.name))
            {
                myGoldRank = i + 1 + "";
                goChild.GetComponent<Image>().color = new Color(242/(float)255, 228 / (float)255, 66 / (float)255, 1);
            }

            goChild.layer = Content.layer;
            var Text_Ranking = goChild.transform.Find("Text_Ranking");
            Text RankText = Text_Ranking.GetComponent<Text>();
            var Image_Head = goChild.transform.Find("Image_Head");
            var Name = goChild.transform.Find("Name");
            var Count = goChild.transform.Find("Count");
            var Ranking = goChild.transform.Find("Ranking");
            var Image_icon = goChild.transform.Find("Image_icon");
            Image rankImage = Ranking.GetComponent<Image>();

            Count.GetComponent<Text>().text = "" + goldRankItemData.gold;
            Image_Head.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Head/head_" + goldRankItemData.head);
            Name.GetComponent<Text>().text = goldRankItemData.name;

            Image_icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/icon_jinbi");

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

        if (myGoldRank == null)
        {
            JifenRank.text = "未上榜";
        }
        else
        {
            JifenRank.text = "我的排名:" + myGoldRank;
        }

        JifenCount.text = "我的金币:" + UserData.gold;
    }


    public void OnClickGold(bool flag)
    {
        
    }
}