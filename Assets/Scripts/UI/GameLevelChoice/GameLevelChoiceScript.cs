using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLevelChoiceScript : MonoBehaviour {

    public GameObject m_obj;

    public enum GameChangCiType
    {
        GameChangCiType_jingdian,
        GameChangCiType_chaodi,
    }

    GameChangCiType m_gameChangCiType;

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
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void setGameChangCiType(GameChangCiType gameChangCiType)
    {
        m_gameChangCiType = gameChangCiType;

        if (m_gameChangCiType == GameChangCiType.GameChangCiType_jingdian)
        {
            m_obj.transform.localPosition = new Vector3(100,-8,0);
        }
        else if (m_gameChangCiType == GameChangCiType.GameChangCiType_chaodi)
        {
            m_obj.transform.localPosition = new Vector3(520,-8,0);
        }
    }

    public void onClickChuJi()
    {
        if (m_gameChangCiType == GameChangCiType.GameChangCiType_jingdian)
        {
            LogUtil.Log("进入经典初级场");
            GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ChuJi);
        }
        else if (m_gameChangCiType == GameChangCiType.GameChangCiType_chaodi)
        {
            LogUtil.Log("进入抄底初级场");
            GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_ChuJi);
        }
        if (UserData.gold < 1500)
        {
            ToastScript.createToast("金币不足，请前去购买");
            return;
        }
        Destroy(gameObject);

        GameData.getInstance().m_tag = TLJCommon.Consts.Tag_XiuXianChang;
        SceneManager.LoadScene("GameScene");
    }

    public void onClickZhongJi()
    {
        if (m_gameChangCiType == GameChangCiType.GameChangCiType_jingdian)
        {
            LogUtil.Log("进入经典中级场");
            GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ZhongJi);

            if (UserData.gameData.xianxianJDPrimary < 100)
            {
                ToastScript.createToast("新手场必须满足100场");
                return;
            }
        }
        else if (m_gameChangCiType == GameChangCiType.GameChangCiType_chaodi)
        {
            LogUtil.Log("进入抄底中级场");
            GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_ZhongJi);
            if (UserData.gameData.xianxianCDPrimary < 100)
            {
                ToastScript.createToast("新手场必须满足100场");
                return;
            }
        }
        if (UserData.gold < 35000)
        {
            ToastScript.createToast("金币不足，请前去购买");
            return;
        }
        Destroy(gameObject);
        
        GameData.getInstance().m_tag = TLJCommon.Consts.Tag_XiuXianChang;
        SceneManager.LoadScene("GameScene");
    }

    public void onClickGaoJi()
    {
        if (m_gameChangCiType == GameChangCiType.GameChangCiType_jingdian)
        {
            LogUtil.Log("进入经典高级场");
            GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_JingDian_GaoJi);

            if (UserData.gameData.xianxianJDMiddle < 300)
            {
                ToastScript.createToast("精英场必须满足300场");
                return;
            }
        }
        else if (m_gameChangCiType == GameChangCiType.GameChangCiType_chaodi)
        {
            LogUtil.Log("进入抄底高级场");
            GameData.getInstance().setGameRoomType(TLJCommon.Consts.GameRoomType_XiuXian_ChaoDi_GaoJi);
            if (UserData.gameData.xianxianCDMiddle < 300)
            {
                ToastScript.createToast("精英场必须满足300场");
                return;
            }
        }
        if (UserData.gold < 100000)
        {
            ToastScript.createToast("金币不足，请前去购买");
            return;
        }

        Destroy(gameObject);

       
        GameData.getInstance().m_tag = TLJCommon.Consts.Tag_XiuXianChang;
        SceneManager.LoadScene("GameScene");
    }
}
