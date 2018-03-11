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
    public GameObject MyRank;
    public string mymedalRank;
    public static List<MedalRankItemData> _medalRankItemDatas;

    public static RankListCaifuScript Instance;

    void Start()
    {
        OtherData.s_rankListCaifuScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("RankListCaifuScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.RankListCaifuScript_hotfix", "Start", null, null);
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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("RankListCaifuScript_hotfix", "InitUI"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.RankListCaifuScript_hotfix", "InitUI", null, null);
            return;
        }

        //        mymedalRank = null;
        RectTransform ContentRect = Content.GetComponent<RectTransform>();
        RectTransform ItemRect = Item.GetComponent<RectTransform>();
        Vector2 itemRectSizeDelta = ItemRect.sizeDelta;

        //自己是否上榜
        for (int i = 0; i < _medalRankItemDatas.Count; i++)
        {
            if (_medalRankItemDatas[i].name.Equals(UserData.name))
            {
                mymedalRank = i + 1 + "";
            }
        }

        float f;
        if (string.IsNullOrEmpty(mymedalRank))
        {
            f = itemRectSizeDelta.y * _medalRankItemDatas.Count;
        }
        else
        {
            LogUtil.Log("----1");
            f = itemRectSizeDelta.y * (_medalRankItemDatas.Count - 1);
        }
        ContentRect.sizeDelta = new Vector2(0, f);
        for (int i = 0; i < _medalRankItemDatas.Count; i++)
        {
            MedalRankItemData medalRankItemData = _medalRankItemDatas[i];

            if (medalRankItemData.name.Equals(UserData.name))
            {
                continue;
            }

            GameObject goChild = GameObject.Instantiate(Item, Content.transform);

            if (medalRankItemData.name.Equals(UserData.name))
            {
                mymedalRank = i + 1 + "";
                //                goChild.GetComponent<Image>().color = new Color(255 / (float)255, 255 / (float)255, 0 / (float)255, 1);
                CommonUtil.setImageSpriteByAssetBundle(goChild.GetComponent<Image>(), "main.unity3d", "di7");
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
            CommonUtil.setImageSpriteByAssetBundle(Image_vip, "vip.unity3d", "user_vip_" + VipUtil.GetVipLevel(medalRankItemData.recharge));
            Count.GetComponent<Text>().text = "" + medalRankItemData.medal;
            //Image_Head.GetComponent<Image>().sprite =Resources.Load<Sprite>("Sprites/Head/head_" + medalRankItemData.head);
            goChild.GetComponent<HeadIconScript>().setIcon("head_" + medalRankItemData.head);
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
                    CommonUtil.setImageSpriteByAssetBundle(rankImage, "main.unity3d", "award_2");
                }
                else if (i == 2)
                {
                    CommonUtil.setImageSpriteByAssetBundle(rankImage, "main.unity3d", "award_3");
                }
            }
            else
            {
                Text_Ranking.gameObject.SetActive(true);
                Ranking.gameObject.SetActive(false);
                RankText.text = i + 1 + "";
            }
        }
       
        if (string.IsNullOrEmpty(mymedalRank))
        {
            LogUtil.Log("未上榜");
            CaifuRank.text = "我的排名:未上榜";
        }
        else
        {
            CaifuRank.text = "我的排名:" + mymedalRank;
        }

        CaifuCount.text = "我的徽章:" + UserData.medal;

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
        CommonUtil.setImageSpriteByAssetBundle(Image_vip, "vip.unity3d", "user_vip_" + VipUtil.GetVipLevel(UserData.rechargeVip));
        Image rankImage = Ranking.GetComponent<Image>();

        Count.GetComponent<Text>().text = "" + UserData.medal;
        string s = "Sprites/Head/head_" + UserData.head;
        LogUtil.Log("head" + s);
        Image_Head.GetComponent<Image>().sprite = AssetBundlesManager.getInstance().getAssetBundlesDataByName("head.unity3d").LoadAsset<Sprite>(UserData.head);
        Name.GetComponent<Text>().text = UserData.name;
        if (VipUtil.GetVipLevel(UserData.rechargeVip) > 0)
        {
            Name.GetComponent<Text>().color = new Color(253 / (float)255, 239 / (float)255, 82 / (float)255, 1);
        }
        
        CommonUtil.setImageSpriteByAssetBundle(MyRank.GetComponent<Image>(), "main.unity3d", "di7");

        Image_icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/icon_jinbi");

        if (string.IsNullOrEmpty(mymedalRank))
        {
            Text_Ranking.gameObject.SetActive(true);
            Ranking.gameObject.SetActive(false);
            RankText.text = "无";
        }
        else
        {
            int i = int.Parse(mymedalRank) - 1;
            if (i < 3)
            {
                Text_Ranking.gameObject.SetActive(false);
                Ranking.gameObject.SetActive(true);
                if (i == 1)
                {
                    CommonUtil.setImageSpriteByAssetBundle(rankImage, "main.unity3d", "award_2");
                }
                else if (i == 2)
                {
                    CommonUtil.setImageSpriteByAssetBundle(rankImage, "main.unity3d", "award_3");
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