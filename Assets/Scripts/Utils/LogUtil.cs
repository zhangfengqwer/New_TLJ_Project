using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogUtil : MonoBehaviour
{
    static bool isShowLog = false;

    public static void Log(object obj)
    {
        if (isShowLog)
        {
            Debug.Log(obj);
        }
    }

    public static void LogWarning(object obj)
    {
        if (isShowLog)
        {
            Debug.LogWarning(obj);
        }
    }

    public static void LogError(string obj)
    {
        if (isShowLog)
        {
            Debug.LogError(obj);
        }
    }
}
