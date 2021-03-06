﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PVPEndPanelScript : MonoBehaviour {

    public GameScript m_parentScript;

    public Image m_image_itemContent;
    public Text m_text_mingci;

    public static GameObject create(GameScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/PVPEndPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<PVPEndPanelScript>().m_parentScript = parentScript;

        return obj;
    }

    // Use this for initialization
    void Start()
    {
        OtherData.s_pvpEndPanelScript = this;
        
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPEndPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPEndPanelScript_hotfix", "Start", null, null);
            return;
        }

        initUI_Image();
    }

    public void initUI_Image()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPEndPanelScript_hotfix", "initUI_Image"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPEndPanelScript_hotfix", "initUI_Image", null, null);
            return;
        }

        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_bg/Image").GetComponent<Image>(), "gameresult.unity3d", "bg");
    }

    public void setData(int mingci,string pvpreward)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPEndPanelScript_hotfix", "setData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPEndPanelScript_hotfix", "setData", null, mingci, pvpreward);
            return;
        }

        m_text_mingci.text = mingci.ToString();

        List<string> list1 = new List<string>();
        CommonUtil.splitStr(pvpreward, list1, ';');

        for (int i = 0; i < list1.Count; i++)
        {
            List<string> list2 = new List<string>();
            CommonUtil.splitStr(list1[i], list2, ':');

            int id = int.Parse(list2[0]);
            int num = int.Parse(list2[1]);

            GameObject prefab = Resources.Load("Prefabs/UI/Item/Item_reward") as GameObject;
            GameObject obj = GameObject.Instantiate(prefab, m_image_itemContent.transform);
            obj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            CommonUtil.setImageSprite(obj.transform.Find("Image_icon").GetComponent<Image>(), GameUtil.getPropIconPath(id));
            obj.transform.Find("Text_num").GetComponent<Text>().text = "x" + num;

            float x = CommonUtil.getPosX(list1.Count, 130, i, 0);
            obj.transform.localPosition = new Vector3(x, 0, 0);
        }
    }

    public void onClickExit()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPEndPanelScript_hotfix", "onClickExit"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPEndPanelScript_hotfix", "onClickExit", null, null);
            return;
        }

        //m_parentScript.onClickExitRoom();
        SceneManager.LoadScene("MainScene");
    }

    public void onClickShare()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PVPEndPanelScript_hotfix", "onClickShare"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PVPEndPanelScript_hotfix", "onClickShare", null, null);
            return;
        }

        //m_parentScript.onClickExitRoom();
        //        ToastScript.createToast("暂未开放");
        ChoiceShareScript.Create("我在疯狂升级比赛场赢得了胜利，话费、徽章等你来拿！", "");
    }
}
