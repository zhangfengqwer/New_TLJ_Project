using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListViewScript2 : MonoBehaviour
{
    List<GameObject> m_itemList = new List<GameObject>();
    public GameObject m_content = null;

    Vector2 m_listSize;
    Vector2 m_contentSize;

    int m_maxItemCount = -1;        // -1代表无限
    public int m_jiange = 0;

    public static GameObject CreateListView()
    {
        GameObject pre = Resources.Load("Prefabs/Compont/ListView/ListView") as GameObject;
        GameObject obj = Instantiate(pre);

        return obj;
    }

    private void Awake()
    {
        m_listSize = gameObject.GetComponent<RectTransform>().sizeDelta;
        m_content = gameObject.transform.Find("Viewport").Find("Content").gameObject;
    }

    void Start()
    {
        
    }

    public List<GameObject> getItemList()
    {
        return m_itemList;
    }

    public int getItemCount()
    {
        return m_itemList.Count;
    }

    public void setMaxItemCount(int count)
    {
        m_maxItemCount = count;
    }

    public void setItemJianGe(int jiange)
    {
        m_jiange = jiange;
    }

    public void addItem(GameObject item)
    {
        m_itemList.Add(item);

        if (m_maxItemCount != -1)
        {
            // 检测是否超出限制item数量上线
            if (m_itemList.Count > m_maxItemCount)
            {
                m_itemList.RemoveAt(0);
            }
        }
        
        initItem();
    }

    public void removeItem(GameObject item)
    {
        m_itemList.Remove(item);
        Destroy(item);

        initItem();
    }

    public void initItem()
    {
        if (m_content == null)
        {
            return;
        }

        {
            m_contentSize = new Vector2(0, 0);

            for (int i = 0; i < m_itemList.Count; i++)
            {
                m_contentSize.y += m_itemList[i].GetComponent<RectTransform>().sizeDelta.y;
            }

            m_contentSize.y += (m_itemList.Count - 1) * m_jiange;
            m_contentSize.x = m_listSize.x;
            m_content.GetComponent<RectTransform>().sizeDelta = m_contentSize;
        }

        float beforeY = 0;
        for (int i = 0; i < m_itemList.Count; i++)
        {
            m_itemList[i].transform.SetParent(m_content.transform);
            m_itemList[i].transform.localScale = new Vector3(1, 1, 1);

            if (i == 0)
            {
                beforeY = - m_itemList[i].GetComponent<RectTransform>().sizeDelta.y / 2;
                m_itemList[i].transform.localPosition = new Vector3(0, beforeY, 0);
            }
            else
            {
                beforeY = (beforeY - m_itemList[i - 1].GetComponent<RectTransform>().sizeDelta.y / 2 - m_itemList[i].GetComponent<RectTransform>().sizeDelta.y / 2 - m_jiange);
                m_itemList[i].transform.localPosition = new Vector3(0, beforeY, 0);
            }
        }
    }

    public void setListToTop()
    {
        m_content.transform.localPosition = new Vector3(m_contentSize.x / 2, 0, 0);
    }

    public void setListToBottom()
    {
        float temp = m_contentSize.y - m_listSize.y;
        temp = temp > 0 ? temp : 0;
        m_content.transform.localPosition = new Vector3(m_contentSize.x / 2, temp, 0);
    }

    public void clear()
    {
        if (m_content == null)
        {
            return;
        }

        for (int i = 0; i < m_content.transform.childCount; i++)
        {
            Destroy(m_content.transform.GetChild(i).gameObject);
        }

        m_itemList.Clear();

        m_contentSize = m_listSize;
        m_content.GetComponent<RectTransform>().sizeDelta = m_contentSize;
        m_content.transform.localPosition = new Vector3(m_contentSize.x, 0,0);
    }
}
