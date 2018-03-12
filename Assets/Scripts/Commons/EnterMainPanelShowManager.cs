using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMainPanelShowManager
{
    public static EnterMainPanelShowManager s_instance = null;

    public List<EnterMainPanelObj> s_panelObjList = new List<EnterMainPanelObj>();

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EnterMainPanelShowManager_hotfix", "init"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EnterMainPanelShowManager_hotfix", "init", null, null);
            return;
        }

        s_panelObjList.Add(new EnterMainPanelObj("sign", false));
        s_panelObjList.Add(new EnterMainPanelObj("newPlayerTuiGuang", false));
        s_panelObjList.Add(new EnterMainPanelObj("activity", false));
        s_panelObjList.Add(new EnterMainPanelObj("huizhangduihuan", false));
    }

    public void showNextPanel()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EnterMainPanelShowManager_hotfix", "showNextPanel"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EnterMainPanelShowManager_hotfix", "showNextPanel", null, null);
            return;
        }

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
        else if (!getEnterMainPanelObjIShowByName("huizhangduihuan"))
        {
            setEnterMainPanelObjIShowByName("huizhangduihuan", true);

            string time = "isShowHuiZhangDuiHuan_" + CommonUtil.getCurYearMonthDay();
            if (PlayerPrefs.GetInt(time, 0) == 0)
            {
                PlayerPrefs.SetInt(time, 1);

                MedalDuiHuanPanelScript.create();
            }
        }
        else
        {
        }
    }

    public bool getEnterMainPanelObjIShowByName(string panelName)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EnterMainPanelShowManager_hotfix", "getEnterMainPanelObjIShowByName"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EnterMainPanelShowManager_hotfix", "getEnterMainPanelObjIShowByName", null, panelName);
            return b;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EnterMainPanelShowManager_hotfix", "setEnterMainPanelObjIShowByName"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EnterMainPanelShowManager_hotfix", "setEnterMainPanelObjIShowByName", null, panelName, isShow);
            return;
        }

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