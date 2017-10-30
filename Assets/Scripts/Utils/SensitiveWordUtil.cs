using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

public class SensitiveWordUtil
{
    private static string[] WordsDatas;

    public static void InitWords()
    {
        FileStream fileStream = null;
        try
        {
            string jsonData = Resources.Load("Entity/stopwords").ToString();
            WordsDatas = jsonData.Split(',');
            Debug.Log("屏蔽词：" + WordsDatas.Length);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static bool IsSensitiveWord(string str)
    {
        if (string.IsNullOrEmpty(str)) return false;
        new Thread(() =>
        {
            foreach (var VARIABLE in WordsDatas)
            {
                Debug.Log(VARIABLE);
            }
        }).Start();
       



        foreach (var words in WordsDatas)
        {
            if (!string.IsNullOrEmpty(words))
            {
                if (CommonUtil.isStrContain(str, words))
                {
                    Debug.Log(words);
                    return true;
                }
            }
        }
        return false;
    }
}