using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticePanelScript : MonoBehaviour
{
    public GameObject m_listView;
    public ListViewScript m_ListViewScript;

    public Image m_tab_bg;
    public Button m_button_huodong;
    public Button m_button_gonggao;
    public Image m_image_huodong_redPoint;
    public Image m_image_gonggao_redPoint;
    public Text m_text_zanwu;
    public bool m_curShowHuoDong = true;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/NoticePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
	{
        OtherData.s_noticePanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NoticePanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NoticePanelScript_hotfix", "Start", null, null);
            return;
        }

        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        // 拉取公告活动
        {
            LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().CallBack = onReceive_GetNotice;
            LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().OnRequest();
        }
    }

    private void OnDestroy()
    {
        if (LogicEnginerScript.Instance != null)
        {
            LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().CallBack = null;
        }
    }

    public void checkRedPoint()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NoticePanelScript_hotfix", "checkRedPoint"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NoticePanelScript_hotfix", "checkRedPoint", null, null);
            return;
        }

        bool isShowRedPoint_huodong = false;
        bool isShowRedPoint_gonggao = false;
        for (int i = 0; i < NoticelDataScript.getInstance().getNoticeDataList().Count; i++)
        {
            // 活动
            if (NoticelDataScript.getInstance().getNoticeDataList()[i].type == 0)
            {
                if (NoticelDataScript.getInstance().getNoticeDataList()[i].state == 0)
                {
                    isShowRedPoint_huodong = true;
                }
            }
            // 公告
            else if (NoticelDataScript.getInstance().getNoticeDataList()[i].type == 1)
            {
                if (NoticelDataScript.getInstance().getNoticeDataList()[i].state == 0)
                {
                    isShowRedPoint_gonggao = true;
                }
            }
        }

        if (isShowRedPoint_huodong)
        {
            GameUtil.showGameObject(m_image_huodong_redPoint.gameObject);
        }
        else
        {
            GameUtil.hideGameObject(m_image_huodong_redPoint.gameObject);
        }

        if (isShowRedPoint_gonggao)
        {
            GameUtil.showGameObject(m_image_gonggao_redPoint.gameObject);
        }
        else
        {
            GameUtil.hideGameObject(m_image_gonggao_redPoint.gameObject);
        }
    }

    // 显示活动
    public void loadHuoDong()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NoticePanelScript_hotfix", "loadHuoDong"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NoticePanelScript_hotfix", "loadHuoDong", null, null);
            return;
        }

        m_curShowHuoDong = true;

        m_ListViewScript.clear();

        for (int i = 0; i < NoticelDataScript.getInstance().getNoticeDataList().Count; i++)
        {
            if (NoticelDataScript.getInstance().getNoticeDataList()[i].type == 0)
            {
                GameObject prefab = Resources.Load("Prefabs/UI/Item/Item_Notice_List") as GameObject;
                GameObject obj = MonoBehaviour.Instantiate(prefab);
                obj.GetComponent<Item_Notice_List_Script>().m_parentScript = this;
                obj.GetComponent<Item_Notice_List_Script>().setNoticeData(NoticelDataScript.getInstance().getNoticeDataList()[i]);

                obj.transform.name = NoticelDataScript.getInstance().getNoticeDataList()[i].notice_id.ToString();

                m_ListViewScript.addItem(obj);
            }
        }

        m_ListViewScript.addItemEnd();

        if (m_ListViewScript.getItemCount() > 0)
        {
            m_text_zanwu.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            m_text_zanwu.transform.localScale = new Vector3(1, 1, 1);
            m_text_zanwu.text = "暂无活动";
        }
    }

    // 显示公告
    public void loadGongGao()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NoticePanelScript_hotfix", "loadGongGao"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NoticePanelScript_hotfix", "loadGongGao", null, null);
            return;
        }

        m_curShowHuoDong = false;

        m_ListViewScript.clear();
        
        for (int i = 0; i < NoticelDataScript.getInstance().getNoticeDataList().Count; i++)
        {
            if (NoticelDataScript.getInstance().getNoticeDataList()[i].type == 1)
            {
                GameObject prefab = Resources.Load("Prefabs/UI/Item/Item_Notice_List") as GameObject;
                GameObject obj = MonoBehaviour.Instantiate(prefab);
                obj.GetComponent<Item_Notice_List_Script>().m_parentScript = this;
                obj.GetComponent<Item_Notice_List_Script>().setNoticeData(NoticelDataScript.getInstance().getNoticeDataList()[i]);

                obj.transform.name = NoticelDataScript.getInstance().getNoticeDataList()[i].notice_id.ToString();

                m_ListViewScript.addItem(obj);
            }
        }

        m_ListViewScript.addItemEnd();

        if (m_ListViewScript.getItemCount() > 0)
        {
            m_text_zanwu.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            m_text_zanwu.transform.localScale = new Vector3(1, 1, 1);
            m_text_zanwu.text = "暂无公告";
        }
    }

    public void setNoticeReaded(int notice_id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NoticePanelScript_hotfix", "setNoticeReaded"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NoticePanelScript_hotfix", "setNoticeReaded", null, notice_id);
            return;
        }

        NoticelDataScript.getInstance().setNoticeReaded(notice_id);

        for (int i = 0; i < m_ListViewScript.getItemList().Count; i++)
        {
            if (m_ListViewScript.getItemList()[i].GetComponent<Item_Notice_List_Script>().getNoticeData().notice_id == notice_id)
            {
                m_ListViewScript.getItemList()[i].GetComponent<Item_Notice_List_Script>().m_redPoint.transform.localScale = new Vector3(0, 0, 0);

                break;
            }
        }

        checkRedPoint();
    }

    public void onClickHuoDong()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NoticePanelScript_hotfix", "onClickHuoDong"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NoticePanelScript_hotfix", "onClickHuoDong", null, null);
            return;
        }

        if (m_curShowHuoDong)
        {
            return;
        }

        loadHuoDong();

        m_tab_bg.transform.localPosition = new Vector3(-83,0,0);
        m_button_huodong.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("Sprites/Notice/huodong_xuanze", typeof(Sprite)) as Sprite;
        m_button_gonggao.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("Sprites/Notice/gonggao_weixuanze", typeof(Sprite)) as Sprite;
    }

    public void onClickGongGao()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NoticePanelScript_hotfix", "onClickGongGao"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NoticePanelScript_hotfix", "onClickGongGao", null, null);
            return;
        }

        if (!m_curShowHuoDong)
        {
            return;
        }

        loadGongGao();

        m_tab_bg.transform.localPosition = new Vector3(83, 0, 0);
        m_button_gonggao.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("Sprites/Notice/gonggao_xuanze", typeof(Sprite)) as Sprite;
        m_button_huodong.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("Sprites/Notice/huodong_weixuanze", typeof(Sprite)) as Sprite;
    }

    public void onReceive_GetNotice(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NoticePanelScript_hotfix", "onReceive_GetNotice"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NoticePanelScript_hotfix", "onReceive_GetNotice", null, data);
            return;
        }

        NoticelDataScript.getInstance().initJson(data);

        checkRedPoint();

        loadHuoDong();
    }
}