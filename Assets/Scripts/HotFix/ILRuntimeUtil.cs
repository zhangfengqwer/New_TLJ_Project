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

    static List<string> s_funcList = new List<string>();

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
        s_appdomain.LoadAssembly(fs, null, new Mono.Cecil.Pdb.PdbReaderProvider());

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

        /*
         * 保存dll所有的类-函数数据，这样只用调用一次dll就行了，不用每次都要问一下dll是否有xxx类-函数
         * 避免来回调用dll浪费时间
         */
        {
            s_funcList.Clear();
            List<string> funcList = (List<string>)s_appdomain.Invoke("HotFix_Project.ClassRegister", "getFuncList", null, null);
            if (funcList != null)
            {
                for (int i = 0; i < funcList.Count; i++)
                {
                    LogUtil.Log("dll包含的类-函数：" + funcList[i]);
                    s_funcList.Add(funcList[i]);
                }
            }
            else
            {
                LogUtil.Log("没有dll可用");
            }
        }

        NetLoading.getInstance().Close();
        PlayerPrefs.SetInt("codeVersion", OtherData.s_loginScript.m_codeVersion);
    }

    public static bool checkClassHasFunc(string funcName)
    {
        for (int i = 0; i < s_funcList.Count; i++)
        {
            if (s_funcList[i].CompareTo(funcName) == 0)
            {
                return true;
            }
        }

        return false;
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

        return checkClassHasFunc(param);
    }
}
