using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


public class OtherPlayerUIScript : MonoBehaviour {

    public enum Direction
    {
        Direction_Null,
        Direction_Up,
        Direction_Left,
        Direction_Right,
        Direction_Down,
    }

    public GameObject m_headIcon;
    public Text m_textName;
    public Text m_textGoldNum;
    public Image m_imageZhuangJiaIcon;
    public Image m_imageVipLevel;

    public string m_uid;
    public Direction m_direction = Direction.Direction_Null;

    static public GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/Game/OtherPlayerHeadUI") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab);
        obj.transform.SetParent(GameObject.Find("Canvas").transform);
        obj.transform.localScale = new Vector3(1,1,1);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OtherPlayerUIScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OtherPlayerUIScript_hotfix", "Start", null, null);
            return;
        }
    }

    // 此函数暂时没有地方引用
    public void setHead(string headPath)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OtherPlayerUIScript_hotfix", "setHead"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OtherPlayerUIScript_hotfix", "setHead", null, headPath);
            return;
        }

        gameObject.GetComponent<Image>().sprite = Resources.Load("Sprites/Head/head_1", typeof(Sprite)) as Sprite;
    }

    public void setName(string name)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OtherPlayerUIScript_hotfix", "setName"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OtherPlayerUIScript_hotfix", "setName", null, name);
            return;
        }

        m_textName.text = name;
    }

    public void setVipLevel(int vipLevel)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OtherPlayerUIScript_hotfix", "setVipLevel"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OtherPlayerUIScript_hotfix", "setVipLevel", null, vipLevel);
            return;
        }

        CommonUtil.setImageSprite(m_imageVipLevel, "Sprites/Vip/user_vip_" + vipLevel);
        
        GameUtil.setNickNameFontColor(m_textName,vipLevel);
    }

    public void setGoldNum(int goldNum)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OtherPlayerUIScript_hotfix", "setGoldNum"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OtherPlayerUIScript_hotfix", "setGoldNum", null, goldNum);
            return;
        }

        m_textGoldNum.text = goldNum.ToString();
    }

    public void onClickHead()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OtherPlayerUIScript_hotfix", "onClickHead"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OtherPlayerUIScript_hotfix", "onClickHead", null, null);
            return;
        }

        GameUserInfoPanelScript.create(m_uid);
    }
}
