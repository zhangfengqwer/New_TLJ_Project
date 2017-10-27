﻿using System;
using System.IO;
using System.Text.RegularExpressions;
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
      
        foreach (var words in WordsDatas)
        {
            if (str.Equals(words))
            {
                return true;
            }
//            //使用Match方法。
//            Regex regex = new Regex(str, RegexOptions.IgnoreCase);
//            Match m = regex.Match(words);
//           
//            if (Regex.IsMatch(str, words))
//            {
//                return true;
//            }
//            if (str.Contains(words))
//            {
//                return true;
//            }
        }
        return false;
    }
}