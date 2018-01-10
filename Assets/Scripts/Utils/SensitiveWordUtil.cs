using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

public class SensitiveWordUtil
{
    public static string[] WordsDatas;

    public static void reqNet()
    {
        UnityWebReqUtil.Instance.Get(OtherData.getWebUrl() + "stopwords.txt", httpCallBack);
    }

    static void httpCallBack(string tag, string data)
    {
        try
        {
            // 读取配置文件
            {
                InitWords(data);
            }
        }
        catch (Exception ex)
        {
            LogUtil.Log("获取屏蔽词配置文件出错：" + ex.Message);
            ToastScript.createToast("获取屏蔽词配置文件出错");

            //throw ex;
        }
    }

    public static void InitWords(string data)
    {
        WordsDatas = data.Split(',');
    }

    public static bool IsSensitiveWord(string str)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SensitiveWordUtil", "IsSensitiveWord"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SensitiveWordUtil", "IsSensitiveWord", null, str);
            return b;
        }

        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
      
        foreach (var words in WordsDatas)
        {
            if (CommonUtil.isStrContain(str, words))
            {
                LogUtil.Log("敏感词：" + words);
                return true;
            }
        }
        return false;
    }

    public static string deleteSensitiveWord(string str)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SensitiveWordUtil", "deleteSensitiveWord"))
        {
            string s = (string)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SensitiveWordUtil", "deleteSensitiveWord", null, str);
            return s;
        }

        string final_str = str;
        if (string.IsNullOrEmpty(final_str))
        {
            return final_str;
        }

        foreach (var words in WordsDatas)
        {
            if (CommonUtil.isStrContain(final_str, words))
            {
                LogUtil.Log("敏感词：" + words);

                string temp = "";
                for (int i = 0; i < words.Length; i++)
                {
                    LogUtil.Log("a");
                    temp += "*";
                }

                final_str = final_str.Replace(words, temp);
                LogUtil.Log(final_str + "  " + temp);
            }
        }

        return final_str;
    }
}