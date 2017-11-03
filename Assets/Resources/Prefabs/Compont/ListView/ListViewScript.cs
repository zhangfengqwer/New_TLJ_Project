using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListViewScript : MonoBehaviour {

    List<GameObject> m_itemList = new List<GameObject>();
    public GameObject m_content = null;

    Vector2 m_listSize;
    public Vector2 m_itemSize;
    public int m_jiange = 0;

    public static GameObject CreateListView()
    {
        GameObject pre = Resources.Load("Prefabs/Compont/ListView/ListView") as GameObject;
        GameObject obj = Instantiate(pre);

        return obj;
    }
    
    void Start()
    {
        m_listSize = gameObject.GetComponent<RectTransform>().sizeDelta;
        m_content = gameObject.transform.Find("Viewport").Find("Content").gameObject;
    }

    public List<GameObject> getItemList()
    {
        return m_itemList;
    }

    public int getItemCount()
    {
        return m_itemList.Count;
    }

    public void setSize(Vector2 listSize, Vector2 contentSize)
    {
        m_listSize = listSize;
        m_itemSize = contentSize;
        
        gameObject.GetComponent<RectTransform>().sizeDelta = m_listSize;
    }

    public void setItemJianGe(int jiange)
    {
        m_jiange = jiange;
    }

    public void addItem(GameObject item)
    {
        m_itemList.Add(item);
    }

    public void addItemEnd()
    {
        if (m_content == null)
        {
            return;
        }

        float contentLength = m_itemList.Count * m_itemSize.y + (m_itemList.Count - 1) * m_jiange;
        m_content.GetComponent<RectTransform>().sizeDelta = new Vector2(m_listSize.x , contentLength);

        for (int i = 0; i < m_itemList.Count; i++)
        {
            m_itemList[i].transform.SetParent(m_content.transform);
            m_itemList[i].transform.localScale = new Vector3(1, 1, 1);

            float y = -m_itemSize.y / 2 - i * (m_itemSize.y + m_jiange);
            m_itemList[i].transform.localPosition = new Vector3(0 , y , 0);
        }
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
    }
}
