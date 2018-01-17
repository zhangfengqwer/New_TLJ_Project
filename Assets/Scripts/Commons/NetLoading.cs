using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetLoading : MonoBehaviour {

    public static NetLoading s_instance = null;
    public GameObject s_loadingPanel = null;

    public static NetLoading getInstance()
    {
        if (s_instance == null)
        {
            GameObject prefab = Resources.Load("Prefabs/Commons/NetLoadingUtil") as GameObject;
            GameObject obj = GameObject.Instantiate(prefab);

            DontDestroyOnLoad(obj);

            s_instance = obj.GetComponent<NetLoading>();
        }

        return s_instance;
    }

    public void Show()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NetLoading_hotfix", "Show"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NetLoading_hotfix", "Show", null, null);
            return;
        }

        if (s_loadingPanel != null)
        {
            Destroy(s_loadingPanel);
        }

        GameObject prefab = Resources.Load("Prefabs/Commons/LoadingPanel") as GameObject;
        s_loadingPanel = GameObject.Instantiate(prefab, GameObject.Find("Canvas_High").transform);

        Invoke("onInvoke",10);
    }

    public void Close()
    {
        if (s_loadingPanel != null)
        {
            Destroy(s_loadingPanel);

            s_loadingPanel = null;
        }

        CancelInvoke("onInvoke");
    }

    public void onInvoke()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NetLoading_hotfix", "onInvoke"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NetLoading_hotfix", "onInvoke", null, null);
            return;
        }

        ToastScript.createToast("网络超时");
        Close();
    }
}
