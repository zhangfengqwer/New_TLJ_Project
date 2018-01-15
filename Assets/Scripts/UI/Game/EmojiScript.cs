using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmojiScript : MonoBehaviour
{
    public float m_zhenlv = 5;
    public int m_emoji_id = 1;
    public int m_curindex = 0;
    public int repeatCount = 0;

    public Image m_image;

    public static GameObject create(int emoji_id,Vector2 pos)
    {
        try
        {
            GameObject prefab = Resources.Load("Prefabs/Game/Emoji") as GameObject;
            GameObject obj = MonoBehaviour.Instantiate(prefab);

            obj.transform.SetParent(GameObject.Find("Canvas_Middle").transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.localPosition = pos;

            obj.GetComponent<EmojiScript>().setData(emoji_id);

            return obj;
        }
        catch (Exception ex)
        {
            LogUtil.Log("EmojiScript.create:"+ex.Message);
        }

        return null;
    }

    // Use this for initialization
    void Start ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("EmojiScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.EmojiScript", "Start", null, null);
            return;
        }
    }

    public void setData(int emoji_id)
    {
        m_emoji_id = emoji_id;
        m_image = gameObject.GetComponent<Image>();
        string path = "Sprites/Emoji/Expression-" + m_emoji_id + "_1";
        CommonUtil.setImageSprite(m_image, path);

        InvokeRepeating("onInvoke", 1.0f / m_zhenlv, 1.0f / m_zhenlv);
    }

    void onInvoke()
    {
        try
        {
            ++m_curindex;
            string path = "Sprites/Emoji/Expression-" + m_emoji_id + "_" + m_curindex;
            Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            if (sprite == null)
            {
                ++repeatCount;

                if ((m_emoji_id == 4) ||
                    (m_emoji_id == 5) ||
                    (m_emoji_id == 7) ||
                    (m_emoji_id == 8) ||
                    (m_emoji_id == 12) ||
                    (m_emoji_id == 13) ||
                    (m_emoji_id == 15) ||
                    (m_emoji_id == 16))
                {
                    if (repeatCount == 2)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        m_curindex = 0;
                    }
                }
                else
                {
                    
                    Destroy(gameObject);
                }
            }
            else
            {
                CommonUtil.setImageSprite(m_image, path);
            }
        }
        catch (Exception ex)
        {
            LogUtil.Log("EmojiScript.onInvoke:" + ex.Message);
            Destroy(gameObject);
        }
    }
}
