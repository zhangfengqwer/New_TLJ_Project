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
        string url = "http://fwdown.hy51v.com/online/file/AssetBundles/" + m_needDownlist[m_curDownIndex];
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.Send();

        byte[] bytes = request.downloadHandler.data;

        AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromMemory(bytes);
        for (int i = 0; i < AssetBundlesManager.getInstance().m_assetBundlesDatalist.Count; i++)
        {
            if (AssetBundlesManager.getInstance().m_assetBundlesDatalist[i].m_name.CompareTo(m_needDownlist[m_curDownIndex]) == 0)
            {
                AssetBundlesManager.getInstance().m_assetBundlesDatalist[i].m_assetBundle = myLoadedAssetBundle;
                break;
            }
        }

        //保存ab到本地
        string filePath = fileRootPath + "/" + m_needDownlist[m_curDownIndex];

        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            fs.Write(bytes, 0, bytes.Length);
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