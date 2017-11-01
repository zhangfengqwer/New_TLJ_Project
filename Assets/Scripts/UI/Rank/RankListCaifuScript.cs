using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankListCaifuScript : MonoBehaviour
{
    private UIWarpContent uiWarpContent;
    public GameObject Content;
    public GameObject Item;
    public Text CaifuRank;
    public Text CaifuCount;
    private static string mymedalRank;
    private static List<MedalRankItemData> _medalRankItemDatas;

    public static RankListCaifuScript Instance;

    void Start()
    {
        Instance = this;
//        InitData();
//        InitUI();
    }

    public void InitData()
    {
        _medalRankItemDatas = RankData.medalRankDataList;
    }


    public void InitUI()
    {
        RectTransform ContentRect = Content.GetComponent<RectTransform>();
        RectTransform ItemRect = Item.GetComponent<RectTransform>();
        Vector2 itemRectSizeDelta = ItemRect.sizeDelta;
        ContentRect.sizeDelta = new Vector2(0, itemRectSizeDelta.y * _medalRankItemDatas.Count);
        for (int i = 0; i < _medalRankItemDatas.Count; i++)
        {
            GameObject goChild = GameObject.Instantiate(Item, Content.transform);
            MedalRankItemData medalRankItemData = _medalRankItemDatas[i];
            if (UserData.name.Equals(medalRankItemData.name))
            {
                mymedalRank = i + 1 + "";
                Color color = new Color
                {
                    a = 255,
                    r = 242,
                    g = 228,
                    b = 66
                };
                goChild.GetComponent<Image>().color =
                    new Color(24 / (float) 255, 171 / (float) 255, 201 / (float) 255, 1);
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

            Count.GetComponent<Text>().text = "" + medalRankItemData.medal;
            Image_Head.GetComponent<Image>().sprite =
                Resources.Load<Sprite>("Sprites/Head/head_" + medalRankItemData.head);
            Name.GetComponent<Text>().text = medalRankItemData.name;
            Image_icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/icon_huizhang");

            Image_icon.localScale = new Vector3((float) 1.2, (float) 1.2, 1);
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

        if (mymedalRank == null)
        {
            CaifuRank.text = "我的排名:未上榜";
        }
        else
        {
            CaifuRank.text = "我的排名:" + mymedalRank;
        }

        CaifuCount.text = "我的徽章:" + UserData.medal;
    }
}