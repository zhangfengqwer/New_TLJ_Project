﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaBaPanelScript : MonoBehaviour {
    
    public GameObject m_havePanel;
    public GameObject m_nohavePanel;
    public InputField m_inputField;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/LaBaPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        

        return obj;
    }

    void Start()
    {
        OtherData.s_laBaPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LaBaPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LaBaPanelScript_hotfix", "Start", null, null);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LaBaPanelScript_hotfix", "onClickBuy"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LaBaPanelScript_hotfix", "onClickBuy", null, null);
            return;
        }

        Destroy(gameObject);

        ShopPanelScript.create(3);
    }

    public void onClickSend()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LaBaPanelScript_hotfix", "onClickSend"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LaBaPanelScript_hotfix", "onClickSend", null, null);
            return;
        }

        if (m_inputField.text.Length > 20)
        {
            ToastScript.createToast("发送内容不可超过20个字符");

            return;
        }

        if (string.IsNullOrEmpty(m_inputField.text))
        {
            ToastScript.createToast("发送内容不可为空");

            return;
        }

        for (int i = 0; i < UserData.propData.Count; i++)
        {
            if ((UserData.propData[i].prop_id == 106) && ((UserData.propData[i].prop_num > 0)))
            {
                string content = SensitiveWordUtil.deleteSensitiveWord(m_inputField.text);

                LogicEnginerScript.Instance.GetComponent<UseLaBaRequest>().SetText(UserData.name + "：" + content);
                LogicEnginerScript.Instance.GetComponent<UseLaBaRequest>().CallBack = onReceive_UseLaBa;
                LogicEnginerScript.Instance.GetComponent<UseLaBaRequest>().OnRequest();

                break;
            }
        }
    }

    public void onReceive_UseLaBa(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("LaBaPanelScript_hotfix", "onReceive_UseLaBa"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LaBaPanelScript_hotfix", "onReceive_UseLaBa", null, data);
            return;
        }

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
