using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalDuiHuanPanelScript : MonoBehaviour
{
    public enum CurShowTab
    {
        CurShowTab_duihuanshangpin,
        CurShowTab_duihuanjilu,
    }

    public CurShowTab m_curShowTab = CurShowTab.CurShowTab_duihuanshangpin;

    public Button m_btn_duihuanshangpin;
    public Button m_btn_duihuanjilu;

    public GameObject m_tab_duihuanshangpin;
    public GameObject m_tab_duihuanjilu;

    public Text m_text_myMedalNum;

    public GameObject m_obj_duihuanshangpin;
    public GameObject m_obj_duihuanjilu;

    public ListViewScript m_listView_duihuanshangpin;
    public ListViewScript m_listView_duihuanjilu;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/MedalDuiHuanPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start()
    {
        OtherData.s_medalDuiHuanPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanPanelScript_hotfix", "Start", null, null);
            return;
        }

        m_listView_duihuanshangpin = m_obj_duihuanshangpin.GetComponent<ListViewScript>();
        m_listView_duihuanjilu = m_obj_duihuanjilu.GetComponent<ListViewScript>();

        m_text_myMedalNum.text = UserData.medal.ToString();

        NetLoading.getInstance().Show();

        LogicEnginerScript.Instance.GetComponent<GetMedalDuiHuanRewardRequest>().CallBack = onCallBackGetMedalDuiHuanReward;
        LogicEnginerScript.Instance.GetComponent<GetMedalDuiHuanRewardRequest>().OnRequest();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onCallBackGetMedalDuiHuanReward(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanPanelScript_hotfix", "onCallBackGetMedalDuiHuanReward"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanPanelScript_hotfix", "onCallBackGetMedalDuiHuanReward", null, data);
            return;
        }

        NetLoading.getInstance().Close();

        MedalDuiHuanRewardData.getInstance().initJson(data);

        LogicEnginerScript.Instance.GetComponent<GetMedalDuiHuanRecordRequest>().CallBack = onCallBackGetMedalDuiHuanRecord;
        LogicEnginerScript.Instance.GetComponent<GetMedalDuiHuanRecordRequest>().OnRequest();
    }

    public void onCallBackGetMedalDuiHuanRecord(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanPanelScript_hotfix", "onCallBackGetMedalDuiHuanRecord"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanPanelScript_hotfix", "onCallBackGetMedalDuiHuanRecord", null, data);
            return;
        }

        MedalDuiHuanRecordData.getInstance().initJson(data);

        showTab(m_curShowTab);
    }

    public void showTab(CurShowTab m_curShowTab)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanPanelScript_hotfix", "showTab"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanPanelScript_hotfix", "showTab", null, m_curShowTab);
            return;
        }

        switch (m_curShowTab)
        {
            case CurShowTab.CurShowTab_duihuanshangpin:
                {
                    GameUtil.showGameObject(m_tab_duihuanshangpin);
                    GameUtil.hideGameObject(m_tab_duihuanjilu);

                    {
                        CommonUtil.setImageSprite(m_btn_duihuanshangpin.GetComponent<Image>(), "Sprites/Common/tab_xuanze");
                        CommonUtil.setImageSprite(m_btn_duihuanjilu.GetComponent<Image>(), "Sprites/Common/tab_weixuanze");

                        m_btn_duihuanshangpin.GetComponent<Image>().SetNativeSize();
                        m_btn_duihuanjilu.GetComponent<Image>().SetNativeSize();

                        CommonUtil.setImageSprite(m_btn_duihuanshangpin.transform.Find("Image").GetComponent<Image>(), "Sprites/MedalDuiHuan/dhsp");
                        CommonUtil.setImageSprite(m_btn_duihuanjilu.transform.Find("Image").GetComponent<Image>(), "Sprites/MedalDuiHuan/dhjl");
                    }

                    loadDuiHuanShangPin();
                }
                break;

            case CurShowTab.CurShowTab_duihuanjilu:
                {
                    GameUtil.showGameObject(m_tab_duihuanjilu);
                    GameUtil.hideGameObject(m_tab_duihuanshangpin);

                    {
                        CommonUtil.setImageSprite(m_btn_duihuanshangpin.GetComponent<Image>(), "Sprites/Common/tab_weixuanze");
                        CommonUtil.setImageSprite(m_btn_duihuanjilu.GetComponent<Image>(), "Sprites/Common/tab_xuanze");

                        m_btn_duihuanshangpin.GetComponent<Image>().SetNativeSize();
                        m_btn_duihuanjilu.GetComponent<Image>().SetNativeSize();

                        CommonUtil.setImageSprite(m_btn_duihuanshangpin.transform.Find("Image").GetComponent<Image>(), "Sprites/MedalDuiHuan/dhsp02");
                        CommonUtil.setImageSprite(m_btn_duihuanjilu.transform.Find("Image").GetComponent<Image>(), "Sprites/MedalDuiHuan/dhjl02");
                    }

                    loadDuiHuanJiLu();
                }
                break;
        }
    }

    public void onClickDuiHuanShangPin()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanPanelScript_hotfix", "onClickDuiHuanShangPin"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanPanelScript_hotfix", "onClickDuiHuanShangPin", null, null);
            return;
        }

        if (m_curShowTab != CurShowTab.CurShowTab_duihuanshangpin)
        {
            m_curShowTab = CurShowTab.CurShowTab_duihuanshangpin;
            showTab(m_curShowTab);
        }
    }

    public void onClickDuiHuanJiLu()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanPanelScript_hotfix", "onClickDuiHuanJiLu"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanPanelScript_hotfix", "onClickDuiHuanJiLu", null, null);
            return;
        }

        if (m_curShowTab != CurShowTab.CurShowTab_duihuanjilu)
        {
            m_curShowTab = CurShowTab.CurShowTab_duihuanjilu;
            showTab(m_curShowTab);
        }
    }

    public void loadDuiHuanShangPin()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanPanelScript_hotfix", "loadDuiHuanShangPin"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanPanelScript_hotfix", "loadDuiHuanShangPin", null, null);
            return;
        }

        m_listView_duihuanshangpin.clear();

        int count = MedalDuiHuanRewardData.getInstance().getDataList().Count;
        int rows = 0;
        if (count == 0)
        {
            rows = 0;
        }
        else if (count < 3)
        {
            rows = 1;
        }
        else
        {
            rows = count / 3;
            if ((count % 3) > 0)
            {
                ++rows;
            }
        }

        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            GameObject pre = Resources.Load("Prefabs/UI/Item/Item_medal_duihuan") as GameObject;
            GameObject obj = Instantiate(pre);

            for (int j = 0; j < 3; j++)
            {
                MedalDuiHuanRewardDataContent temp = MedalDuiHuanRewardData.getInstance().getMedalDuiHuanRewardDataContentByIndex(index++);

                Button btn = obj.transform.Find("Button_" + (j + 1)).GetComponent<Button>();
                if (temp != null)
                {
                    btn.onClick.AddListener(() => onClickItemMedalDuiHuan(btn));
                    
                    btn.transform.name = temp.goods_id.ToString();

                    // 图标
                    CommonUtil.setImageSprite(btn.transform.Find("Image").GetComponent<Image>(), GameUtil.getPropIconPath(GameUtil.getPropIdFromReward(temp.reward_prop)));

                    // 名字
                    btn.transform.Find("Image_btn/Text_price").GetComponent<Text>().text = temp.price.ToString();

                    // vip等级
                    CommonUtil.setImageSpriteByAssetBundle(btn.transform.Find("Image_vip").GetComponent<Image>(),"vip.unity3d", "user_vip_" + temp.vipLevel);
                }
                else
                {
                    GameUtil.hideGameObject(btn.gameObject);
                }
            }

            m_listView_duihuanshangpin.addItem(obj);
        }

        m_listView_duihuanshangpin.addItemEnd();
    }

    public void loadDuiHuanJiLu()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanPanelScript_hotfix", "loadDuiHuanJiLu"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanPanelScript_hotfix", "loadDuiHuanJiLu", null, null);
            return;
        }

        m_listView_duihuanjilu.clear();

        int count = MedalDuiHuanRecordData.getInstance().getDataList().Count;
        for (int i = 0; i < count; i++)
        {
            GameObject pre = Resources.Load("Prefabs/UI/Item/Item_medalDuiHuanJilu") as GameObject;
            GameObject obj = Instantiate(pre);

            MedalDuiHuanRecordDataContent temp = MedalDuiHuanRecordData.getInstance().getDataList()[i];
            if (temp != null)
            {
                MedalDuiHuanRewardDataContent temp2 = MedalDuiHuanRewardData.getInstance().getMedalDuiHuanRewardDataContentById(temp.goods_id);
                obj.transform.Find("Text_name").GetComponent<Text>().text = (temp2.name + "*" + temp.num.ToString());
                obj.transform.Find("Text_time").GetComponent<Text>().text = temp.time;
            }

            m_listView_duihuanjilu.addItem(obj);
        }

        m_listView_duihuanjilu.addItemEnd();
    }

    public void onClickItemMedalDuiHuan(Button btn)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanPanelScript_hotfix", "onClickItemMedalDuiHuan"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanPanelScript_hotfix", "onClickItemMedalDuiHuan", null, btn);
            return;
        }

        int goods_id = int.Parse(btn.transform.name);
        MedalDuiHuanQueRenPanelScript.create(goods_id);
    }

    public void OnClickClose()
    {
        Destroy(gameObject);

        AudioScript.getAudioScript().playSound_LayerClose();

        EnterMainPanelShowManager.getInstance().showNextPanel();
    }
}