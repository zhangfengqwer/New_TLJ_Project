using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailPanelScript : MonoBehaviour
{
    public GameObject m_listView;
    ListViewScript m_ListViewScript;

    public Button m_button_oneKeyRead;
    public Button m_button_oneKeyDelete;
    public Text m_text_mailNum;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/EmailPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    void Start ()
    {
        m_button_oneKeyRead.transform.localScale = new Vector3(0, 0, 0);
        m_button_oneKeyDelete.transform.localScale = new Vector3(0, 0, 0);

        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        // 拉取邮件
        {
            LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().CallBack = onReceive_GetMail;
            LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().OnRequest();
        }
    }

    private void OnDestroy()
    {
        LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().CallBack = null;
    }

    void loadMail()
    {
        m_ListViewScript.clear();
        
        for (int i = 0; i < UserMailData.getInstance().getUserMailDataList().Count; i++)
        {
            GameObject prefab = Resources.Load("Prefabs/UI/Item/Mail_List_Item") as GameObject;
            GameObject obj = MonoBehaviour.Instantiate(prefab);
            obj.GetComponent<Mail_List_Item_Script>().m_parentScript = this;
            obj.GetComponent<Mail_List_Item_Script>().setMailData(UserMailData.getInstance().getUserMailDataList()[i]);

            obj.transform.name = UserMailData.getInstance().getUserMailDataList()[i].m_email_id.ToString();

            m_ListViewScript.addItem(obj);
        }

        m_ListViewScript.addItemEnd();

        // 判断是否启用：一键领取、一键删除
        {
            bool canUseOneKeyRead = false;
            bool canUseOneKeyDelete = false;

            if (m_button_oneKeyRead != null)
            {
                m_button_oneKeyRead.transform.localScale = new Vector3(0, 0, 0);
                m_button_oneKeyDelete.transform.localScale = new Vector3(0, 0, 0);
            }

            for (int i = 0; i < UserMailData.getInstance().getUserMailDataList().Count; i++)
            {
                if (UserMailData.getInstance().getUserMailDataList()[i].m_state == 0)
                {
                    canUseOneKeyRead = true;
                }

                if (UserMailData.getInstance().getUserMailDataList()[i].m_state == 1)
                {
                    canUseOneKeyDelete = true;
                }
            }

            if (canUseOneKeyRead && canUseOneKeyDelete)
            {
                m_button_oneKeyRead.transform.localScale = new Vector3(1, 1, 1);
                m_button_oneKeyDelete.transform.localScale = new Vector3(1, 1, 1);

                m_button_oneKeyRead.transform.localPosition = new Vector3(-130, -212.41f, 0);
                m_button_oneKeyDelete.transform.localPosition = new Vector3(130, -212.41f, 0);
            }
            else
            {
                if (canUseOneKeyRead)
                {
                    m_button_oneKeyRead.transform.localScale = new Vector3(1, 1, 1);
                    m_button_oneKeyDelete.transform.localScale = new Vector3(0, 0, 0);

                    m_button_oneKeyRead.transform.localPosition = new Vector3(0, -212.41f, 0);
                }

                if (canUseOneKeyDelete)
                {
                    m_button_oneKeyDelete.transform.localScale = new Vector3(1, 1, 1);
                    m_button_oneKeyRead.transform.localScale = new Vector3(0, 0, 0);

                    m_button_oneKeyDelete.transform.localPosition = new Vector3(0, -212.41f, 0);
                }
            }
        }

        m_text_mailNum.text = UserMailData.getInstance().getUserMailDataList().Count + "/50";
    }

    public void setMailReaded(int email_id)
    {
        UserMailData.getInstance().setMailReaded(email_id);

        for (int i = 0; i < m_ListViewScript.getItemList().Count; i++)
        {
            if (m_ListViewScript.getItemList()[i].GetComponent<Mail_List_Item_Script>().getMailData().m_email_id == email_id)
            {
                m_ListViewScript.getItemList()[i].GetComponent<Mail_List_Item_Script>().m_redPoint.transform.localScale = new Vector3(0,0,0);
            }
        }

        // 判断是否启用：一键领取、一键删除
        {
            bool canUseOneKeyRead = false;
            bool canUseOneKeyDelete = false;

            for (int i = 0; i < UserMailData.getInstance().getUserMailDataList().Count; i++)
            {
                if (UserMailData.getInstance().getUserMailDataList()[i].m_state == 0)
                {
                    canUseOneKeyRead = true;
                }

                if (UserMailData.getInstance().getUserMailDataList()[i].m_state == 1)
                {
                    canUseOneKeyDelete = true;
                }
            }

            if (canUseOneKeyRead && canUseOneKeyDelete)
            {
                m_button_oneKeyRead.transform.localScale = new Vector3(1, 1, 1);
                m_button_oneKeyDelete.transform.localScale = new Vector3(1, 1, 1);

                m_button_oneKeyRead.transform.localPosition = new Vector3(-130, -212.41f, 0);
                m_button_oneKeyDelete.transform.localPosition = new Vector3(130, -212.41f, 0);
            }
            else
            {
                if (canUseOneKeyRead)
                {
                    m_button_oneKeyRead.transform.localScale = new Vector3(1, 1, 1);
                    m_button_oneKeyDelete.transform.localScale = new Vector3(0, 0, 0);

                    m_button_oneKeyRead.transform.localPosition = new Vector3(0, -212.41f, 0);
                }

                if (canUseOneKeyDelete)
                {
                    m_button_oneKeyDelete.transform.localScale = new Vector3(1, 1, 1);
                    m_button_oneKeyRead.transform.localScale = new Vector3(0, 0, 0);

                    m_button_oneKeyDelete.transform.localPosition = new Vector3(0, -212.41f, 0);
                }
            }
        }
    }

    public void deleteMail(int email_id)
    {
        UserMailData.getInstance().deleteMail(email_id);

        loadMail();
    }

    public void setAllMailReaded()
    {
        UserMailData.getInstance().setAllMailReaded();

        loadMail();
    }

    public void deleteAllMail()
    {
        UserMailData.getInstance().deleteAllMail();

        loadMail();
    }

    public void onClickOneKeyReadMail()
    {
        LogicEnginerScript.Instance.GetComponent<OneKeyReadEmailRequest>().CallBack = onReceive_OneKeyReadMail;
        LogicEnginerScript.Instance.GetComponent<OneKeyReadEmailRequest>().OnRequest();
    }

    public void onClickOneKeyDeleteMail()
    {
        LogicEnginerScript.Instance.GetComponent<OneKeyDeleteEmailRequest>().CallBack = onReceive_OneKeyDeleteMail;
        LogicEnginerScript.Instance.GetComponent<OneKeyDeleteEmailRequest>().OnRequest();
    }

    public void onReceive_OneKeyReadMail(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            setAllMailReaded();
        }
    }

    public void onReceive_OneKeyDeleteMail(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            deleteAllMail();
        }
    }

    public void onReceive_GetMail(string data)
    {
        UserMailData.getInstance().initJson(data);

        loadMail();
    }
}
