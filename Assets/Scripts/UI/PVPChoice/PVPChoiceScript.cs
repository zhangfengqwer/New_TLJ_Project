using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVPChoiceScript : MonoBehaviour {

    public GameObject m_listView;
    ListViewScript m_ListViewScript;

    public Button m_button_tiaozhansai;
    public Button m_button_huafeisai;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/Panel_PVP_Choice") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab, GameObject.Find("Canvas").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        m_ListViewScript = m_listView.GetComponent<ListViewScript>();
        showJinBiChang();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void onClickJinBiChang()
    {
        showJinBiChang();
    }

    public void onClickHuaFeiChang()
    {
        showHuaFeiChang();
    }

    public void showJinBiChang()
    {
        m_ListViewScript.clear();

        {
            CommonUtil.setImageSprite(m_button_tiaozhansai.GetComponent<Image>(), "Sprites/Game/ChoiceChangCi/yeqie_xuanze");
            CommonUtil.setImageSprite(m_button_tiaozhansai.transform.Find("Image").GetComponent<Image>(), "Sprites/Game/ChoiceChangCi/zi_tzs02");
            m_button_tiaozhansai.GetComponent<Image>().SetNativeSize();

            CommonUtil.setImageSprite(m_button_huafeisai.GetComponent<Image>(), "Sprites/Game/ChoiceChangCi/yeqie_weixuan");
            CommonUtil.setImageSprite(m_button_huafeisai.transform.Find("Image").GetComponent<Image>(), "Sprites/Game/ChoiceChangCi/zi_hf01");
            m_button_huafeisai.GetComponent<Image>().SetNativeSize();
        }

        for (int i = 0; i < 10; i++)
        {
            GameObject prefab = Resources.Load("Prefabs/UI/Item/PVP_List_Item") as GameObject;
            GameObject obj = MonoBehaviour.Instantiate(prefab);
            obj.GetComponent<PVP_List_Item_Script>().setChangCi("金币场一万（抄底）");
            obj.GetComponent<PVP_List_Item_Script>().setKaiSaiRenShu("满" + (i + 1) + "人开赛");
            obj.GetComponent<PVP_List_Item_Script>().setBaoMingFei("报名费 金币*" + (i + 1) * 1000);
            obj.GetComponent<PVP_List_Item_Script>().setBaoMingRenShu("已报名人数：" + i);


            //GameObject temp = new GameObject();
            //temp.AddComponent<Image>();
            //temp.GetComponent<Image>().GetComponent<RectTransform>().sizeDelta = new Vector2(300,50);

            m_ListViewScript.addItem(obj);
        }

        m_ListViewScript.addItemEnd();
    }

    public void showHuaFeiChang()
    {
        m_ListViewScript.clear();

        {
            CommonUtil.setImageSprite(m_button_tiaozhansai.GetComponent<Image>(), "Sprites/Game/ChoiceChangCi/yeqie_weixuan");
            CommonUtil.setImageSprite(m_button_tiaozhansai.transform.Find("Image").GetComponent<Image>(), "Sprites/Game/ChoiceChangCi/zi_tzs01");
            m_button_tiaozhansai.GetComponent<Image>().SetNativeSize();

            CommonUtil.setImageSprite(m_button_huafeisai.GetComponent<Image>(), "Sprites/Game/ChoiceChangCi/yeqie_xuanze");
            CommonUtil.setImageSprite(m_button_huafeisai.transform.Find("Image").GetComponent<Image>(), "Sprites/Game/ChoiceChangCi/zi_hf02");
            m_button_huafeisai.GetComponent<Image>().SetNativeSize();
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject prefab = Resources.Load("Prefabs/UI/Item/PVP_List_Item") as GameObject;
            GameObject obj = MonoBehaviour.Instantiate(prefab);
            obj.GetComponent<PVP_List_Item_Script>().setChangCi("话费" + (i+1) + "元（抄底）");
            obj.GetComponent<PVP_List_Item_Script>().setKaiSaiRenShu("满" + (i + 1) + "人开赛");
            obj.GetComponent<PVP_List_Item_Script>().setBaoMingFei("报名费 蓝钻石*" + (i + 1));
            obj.GetComponent<PVP_List_Item_Script>().setBaoMingRenShu("已报名人数：" + i);

            m_ListViewScript.addItem(obj);
        }

        m_ListViewScript.addItemEnd();
    }
}
