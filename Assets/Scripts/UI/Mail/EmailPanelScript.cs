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

        for(int i = 0; i < 6; i++)
        {
            MailData mailData = new MailData();
            mailData.m_email_id = i;
            mailData.m_title = "邮件";
            mailData.m_content = "正文" + i;
            mailData.m_time = "2017-10-11";
            UserMailData.getInstance().addMailData(mailData);
        }

        for (int i = 0; i < UserMailData.getInstance().getUserMailDataList().Count; i++)
        {
            GameObject prefab = Resources.Load("Prefabs/UI/Item/Mail_List_Item") as GameObject;
            GameObject obj = MonoBehaviour.Instantiate(prefab);
            obj.GetComponent<Mail_List_Item_Script>().setTitle(UserMailData.getInstance().getUserMailDataList()[i].m_title);
            obj.GetComponent<Mail_List_Item_Script>().setTime(UserMailData.getInstance().getUserMailDataList()[i].m_time);

            obj.transform.name = UserMailData.getInstance().getUserMailDataList()[i].m_email_id.ToString();

            m_ListViewScript.addItem(obj);
        }

        m_ListViewScript.addItemEnd();
    }
}
