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
        m_assetBundlesDatalist.Add(new AssetBundlesData("audios.unity3d", null));

        m_assetBundlesDatalist.Add(new AssetBundlesData("head.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("emoji.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("gameresult.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("main.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("vip.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("tuiguang.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("turntable.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("shouchong.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("shop.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("game.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("poker.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("kefu.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("medalduihuan.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("animations.unity3d", null));
        m_assetBundlesDatalist.Add(new AssetBundlesData("doudizhu.unity3d", null));
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