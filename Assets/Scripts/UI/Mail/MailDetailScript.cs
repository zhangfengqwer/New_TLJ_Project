using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailDetailScript : MonoBehaviour {

    public EmailPanelScript m_parentScript;
    public GameObject EmailReward;
    public Text m_content;
    MailData m_mailData = null;

    public static GameObject create(int email_id, EmailPanelScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/MailDetailPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<MailDetailScript>().setEmailId(email_id);
        obj.GetComponent<MailDetailScript>().m_parentScript = parentScript;
      
        return obj;
    }

    // Use this for initialization
    void Start ()
    {
      
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setEmailId(int email_id)
    {
        for (int i = 0; i < UserMailData.getInstance().getUserMailDataList().Count; i++)
        {
            if (UserMailData.getInstance().getUserMailDataList()[i].m_email_id == email_id)
            {
                m_mailData = UserMailData.getInstance().getUserMailDataList()[i];
                m_content.text = m_mailData.m_content;

                break;
            }
        }

        setData(m_mailData.m_reward);
    }

    public void setData(string reward)
    {
        List<string> list1 = new List<string>();
        CommonUtil.splitStr(reward, list1, ';');

        for (int i = 0; i < list1.Count; i++)
        {
            List<string> list2 = new List<string>();
            CommonUtil.splitStr(list1[i], list2, ':');

            int id = int.Parse(list2[0]);
            int num = int.Parse(list2[1]);

            GameObject prefab = Resources.Load("Prefabs/UI/Item/item_email_reward") as GameObject;
            GameObject obj = GameObject.Instantiate(prefab, EmailReward.transform);

            CommonUtil.setImageSprite(obj.transform.Find("Image_icon").GetComponent<Image>(), GameUtil.getPropIconPath(id));
            obj.transform.Find("Text_num").GetComponent<Text>().text = "" + num;
//
//            float x = CommonUtil.getPosX(list1.Count, 130, i, 0);
//            obj.transform.localPosition = new Vector3(x, 0, 0);
        }
    }

    public void onClickDelete()
    {
        LogicEnginerScript.Instance.GetComponent<DeleteEmailRequest>().setEmailId(m_mailData.m_email_id);
        LogicEnginerScript.Instance.GetComponent<DeleteEmailRequest>().CallBack = onReceive_DeleteMail;
        LogicEnginerScript.Instance.GetComponent<DeleteEmailRequest>().OnRequest();
    }

    public void onReceive_DeleteMail(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];
        int email_id = (int)jd["email_id"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            m_parentScript.deleteMail(email_id);
        }

        Destroy(gameObject);
    }
}
