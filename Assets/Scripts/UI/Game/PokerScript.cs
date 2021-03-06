﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PokerScript : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerClickHandler,IPointerUpHandler
{
    public int m_num;
    public int m_pokerType;

    public bool m_isSelect = false;
    public bool m_isJump = false;
    public bool m_canTouch = false;

    public Image m_image_num;
    public Image m_image_small_icon;
    public Image m_image_big_icon;
    public Image m_image_zhupai;

    public static GameObject createPoker()
    {
        GameObject prefabs = Resources.Load("Prefabs/Game/Poker") as GameObject;
        GameObject obj = Instantiate(prefabs);
        return obj;
    }

	// Use this for initialization
	void Start ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PokerScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PokerScript_hotfix", "Start", null, null);
            return;
        }

        m_isSelect = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void initPoker(int num,int pokerType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PokerScript_hotfix", "initPoker"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PokerScript_hotfix", "initPoker", null, num, pokerType);
            return;
        }

        m_num = num;
        m_pokerType = pokerType;
        
        switch (m_pokerType)
        {
            case (int)TLJCommon.Consts.PokerType.PokerType_FangKuai:
                {
                    CommonUtil.setImageSpriteByAssetBundle(m_image_num,"poker.unity3d", "red_" + m_num);
                    CommonUtil.setImageSpriteByAssetBundle(m_image_small_icon, "poker.unity3d", "icon_fangkuai");
                    CommonUtil.setImageSpriteByAssetBundle(m_image_big_icon, "poker.unity3d", "icon_fangkuai");
                }
                break;

            case (int)TLJCommon.Consts.PokerType.PokerType_HeiTao:
                {
                    CommonUtil.setImageSpriteByAssetBundle(m_image_num, "poker.unity3d", "black_" + m_num);
                    CommonUtil.setImageSpriteByAssetBundle(m_image_small_icon, "poker.unity3d", "icon_heitao");
                    CommonUtil.setImageSpriteByAssetBundle(m_image_big_icon, "poker.unity3d", "icon_heitao");
                }
                break;

            case (int)TLJCommon.Consts.PokerType.PokerType_HongTao:
                {
                    CommonUtil.setImageSpriteByAssetBundle(m_image_num, "poker.unity3d", "red_" + m_num);
                    CommonUtil.setImageSpriteByAssetBundle(m_image_small_icon, "poker.unity3d", "icon_hongtao");
                    CommonUtil.setImageSpriteByAssetBundle(m_image_big_icon, "poker.unity3d", "icon_hongtao");
                }
                break;

            case (int)TLJCommon.Consts.PokerType.PokerType_MeiHua:
                {
                    CommonUtil.setImageSpriteByAssetBundle(m_image_num, "poker.unity3d", "black_" + m_num);
                    CommonUtil.setImageSpriteByAssetBundle(m_image_small_icon, "poker.unity3d", "icon_meihua");
                    CommonUtil.setImageSpriteByAssetBundle(m_image_big_icon, "poker.unity3d", "icon_meihua");
                }
                break;

            case (int)TLJCommon.Consts.PokerType.PokerType_Wang:
                {
                    if (num == 15)
                    {
                        CommonUtil.setImageSpriteByAssetBundle(m_image_num, "poker.unity3d", "black_" + m_num);
                        CommonUtil.setImageSpriteByAssetBundle(m_image_big_icon, "poker.unity3d", "icon_xiaowang");
                    }
                    else if (num == 16)
                    {
                        CommonUtil.setImageSpriteByAssetBundle(m_image_num, "poker.unity3d", "red_" + m_num);
                        CommonUtil.setImageSpriteByAssetBundle(m_image_big_icon, "poker.unity3d", "icon_dawang");
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

    public void setIsSelect(bool isSelect)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PokerScript_hotfix", "setIsSelect"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PokerScript_hotfix", "setIsSelect", null, isSelect);
            return;
        }

        m_isSelect = isSelect;

        if (m_isSelect)
        {
            CommonUtil.setImageColor(gameObject.GetComponent<Image>(), 195, 195, 195);
        }
        else
        {
            if (getIsJump())
            {
                CommonUtil.setImageColor(gameObject.GetComponent<Image>(), 195, 195, 195);
            }
            else
            {
                gameObject.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public bool getIsSelect()
    {
        return m_isSelect;
    }

    public void setIsJump(bool isJump)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PokerScript_hotfix", "setIsJump"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PokerScript_hotfix", "setIsJump", null, isJump);
            return;
        }

        m_isJump = isJump;

        gameObject.GetComponent<Image>().color = Color.white;

        if (m_isJump)
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, -195, 0);
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, -225, 0);
        }
    }

    public bool getIsJump()
    {
        return m_isJump;
    }

    public void showZhuPaiLogo()
    {
        m_image_zhupai.transform.localScale = new Vector3(1,1,1);
    }

    public void closeZhuPaiLogo()
    {
        m_image_zhupai.transform.localScale = new Vector3(0,0,0);
    }

    //------------------------------------------------------------------------------------------------------
    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_canTouch)
        {
            AudioScript.getAudioScript().playSound_XuanPai();
            
            setIsSelect(!m_isSelect);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PokerScript_hotfix", "OnPointerUp"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PokerScript_hotfix", "OnPointerUp", null, eventData);
            return;
        }

        if (m_canTouch)
        {
            // 升级
            for (int i = 0; i < GameData.getInstance().m_myPokerObjList.Count; i++)
            {
                PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>();

                pokerScript.setIsJump(pokerScript.getIsSelect());
            }

            // 斗地主
            for (int i = 0; i < DDZ_GameData.getInstance().m_myPokerObjList.Count; i++)
            {
                PokerScript pokerScript = DDZ_GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>();

                pokerScript.setIsJump(pokerScript.getIsSelect());
            }
        }
    }

    public static void setAllPokerWeiXuanZe()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PokerScript_hotfix", "setAllPokerWeiXuanZe"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PokerScript_hotfix", "setAllPokerWeiXuanZe", null, null);
            return;
        }

        // 升级
        for (int i = 0; i < GameData.getInstance().m_myPokerObjList.Count; i++)
        {
            PokerScript pokerScript = GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>();
            
            pokerScript.setIsSelect(false);
            pokerScript.setIsJump(false);
        }

        // 斗地主
        for (int i = 0; i < DDZ_GameData.getInstance().m_myPokerObjList.Count; i++)
        {
            PokerScript pokerScript = DDZ_GameData.getInstance().m_myPokerObjList[i].GetComponent<PokerScript>();

            pokerScript.setIsSelect(false);
            pokerScript.setIsJump(false);
        }
    }
}
