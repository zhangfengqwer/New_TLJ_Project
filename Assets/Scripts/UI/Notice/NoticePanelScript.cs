using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticePanelScript : MonoBehaviour
{
    public GameObject m_listView;
    ListViewScript m_ListViewScript;

    public Button m_button_tab;
    public Text m_text_zanwu;

    int m_tabType = 1;      // 1：活动   2：公告

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/NoticePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
	{
        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        // 拉取公告活动
        {
            LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().CallBack = onReceive_GetNotice;
            LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().OnRequest();
        }
    }

    private void OnDestroy()
    {
        LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().CallBack = null;
    }

    // 显示活动
    public void loadHuoDong()
    {
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
        NoticelDataScript.getInstance().setNoticeReaded(notice_id);

        for (int i = 0; i < m_ListViewScript.getItemList().Count; i++)
        {
            if (m_ListViewScript.getItemList()[i].GetComponent<Item_Notice_List_Script>().getNoticeData().notice_id == notice_id)
            {
                m_ListViewScript.getItemList()[i].GetComponent<Item_Notice_List_Script>().m_redPoint.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

    public void onClickTab()
    {
        if (m_tabType == 1)
        {
            m_tabType = 2;
            loadGongGao();

            m_button_tab.GetComponent<Image>().sprite = Resources.Load("Sprites/Notice/biaoti_gonggao", typeof(Sprite)) as Sprite;
        }
        else
        {
            m_tabType = 1;
            loadHuoDong();

            m_button_tab.GetComponent<Image>().sprite = Resources.Load("Sprites/Notice/biaoti_huodong", typeof(Sprite)) as Sprite;
        }
    }

    public void onReceive_GetNotice(string data)
    {
        NoticelDataScript.getInstance().initJson(data);

        loadHuoDong();
    }
}