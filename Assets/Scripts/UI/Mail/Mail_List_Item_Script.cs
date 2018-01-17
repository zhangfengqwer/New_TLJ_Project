using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mail_List_Item_Script : MonoBehaviour {

    public EmailPanelScript m_parentScript;

    public Text m_text_title;
    public Text m_text_time;
    public Image m_redPoint;

    public Button m_button_item;

    public MailData m_mailData;

    // Use this for initialization
    void Start () {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Mail_List_Item_Script_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Mail_List_Item_Script_hotfix", "Start", null, null);
            return;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setMailData(MailData mailData)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Mail_List_Item_Script_hotfix", "setMailData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Mail_List_Item_Script_hotfix", "setMailData", null, mailData);
            return;
        }

        m_mailData = mailData;

        {
            m_text_title.text = m_mailData.m_title;
            
            // 日期
            {
                string data_mail = m_mailData.m_time;
                string data_now = CommonUtil.getCurYear() + "-" + CommonUtil.getCurMonth() + "-" + CommonUtil.getCurDay() + " 0:0:0";
                int days = CommonUtil.tianshucha(data_mail, data_now);

                if (days == 0)
                {
                    m_text_time.text = "今天";
                }
                else if (days <= 7)
                {
                    m_text_time.text = "1天前";
                }
                else if (days <= 30)
                {
                    m_text_time.text = "1周前";
                }
                else if (days <= 365)
                {
                    m_text_time.text = "1月前";
                }
                else
                {
                    m_text_time.text = "1年前";
                }
            }

            // 已读
            if (m_mailData.m_state == 1)
            {
                m_redPoint.transform.localScale = new Vector3(0,0,0);
            }
        }
    }

    public MailData getMailData()
    {
        return m_mailData;
    }

    public void onClickItem()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Mail_List_Item_Script_hotfix", "onClickItem"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Mail_List_Item_Script_hotfix", "onClickItem", null, null);
            return;
        }

        // 未领取的邮件先请求服务器
        if (m_mailData.m_state == 0)
        {
            LogicEnginerScript.Instance.GetComponent<ReadEmailRequest>().setEmailId(int.Parse(gameObject.transform.name));
            LogicEnginerScript.Instance.GetComponent<ReadEmailRequest>().CallBack = onReceive_ReadMail;
            LogicEnginerScript.Instance.GetComponent<ReadEmailRequest>().OnRequest();
        }
        // 已领取的直接显示
        else
        {
            MailDetailScript.create(int.Parse(gameObject.transform.name), m_parentScript);
        }
    }

    public void onReceive_ReadMail(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Mail_List_Item_Script_hotfix", "onReceive_ReadMail"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Mail_List_Item_Script_hotfix", "onReceive_ReadMail", null, data);
            return;
        }

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];
        int email_id = (int)jd["email_id"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            m_parentScript.setMailReaded(email_id);

            if (OtherData.s_mainScript != null)
            {
                OtherData.s_mainScript.checkRedPoint();
            }

            //ShowRewardPanelScript.create().GetComponent<ShowRewardPanelScript>().setData(m_mailData.m_reward);
            if (!string.IsNullOrEmpty(m_mailData.m_reward))
            {
                ShowRewardPanelScript.Show(m_mailData.m_reward,false);
            }
        }

        MailDetailScript.create(int.Parse(gameObject.transform.name), m_parentScript);
    }
}
