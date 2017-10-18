using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUserInfoPanelScript : MonoBehaviour {

    public GameObject m_gameobj_up;
    public GameObject m_gameobj_down;
    public GameObject m_scrollView;
    ScrollViewScript m_scrollViewScript;

    public static GameObject create(string uid)
    {
        GameObject prefab = Resources.Load("Prefabs/Game/GameUserInfoPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);

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
            m_gameobj_down.transform.localScale = new Vector3(0, 0, 0);
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
