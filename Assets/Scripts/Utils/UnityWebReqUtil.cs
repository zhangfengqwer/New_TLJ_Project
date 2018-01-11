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
        StartCoroutine(DoGet(url, callback));
    }

    IEnumerator DoGet(string url, CallBack callback)
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