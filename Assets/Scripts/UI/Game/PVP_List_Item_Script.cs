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
        SceneManager.LoadScene("GameScene");
    }
}
