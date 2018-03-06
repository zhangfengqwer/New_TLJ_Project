using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TuiGuangYouLiPanelScript : MonoBehaviour {

    public enum CurShowTab
    {
        CurShowTab_wodetuiguang,
        CurShowTab_tuiguangguize,
        CurShowTab_bulingjiangli,
    }

    public CurShowTab m_curShowTab = CurShowTab.CurShowTab_wodetuiguang;

    public Button m_btn_myTuiGuang;
    public Button m_btn_tuigungguize;
    public Button m_btn_bulingjiangli;
    public Button m_btn_yijianlingqu;

    public Text m_text_myTuiGuangCode;

    public GameObject m_tab_wodetuiguang;
    public GameObject m_tab_tuiguangguize;
    public GameObject m_tab_bulingjiangli;

    public GameObject m_obj_tuiGuangList;
    public ListViewScript m_listview_player;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/TuiGuangYouLiPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        OtherData.s_tuiGuangYouLiPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TuiGuangYouLiPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TuiGuangYouLiPanelScript_hotfix", "Start", null, null);
            return;
        }

        m_text_myTuiGuangCode.text = "我的推广码：" + UserData.myTuiGuangCode;
        m_listview_player = m_obj_tuiGuangList.GetComponent<ListViewScript>();

        LogicEnginerScript.Instance.GetComponent<MyTuiGuangYouLiDataRequest>().CallBack = onCallBackMyTuiGuangYouLiData;
        LogicEnginerScript.Instance.GetComponent<MyTuiGuangYouLiDataRequest>().OnRequest();
    }

    public void onCallBackMyTuiGuangYouLiData(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TuiGuangYouLiPanelScript_hotfix", "onCallBackMyTuiGuangYouLiData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TuiGuangYouLiPanelScript_hotfix", "onCallBackMyTuiGuangYouLiData", null, data);
            return;
        }

        MyTuiGuangData.getInstance().initJson(data);

        showTab(m_curShowTab);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void showTab(CurShowTab m_curShowTab)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TuiGuangYouLiPanelScript_hotfix", "showTab"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TuiGuangYouLiPanelScript_hotfix", "showTab", null, m_curShowTab);
            return;
        }

        switch (m_curShowTab)
        {
            case CurShowTab.CurShowTab_wodetuiguang:
                {
                    GameUtil.showGameObject(m_tab_wodetuiguang);
                    GameUtil.hideGameObject(m_tab_tuiguangguize);
                    GameUtil.hideGameObject(m_tab_bulingjiangli);

                    GameUtil.showGameObject(m_btn_yijianlingqu.gameObject);

                    {
                        CommonUtil.setImageSprite(m_btn_myTuiGuang.GetComponent<Image>(), "Sprites/Common/tab_xuanze");
                        CommonUtil.setImageSprite(m_btn_tuigungguize.GetComponent<Image>(), "Sprites/Common/tab_weixuanze");
                        CommonUtil.setImageSprite(m_btn_bulingjiangli.GetComponent<Image>(), "Sprites/Common/tab_weixuanze");

                        m_btn_myTuiGuang.GetComponent<Image>().SetNativeSize();
                        m_btn_tuigungguize.GetComponent<Image>().SetNativeSize();
                        m_btn_bulingjiangli.GetComponent<Image>().SetNativeSize();

                        CommonUtil.setImageSprite(m_btn_myTuiGuang.transform.Find("Image").GetComponent<Image>(), "Sprites/TuiGuang/wdtg02");
                        CommonUtil.setImageSprite(m_btn_tuigungguize.transform.Find("Image").GetComponent<Image>(), "Sprites/TuiGuang/tg01");
                        CommonUtil.setImageSprite(m_btn_bulingjiangli.transform.Find("Image").GetComponent<Image>(), "Sprites/TuiGuang/bljl01");
                    }

                    loadMyTuiGuangData();
                }
                break;

            case CurShowTab.CurShowTab_tuiguangguize:
                {
                    GameUtil.hideGameObject(m_tab_wodetuiguang);
                    GameUtil.showGameObject(m_tab_tuiguangguize);
                    GameUtil.hideGameObject(m_tab_bulingjiangli);

                    GameUtil.hideGameObject(m_btn_yijianlingqu.gameObject);

                    {
                        CommonUtil.setImageSprite(m_btn_myTuiGuang.GetComponent<Image>(), "Sprites/Common/tab_weixuanze");
                        CommonUtil.setImageSprite(m_btn_tuigungguize.GetComponent<Image>(), "Sprites/Common/tab_xuanze");
                        CommonUtil.setImageSprite(m_btn_bulingjiangli.GetComponent<Image>(), "Sprites/Common/tab_weixuanze");

                        m_btn_myTuiGuang.GetComponent<Image>().SetNativeSize();
                        m_btn_tuigungguize.GetComponent<Image>().SetNativeSize();
                        m_btn_bulingjiangli.GetComponent<Image>().SetNativeSize();

                        CommonUtil.setImageSprite(m_btn_myTuiGuang.transform.Find("Image").GetComponent<Image>(), "Sprites/TuiGuang/wdtg01");
                        CommonUtil.setImageSprite(m_btn_tuigungguize.transform.Find("Image").GetComponent<Image>(), "Sprites/TuiGuang/tg02");
                        CommonUtil.setImageSprite(m_btn_bulingjiangli.transform.Find("Image").GetComponent<Image>(), "Sprites/TuiGuang/bljl01");
                    }
                }
                break;

            case CurShowTab.CurShowTab_bulingjiangli:
                {
                    GameUtil.hideGameObject(m_tab_wodetuiguang);
                    GameUtil.hideGameObject(m_tab_tuiguangguize);
                    GameUtil.showGameObject(m_tab_bulingjiangli);

                    GameUtil.hideGameObject(m_btn_yijianlingqu.gameObject);

                    {
                        CommonUtil.setImageSprite(m_btn_myTuiGuang.GetComponent<Image>(), "Sprites/Common/tab_weixuanze");
                        CommonUtil.setImageSprite(m_btn_tuigungguize.GetComponent<Image>(), "Sprites/Common/tab_weixuanze");
                        CommonUtil.setImageSprite(m_btn_bulingjiangli.GetComponent<Image>(), "Sprites/Common/tab_xuanze");

                        m_btn_myTuiGuang.GetComponent<Image>().SetNativeSize();
                        m_btn_tuigungguize.GetComponent<Image>().SetNativeSize();
                        m_btn_bulingjiangli.GetComponent<Image>().SetNativeSize();

                        CommonUtil.setImageSprite(m_btn_myTuiGuang.transform.Find("Image").GetComponent<Image>(), "Sprites/TuiGuang/wdtg01");
                        CommonUtil.setImageSprite(m_btn_tuigungguize.transform.Find("Image").GetComponent<Image>(), "Sprites/TuiGuang/tg01");
                        CommonUtil.setImageSprite(m_btn_bulingjiangli.transform.Find("Image").GetComponent<Image>(), "Sprites/TuiGuang/bljl02");
                    }
                }
                break;
        }
    }

    public void loadMyTuiGuangData()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TuiGuangYouLiPanelScript_hotfix", "loadMyTuiGuangData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TuiGuangYouLiPanelScript_hotfix", "loadMyTuiGuangData", null, null);
            return;
        }

        m_listview_player.clear();

        for (int i = 0; i < MyTuiGuangData.getInstance().getMyTuiGuangDataList().Count; i++)
        {
            MyTuiGuangDataContent temp = MyTuiGuangData.getInstance().getMyTuiGuangDataList()[i];

            GameObject pre = Resources.Load("Prefabs/UI/Item/Item_TuiJianPlayer") as GameObject;
            GameObject obj = Instantiate(pre);
            obj.transform.name = temp.uid;
            obj.transform.Find("Text_name").GetComponent<Text>().text = temp.name;

            // 任务1
            {
                if (temp.task1_state == 1)
                {
                    obj.transform.Find("Text_task1_state").GetComponent<Text>().text = "未完成";
                }
                else if (temp.task1_state == 2)
                {
                    obj.transform.Find("Text_task1_state").GetComponent<Text>().text = "可领取";
                    CommonUtil.setFontColor(obj.transform.Find("Text_task1_state").GetComponent<Text>(), 251, 75, 9);

                    // 启用一键领取按钮
                    m_btn_yijianlingqu.interactable = true;
                }
                // 已领取
                else if (temp.task1_state == 3)
                {
                    obj.transform.Find("Text_task1_state").GetComponent<Text>().text = "";
                    obj.transform.Find("Text_task1_state/Image_yilingqu").transform.localScale = new Vector3(1,1,1);
                }
            }

            // 任务2
            {
                if (temp.task2_state == 1)
                {
                    obj.transform.Find("Text_task2_state").GetComponent<Text>().text = "未完成";
                }
                else if (temp.task2_state == 2)
                {
                    obj.transform.Find("Text_task2_state").GetComponent<Text>().text = "可领取";
                    CommonUtil.setFontColor(obj.transform.Find("Text_task2_state").GetComponent<Text>(), 251, 75, 9);
                }
                // 已领取
                else if (temp.task2_state == 3)
                {
                    obj.transform.Find("Text_task2_state").GetComponent<Text>().text = "";
                    obj.transform.Find("Text_task2_state/Image_yilingqu").transform.localScale = new Vector3(1, 1, 1);
                }
            }

            m_listview_player.addItem(obj);
        }

        m_listview_player.addItemEnd();
    }

    public void onClickMyTuiGuang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TuiGuangYouLiPanelScript_hotfix", "onClickMyTuiGuang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TuiGuangYouLiPanelScript_hotfix", "onClickMyTuiGuang", null, null);
            return;
        }

        if (m_curShowTab != CurShowTab.CurShowTab_wodetuiguang)
        {
            m_curShowTab = CurShowTab.CurShowTab_wodetuiguang;
            showTab(m_curShowTab);
        }
    }

    public void onClickTuiGuangGuize()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TuiGuangYouLiPanelScript_hotfix", "onClickTuiGuangGuize"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TuiGuangYouLiPanelScript_hotfix", "onClickTuiGuangGuize", null, null);
            return;
        }

        if (m_curShowTab != CurShowTab.CurShowTab_tuiguangguize)
        {
            m_curShowTab = CurShowTab.CurShowTab_tuiguangguize;
            showTab(m_curShowTab);
        }
    }

    public void onClickBuLingJiangLi()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TuiGuangYouLiPanelScript_hotfix", "onClickBuLingJiangLi"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TuiGuangYouLiPanelScript_hotfix", "onClickBuLingJiangLi", null, null);
            return;
        }

        if (m_curShowTab != CurShowTab.CurShowTab_bulingjiangli)
        {
            m_curShowTab = CurShowTab.CurShowTab_bulingjiangli;
            showTab(m_curShowTab);
        }
    }

    public void onClickYaoQing()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TuiGuangYouLiPanelScript_hotfix", "onClickYaoQing"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TuiGuangYouLiPanelScript_hotfix", "onClickYaoQing", null, null);
            return;
        }
        
        string str = "快来玩疯狂升级，输入我的推广码：" + UserData.myTuiGuangCode + "即可得话费礼包！！";
        PlatformHelper.WXShareFriends("AndroidCallBack", "OnWxShareFriends", str);
    }

    public void onClickYiJianLingQu()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TuiGuangYouLiPanelScript_hotfix", "onClickYiJianLingQu"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TuiGuangYouLiPanelScript_hotfix", "onClickYiJianLingQu", null, null);
            return;
        }

        LogicEnginerScript.Instance.GetComponent<OneKeyGetAllTuiGuangRewardRequest>().CallBack = onCallBackOneKeyGetAllTuiGuangReward;
        LogicEnginerScript.Instance.GetComponent<OneKeyGetAllTuiGuangRewardRequest>().OnRequest();
    }

    public void onClickShuRuTuiGuangCode()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TuiGuangYouLiPanelScript_hotfix", "onClickShuRuTuiGuangCode"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TuiGuangYouLiPanelScript_hotfix", "onClickShuRuTuiGuangCode", null, null);
            return;
        }

        ShuRuTuiGuangCodePanelScript.create();
    }

    public void onCallBackOneKeyGetAllTuiGuangReward(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TuiGuangYouLiPanelScript_hotfix", "onCallBackOneKeyGetAllTuiGuangReward"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TuiGuangYouLiPanelScript_hotfix", "onCallBackOneKeyGetAllTuiGuangReward", null, data);
            return;
        }

        JsonData jsonData = JsonMapper.ToObject(data);
        int code = (int)jsonData["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            // 禁用一键领取按钮
            m_btn_yijianlingqu.interactable = false;

            string reward = (string)jsonData["reward"];

            if (reward.CompareTo("") != 0)
            {
                List<string> list = new List<string>();
                CommonUtil.splitStr(reward,list,';');
                for (int i = 0; i < list.Count; i++)
                {
                    GameUtil.changeData(GameUtil.getPropIdFromReward(list[i]), GameUtil.getPropNumFromReward(list[i]));

                    ShowRewardPanelScript.Show(list[i], false);
                }

                for (int i = 0; i < MyTuiGuangData.getInstance().getMyTuiGuangDataList().Count; i++)
                {
                    MyTuiGuangDataContent temp = MyTuiGuangData.getInstance().getMyTuiGuangDataList()[i];

                    GameObject obj = null;
                    for (int j = 0; j < m_listview_player.getItemList().Count; j++)
                    {
                        if (m_listview_player.getItemList()[j].transform.name.CompareTo(temp.uid) == 0)
                        {
                            obj = m_listview_player.getItemList()[j];
                            break;
                        }
                    }

                    if (temp.task1_state == 2)
                    {
                        temp.task1_state = 3;
                        
                        obj.transform.Find("Text_task1_state").GetComponent<Text>().text = "";
                        obj.transform.Find("Text_task1_state/Image_yilingqu").transform.localScale = new Vector3(1, 1, 1);
                        CommonUtil.setFontColor(obj.transform.Find("Text_task1_state").GetComponent<Text>(), 99, 99, 99);
                    }

                    if (temp.task2_state == 2)
                    {
                        temp.task2_state = 3;

                        obj.transform.Find("Text_task2_state").GetComponent<Text>().text = "";
                        obj.transform.Find("Text_task2_state/Image_yilingqu").transform.localScale = new Vector3(1, 1, 1);
                        CommonUtil.setFontColor(obj.transform.Find("Text_task2_state").GetComponent<Text>(), 99, 99, 99);
                    }
                }

                ToastScript.createToast("领取奖励成功");
            }
            else
            {
                ToastScript.createToast("当前没有奖励可领取");
            }
        }
        else
        {
            string msg = (string)jsonData["msg"];

            ToastScript.createToast(msg);
        }
    }
}
