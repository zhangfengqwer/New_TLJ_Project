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

    public PVPGameRoomData m_PVPGameRoomData;

    // Use this for initialization
    void Start () {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVP_List_Item_Script", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVP_List_Item_Script", "Start", null, null);
            return;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void setPVPGameRoomData(PVPGameRoomData PVPGameRoomData)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVP_List_Item_Script", "setPVPGameRoomData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVP_List_Item_Script", "setPVPGameRoomData", null, PVPGameRoomData);
            return;
        }

        m_PVPGameRoomData = PVPGameRoomData;

        m_text_changci.text = m_PVPGameRoomData.gameroomname;
        m_text_kaisairenshu.text = "满" + m_PVPGameRoomData.kaisairenshu.ToString() + "人开赛";

        // 左边场次类型图标：金币、蓝钻石
        {
            CommonUtil.setImageSprite(m_image_changci_icon, GameUtil.getPropIconPath(m_PVPGameRoomData.reward_id));
        }

        if (m_PVPGameRoomData.baomingfei.CompareTo("0") == 0)
        {
            m_text_baomingfei.text = "免费";
            m_image_baomingfei_icon.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            // 报名费类型：金币、蓝钻石
            {
                List<string> list = new List<string>();
                CommonUtil.splitStr(m_PVPGameRoomData.baomingfei, list, ':');

                CommonUtil.setImageSprite(m_image_baomingfei_icon, GameUtil.getPropIconPath(m_PVPGameRoomData.baomingfei_id));
                m_text_baomingfei.text = " *" + m_PVPGameRoomData.baomingfei_num;
            }
        }

        m_text_baomingrenshu.text = "已报名人数：" + m_PVPGameRoomData.baomingrenshu;

        InvokeRepeating("onInvoke",5,5);
    }

    void onInvoke()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVP_List_Item_Script", "onInvoke"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVP_List_Item_Script", "onInvoke", null, null);
            return;
        }

        int i = RandomUtil.getRandom(-30,30);
        m_text_baomingrenshu.text = "已报名人数：" + (m_PVPGameRoomData.baomingrenshu + i);
    }

    public void onClickBaoMing()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVP_List_Item_Script", "onClickBaoMing"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVP_List_Item_Script", "onClickBaoMing", null, null);
            return;
        }

        // 检查是否有足够的报名费
        {
            if (m_PVPGameRoomData.baomingfei.CompareTo("0") != 0)
            {
                // 报名费类型：金币、蓝钻石
                {
                    List<string> list = new List<string>();
                    CommonUtil.splitStr(m_PVPGameRoomData.baomingfei, list, ':');

                    int id = int.Parse(list[0]);
                    int num = int.Parse(list[1]);

                    // 金币
                    if (id == 1)
                    {
                        if (UserData.gold < num)
                        {
                            ToastScript.createToast("您的报名费不足");

                            return;
                        }
                    }
                    // 蓝钻石
                    else
                    {
                        bool isOK = false;
                        for (int i = 0; i < UserData.propData.Count; i++)
                        {
                            if (UserData.propData[i].prop_id == id)
                            {
                                if (UserData.propData[i].prop_num >= num)
                                {
                                    isOK = true;
                                }
                            }
                        }

                        if (!isOK)
                        {
                            ToastScript.createToast("您的报名费不足");

                            return;
                        }
                    }
                }
            }
        }

        QueRenBaoMingPanelScript queRenBaoMingPanelScript = QueRenBaoMingPanelScript.create().GetComponent<QueRenBaoMingPanelScript>() ;
        queRenBaoMingPanelScript.setData(m_PVPGameRoomData);
    }
}
