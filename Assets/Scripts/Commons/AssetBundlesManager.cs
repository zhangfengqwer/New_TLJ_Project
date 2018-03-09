using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AssetBundlesManager
{
    public static AssetBundlesManager s_instance = null;

    public List<AssetBundlesData> m_assetBundlesDatalist = new List<AssetBundlesData>();

    public static AssetBundlesManager getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new AssetBundlesManager();
            s_instance.init();
        }

        return s_instance;
    }

    public void init()
    {
        m_assetBundlesDatalist.Add(new AssetBundlesData("head.unity3d", null));
    }

    public AssetBundle getAssetBundlesDataByName(string name)
    {
        for (int i = 0; i < m_assetBundlesDatalist.Count; i++)
        {
            if (m_assetBundlesDatalist[i].m_name.CompareTo(name) == 0)
            {
                return m_assetBundlesDatalist[i].m_assetBundle;
            }
        }

        return null;
    }
}

public class AssetBundlesData
{
    public string m_name;
    public AssetBundle m_assetBundle = null;

    public AssetBundlesData(string name, AssetBundle assetBundle)
    {
        m_name = name;
        m_assetBundle = assetBundle;
    }
}