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

    public static bool s_isShowBiSaiChang = false;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/Panel_PVP_Choice") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    public static GameObject create(bool isShowBiSaiChang)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/Panel_PVP_Choice") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        s_isShowBiSaiChang = isShowBiSaiChang;

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        OtherData.s_pvpChoiceScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPChoiceScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPChoiceScript_hotfix", "Start", null, null);
            return;
        }

        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        if (s_isShowBiSaiChang)
        {
            showHuaFeiChang();
            showMyBaoMingFei(false);
        }
        else
        {
            showJinBiChang();
            showMyBaoMingFei(true);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void onClickJinBiChang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPChoiceScript_hotfix", "onClickJinBiChang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPChoiceScript_hotfix", "onClickJinBiChang", null, null);
            return;
        }

        if (m_curShowTiaoZhanSai)
        {
            return;
        }

        showMyBaoMingFei(true);
        showJinBiChang();
    }

    public void onClickHuaFeiChang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPChoiceScript_hotfix", "onClickHuaFeiChang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPChoiceScript_hotfix", "onClickHuaFeiChang", null, null);
            return;
        }

        if (!m_curShowTiaoZhanSai)
        {
            return;
        }

        showMyBaoMingFei(false);
        showHuaFeiChang();
    }

    public void showJinBiChang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPChoiceScript_hotfix", "showJinBiChang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPChoiceScript_hotfix", "showJinBiChang", null, null);
            return;
        }

        m_curShowTiaoZhanSai = true;

        m_ListViewScript.clear();

        {
            m_tab_bg.transform.localPosition = new Vector3(-83,0,0);
            CommonUtil.setImageSpriteByAssetBundle(m_button_tiaozhansai.transform.Find("Image").GetComponent<Image>(), "game.unity3d", "tiaozhansai_xuanze");
            CommonUtil.setImageSpriteByAssetBundle(m_button_huafeisai.transform.Find("Image").GetComponent<Image>(), "game.unity3d", "huafeisai_weixuanze");
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
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPChoiceScript_hotfix", "showHuaFeiChang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPChoiceScript_hotfix", "showHuaFeiChang", null, null);
            return;
        }

        m_curShowTiaoZhanSai = false;

        m_ListViewScript.clear();

        {
            m_tab_bg.transform.localPosition = new Vector3(83, 0, 0);

            CommonUtil.setImageSpriteByAssetBundle(m_button_tiaozhansai.transform.Find("Image").GetComponent<Image>(), "game.unity3d", "tiaozhansai_weixuanze");
            CommonUtil.setImageSpriteByAssetBundle(m_button_huafeisai.transform.Find("Image").GetComponent<Image>(), "game.unity3d", "huafeisai_xuanze");
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

    public void showMyBaoMingFei(bool isGold)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPChoiceScript_hotfix", "showMyBaoMingFei"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPChoiceScript_hotfix", "showMyBaoMingFei", null, isGold);
            return;
        }

        {
            int num = 0;

            if (isGold)
            {
                num = UserData.gold;
            }
            else
            {
                for (int i = 0; i < UserData.propData.Count; i++)
                {
                    if (UserData.propData[i].prop_id == (int)TLJCommon.Consts.Prop.Prop_lanzuanshi)
                    {
                        num = UserData.propData[i].prop_num;
                        break;
                    }
                }
            }

            if (num > 99999)
            {
                num = num / 10000;
                transform.Find("Image_bg/mybaomingfei").GetComponent<Text>().text = "我的     :" + num.ToString() + "万+";
            }
            else
            {
                transform.Find("Image_bg/mybaomingfei").GetComponent<Text>().text = "我的     :" + num.ToString();
            }

            if (isGold)
            {
                CommonUtil.setImageSprite(transform.Find("Image_bg/mybaomingfei/Image").GetComponent<Image>(), GameUtil.getPropIconPath(1));
            }
            else
            {
                CommonUtil.setImageSprite(transform.Find("Image_bg/mybaomingfei/Image").GetComponent<Image>(), GameUtil.getPropIconPath(107));
            }
        }
    }
}
