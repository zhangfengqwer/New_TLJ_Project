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

    PVPGameRoomData m_PVPGameRoomData;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void setPVPGameRoomData(PVPGameRoomData PVPGameRoomData)
    {
        m_PVPGameRoomData = PVPGameRoomData;

        m_text_changci.text = m_PVPGameRoomData.gameroomname;
        m_text_kaisairenshu.text = "满" + m_PVPGameRoomData.kaisairenshu.ToString() + "人开赛";

        if (m_PVPGameRoomData.baomingfei.CompareTo("0") == 0)
        {
            m_text_baomingfei.text = "报名费:免费";
        }
        else
        {
            List<string> list = new List<string>();
            CommonUtil.splitStr(m_PVPGameRoomData.baomingfei, list, ':');

            if (list[0].CompareTo("1") == 0)
            {
                m_text_baomingfei.text = "报名费:金币*" + list[1];
            }
            else
            {
                string prop_name = PropData.getInstance().getPropInfoById(int.Parse(list[0])).m_name;
                m_text_baomingfei.text = "报名费:" + prop_name + "*" + list[1];
            }
        }
        
        m_text_baomingrenshu.text = "已报名人数：" + m_PVPGameRoomData.baomingrenshu;
    }

    public void onClickBaoMing()
    {
        QueRenBaoMingPanelScript queRenBaoMingPanelScript = QueRenBaoMingPanelScript.create().GetComponent<QueRenBaoMingPanelScript>() ;
        queRenBaoMingPanelScript.setData(m_PVPGameRoomData);
    }
}
