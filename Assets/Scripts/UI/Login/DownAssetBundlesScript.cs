using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownAssetBundlesScript : MonoBehaviour
{
    public List<string> m_needDownlist = new List<string>();
    public int m_curDownIndex = 0;
    public static string fileRootPath;

    public Text m_text;


    void Start()
    {
        if (Application.isMobilePlatform)
            fileRootPath = Application.persistentDataPath + "/streamingAssets";
        else
            fileRootPath = Application.dataPath + "/../streamingAssets";
    }

    public bool checkDown()
    {
        for (int i = 0; i < AssetBundlesManager.getInstance().m_assetBundlesDatalist.Count; i++)
        {
            string name = AssetBundlesManager.getInstance().m_assetBundlesDatalist[i].m_name;
            string filePath = fileRootPath + "/" + name;
            if (!File.Exists(filePath))
            {
                m_needDownlist.Add(name);
            }
            else
            {
                AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(filePath);
                LogUtil.Log("加载缓存ab:" + myLoadedAssetBundle.name);
                AssetBundlesManager.getInstance().m_assetBundlesDatalist[i].m_assetBundle = myLoadedAssetBundle;
            }
        }

        if (m_needDownlist.Count > 0)
        {
            GameUtil.showGameObject(gameObject);

            startDown();

            InvokeRepeating("onInvoke",0.5f,0.5f);

            return true;
        }

        return false;
    }

    public void onInvoke()
    {
        if (m_text.text.CompareTo("正在下载资源...") == 0)
        {
            m_text.text = "正在下载资源";
        }
        else if (m_text.text.CompareTo("正在下载资源") == 0)
        {
            m_text.text = "正在下载资源.";
        }
        else if (m_text.text.CompareTo("正在下载资源.") == 0)
        {
            m_text.text = "正在下载资源..";
        }
        else if (m_text.text.CompareTo("正在下载资源..") == 0)
        {
            m_text.text = "正在下载资源...";
        }
    }

    public void startDown()
    {
        StartCoroutine("onStartDown");
    }

    IEnumerator onStartDown()
    {
        string url;

#if UNITY_ANDROID
        url = OtherData.getWebUrl() + "AssetBundles/android/" + m_needDownlist[m_curDownIndex];
#endif

#if UNITY_IPHONE
        url = OtherData.getWebUrl() + "AssetBundles/ios/" + m_needDownlist[m_curDownIndex];
#endif

#if UNITY_STANDALONE_WIN
        url = OtherData.getWebUrl() + "AssetBundles/pc/" + m_needDownlist[m_curDownIndex];
#endif
        
        string ab_name = m_needDownlist[m_curDownIndex];
        LogUtil.Log("下载ab:" + ab_name + "    " + url);

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.Send();

        AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromMemory(request.downloadHandler.data);
        for (int i = 0; i < AssetBundlesManager.getInstance().m_assetBundlesDatalist.Count; i++)
        {
            if (AssetBundlesManager.getInstance().m_assetBundlesDatalist[i].m_name.CompareTo(ab_name) == 0)
            {
                AssetBundlesManager.getInstance().m_assetBundlesDatalist[i].m_assetBundle = myLoadedAssetBundle;
                break;
            }
        }

        //保存ab到本地
        {
            byte[] bytes = request.downloadHandler.data;

            string filePath = fileRootPath + "/" + ab_name;

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
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

                CancelInvoke("onInvoke");

                GameUtil.hideGameObject(gameObject);

                OtherData.s_loginScript.netDataDown();
            }
        }
    }
}