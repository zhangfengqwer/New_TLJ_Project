using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

public class SensitiveWordUtil
{
    private static string[] WordsDatas;

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
        if (string.IsNullOrEmpty(str)) return false;
      
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
}