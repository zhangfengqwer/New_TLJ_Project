using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVPChoiceScript : MonoBehaviour {

    public GameObject m_listView;
    ListViewScript m_ListViewScript;

    public Image m_image_tab_xuanze;
    //public Button m_button_tiaozhansai;
    //public Button m_button_huafeisai;

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
            m_image_tab_xuanze.transform.localPosition = new Vector3(-100,0,0);
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
            m_image_tab_xuanze.transform.localPosition = new Vector3(100, 0, 0);
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
}
