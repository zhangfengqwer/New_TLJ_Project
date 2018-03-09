using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownAssetBundlesScript : MonoBehaviour {

    public List<string> m_needDownlist = new List<string>();
    public int m_curDownIndex = 0;
    
    void Start ()
    {
        
    }

    public bool checkDown()
    {
        m_needDownlist.Add("head.unity3d");

        if (m_needDownlist.Count > 0)
        {
            GameUtil.showGameObject(gameObject);

            startDown();

            return true;
        }

        return false;
    }

    public void startDown()
    {
        StartCoroutine("onStartDown");
    }

    IEnumerator onStartDown()
    {
        UnityWebRequest request = UnityWebRequest.GetAssetBundle("http://fwdown.hy51v.com/online/file/AssetBundles/" + m_needDownlist[m_curDownIndex]);
        yield return request.Send();

        AssetBundle myLoadedAssetBundle = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
        for (int i = 0; i < AssetBundlesManager.getInstance().m_assetBundlesDatalist.Count; i++)
        {
            if (AssetBundlesManager.getInstance().m_assetBundlesDatalist[i].m_name.CompareTo(m_needDownlist[m_curDownIndex]) == 0)
            {
                AssetBundlesManager.getInstance().m_assetBundlesDatalist[i].m_assetBundle = myLoadedAssetBundle;
                break;
            }
        }

        {
            if (m_curDownIndex < (m_needDownlist.Count - 1))
            {
                ++m_curDownIndex;

                startDown();
            }
            else
            {
                LogUtil.Log("下载完毕");

                GameUtil.hideGameObject(gameObject);

                OtherData.s_loginScript.netDataDown();
            }
        }
    }
}
