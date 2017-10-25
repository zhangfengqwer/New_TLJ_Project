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
            m_obj.transform.localPosition = new Vector3(54,120,0);
        }
        else if (m_gameChangCiType == GameChangCiType.GameChangCiType_chaodi)
        {
            m_obj.transform.localPosition = new Vector3(500,120,0);
        }
    }

    public void onClickChuJi()
    {
        if (m_gameChangCiType == GameChangCiType.GameChangCiType_jingdian)
        {
            Debug.Log("进入经典初级场");
        }
        else if (m_gameChangCiType == GameChangCiType.GameChangCiType_chaodi)
        {
            Debug.Log("进入抄底初级场");
        }

        Destroy(gameObject);
        SceneManager.LoadScene("GameScene");
        GameData.getInstance().m_gameRoomType = TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ChuJi;
    }

    public void onClickZhongJi()
    {
        if (m_gameChangCiType == GameChangCiType.GameChangCiType_jingdian)
        {
            Debug.Log("进入经典中级场");
        }
        else if (m_gameChangCiType == GameChangCiType.GameChangCiType_chaodi)
        {
            Debug.Log("进入抄底中级场");
        }

        Destroy(gameObject);
        SceneManager.LoadScene("GameScene");
        GameData.getInstance().m_gameRoomType = TLJCommon.Consts.GameRoomType_XiuXian_JingDian_ZhongJi;
    }

    public void onClickGaoJi()
    {
        if (m_gameChangCiType == GameChangCiType.GameChangCiType_jingdian)
        {
            Debug.Log("进入经典高级场");
        }
        else if (m_gameChangCiType == GameChangCiType.GameChangCiType_chaodi)
        {
            Debug.Log("进入抄底高级场");
        }

        Destroy(gameObject);
        SceneManager.LoadScene("GameScene");
        GameData.getInstance().m_gameRoomType = TLJCommon.Consts.GameRoomType_XiuXian_JingDian_GaoJi;
    }
}
