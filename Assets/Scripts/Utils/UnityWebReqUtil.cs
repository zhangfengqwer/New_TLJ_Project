using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class UnityWebReqUtil:MonoBehaviour
{
    public static UnityWebReqUtil _Instance;

    public delegate void CallBack(string tag, string data);

    public static UnityWebReqUtil Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new UnityWebReqUtil();
                GameObject obj = new GameObject("UnityWebReqUtil");
                obj.transform.name = "UnityWebReqUtil";
                MonoBehaviour.DontDestroyOnLoad(obj);
                _Instance = obj.AddComponent<UnityWebReqUtil>();
            }

            return _Instance;
        }
    }

    public void Get(string url, CallBack callback)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("UnityWebReqUtil_hotfix", "Get"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.UnityWebReqUtil_hotfix", "Get", null, url, callback);
            return;
        }

        // 防止缓存
        url += ("?" + CommonUtil.getCurTime());

        LogUtil.Log("web请求：" + url);

        StartCoroutine(DoGet(url, callback));
    }

    public IEnumerator DoGet(string url, CallBack callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.Send();

            if (www.isError)
            {
                LogUtil.Log(www.error);
                callback("get", www.error);
            }
            else
            {
                callback("get", www.downloadHandler.text.TrimStart());
            }
        }
    }

    public void Post(string url, WWWForm form, CallBack callback)
    {
        StartCoroutine(DoPost(url, form, callback));
    }

    IEnumerator DoPost(string url, WWWForm form, CallBack callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.Send();

            if (www.isError)
            {
                LogUtil.Log(www.error);
                callback("post",www.error);
            }
            else
            {
                callback("post",www.downloadHandler.text.TrimStart());
            }
        }
    }
}