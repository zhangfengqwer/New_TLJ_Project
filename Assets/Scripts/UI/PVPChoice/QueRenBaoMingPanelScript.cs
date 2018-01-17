using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QueRenBaoMingPanelScript : MonoBehaviour {

    public Text m_text_roomname;
    public Text m_text_baomingfei;
    public Image m_image_baomingfei_icon;
    public PVPGameRoomData m_PVPGameRoomData;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/QueRenBaoMingPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start()
    {
        OtherData.s_queRenBaoMingPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("QueRenBaoMingPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.QueRenBaoMingPanelScript_hotfix", "Start", null, null);
            return;
        }
    }

    public void setData(PVPGameRoomData pVPGameRoomData)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("QueRenBaoMingPanelScript_hotfix", "setData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.QueRenBaoMingPanelScript_hotfix", "setData", null, pVPGameRoomData);
            return;
        }

        m_PVPGameRoomData = pVPGameRoomData;

        m_text_roomname.text = m_PVPGameRoomData.gameroomname;

        if (m_PVPGameRoomData.baomingfei.CompareTo("0") == 0)
        {
            m_text_baomingfei.text = "免费";
            m_image_baomingfei_icon.transform.localScale = new Vector3(0,0,0);
        }
        else
        {
            List<string> list = new List<string>();
            CommonUtil.splitStr(m_PVPGameRoomData.baomingfei, list, ':');

            // 报名费
            m_text_baomingfei.text = list[1];

            // 报名费类型：金币、蓝钻石
            m_image_baomingfei_icon.transform.localScale = new Vector3(1, 1, 1);
            CommonUtil.setImageSprite(m_image_baomingfei_icon,GameUtil.getPropIconPath(int.Parse(list[0])));
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickBaoMing()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("QueRenBaoMingPanelScript_hotfix", "onClickBaoMing"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.QueRenBaoMingPanelScript_hotfix", "onClickBaoMing", null, null);
            return;
        }

        Destroy(gameObject);
        reqJoinRoom();
    }

    //-------------------------------------------------------------
    // 请求加入房间
    public void reqJoinRoom()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("QueRenBaoMingPanelScript_hotfix", "reqJoinRoom"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.QueRenBaoMingPanelScript_hotfix", "reqJoinRoom", null, null);
            return;
        }

        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_JingJiChang;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_JoinGame;
        data["gameroomtype"] = m_PVPGameRoomData.gameroomtype;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());

        GameData.getInstance().m_tag = TLJCommon.Consts.Tag_JingJiChang;
        GameData.getInstance().setGameRoomType(m_PVPGameRoomData.gameroomtype);
    }
}
