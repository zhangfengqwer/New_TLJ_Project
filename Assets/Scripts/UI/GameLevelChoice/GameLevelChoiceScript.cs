﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLevelChoiceScript : MonoBehaviour {

    public GameObject m_obj;

    public Text m_text_chuji_onlineCount;
    public Text m_text_zhongji_onlineCount;
    public Text m_text_gaoji_onlineCount;

    public enum GameChangCiType
    {
        GameChangCiType_jingdian,
        GameChangCiType_chaodi,
    }

    public GameChangCiType m_gameChangCiType;

    public static GameObject create(GameChangCiType gameChangCiType)
    {
        GameObject prefab = Resources.Load("Prefabs/Game/GameLevelChoice") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

        obj.GetComponent<GameLevelChoiceScript>().setGameChangCiType(gameChangCiType);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        OtherData.s_gameLevelChoiceScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameLevelChoiceScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameLevelChoiceScript_hotfix", "Start", null, null);
            return;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void setGameChangCiType(GameChangCiType gameChangCiType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameLevelChoiceScript_hotfix", "setGameChangCiType"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameLevelChoiceScript_hotfix", "setGameChangCiType", null, gameChangCiType);
            return;
        }

        m_gameChangCiType = gameChangCiType;

        if (m_gameChangCiType == GameChangCiType.GameChangCiType_jingdian)
        {
            m_obj.transform.localPosition = new Vector3(100,-8,0);
        }
        else if (m_gameChangCiType == GameChangCiType.GameChangCiType_chaodi)
        {
            m_obj.transform.localPosition = new Vector3(520,-8,0);
        }

        // 在线人数
        {
            m_text_chuji_onlineCount.text = RandomUtil.getRandom(100,500).ToString();
            m_text_zhongji_onlineCount.text = RandomUtil.getRandom(100, 500).ToString();
            m_text_gaoji_onlineCount.text = RandomUtil.getRandom(100, 500).ToString();
        }
    }

    public void onClickChuJi()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameLevelChoiceScript_hotfix", "onClickChuJi"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameLevelChoiceScript_hotfix", "onClickChuJi", null, null);
            return;
        }

        if (!GameUtil.checkCanEnterRoom(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ChuJi))
        {
            return;
        }

        if (m_gameChangCiType == GameChangCiType.GameChangCiType_jingdian)
        {
            GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ChuJi);
        }
        else if (m_gameChangCiType == GameChangCiType.GameChangCiType_chaodi)
        {
            GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_ChuJi);
        }
       
        Destroy(gameObject);

        if (OtherData.s_mainScript != null)
        {
            OtherData.s_mainScript.reqIsJoinRoom();
        }
        //GameData.getInstance().m_tag = TLJCommon.Consts.Tag_XiuXianChang;
        //SceneManager.LoadScene("GameScene");
    }

    public void onClickZhongJi()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameLevelChoiceScript_hotfix", "onClickZhongJi"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameLevelChoiceScript_hotfix", "onClickZhongJi", null, null);
            return;
        }

        if (!GameUtil.checkCanEnterRoom(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ZhongJi))
        {
            return;
        }

        if (m_gameChangCiType == GameChangCiType.GameChangCiType_jingdian)
        {
            GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ZhongJi);
        }
        else if (m_gameChangCiType == GameChangCiType.GameChangCiType_chaodi)
        {
            GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_ZhongJi);
        }
     
        Destroy(gameObject);

        if (OtherData.s_mainScript != null)
        {
            OtherData.s_mainScript.reqIsJoinRoom();
        }
        //GameData.getInstance().m_tag = TLJCommon.Consts.Tag_XiuXianChang;
        //SceneManager.LoadScene("GameScene");
    }

    public void onClickGaoJi()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("GameLevelChoiceScript_hotfix", "onClickGaoJi"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.GameLevelChoiceScript_hotfix", "onClickGaoJi", null, null);
            return;
        }

        if (!GameUtil.checkCanEnterRoom(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_GaoJi))
        {
            return;
        }

        if (m_gameChangCiType == GameChangCiType.GameChangCiType_jingdian)
        {
            GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_GaoJi);
        }
        else if (m_gameChangCiType == GameChangCiType.GameChangCiType_chaodi)
        {
            GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_GaoJi);
        }

        Destroy(gameObject);

        if (OtherData.s_mainScript != null)
        {
            OtherData.s_mainScript.reqIsJoinRoom();
        }
        //GameData.getInstance().m_tag = TLJCommon.Consts.Tag_XiuXianChang;
        //SceneManager.LoadScene("GameScene");
    }
}
