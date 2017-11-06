﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaBaPanelScript : MonoBehaviour {

    public MainScript m_mainScript = null;

    public GameObject m_havePanel;
    public GameObject m_nohavePanel;
    public InputField m_inputField;

    public static GameObject create(MainScript mainScript)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/LaBaPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<LaBaPanelScript>().m_mainScript = mainScript;

        return obj;
    }

    void Start()
    {
        bool isHaveLaBa = false;
        for (int i = 0; i < UserData.propData.Count; i++)
        {
            if ((UserData.propData[i].prop_id == 106) && ((UserData.propData[i].prop_num > 0)))
            {
                isHaveLaBa = true;
                break;
            }
        }

        if (isHaveLaBa)
        {
            m_nohavePanel.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            m_havePanel.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void onClickBuy()
    {
        Destroy(gameObject);

        ShopPanelScript.create(m_mainScript);
    }

    public void onClickSend()
    {
        if (m_inputField.text.Length > 30)
        {
            ToastScript.createToast("发送内容不可超过30个字符");

            return;
        }

        if (SensitiveWordUtil.IsSensitiveWord(m_inputField.text))
        {
            ToastScript.createToast("您的内容有敏感词");

            return;
        }

        for (int i = 0; i < UserData.propData.Count; i++)
        {
            if ((UserData.propData[i].prop_id == 106) && ((UserData.propData[i].prop_num > 0)))
            {
                LogicEnginerScript.Instance.GetComponent<UseLaBaRequest>().SetText(UserData.name + "：" + m_inputField.text);
                LogicEnginerScript.Instance.GetComponent<UseLaBaRequest>().CallBack = onReceive_UseLaBa;
                LogicEnginerScript.Instance.GetComponent<UseLaBaRequest>().OnRequest();

                break;
            }
        }
    }

    public void onReceive_UseLaBa(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];
        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            ToastScript.createToast("使用成功");

            GameUtil.changeData(106,-1);

            if (BagPanelScript.Instance != null)
            {
                BagPanelScript.Instance.UpdateUI();
            }

            Destroy(gameObject);
        }
    }
}
