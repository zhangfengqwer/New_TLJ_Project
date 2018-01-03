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
    }

    public void setHead(string headPath)
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load("Sprites/Head/head_1", typeof(Sprite)) as Sprite;
    }

    public void setName(string name)
    {
        m_textName.text = name;
    }

    public void setVipLevel(int vipLevel)
    {
        CommonUtil.setImageSprite(m_imageVipLevel, "Sprites/Vip/user_vip_" + vipLevel);
        
        GameUtil.setNickNameFontColor(m_textName,vipLevel);
    }

    public void setGoldNum(int goldNum)
    {
        m_textGoldNum.text = goldNum.ToString();
    }

    public void onClickHead()
    {
        GameUserInfoPanelScript.create(m_uid);
    }
}
