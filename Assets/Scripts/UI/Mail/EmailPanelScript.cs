using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailPanelScript : MonoBehaviour
{
    public GameObject m_listView;
    public ListViewScript m_ListViewScript;

    public Button m_button_oneKeyRead;
    public Button m_button_oneKeyDelete;
    public Text m_text_mailNum;
    public Text m_text_zanwu;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/EmailPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    void Start ()
    {
        OtherData.s_emailPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EmailPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EmailPanelScript_hotfix", "Start", null, null);
            return;
        }

        m_button_oneKeyRead.transform.localScale = new Vector3(0, 0, 0);
        m_button_oneKeyDelete.transform.localScale = new Vector3(0, 0, 0);

        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        // 拉取邮件
        {
            NetLoading.getInstance().Show();

            LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().CallBack = onReceive_GetMail;
            LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().OnRequest();
        }
    }

    private void OnDestroy()
    {
        if (LogicEnginerScript.Instance != null)
        {
            LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().CallBack = null;
        }
    }

    public void loadMail()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EmailPanelScript_hotfix", "loadMail"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EmailPanelScript_hotfix", "loadMail", null, null);
            return;
        }

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

                m_button_oneKeyRead.transform.localPosition = new Vector3(-130, -230.31f, 0);
                m_button_oneKeyDelete.transform.localPosition = new Vector3(130, -230.31f, 0);
            }
            else
            {
                if (canUseOneKeyRead)
                {
                    m_button_oneKeyRead.transform.localScale = new Vector3(1, 1, 1);
                    m_button_oneKeyDelete.transform.localScale = new Vector3(0, 0, 0);

                    m_button_oneKeyRead.transform.localPosition = new Vector3(0, -230.31f, 0);
                }

                if (canUseOneKeyDelete)
                {
                    m_button_oneKeyDelete.transform.localScale = new Vector3(1, 1, 1);
                    m_button_oneKeyRead.transform.localScale = new Vector3(0, 0, 0);

                    m_button_oneKeyDelete.transform.localPosition = new Vector3(0, -230.31f, 0);
                }
            }
        }

        m_text_mailNum.text = UserMailData.getInstance().getUserMailDataList().Count + "/50";

        //暂无邮件
        m_text_zanwu.transform.localScale = UserMailData.getInstance().getUserMailDataList().Count == 0 ? Vector3.one : Vector3.zero;
    }

    public void setMailReaded(int email_id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EmailPanelScript_hotfix", "setMailReaded"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EmailPanelScript_hotfix", "setMailReaded", null, email_id);
            return;
        }

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

                m_button_oneKeyRead.transform.localPosition = new Vector3(-130, -230.31f, 0);
                m_button_oneKeyDelete.transform.localPosition = new Vector3(130, -230.31f, 0);
            }
            else
            {
                if (canUseOneKeyRead)
                {
                    m_button_oneKeyRead.transform.localScale = new Vector3(1, 1, 1);
                    m_button_oneKeyDelete.transform.localScale = new Vector3(0, 0, 0);

                    m_button_oneKeyRead.transform.localPosition = new Vector3(0, -230.31f, 0);
                }

                if (canUseOneKeyDelete)
                {
                    m_button_oneKeyDelete.transform.localScale = new Vector3(1, 1, 1);
                    m_button_oneKeyRead.transform.localScale = new Vector3(0, 0, 0);

                    m_button_oneKeyDelete.transform.localPosition = new Vector3(0, -230.31f, 0);
                }
            }
        }
    }

    public void deleteMail(int email_id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EmailPanelScript_hotfix", "deleteMail"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EmailPanelScript_hotfix", "deleteMail", null, email_id);
            return;
        }

        UserMailData.getInstance().deleteMail(email_id);

        loadMail();
    }

    public void setAllMailReaded()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EmailPanelScript_hotfix", "setAllMailReaded"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EmailPanelScript_hotfix", "setAllMailReaded", null, null);
            return;
        }

        UserMailData.getInstance().setAllMailReaded();

        loadMail();
    }

    public void deleteAllMail()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EmailPanelScript_hotfix", "deleteAllMail"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EmailPanelScript_hotfix", "deleteAllMail", null, null);
            return;
        }

        UserMailData.getInstance().deleteAllMail();

        loadMail();
    }

    public void onClickOneKeyReadMail()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EmailPanelScript_hotfix", "onClickOneKeyReadMail"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EmailPanelScript_hotfix", "onClickOneKeyReadMail", null, null);
            return;
        }

        LogicEnginerScript.Instance.GetComponent<OneKeyReadEmailRequest>().CallBack = onReceive_OneKeyReadMail;
        LogicEnginerScript.Instance.GetComponent<OneKeyReadEmailRequest>().OnRequest();
    }

    public void onClickOneKeyDeleteMail()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EmailPanelScript_hotfix", "onClickOneKeyDeleteMail"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EmailPanelScript_hotfix", "onClickOneKeyDeleteMail", null, null);
            return;
        }

        LogicEnginerScript.Instance.GetComponent<OneKeyDeleteEmailRequest>().CallBack = onReceive_OneKeyDeleteMail;
        LogicEnginerScript.Instance.GetComponent<OneKeyDeleteEmailRequest>().OnRequest();
    }

    public void onReceive_OneKeyReadMail(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EmailPanelScript_hotfix", "onReceive_OneKeyReadMail"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EmailPanelScript_hotfix", "onReceive_OneKeyReadMail", null, data);
            return;
        }

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            setAllMailReaded();

            if (OtherData.s_mainScript != null)
            {
                OtherData.s_mainScript.checkRedPoint();
            }
        }
    }

    public void onReceive_OneKeyDeleteMail(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EmailPanelScript_hotfix", "onReceive_OneKeyDeleteMail"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EmailPanelScript_hotfix", "onReceive_OneKeyDeleteMail", null, data);
            return;
        }

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            deleteAllMail();
        }
    }

    public void onReceive_GetMail(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EmailPanelScript_hotfix", "onReceive_GetMail"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EmailPanelScript_hotfix", "onReceive_GetMail", null, data);
            return;
        }

        NetLoading.getInstance().Close();

        UserMailData.getInstance().initJson(data);
        
        loadMail();
    }
}
