using System.Collections.Generic;
using UnityEngine;

public class AssetBundlesManager
{
    public static AssetBundlesManager s_instance = null;
    public Dictionary<string, AssetBundle> ABDics = new Dictionary<string, AssetBundle>();

    public static AssetBundlesManager getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new AssetBundlesManager();
        }

        return s_instance;
    }

    public AssetBundle getAssetBundlesDataByName(string name)
    {
        AssetBundle ab;
        if (!ABDics.TryGetValue(name, out ab))
        {
            LogUtil.LogError("ab包不存在:" + name);
        }

        return ab;
    }
}