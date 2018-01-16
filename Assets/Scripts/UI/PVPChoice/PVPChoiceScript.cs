using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVPChoiceScript : MonoBehaviour {

    public GameObject m_listView;
    public ListViewScript m_ListViewScript;

    public Image m_tab_bg;
    public Button m_button_tiaozhansai;
    public Button m_button_huafeisai;
    public bool m_curShowTiaoZhanSai = true;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/Panel_PVP_Choice") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        OtherData.s_pvpChoiceScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPChoiceScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPChoiceScript", "Start", null, null);
            return;
        }

        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        showJinBiChang();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void onClickJinBiChang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPChoiceScript", "onClickJinBiChang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPChoiceScript", "onClickJinBiChang", null, null);
            return;
        }

        if (m_curShowTiaoZhanSai)
        {
            return;
        }

        showJinBiChang();
    }

    public void onClickHuaFeiChang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPChoiceScript", "onClickHuaFeiChang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPChoiceScript", "onClickHuaFeiChang", null, null);
            return;
        }

        if (!m_curShowTiaoZhanSai)
        {
            return;
        }

        showHuaFeiChang();
    }

    public void showJinBiChang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPChoiceScript", "showJinBiChang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPChoiceScript", "showJinBiChang", null, null);
            return;
        }

        m_curShowTiaoZhanSai = true;

        m_ListViewScript.clear();

        {
            m_tab_bg.transform.localPosition = new Vector3(-83,0,0);
            m_button_tiaozhansai.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/ChoiceChangCi/tiaozhansai_xuanze", typeof(Sprite)) as Sprite;
            m_button_huafeisai.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/ChoiceChangCi/huafeisai_weixuanze", typeof(Sprite)) as Sprite;
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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPChoiceScript", "showHuaFeiChang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPChoiceScript", "showHuaFeiChang", null, null);
            return;
        }

        m_curShowTiaoZhanSai = false;

        m_ListViewScript.clear();

        {
            m_tab_bg.transform.localPosition = new Vector3(83, 0, 0);
            m_button_tiaozhansai.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/ChoiceChangCi/tiaozhansai_weixuanze", typeof(Sprite)) as Sprite;
            m_button_huafeisai.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/ChoiceChangCi/huafeisai_xuanze", typeof(Sprite)) as Sprite;
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
