using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAnimation : MonoBehaviour {

    public Image m_img = null;
    public float m_speed = 0.03f;
    public int m_curIndex = 1;

    public string m_assetbundleName;
    public string m_animationName;
    public bool m_isLoop;

    // Use this for initialization
    void Start ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayAnimation_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayAnimation_hotfix", "Start", null, null);
            return;
        }

        if (m_img == null)
        {
            m_img = gameObject.GetComponent<Image>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void start(string assetbundleName, string animationName,bool isLoop)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayAnimation_hotfix", "start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayAnimation_hotfix", "start", null, assetbundleName, animationName);
            return;
        }

        m_assetbundleName = assetbundleName;
        m_animationName = animationName;
        m_isLoop = isLoop;

        if (m_img == null)
        {
            m_img = gameObject.GetComponent<Image>();
        }

        m_curIndex = 1;
        string imgName = m_animationName + m_curIndex;
        if (CommonUtil.checkSpriteIsExistByAssetBundle(m_assetbundleName, imgName))
        {
            CommonUtil.setImageSpriteByAssetBundle(m_img, m_assetbundleName, imgName);
            InvokeRepeating("onInvoke", m_speed, m_speed);
        }
    }

    public void start(string assetbundleName, string animationName, bool isLoop,float speed)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayAnimation_hotfix", "start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayAnimation_hotfix", "start", null, assetbundleName, animationName);
            return;
        }

        m_assetbundleName = assetbundleName;
        m_animationName = animationName;
        m_isLoop = isLoop;
        m_speed = speed;

        if (m_img == null)
        {
            m_img = gameObject.GetComponent<Image>();
        }

        m_curIndex = 1;
        string imgName = m_animationName + m_curIndex;
        if (CommonUtil.checkSpriteIsExistByAssetBundle(m_assetbundleName, imgName))
        {
            CommonUtil.setImageSpriteByAssetBundle(m_img, m_assetbundleName, imgName);
            InvokeRepeating("onInvoke", m_speed, m_speed);
        }
    }

    public void onInvoke()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PlayAnimation_hotfix", "onInvoke"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PlayAnimation_hotfix", "onInvoke", null, null);
            return;
        }

        ++m_curIndex;

        string imgName = m_animationName + m_curIndex;
        if (CommonUtil.checkSpriteIsExistByAssetBundle(m_assetbundleName, imgName))
        {
            CommonUtil.setImageSpriteByAssetBundle(m_img, m_assetbundleName, imgName);
        }
        else
        {
            if (m_isLoop)
            {
                m_curIndex = 1;
                imgName = m_animationName + m_curIndex;
                if (CommonUtil.checkSpriteIsExistByAssetBundle(m_assetbundleName, imgName))
                {
                    CommonUtil.setImageSpriteByAssetBundle(m_img, m_assetbundleName, imgName);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
