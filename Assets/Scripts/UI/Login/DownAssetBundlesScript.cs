using System;
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
    private VersionConfig webVersionConfig;


    void Start()
    {
        if (Application.isMobilePlatform)
            fileRootPath = Application.persistentDataPath + "/streamingAssets";
        else
            fileRootPath = Application.dataPath + "/../streamingAssets";
    }

    public bool checkDown()
    {
        //
        string url;
#if UNITY_ANDROID
        url = OtherData.getWebUrl() + "AssetBundles/android/Version.txt";
#endif

#if UNITY_IPHONE
        url = OtherData.getWebUrl() + "AssetBundles/ios/Version.txt";
#endif

#if UNITY_STANDALONE_WIN
        url = OtherData.getWebUrl() + "AssetBundles/pc/Version.txt";
#endif
        DownVersion(url);
        return false;
    }

    private void DownVersion(string url)
    {
        UnityWebReqUtil.Instance.Get(url, VersionBack);
    }

    private void VersionBack(string s, string data)
    {
        LogUtil.Log(data);
        try
        {
            //网络
            webVersionConfig = LitJson.JsonMapper.ToObject<VersionConfig>(data);
            Dictionary<string, FileVersionInfo> webDic = new Dictionary<string, FileVersionInfo>();

            foreach (var item in webVersionConfig.FileVersionInfos)
            {
                webDic.Add(item.File, item);
                webVersionConfig.TotalSize += item.Size;
            }

            //本地
            string versionPath = Path.Combine(fileRootPath, "Version.txt");

            if (!File.Exists(versionPath))
            {
                for (int i = 0; i < webVersionConfig.FileVersionInfos.Count; i++)
                {
                    if (webVersionConfig.FileVersionInfos[i].File.EndsWith(".unity3d"))
                    {
                        m_needDownlist.Add(webVersionConfig.FileVersionInfos[i].File);
                    }
                }
                LogUtil.Log("本地version不存在");
            }
            else
            {
                LogUtil.Log("本地version存在");
                VersionConfig localVersionConfig = LitJson.JsonMapper.ToObject<VersionConfig>(File.ReadAllText(versionPath));
                Dictionary<string, FileVersionInfo> localDic = new Dictionary<string, FileVersionInfo>();
                foreach (var item in localVersionConfig.FileVersionInfos)
                {
                    localDic.Add(item.File, item);
                    localVersionConfig.TotalSize += item.Size;
                }

                // 先删除服务器端没有的ab
                foreach (FileVersionInfo fileVersionInfo in localVersionConfig.FileVersionInfos)
                {
                    if (webDic.ContainsKey(fileVersionInfo.File))
                    {
                        continue;
                    }
                    string abPath = Path.Combine(fileRootPath, fileVersionInfo.File);
                    File.Delete(abPath);
                }

                // 再下载
                foreach (FileVersionInfo fileVersionInfo in webVersionConfig.FileVersionInfos)
                {
                    FileVersionInfo localVersionInfo;

                    if (localDic.TryGetValue(fileVersionInfo.File, out localVersionInfo))
                    {
                        if (fileVersionInfo.MD5 == localVersionInfo.MD5)
                        {
                            string filePath = fileRootPath + "/" + fileVersionInfo.File;
                            //缓存ab包
                            if (filePath.EndsWith(".unity3d"))
                            {
                                AssetBundle loadFromFile = AssetBundle.LoadFromFile(fileRootPath + "/" + fileVersionInfo.File);
                                AssetBundlesManager.getInstance().ABDic.Add(fileVersionInfo.File, loadFromFile);
                            }
                            continue;
                        }
                    }

                    if (fileVersionInfo.File == "Version.txt")
                    {
                        continue;
                    }

                    if (fileVersionInfo.File.EndsWith(".unity3d"))
                    {
                        m_needDownlist.Add(fileVersionInfo.File);
                    }
                }
            }

            if (m_needDownlist.Count > 0)
            {
                GameUtil.showGameObject(gameObject);
                startDown();
                InvokeRepeating("onInvoke", 0.5f, 0.5f);
            }
        }
        catch (Exception e)
        {
            LogUtil.LogError(e.ToString());
        }
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

        //缓存ab包
        AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromMemory(request.downloadHandler.data);
        AssetBundlesManager.getInstance().ABDic.Add(ab_name, myLoadedAssetBundle);

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

                using (FileStream fs = new FileStream(Path.Combine(fileRootPath, "Version.txt"), FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(LitJson.JsonMapper.ToJson(webVersionConfig));
                }
            }
        }
    }
}