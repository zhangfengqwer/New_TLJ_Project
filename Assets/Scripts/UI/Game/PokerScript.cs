using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerScript : MonoBehaviour {

    int m_num;
    int m_pokerType;
    bool m_isSelect = false;

    public Image m_image_num;
    public Image m_image_small_icon;
    public Image m_image_big_icon;

    public static GameObject createPoker()
    {
        GameObject prefabs = Resources.Load("Prefabs/Game/Poker") as GameObject;
        GameObject obj = Instantiate(prefabs);
        return obj;
    }

	// Use this for initialization
	void Start ()
    {
        m_isSelect = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void initPoker(int num,int pokerType)
    {
        m_num = num;
        m_pokerType = pokerType;
        
        switch (m_pokerType)
        {
            case (int)TLJCommon.Consts.PokerType.PokerType_FangKuai:
                {
                    CommonUtil.setImageSprite(m_image_num, "Sprites/Game/Poker/red_" + m_num);
                    CommonUtil.setImageSprite(m_image_small_icon, "Sprites/Game/Poker/icon_fangkuai");
                    CommonUtil.setImageSprite(m_image_big_icon, "Sprites/Game/Poker/icon_fangkuai");
                }
                break;

            case (int)TLJCommon.Consts.PokerType.PokerType_HeiTao:
                {
                    CommonUtil.setImageSprite(m_image_num, "Sprites/Game/Poker/black_" + m_num);
                    CommonUtil.setImageSprite(m_image_small_icon, "Sprites/Game/Poker/icon_heitao");
                    CommonUtil.setImageSprite(m_image_big_icon, "Sprites/Game/Poker/icon_heitao");
                }
                break;

            case (int)TLJCommon.Consts.PokerType.PokerType_HongTao:
                {
                    CommonUtil.setImageSprite(m_image_num, "Sprites/Game/Poker/red_" + m_num);
                    CommonUtil.setImageSprite(m_image_small_icon, "Sprites/Game/Poker/icon_hongtao");
                    CommonUtil.setImageSprite(m_image_big_icon, "Sprites/Game/Poker/icon_hongtao");
                }
                break;

            case (int)TLJCommon.Consts.PokerType.PokerType_MeiHua:
                {
                    CommonUtil.setImageSprite(m_image_num, "Sprites/Game/Poker/black_" + m_num);
                    CommonUtil.setImageSprite(m_image_small_icon, "Sprites/Game/Poker/icon_meihua");
                    CommonUtil.setImageSprite(m_image_big_icon, "Sprites/Game/Poker/icon_meihua");
                }
                break;

            case (int)TLJCommon.Consts.PokerType.PokerType_Wang:
                {
                    if (num == 15)
                    {
                        CommonUtil.setImageSprite(m_image_num, "Sprites/Game/Poker/black_" + m_num);
                        CommonUtil.setImageSprite(m_image_big_icon, "Sprites/Game/Poker/icon_xiaowang");
                    }
                    else if (num == 16)
                    {
                        CommonUtil.setImageSprite(m_image_num, "Sprites/Game/Poker/red_" + m_num);
                        CommonUtil.setImageSprite(m_image_big_icon, "Sprites/Game/Poker/icon_dawang");
                    }

                    m_image_num.SetNativeSize();
                    m_image_big_icon.SetNativeSize();

                    m_image_small_icon.transform.localScale = new Vector3(0,0,0);
                    m_image_big_icon.transform.localScale = new Vector3(1, 1, 1);
                }
                break;
        }
    }

    public int getPokerNum()
    {
        return m_num;
    }

    public int getPokerType()
    {
        return m_pokerType;
    }
    
    public bool getIsSelect()
    {
        return m_isSelect;
    }

    public void setIsSelect(bool isSelect)
    {
        m_isSelect = isSelect;
    }

    public void onClickPoker()
    {
        if (m_isSelect)
        {
            //gameObject.transform.localPosition -= new Vector3(0,30,0);
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x,-240,0);
            m_isSelect = false;
        }
        else
        {
            //gameObject.transform.localPosition += new Vector3(0, 30, 0);
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, -210, 0);
            m_isSelect = true;
        }
    }
}
