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
        GameObject obj = MonoBehaviour.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        // 拉取公告活动
        {
            LogicEnginerScript.Instance.GetComponent<GetPVPRoomRequest>().CallBack = onReceive_GetPVPRoom;
            LogicEnginerScript.Instance.GetComponent<GetPVPRoomRequest>().OnRequest();
        }
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

        for (int i = 0; i < PVPGameRoomDataScript.getInstance().getDataList().Count; i++)
        {
            List<string> list = new List<string>();
            CommonUtil.splitStr(PVPGameRoomDataScript.getInstance().getDataList()[i].gameroomtype,list,'_');

            if(list[1].CompareTo("JinBi") == 0)
            {
                GameObject prefab = Resources.Load("Prefabs/UI/Item/PVP_List_Item") as GameObject;
                GameObject obj = MonoBehaviour.Instantiate(prefab);

                obj.GetComponent<PVP_List_Item_Script>().setPVPGameRoomData(PVPGameRoomDataScript.getInstance().getDataList()[i]);
                m_ListViewScript.addItem(obj);
            }
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

        for (int i = 0; i < PVPGameRoomDataScript.getInstance().getDataList().Count; i++)
        {
            List<string> list = new List<string>();
            CommonUtil.splitStr(PVPGameRoomDataScript.getInstance().getDataList()[i].gameroomtype, list, '_');

            if (list[1].CompareTo("HuaFei") == 0)
            {
                GameObject prefab = Resources.Load("Prefabs/UI/Item/PVP_List_Item") as GameObject;
                GameObject obj = MonoBehaviour.Instantiate(prefab);

                obj.GetComponent<PVP_List_Item_Script>().setPVPGameRoomData(PVPGameRoomDataScript.getInstance().getDataList()[i]);
                m_ListViewScript.addItem(obj);
            }
        }

        m_ListViewScript.addItemEnd();
    }

    void onReceive_GetPVPRoom(string data)
    {
        PVPGameRoomDataScript.getInstance().initJson(data);

        showJinBiChang();
    }
}
