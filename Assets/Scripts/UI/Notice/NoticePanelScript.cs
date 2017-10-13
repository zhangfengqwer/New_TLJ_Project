using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticePanelScript : MonoBehaviour
{
    public GameObject m_listView;
    ListViewScript m_ListViewScript;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/NoticePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

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

    // 显示活动
    public void loadHuoDong()
    {
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

    public void onClickHuoDong()
    {
        loadHuoDong();
    }

    public void onClickGongGao()
    {
        loadGongGao();
    }

    public void onReceive_GetNotice(string data)
    {
        NoticelDataScript.getInstance().initJson(data);

        loadHuoDong();
    }
}