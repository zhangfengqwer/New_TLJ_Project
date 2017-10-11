using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailPanelScript : MonoBehaviour
{
    public GameObject m_listView;
    ListViewScript m_ListViewScript;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/EmailPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

        return obj;
    }

    void Start ()
    {
        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        loadMail();
    }

    public void loadMail()
    {
        m_ListViewScript.clear();

        for (int i = 0; i < 10; i++)
        {
            GameObject prefab = Resources.Load("Prefabs/UI/Item/Mail_List_Item") as GameObject;
            GameObject obj = MonoBehaviour.Instantiate(prefab);
            obj.GetComponent<Mail_List_Item_Script>().setTitle("这是邮件标题");
            obj.GetComponent<Mail_List_Item_Script>().setTime("2017-10-11");


            //GameObject temp = new GameObject();
            //temp.AddComponent<Image>();
            //temp.GetComponent<Image>().GetComponent<RectTransform>().sizeDelta = new Vector2(300,50);

            m_ListViewScript.addItem(obj);
        }

        m_ListViewScript.addItemEnd();
    }
}
