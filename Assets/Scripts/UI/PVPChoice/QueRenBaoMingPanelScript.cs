using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QueRenBaoMingPanelScript : MonoBehaviour {

    public Text m_text_roomname;
    public Text m_text_baomingfei;
    public PVPGameRoomData m_PVPGameRoomData;

    MainScript m_mainScript;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/QueRenBaoMingPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    public void setData(PVPGameRoomData pVPGameRoomData)
    {
        m_PVPGameRoomData = pVPGameRoomData;

        m_text_roomname.text = m_PVPGameRoomData.gameroomname;

        if (m_PVPGameRoomData.baomingfei.CompareTo("0") == 0)
        {
            m_text_baomingfei.text = "免费";
        }
        else
        {
            List<string> list = new List<string>();
            CommonUtil.splitStr(m_PVPGameRoomData.baomingfei, list, ':');

            if (list[0].CompareTo("1") == 0)
            {
                m_text_baomingfei.text = "金币*" + list[1];
            }
            else
            {
                string prop_name = PropData.getInstance().getPropInfoById(int.Parse(list[0])).m_name;
                m_text_baomingfei.text = prop_name + "*" + list[1];
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        m_mainScript = GameObject.Find("Canvas").GetComponent<MainScript>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickBaoMing()
    {
        Destroy(gameObject);
        reqJoinRoom();
    }

    //-------------------------------------------------------------
    // 请求加入房间
    public void reqJoinRoom()
    {
        m_mainScript.showWaitMatchPanel(10);

        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_JingJiChang;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_JoinGame;
        data["gameroomtype"] = m_PVPGameRoomData.gameroomtype;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());

        GameData.getInstance().m_tag = TLJCommon.Consts.Tag_JingJiChang;
        GameData.getInstance().m_gameRoomType = m_PVPGameRoomData.gameroomtype;
    }
}
