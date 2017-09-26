using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class UnityWebReqUtil:MonoBehaviour
{
    private static UnityWebReqUtil _Instance;

    public delegate void CallBack(string tag, string data);
    CallBack s_callback = null;

    public static UnityWebReqUtil Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new UnityWebReqUtil();

                GameObject obj = new GameObject();
                obj.transform.name = "UnityWebReqUtil";
                MonoBehaviour.DontDestroyOnLoad(obj);
                _Instance = obj.AddComponent<UnityWebReqUtil>();
            }

            return _Instance;
        }
    }    

    public void setCallBack(CallBack callback)
    {
        s_callback = callback;
    }

    public void Get(string url)
    {
        StartCoroutine(DoGet(url));
    }

    IEnumerator DoGet(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.Send();

            if (www.isError)
            {
                Debug.Log(www.error);
                if (s_callback != null)
                {
                    s_callback("get", www.error);
                }
            }
            else
            {
                if (s_callback != null)
                {
                    s_callback("get", www.downloadHandler.text);
                }
            }
        }
    }

    public void Post(string url, WWWForm form)
    {
        StartCoroutine(DoPost(url, form));
    }

    IEnumerator DoPost(string url, WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.Send();

            if (www.isError)
            {
                Debug.Log(www.error);
                s_callback("post",www.error);
            }
            else
            {
                s_callback("post",www.downloadHandler.text);
            }
        }
    }
}