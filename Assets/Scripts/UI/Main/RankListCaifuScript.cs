using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankListCaifuScript : MonoBehaviour
{
    private UIWarpContent uiWarpContent;
    private List<string> _list;
    public GameObject Content;
    public GameObject Item;
    public Text CaifuRank;
    public Text CaifuCount;
    void Start()
    {
        InitData();
        InitUI();

        CaifuRank.text = "我的排名:" + 12;
        CaifuCount.text = "我的财富:" + 50;
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
            var Name = goChild.transform.Find("Name");
            var Count = goChild.transform.Find("Count");
            var Ranking = goChild.transform.Find("Ranking");
            Image rankImage = Ranking.GetComponent<Image>();
            Count.GetComponent<Text>().text = "财富：" + i;

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

//        uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
//        uiWarpContent.onInitializeItem = onInitializeItem;
//        uiWarpContent.Init(_list.Count);
    }

//    private void onInitializeItem(GameObject go, int dataindex)
//    {
//        print(dataindex);
//        var ranking = go.transform.GetChild(0);
//        var headImage = go.transform.GetChild(1);
//        var name = go.transform.GetChild(2);
////        var textRank = go.transform.GetChild(3);
//        var component = headImage.GetComponent<Image>();
//        component.sprite = Resources.Load<Sprite>("Sprites/Head/head_1");
//        var count = name.GetChild(0);
//        Text text = ranking.GetComponent<Text>();
//        text.text = dataindex + 3 + "";
//        if (dataindex == 0)
//        {
//            image.sprite = Resources.Load<Sprite>("Sprites/Shengji/images/award_1");
//            textRank.gameObject.active = false;
//        }
//        else if (dataindex == 1)
//        {
//            image.sprite = Resources.Load<Sprite>("Sprites/Shengji/images/award_2");
//            textRank.gameObject.active = false;
//        }
//        else if (dataindex == 2)
//        {
//            image.sprite = Resources.Load<Sprite>("Sprites/Shengji/images/award_3");
//            textRank.gameObject.active = false;
//        }
//        else
//        {
//           
//        }
//    }
}