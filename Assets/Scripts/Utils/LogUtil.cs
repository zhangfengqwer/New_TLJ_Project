using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogUtil : MonoBehaviour
{
    public static bool s_isShowLog = true;

    public static void Log(object obj)
    {
        if (s_isShowLog)
        {
            Debug.Log(obj);
        }
    }

    public static void LogWarning(object obj)
    {
        if (s_isShowLog)
        {
            Debug.LogWarning(obj);
        }
    }

    public static void LogError(string obj)
    {
        if (s_isShowLog)
        {
            Debug.LogError(obj);
        }
    }
}
