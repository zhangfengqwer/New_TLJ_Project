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
    public Text m_text_shenglv;
    public Text m_text_taopaolv;
    public Text m_text_meilizhi;

    public GameObject m_scrollView;
    ScrollViewScript m_scrollViewScript;

    public static GameObject create(string uid)
    {
        GameObject prefab = Resources.Load("Prefabs/Game/GameUserInfoPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<GameUserInfoPanelScript>().setPlayer(uid);

        return obj;
    }

    void Start()
    {
        m_scrollViewScript = m_scrollView.GetComponent<ScrollViewScript>();

        loadHuDongDaoJu();
    }

    public void setPlayer(string uid)
    {
        if (uid.CompareTo(UserData.uid) == 0)
        {
            m_text_name.text = UserData.name;

            float shenglv = (float)UserData.gameData.winCount / (float)UserData.gameData.allGameCount * 100.0f;
            float taopaolv = (float)UserData.gameData.runCount / (float)UserData.gameData.allGameCount * 100.0f;

            m_text_shenglv.text = (int)shenglv + "%";
            m_text_taopaolv.text = (int)taopaolv + "%";
            
            m_text_meilizhi.text = UserData.gameData.meiliZhi.ToString();

            m_gameobj_down.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            PlayerData playerData = GameData.getInstance().getPlayerDataByUid(uid);
                        
            m_text_name.text = playerData.m_name;

            float shenglv = (float)playerData.m_winCount / (float)playerData.m_allGameCount * 100.0f;
            float taopaolv = (float)playerData.m_runCount / (float)playerData.m_allGameCount * 100.0f;

            m_text_shenglv.text = (int)shenglv + "%";
            m_text_taopaolv.text = (int)taopaolv + "%";

            m_text_meilizhi.text = playerData.m_meiliZhi.ToString();

            m_headIcon.GetComponent<HeadIconScript>().setIcon(playerData.m_head);
        }
    }

    public void loadHuDongDaoJu()
    {
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
