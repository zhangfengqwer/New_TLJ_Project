using System;
using System.IO;
using UnityEngine;

public class StopWordsData
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
}