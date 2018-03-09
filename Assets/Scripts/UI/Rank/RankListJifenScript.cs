using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankListJifenScript : MonoBehaviour
{
    public UIWarpContent uiWarpContent;
    public List<GoldRankItemData> _GoldRankList;
    public GameObject Content;
    public GameObject Item;
    public GameObject MyRank;
    private int myRankLevel;
    public Text JifenRank;
    public Text JifenCount;
    public static RankListJifenScript Instance;
    public string myGoldRank;


    void Start()
    {
        OtherData.s_rankListJifenScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("RankListJifenScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.RankListJifenScript_hotfix", "Start", null, null);
            return;
        }

        Instance = this;
    }

    public void InitData()
    {
        _GoldRankList = RankData.goldRankDataList;
    }

    public void InitUI()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("RankListJifenScript_hotfix", "InitUI"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.RankListJifenScript_hotfix", "InitUI", null, null);
            return;
        }

        RectTransform ContentRect = Content.GetComponent<RectTransform>();
        RectTransform ItemRect = Item.GetComponent<RectTransform>();
        Vector2 itemRectSizeDelta = ItemRect.sizeDelta;

        //自己是否上榜
        for (int i = 0; i < _GoldRankList.Count; i++)
        {
            if (_GoldRankList[i].name.Equals(UserData.name))
            {
                myGoldRank = i + 1 + "";
            }
        }

        float f;
        if (string.IsNullOrEmpty(myGoldRank))
        {
            f = itemRectSizeDelta.y * _GoldRankList.Count;
        }
        else
        {
            LogUtil.Log("----1");
            f = itemRectSizeDelta.y * (_GoldRankList.Count - 1);
        }
        ContentRect.sizeDelta = new Vector2(0, f);


        for (int i = 0; i < _GoldRankList.Count; i++)
        {
            GoldRankItemData goldRankItemData = _GoldRankList[i];

            if (goldRankItemData.name.Equals(UserData.name))
            {
                continue;
            }

            GameObject goChild = GameObject.Instantiate(Item, Content.transform);

            if (goldRankItemData.name.Equals(UserData.name))
            {
                myGoldRank = i + 1 + "";
//                goChild.GetComponent<Image>().color = new Color(255 / (float)255, 255 / (float)255, 0 / (float)255, 1);
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
            var Image_vip = goChild.transform.Find("Image_vip").GetComponent<Image>();
            Image_vip.sprite = Resources.Load<Sprite>("Sprites/Vip/user_vip_" + VipUtil.GetVipLevel(goldRankItemData.recharge));
            Image rankImage = Ranking.GetComponent<Image>();

            Count.GetComponent<Text>().text = "" + goldRankItemData.gold;
            Image_Head.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Head/head_" + goldRankItemData.head);
            Name.GetComponent<Text>().text = goldRankItemData.name;
            if (VipUtil.GetVipLevel(goldRankItemData.recharge) > 0)
            {
                Name.GetComponent<Text>().color = new Color(253 / (float)255, 239 / (float)255, 82 / (float)255, 1);
            }
            //GameUtil.setNickNameFontColor(Name.GetComponent<Text>(), VipUtil.GetVipLevel(goldRankItemData.recharge));

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

        if (string.IsNullOrEmpty(myGoldRank))
        {
            LogUtil.Log("未上榜");
            JifenRank.text = "我的排名:未上榜";
        }
        else
        {
            JifenRank.text = "我的排名:" + myGoldRank;
        }

        JifenCount.text = "我的金币:" + UserData.gold;

        InitMyRank();
    }

    private void InitMyRank()
    {
        var Text_Ranking = MyRank.transform.Find("Text_Ranking");
        Text RankText = Text_Ranking.GetComponent<Text>();
        var Image_Head = MyRank.transform.Find("Image_Head");
        var Name = MyRank.transform.Find("Name");
        var Count = MyRank.transform.Find("Count");
        var Ranking = MyRank.transform.Find("Ranking");
        var Image_icon = MyRank.transform.Find("Image_icon");
        var Image_vip = MyRank.transform.Find("Image_vip").GetComponent<Image>();
        Image_vip.sprite = Resources.Load<Sprite>("Sprites/Vip/user_vip_" + VipUtil.GetVipLevel(UserData.rechargeVip));
        Image rankImage = Ranking.GetComponent<Image>();

        Count.GetComponent<Text>().text = "" + UserData.gold;
        string s = "Sprites/Head/head_" + UserData.head;
        LogUtil.Log("head" + s);
        Image_Head.GetComponent<Image>().sprite = Resources.Load<Sprite>(UserData.head);
        Name.GetComponent<Text>().text = UserData.name;
        if (VipUtil.GetVipLevel(UserData.rechargeVip) > 0)
        {
            Name.GetComponent<Text>().color = new Color(253 / (float)255, 239 / (float)255, 82 / (float)255, 1);
        }

        MyRank.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Main/di7");

        Image_icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/icon_jinbi");

        if (string.IsNullOrEmpty(myGoldRank))
        {
            Text_Ranking.gameObject.SetActive(true);
            Ranking.gameObject.SetActive(false);
            RankText.text = "无";
        }
        else
        {
            int i = int.Parse(myGoldRank) - 1;
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


    public void OnClickGold(bool flag)
    {
        
    }
}