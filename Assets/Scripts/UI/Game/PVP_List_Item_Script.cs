using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PVP_List_Item_Script : MonoBehaviour {

    public Image m_image_changci_icon;
    public Image m_image_baomingfei_icon;
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
            m_text_baomingfei.text = "免费";
            m_image_baomingfei_icon.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            List<string> list = new List<string>();
            CommonUtil.splitStr(m_PVPGameRoomData.baomingfei, list, ':');

            CommonUtil.setImageSprite(m_image_baomingfei_icon, GameUtil.getPropIconPath(int.Parse(list[0])));
            m_text_baomingfei.text = list[1];
        }
        
        m_text_baomingrenshu.text = "已报名人数：" + m_PVPGameRoomData.baomingrenshu;
    }

    public void onClickBaoMing()
    {
        QueRenBaoMingPanelScript queRenBaoMingPanelScript = QueRenBaoMingPanelScript.create().GetComponent<QueRenBaoMingPanelScript>() ;
        queRenBaoMingPanelScript.setData(m_PVPGameRoomData);
    }
}
