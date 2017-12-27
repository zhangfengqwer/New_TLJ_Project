﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherData
{
    public enum DefaultLoginType
    {
        DefaultLoginType_Default,
        DefaultLoginType_GuanFang,
        DefaultLoginType_QQ,
        DefaultLoginType_WeChat,
    }

    public static int s_defaultLoginType = (int)DefaultLoginType.DefaultLoginType_Default;

    public static string s_apkVersion = "1.0.50";   // apk版本号
    public static string s_codeVersion;             // 代码版本
    public static string s_resVersion;              // 资源版本

    public static bool s_isFromSetToLogin = false;
    public static bool s_isFirstOpenGame = true;
    public static bool s_isTest = true;
    public static bool s_hasCheckSecondPSW = false;
    public static bool s_canRecharge = false;


    public static Vector2 s_screenSize;

    // 七牛
    //public static string s_webStorageUrl = "http://p02gqb8lq.bkt.clouddn.com/";

    // web测试服
    public static string s_webStorageUrl = "http://hatest.d51v.com/static/game/";

    public static LoginScript s_loginScript = null;
    public static GetNetEntityFile s_getNetEntityFile = null;
    public static MainScript s_mainScript = null;
    public static UserInfoScript s_userInfoScript = null;
    public static GameScript s_gameScript = null;
}
