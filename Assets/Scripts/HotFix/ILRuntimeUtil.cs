using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

    public void downDll(string url)
    {
        if (s_appdomain == null)
        {
            s_appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        }

        StartCoroutine(LoadHotFixAssembly(url));
    }

    IEnumerator LoadHotFixAssembly(string url)
    {
        WWW www = new WWW(url);

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

        NetLoading.getInstance().Close();
        PlayerPrefs.SetInt("codeVersion", OtherData.s_loginScript.m_codeVersion);
    }

    /*funcName:类名.函数名 如：“MedalExplainPanelScript.onClickSetPsw”
    * 如果有的函数有多个重载，则这样写：MedalExplainPanelScript.onClickSetPsw(1)、MedalExplainPanelScript.onClickSetPsw(2)
    */
    public bool checkDllClassHasFunc(string className,string funcName)
    {
        if (s_appdomain == null)
        {
            s_appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
        }

        string param = className + "." + funcName;
        object obj = s_appdomain.Invoke("HotFix_Project.ClassRegister", "checkClassHasFunc", null, param);
        if (obj == null)
        {
            return false;
        }

        if ((bool)obj)
        {
            return true;
        }

        return false;
    }
}
