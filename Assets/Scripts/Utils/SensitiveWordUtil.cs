using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

public class SensitiveWordUtil
{
    private static string[] WordsDatas;

    public static void reqNet()
    {
        UnityWebReqUtil.Instance.Get("http://oru510uv8.bkt.clouddn.com/stopwords.txt", httpCallBack);
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
            LogUtil.Log("读取网络配置文件出错：" + ex.Message);
        }
    }

    public static void InitWords(string data)
    {
        //FileStream fileStream = null;
        try
        {
            //string jsonData = Resources.Load("Entity/stopwords").ToString();
            WordsDatas = data.Split(',');
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static bool IsSensitiveWord(string str)
    {
        if (string.IsNullOrEmpty(str)) return false;
      
        foreach (var words in WordsDatas)
        {
            if (CommonUtil.isStrContain(str, words))
            {
                return true;
            }
        }
        return false;
    }
}