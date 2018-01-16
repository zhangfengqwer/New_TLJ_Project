using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankListCaifuScript : MonoBehaviour
{
    public UIWarpContent uiWarpContent;
    public GameObject Content;
    public GameObject Item;
    public Text CaifuRank;
    public Text CaifuCount;
    public string mymedalRank;
    public static List<MedalRankItemData> _medalRankItemDatas;

    public static RankListCaifuScript Instance;

    void Start()
    {
        OtherData.s_rankListCaifuScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("RankListCaifuScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.RankListCaifuScript", "Start", null, null);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("RankListCaifuScript", "InitUI"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.RankListCaifuScript", "InitUI", null, null);
            return;
        }

        //        mymedalRank = null;
        RectTransform ContentRect = Content.GetComponent<RectTransform>();
        RectTransform ItemRect = Item.GetComponent<RectTransform>();
        Vector2 itemRectSizeDelta = ItemRect.sizeDelta;
        ContentRect.sizeDelta = new Vector2(0, itemRectSizeDelta.y * _medalRankItemDatas.Count);
        for (int i = 0; i < _medalRankItemDatas.Count; i++)
        {
            GameObject goChild = GameObject.Instantiate(Item, Content.transform);
            MedalRankItemData medalRankItemData = _medalRankItemDatas[i];
            if (medalRankItemData.name.Equals(UserData.name))
            {
                mymedalRank = i + 1 + "";
                //goChild.GetComponent<Image>().color = new Color(255 / (float)255, 255 / (float)255, 0 / (float)255, 1);
                goChild.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Main/di7");
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
            var Image_vip = goChild.transform.Find("Image_vip").GetComponent<Image>();
            Image_vip.sprite = Resources.Load<Sprite>("Sprites/Vip/user_vip_" + VipUtil.GetVipLevel(medalRankItemData.recharge));
            Count.GetComponent<Text>().text = "" + medalRankItemData.medal;
            Image_Head.GetComponent<Image>().sprite =
                Resources.Load<Sprite>("Sprites/Head/head_" + medalRankItemData.head);
            Name.GetComponent<Text>().text = medalRankItemData.name;
            if (VipUtil.GetVipLevel(medalRankItemData.recharge) > 0)
            {
                Name.GetComponent<Text>().color = new Color(253 / (float)255, 239 / (float)255, 82 / (float)255, 1);
            }

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