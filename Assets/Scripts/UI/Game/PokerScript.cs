using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerScript : MonoBehaviour {

    int m_num;
    int m_pokerType;
    bool m_isSelect = false;

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
        if (getIsSelect())
        {
            gameObject.transform.Find("Text_state").GetComponent<Text>().text = "1";
        }
        else
        {
            gameObject.transform.Find("Text_state").GetComponent<Text>().text = "0";
        }
	}

    public void initPoker(int num,int pokerType)
    {
        m_num = num;
        m_pokerType = pokerType;

        if (num >= 2 && num <= 10)
        {
            gameObject.transform.Find("Text").GetComponent<Text>().text = num.ToString();
        }
        else
        {
            if (num == 11)
            {
                gameObject.transform.Find("Text").GetComponent<Text>().text = "J";
            }
            else if (num == 12)
            {
                gameObject.transform.Find("Text").GetComponent<Text>().text = "Q";
            }
            else if (num == 13)
            {
                gameObject.transform.Find("Text").GetComponent<Text>().text = "K";
            }
            else if (num == 14)
            {
                gameObject.transform.Find("Text").GetComponent<Text>().text = "A";
            }
            else if (num == 15)
            {
                gameObject.transform.Find("Text").GetComponent<Text>().text = "小王";
            }
            else if (num == 16)
            {
                gameObject.transform.Find("Text").GetComponent<Text>().text = "大王";
            }
        }

        switch (m_pokerType)
        {
            case (int)TLJCommon.Consts.PokerType.PokerType_FangKuai:
                {
                    gameObject.transform.Find("Image_icon").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/Poker/icon_fangkuai", typeof(Sprite)) as Sprite;
                }
                break;

            case (int)TLJCommon.Consts.PokerType.PokerType_HeiTao:
                {
                    gameObject.transform.Find("Image_icon").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/Poker/icon_heitao", typeof(Sprite)) as Sprite;
                }
                break;

            case (int)TLJCommon.Consts.PokerType.PokerType_HongTao:
                {
                    gameObject.transform.Find("Image_icon").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/Poker/icon_hongtao", typeof(Sprite)) as Sprite;
                }
                break;

            case (int)TLJCommon.Consts.PokerType.PokerType_MeiHua:
                {
                    gameObject.transform.Find("Image_icon").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/Poker/icon_meihua", typeof(Sprite)) as Sprite;
                }
                break;

            case (int)TLJCommon.Consts.PokerType.PokerType_Wang:
                {
                    if (num == 15)
                    {
                        gameObject.transform.Find("Image_icon").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/Poker/icon_xiaowang", typeof(Sprite)) as Sprite;
                    }
                    else if (num == 16)
                    {
                        gameObject.transform.Find("Image_icon").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/Poker/icon_dawang", typeof(Sprite)) as Sprite;
                    }

                    gameObject.transform.Find("Image_icon").transform.localPosition = new Vector3(-2.44f, 7.2f,0);
                    gameObject.transform.Find("Image_icon").transform.localScale = new Vector3(1,1,1);
                    gameObject.transform.Find("Image_icon").GetComponent<Image>().SetNativeSize();

                    gameObject.transform.Find("Text").transform.localScale = new Vector3(0,0,0);
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
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x,-220,0);
            m_isSelect = false;
        }
        else
        {
            //gameObject.transform.localPosition += new Vector3(0, 30, 0);
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, -190, 0);
            m_isSelect = true;
        }
    }
}
