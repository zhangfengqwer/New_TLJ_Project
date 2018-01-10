using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ILRuntimeUtil : MonoBehaviour
{
    static ILRuntimeUtil s_instance = null;
    static AppDomain s_appdomain = null;

    public static ILRuntimeUtil getInstance()
    {
        return s_instance;
    }

    private void Awake()
    {
        s_instance = this;
    }

    public AppDomain getAppDomain()
    {
        return s_appdomain;
    }

    public void downDll()
    {
        if (s_appdomain == null)
        {
            s_appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        }

        StartCoroutine(LoadHotFixAssembly());
    }

    IEnumerator LoadHotFixAssembly()
    {
        WWW www = new WWW(OtherData.getWebUrl() + "hotfix/HotFix_Project.dll");

        while (!www.isDone)
        {
            yield return null;
        }

        if (!string.IsNullOrEmpty(www.error))
        {
            UnityEngine.Debug.LogError(www.error);
        }

        byte[] dll = www.bytes;
        www.Dispose();

        using (System.IO.MemoryStream fs = new MemoryStream(dll))
//        s_appdomain.LoadAssembly(fs, null, new Mono.Cecil.Pdb.PdbReaderProvider());

        InitializeILRuntime();
        OnHotFixLoaded();
    }

    void InitializeILRuntime()
    {
        //这里做一些ILRuntime的注册，HelloWorld示例暂时没有需要注册的
    }

    void OnHotFixLoaded()
    {
        Debug.Log("OnHotFixLoaded");
        //HelloWorld，第一次方法调用
        //s_appdomain.Invoke("HotFix_Project.InstanceClass", "StaticFunTest", null, null);
    }
}
