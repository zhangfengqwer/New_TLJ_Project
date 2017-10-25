using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PVP_List_Item_Script : MonoBehaviour {

    public Text m_text_changci;
    public Text m_text_kaisairenshu;
    public Text m_text_baomingfei;
    public Text m_text_baomingrenshu;

    public Button m_button_baoming;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setChangCi(string str)
    {
        m_text_changci.text = str;
    }

    public void setKaiSaiRenShu(string str)
    {
        m_text_kaisairenshu.text = str;
    }

    public void setBaoMingFei(string str)
    {
        m_text_baomingfei.text = str;
    }

    public void setBaoMingRenShu(string str)
    {
        m_text_baomingrenshu.text = str;
    }

    public void onClickBaoMing()
    {
        //SceneManager.LoadScene("GameScene");

        reqJoinRoom();
    }

    //-------------------------------------------------------------
    // 请求加入房间
    public void reqJoinRoom()
    {
        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_XiuXianChang;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_JoinGame;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());
    }
}
