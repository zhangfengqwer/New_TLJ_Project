﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUserInfoPanelScript : MonoBehaviour {

    public GameObject m_headIcon;
    public GameObject m_gameobj_up;
    public GameObject m_gameobj_down;

    public Text m_text_name;
    public Text m_text_zongjushu;
    public Text m_text_shenglv;
    public Text m_text_taopaolv;
    public Text m_text_meilizhi;

    public GameObject m_scrollView;
    public ScrollViewScript m_scrollViewScript;

    public static GameObject create(string uid)
    {
        GameObject prefab = Resources.Load("Prefabs/Game/GameUserInfoPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<GameUserInfoPanelScript>().setPlayer(uid);

        return obj;
    }

    public static GameObject create_ddz(string uid)
    {
        GameObject prefab = Resources.Load("Prefabs/Game/GameUserInfoPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<GameUserInfoPanelScript>().setPlayer_ddz(uid);

        return obj;
    }

    void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUserInfoPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUserInfoPanelScript_hotfix", "Start", null, null);
            return;
        }

        m_scrollViewScript = m_scrollView.GetComponent<ScrollViewScript>();

        loadHuDongDaoJu();
    }

    public void setPlayer(string uid)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUserInfoPanelScript_hotfix", "setPlayer"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUserInfoPanelScript_hotfix", "setPlayer", null, uid);
            return;
        }

        if (uid.CompareTo(UserData.uid) == 0)
        {
            m_text_name.text = UserData.name;

            if (UserData.gameData.allGameCount == 0)
            {
                m_text_shenglv.text = "0%";
                m_text_taopaolv.text = "0%";
            }
            else
            {
                float shenglv = (float)UserData.gameData.winCount / (float)UserData.gameData.allGameCount * 100.0f;
                float taopaolv = (float)UserData.gameData.runCount / (float)UserData.gameData.allGameCount * 100.0f;

                m_text_shenglv.text = (int)shenglv + "%";
                m_text_taopaolv.text = (int)taopaolv + "%";
            }

            m_text_zongjushu.text = UserData.gameData.allGameCount.ToString();
            m_text_meilizhi.text = UserData.gameData.meiliZhi.ToString();

            m_gameobj_down.transform.localScale = new Vector3(0, 0, 0);

            m_headIcon.GetComponent<HeadIconScript>().setIcon(UserData.head);
        }
        else
        {
            PlayerData playerData = GameData.getInstance().getPlayerDataByUid(uid);
                        
            m_text_name.text = playerData.m_name;

            if (playerData.m_allGameCount == 0)
            {
                m_text_shenglv.text = "0%";
                m_text_taopaolv.text = "0%";
            }
            else
            {
                float shenglv = (float)playerData.m_winCount / (float)playerData.m_allGameCount * 100.0f;
                float taopaolv = (float)playerData.m_runCount / (float)playerData.m_allGameCount * 100.0f;

                m_text_shenglv.text = (int)shenglv + "%";
                m_text_taopaolv.text = (int)taopaolv + "%";
            }

            m_text_zongjushu.text = playerData.m_allGameCount.ToString();
            m_text_meilizhi.text = playerData.m_meiliZhi.ToString();

            m_headIcon.GetComponent<HeadIconScript>().setIcon(playerData.m_head);
        }
    }

    public void setPlayer_ddz(string uid)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUserInfoPanelScript_hotfix", "setPlayer"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUserInfoPanelScript_hotfix", "setPlayer", null, uid);
            return;
        }

        if (uid.CompareTo(UserData.uid) == 0)
        {
            m_text_name.text = UserData.name;

            if (UserData.gameData.allGameCount == 0)
            {
                m_text_shenglv.text = "0%";
                m_text_taopaolv.text = "0%";
            }
            else
            {
                float shenglv = (float)UserData.gameData.winCount / (float)UserData.gameData.allGameCount * 100.0f;
                float taopaolv = (float)UserData.gameData.runCount / (float)UserData.gameData.allGameCount * 100.0f;

                m_text_shenglv.text = (int)shenglv + "%";
                m_text_taopaolv.text = (int)taopaolv + "%";
            }

            m_text_zongjushu.text = UserData.gameData.allGameCount.ToString();
            m_text_meilizhi.text = UserData.gameData.meiliZhi.ToString();

            m_gameobj_down.transform.localScale = new Vector3(0, 0, 0);

            m_headIcon.GetComponent<HeadIconScript>().setIcon(UserData.head);
        }
        else
        {
            PlayerData playerData = DDZ_GameData.getInstance().getPlayerDataByUid(uid);

            m_text_name.text = playerData.m_name;

            if (playerData.m_allGameCount == 0)
            {
                m_text_shenglv.text = "0%";
                m_text_taopaolv.text = "0%";
            }
            else
            {
                float shenglv = (float)playerData.m_winCount / (float)playerData.m_allGameCount * 100.0f;
                float taopaolv = (float)playerData.m_runCount / (float)playerData.m_allGameCount * 100.0f;

                m_text_shenglv.text = (int)shenglv + "%";
                m_text_taopaolv.text = (int)taopaolv + "%";
            }

            m_text_zongjushu.text = playerData.m_allGameCount.ToString();
            m_text_meilizhi.text = playerData.m_meiliZhi.ToString();

            m_headIcon.GetComponent<HeadIconScript>().setIcon(playerData.m_head);
        }
    }

    public void loadHuDongDaoJu()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameUserInfoPanelScript_hotfix", "loadHuDongDaoJu"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameUserInfoPanelScript_hotfix", "loadHuDongDaoJu", null, null);
            return;
        }

        m_scrollViewScript.clear();

        for (int i = 0; i < HuDongData.getInstance().getHuDongDataList().Count; i++)
        {
            GameObject prefab = Resources.Load("Prefabs/UI/Item/Item_hudong_Scroll") as GameObject;
            GameObject obj = MonoBehaviour.Instantiate(prefab);
            obj.GetComponent<Item_hudong_Scroll_Script>().m_parentScript = this;
            obj.GetComponent<Item_hudong_Scroll_Script>().setHuDongPropData(HuDongData.getInstance().getHuDongDataList()[i]);

            obj.transform.name = HuDongData.getInstance().getHuDongDataList()[i].m_id.ToString();

            m_scrollViewScript.addItem(obj);
        }

        m_scrollViewScript.addItemEnd();
    }
}
