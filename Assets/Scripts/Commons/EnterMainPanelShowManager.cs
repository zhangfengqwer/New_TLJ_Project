using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMainPanelShowManager
{
    static EnterMainPanelShowManager s_instance = null;

    List<EnterMainPanelObj> s_panelObjList = new List<EnterMainPanelObj>();

    public static EnterMainPanelShowManager getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new EnterMainPanelShowManager();
            s_instance.init();
        }

        return s_instance;
    }

    public void init()
    {
        s_panelObjList.Add(new EnterMainPanelObj("sign", false));
        s_panelObjList.Add(new EnterMainPanelObj("newPlayerTuiGuang", false));
        s_panelObjList.Add(new EnterMainPanelObj("activity", false));
    }

    public void showNextPanel()
    {
        if (!getEnterMainPanelObjIShowByName("sign"))
        {
            setEnterMainPanelObjIShowByName("sign", true);

            if (!Sign30RecordData.getInstance().todayIsSign())
            {
                Sign30PanelScript.create();
            }
            else
            {
                showNextPanel();
            }
        }
        else if (!getEnterMainPanelObjIShowByName("newPlayerTuiGuang"))
        {
            setEnterMainPanelObjIShowByName("newPlayerTuiGuang", true);

            // 显示新人推广
            if (!OtherData.s_mainScript.checkShowNewPlayerTuiGuang())
            {
                showNextPanel();
            }
        }
        else if (!getEnterMainPanelObjIShowByName("activity"))
        {
            setEnterMainPanelObjIShowByName("activity", true);
            
            NoticePanelScript.create();
        }
        else
        {
        }
    }

    public bool getEnterMainPanelObjIShowByName(string panelName)
    {
        for (int i = 0; i < s_panelObjList.Count; i++)
        {
            if (s_panelObjList[i].m_panelName.CompareTo(panelName) == 0)
            {
                return s_panelObjList[i].m_isShow;
            }
        }

        return true;
    }

    public void setEnterMainPanelObjIShowByName(string panelName,bool isShow)
    {
        for (int i = 0; i < s_panelObjList.Count; i++)
        {
            if (s_panelObjList[i].m_panelName.CompareTo(panelName) == 0)
            {
                s_panelObjList[i].m_isShow = isShow;
                return;
            }
        }
    }
}

public class EnterMainPanelObj
{
    public string m_panelName = "";
    public bool m_isShow = false;

    public EnterMainPanelObj(string panelName,bool isShow)
    {
        m_panelName = panelName;
        m_isShow = isShow;
    }
}