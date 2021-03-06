﻿using UnityEngine;
using System;
using System.Runtime.InteropServices;
using LitJson;


#if UNITY_EDITOR
public class PlatformHelper
{
    // 是否在About框中显示Logo
    public static string GetTextOfAbout()
    {
        return "";
    }

    public static string GetPhoneType()
    {
        return "editor";
    }

    public static int GetPower()
    {
        return 100;
    }

    public static string GetUniqueIdentifier()
    {
        return "Unity_Editor";
    }

    public static bool IsIOSForceUpdate()
    {
        return false;
    }

    /// <summary>
    /// 获取渠道号
    /// </summary>
    public static string GetChannelName()
    {
        return "javgame";
    }

    public static string GetVersionName()
    {
        return "1.5?";
    }

    /// <summary>
    /// 获取渠道类型  
    /// </summary>
    /// <param name='uid'>
    /// Uid.
    /// </param>
    public static string GetLoginType()
    {
        return "";
    }

    /// <summary>
    /// 获取渠道相关参数  
    /// </summary>
    /// <returns></returns>
    public static string GetThirdParameters()
    {
        return "";
    }

    public static void Login(string obj, string func, string data)
    {
    }

    public static void WXShareFriends(string obj, string func, string data)
    {
    }

    public static void WXShareFriendsCircle(string obj, string func, string data)
    {
    }

    public static void iOSLog(string str)
    {
        LogUtil.Log(str);
    }

    public static void setProductName(string str)
    {
    }

    /// <summary>
    /// 在外部设置该安装包是否与360等第三方平台合作 
    /// true -- 合作
    /// </summary>
    //    public static void GetIsCollaborate()
    //    {
    //        GlobalData.IsCollaborate = true;
    //
    //    }
    /// <summary>
    /// QQ sina微博账号登录 
    /// 
    /// </summary>
    /// <param name="type">QQ ：104； SinaWeiBo：105</param>
    /// <param name="obj">登录按钮</param>
    /// <param name="func">回调方法名</param>
    /// <param name="data">待回调返回到服务端的数据</param>
    public static void CommonLogin(int type, string obj, string func, string data)
    {
    }

    public static void setLoginResponse(string loginResponse)
    {
    }

    public static void closeService()
    {
    }

    public static bool ForceUpdate()
    {
        return false;
    }


    public static void onUmUserEvent(int type, String eventID, String label)
    {
    }

    public static string getAndroidStringConfig(String name)
    {
        return null;
    }

    public static string getOrderExpand()
    {
        return "";
    }


    public static void isLoginConnectingScene(bool value)
    {
    }

    /// <summary>
    ///  control 91 toolbar show or hide
    /// </summary>
    public static void show91ToolBar(bool value)
    {
    }

    /// <summary>
    /// 是否是第三方SDK 的退出系统 ，如果是第三方系统退出，退出的时候，将要回调
    /// </summary>
    /// <returns></returns>
    public static bool isThirdSDKQuit()
    {
        return false;
    }


    /// <summary>
    /// 通知第三方SDK 的退出
    /// </summary>
    /// <param name="callObj"></param>
    /// <param name="callFunc"></param>
    /// <param name="data"></param>
    public static void thirdSDKQuit(string callObj, string callFunc, string data)
    {
    }


    /// <summary>
    /// 设置回调方法，，第三方SDK退出的时候被通知
    /// </summary>
    /// <param name="callObj"></param>
    /// <param name="callFunc"></param>
    /// <param name="data"></param>
    public static void setQuitCallback(string callObj, string callFunc, string data)
    {
    }


    /// <summary>
    /// 注销账号的时候，通知第三方
    /// </summary>
    /// <param name="callObj"></param>
    /// <param name="callFunc"></param>
    /// <param name="data"></param>
    public static void logout(string callObj, string callFunc, string data)
    {
    }


    /// <summary>
    /// 设置回调方法，第三方SDK注销账号的时候被通知
    /// </summary>
    /// <param name="callObj"></param>
    /// <param name="callFunc"></param>
    /// <param name="data"></param>
    public static void setLogoutCallback(string callObj, string callFunc, string data)
    {
    }


    /// <summary>
    /// 通用的方法，目的是减少接口变化
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static String commonMethod(String input)
    {
        return null;
    }


    /// <summary>
    /// 通用的第三方方法，目的是减少接口变化
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static String thirdCommonMethod(String input)
    {
        return null;
    }


    /// <summary>
    /// 第三方游戏中心（一般是浮动图标），是否可见
    /// </summary>
    /// <param name="value"></param>
    public static void setThirdCenterVisible(bool value)
    {
    }

    /// <summary>
    /// 弹出交叉推广
    /// </summary>
    /// <param name="value"></param>
    public static void popupExchangeService()
    {
    }

    /// <summary>
    /// 显示LogUtil信息
    /// </summary>
    /// <param name="value"></param>
    public static void showDebugInfo(string name, string unityInfo)
    {
    }

    public static void pay(string payType, string callObj, string callFunc, string data)
    {
    }

    /**
     * 取联网方式
     * 无联网 0
     * wifi 1
     * 2G 2 
     * 3G 3
     * 4G 4
     * @param value
     */
    public static int getNetWork()
    {
        return 1;
    }

    /**
     * 1表示测试包
     */
    public static string getIsTest()
    {
        return "1";
    }

    /**
     * 获取手机服务商信息 
     * 中国移动 1 
     * 中国联通 2
     * 中国电信 3
     * 无卡 0
     */
    public static int getProvidersName()
    {
        return 1;
    }

    public static void DownApk()
    {
    }

    public static bool IsThirdLogin()
    {
        return false;
    }

    public static string GetMacId()
    {
        return "2";
    }
}
#elif UNITY_ANDROID /// <summary>
/// 控制所有对本地化的调用 （除了MallAndroidHelper ，因为MallAndroidHelper 对本地调用比较多而且复杂 。所有Android 调用会调 UnityHelper 类 。
/// </summary>
public class PlatformHelper
{

    // 是否在About框中显示Logo
    public static string GetTextOfAbout() {
        if (Application.platform != RuntimePlatform.Android) {
            return "";
        }

        return GetJC().CallStatic<string>("isShowLogoInAbout");
    }

    private static AndroidJavaClass javaClass = null;

    /// <summary>
    /// 实例化一个JAVA类
    /// </summary>
    private static AndroidJavaClass GetJC(){

        if(javaClass != null){
            return javaClass;
        }

        javaClass = new AndroidJavaClass("com.javgame.utility.UnityHelper");

        return javaClass;
    }

    public static string GetUniqueIdentifier() 
    {
        return "";
    }
    
    public static string GetMacId()
    {
        return GetJC().CallStatic<string>("getUniqueId");;
    }

    /// <summary>
    /// 获取渠道类型  
    /// </summary>
    /// <returns></returns>
    public static string GetLoginType()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return "";      
        }
        return GetJC().CallStatic<string>("getLoginType");
    }
    /// <summary>
    /// 获取渠道相关参数  
    /// </summary>
    /// <returns></returns>
    public static string GetThirdParameters()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return "";
        }
        return GetJC().CallStatic<string>("getThirdParameters");
    }

    /// <summary>
    /// 调用android代码获取第三方登录信息
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="func"></param>
    /// <param name="data"></param>
    public static void Login(string obj, string func, string data)
    {
        GetJC().CallStatic("login", obj, func, data);
    }

    public static void DownApk()
    {
        GetJC().CallStatic("downLoadApk");
    }

    public static bool IsThirdLogin()
    {
        return GetJC().CallStatic<bool>("isThirdLogin");
    }

    //微信分享
    public static void WXShareFriends(string obj, string func, string data)
    {
        GetJC().CallStatic("wxShareFriends", obj, func, data);
    }

    public static void WXShareFriendsCircle(string obj, string func, string data)
    {
        GetJC().CallStatic("wxShareFriendsCircle", obj, func, data);
    }

    /// <summary>
    /// 在外部设置该安装包是否与360等第三方平台合作 
    /// true -- 合作
    /// </summary>
//    public static void GetIsCollaborate()
//    {
//        GlobalData.SetCollaborate(GetJC().CallStatic<String>("GetCollaborate"));
//    }

    public static void iOSLog(string str){
        LogUtil.Log(str);
    }

    public static void setProductName(string str){}

    /// <summary>
    /// QQ sina微博账号登录 
    /// 
    /// </summary>
    /// <param name="type">QQ ：104； SinaWeiBo：105</param>
    /// <param name="obj">登录按钮</param>
    /// <param name="func">回调方法名</param>
    /// <param name="data">待回调返回到服务端的数据</param>
    public static void CommonLogin(int type, string obj, string func, string data)
    {
        GetJC().CallStatic("commonLogin", type, obj, func, data);
    }

    /// <summary>
    /// 将服务端返回的信息传递给android模块
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="func"></param>
    /// <param name="data"></param>
    public static void setLoginResponse(string loginResponse)
    {
        GetJC().CallStatic("setLoginResponse", loginResponse);
    }
    public static string GetPhoneType(){
        if (Application.platform != RuntimePlatform.Android)
        {
            return "editor"; 
        }
        return GetJC().CallStatic<string>("getCellPhoneModel");
    }

    /// <summary>
    /// 获取系统电量
    /// </summary>
    public static int GetPower() 
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return 100;
        }
        return GetJC().CallStatic<int>("getPower");
    }

    public static void isLoginConnectingScene(bool value){}

    /// <summary>
    ///  control 91 toolbar show or hide
    /// </summary>
    public static void show91ToolBar(bool value){}

    /// <summary>
    /// 获取渠道号
    /// </summary>
    public static string GetChannelName(){
        if (Application.platform != RuntimePlatform.Android)
        {
            return "javgame";
        }
        return GetJC().CallStatic<string>("getChannelName");    
    }

    public static string GetVersionName () {
        //LogUtil.LogError(string.Format("GetVersionName(), platform={0}", Application.platform));
        if (Application.platform != RuntimePlatform.Android) {
            return "1.5";
        }
        return GetJC().CallStatic<string>("getVersionName");
    }

    public static string GetVersionCode() {
        return GetJC().CallStatic<int>("getNewVersionCode").ToString();
    }

    public static bool IsIOSForceUpdate()
    {
        return false;
    }


    public static string getIsTest()
    {
        return GetJC().CallStatic<string>("getIsTest").ToString();
    }


    /// <summary>
    /// Closes the service.
    /// </summary>
    public static void closeService()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            GetJC().CallStatic("closeUpdateService");
        }
    }

    /// <summary>
    /// 强制升级检测  
    /// </summary>
    /// <returns></returns>
    public static bool ForceUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {

            if (GetJC().CallStatic<int>("getNewVersionCode") != -1
                    && GetJC().CallStatic<int>("getNewVersionCode") != GetJC().CallStatic<int>("getCurrentVersionCode"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// add user event .
    /// </summary>
    public static void onUmUserEvent(int type, String eventID, String label)
    {
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                GetJC().CallStatic("onUmUserEvent", type, eventID, label);
            }
        }
    }

    /// <summary>
    /// 取android 的配置
    /// </summary>
    public static string getAndroidStringConfig(String name)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            return GetJC().CallStatic<string>("getStringConfig", name);
        }
        else
        {
            return null;
        }
    }

    public static string getOrderExpand() 
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            return GetJC().CallStatic<string>("getOrderExpand");
        }
        else
        {
            return "";
        }
    }



    /// <summary>
    /// 是否是第三方SDK 的退出系统 ，如果是第三方系统退出，退出的时候，将要回调
    /// </summary>
    /// <returns></returns>
    public static bool isThirdSDKQuit(){
        return GetJC().CallStatic<bool>("isThirdSDKQuit");
    }


    /// <summary>
    /// 
    /// 通知第三方SDK 的退出
    /// </summary>
    /// <param name="callObj"></param>
    /// <param name="callFunc"></param>
    /// <param name="data"></param>
    public static void thirdSDKQuit(string callObj, string callFunc, string data){
        GetJC().CallStatic("thirdSDKQuit",callObj,callFunc,data);
    }


    /// <summary>
    /// 设置回调方法，，第三方SDK退出的时候被通知
    /// </summary>
    /// <param name="callObj"></param>
    /// <param name="callFunc"></param>
    /// <param name="data"></param>
    public static void setQuitCallback(string callObj, string callFunc, string data){
        GetJC().CallStatic("setQuitCallback",callObj,callFunc,data);
    }


    /// <summary>
    /// 注销账号的时候，通知第三方
    /// </summary>
    /// <param name="callObj"></param>
    /// <param name="callFunc"></param>
    /// <param name="data"></param>
    public static void logout(string callObj, string callFunc, string data){
        GetJC().CallStatic("logout",callObj,callFunc,data);
    }


    public static void pay(string callObj, string callFunc, string data){
        GetJC().CallStatic("pay",callObj,callFunc,data);
    }

    public static void pay(string payType, string callObj, string callFunc, string data)
    { 
        GetJC().CallStatic("pay",payType,callObj,callFunc,data);
    }

    /// <summary>
    /// 设置回调方法，第三方SDK注销账号的时候被通知
    /// </summary>
    /// <param name="callObj"></param>
    /// <param name="callFunc"></param>
    /// <param name="data"></param>
    public static void setLogoutCallback(string callObj, string callFunc, string data){
        GetJC().CallStatic("setLogoutCallback",callObj,callFunc,data);
    }


    /// <summary>
    /// 通用的方法，目的是减少接口变化
    /// </summary>
    /// 
    /// <param name="input"></param>
    /// <returns></returns>
    public static String commonMethod(String input){
        return GetJC().CallStatic<string>("commonMethod",input);
    }


    /// <summary>
    /// 通用的第三方方法，目的是减少接口变化
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static String thirdCommonMethod(String input){

        return GetJC().CallStatic<string>("thirdCommonMethod",input);
    }


    /// <summary>
    /// 第三方游戏中心（一般是浮动图标），是否可见
    /// </summary>
    /// <param name="value"></param>
    public static void setThirdCenterVisible(bool value){
        GetJC().CallStatic("setThirdCenterVisible",value);
    }

    /// <summary>
    /// 弹出交叉推广
    /// </summary>
    /// <param name="value"></param>
    public static void popupExchangeService()
    {
        GetJC().CallStatic("popupExchangeService");
    }

    /// <summary>
    /// 显示LogUtil信息
    /// </summary>
    /// <param name="value"></param>
    public static void showDebugInfo(string name ,string unityInfo)
    {
        GetJC().CallStatic("showDebugInfo",name,unityInfo);
    }

    /**
     * 取联网方式
     * 无联网 0
     * wifi 1
     * 2G 2 
     * 3G 3
     * 4G 4
     * @param value
     */
    public static int getNetWork()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            return GetJC().CallStatic<int>("getNetWork");
        }
        else
        {
            return 1;
        }
    }

    /**
     * 获取手机服务商信息 
     * 中国移动 1 
     * 中国联通 2
     * 中国电信 3
     * 无卡 0
     */
    public static int getProvidersName() {
        if (Application.platform == RuntimePlatform.Android)
        {
            return GetJC().CallStatic<int>("getProvidersName");
        }
        else
        {
            return 1;
        }
    }

}

#elif UNITY_IPHONE

public class PlatformHelper
{	

    /// <summary>
    /// QQ sina微博账号登录 
    /// </summary>
    /// <param name="obj">登录按钮</param>
    /// <param name="func">回调方法名</param>
    /// <param name="data">待回调返回到服务端的数据</param>
    [DllImport("__Internal")]
    public static extern void Login(string obj, string func, string data); 

    [DllImport("__Internal")]
    public static extern void WXShareFriends(string obj, string func, string data);

    [DllImport("__Internal")]
    public static extern void WXShareFriendsCircle(string obj, string func,string data);

    [DllImport("__Internal")]
    public static extern void pay(string payType, string callObj, string callFunc, string data);

    [DllImport("__Internal")]
    public static extern string GetVersionName();

    [DllImport("__Internal")]
    public static extern string getIsTest();
    
    public static string GetChannelName()
    {
        return "ios";
    }

    public static bool isThirdSDKQuit()
    {
        return false;
    }

    public static void thirdSDKQuit(string callObj, string callFunc, string data)
    {
    }

    public static bool IsThirdLogin()
    {
        return false;
    }

    public static string GetMacId()
    {
        return "1";
    }
}

#else
public class PlatformHelper
{
    // 是否在About框中显示Logo
    public static string GetTextOfAbout()
    {
        return "";
    }

    public static string GetPhoneType()
    {
        return "editor";
    }

    public static int GetPower()
    {
        return 100;
    }

    public static string GetUniqueIdentifier()
    {
        return "Unity_Editor";
    }

    public static bool IsIOSForceUpdate()
    {
        return false;
    }

    /// <summary>
    /// 获取渠道号
    /// </summary>
    public static string GetChannelName()
    {
        return "javgame";
    }

    public static string GetVersionName()
    {
        return "1.5?";
    }

    /// <summary>
    /// 获取渠道类型  
    /// </summary>
    /// <param name='uid'>
    /// Uid.
    /// </param>
    public static string GetLoginType()
    {
        return "";
    }

    /// <summary>
    /// 获取渠道相关参数  
    /// </summary>
    /// <returns></returns>
    public static string GetThirdParameters()
    {
        return "";
    }

    public static void Login(string obj, string func, string data)
    {
    }

    public static void WXShareFriends(string obj, string func, string data)
    {
    }

    public static void WXShareFriendsCircle(string obj, string func, string data)
    {
    }

    public static void iOSLog(string str)
    {
        LogUtil.Log(str);
    }

    public static void setProductName(string str)
    {
    }

    /// <summary>
    /// 在外部设置该安装包是否与360等第三方平台合作 
    /// true -- 合作
    /// </summary>
    //    public static void GetIsCollaborate()
    //    {
    //        GlobalData.IsCollaborate = true;
    //
    //    }
    /// <summary>
    /// QQ sina微博账号登录 
    /// 
    /// </summary>
    /// <param name="type">QQ ：104； SinaWeiBo：105</param>
    /// <param name="obj">登录按钮</param>
    /// <param name="func">回调方法名</param>
    /// <param name="data">待回调返回到服务端的数据</param>
    public static void CommonLogin(int type, string obj, string func, string data)
    {
    }

    public static void setLoginResponse(string loginResponse)
    {
    }

    public static void closeService()
    {
    }

    public static bool ForceUpdate()
    {
        return false;
    }


    public static void onUmUserEvent(int type, String eventID, String label)
    {
    }

    public static string getAndroidStringConfig(String name)
    {
        return null;
    }

    public static string getOrderExpand()
    {
        return "";
    }


    public static void isLoginConnectingScene(bool value)
    {
    }

    /// <summary>
    ///  control 91 toolbar show or hide
    /// </summary>
    public static void show91ToolBar(bool value)
    {
    }

    /// <summary>
    /// 是否是第三方SDK 的退出系统 ，如果是第三方系统退出，退出的时候，将要回调
    /// </summary>
    /// <returns></returns>
    public static bool isThirdSDKQuit()
    {
        return false;
    }


    /// <summary>
    /// 通知第三方SDK 的退出
    /// </summary>
    /// <param name="callObj"></param>
    /// <param name="callFunc"></param>
    /// <param name="data"></param>
    public static void thirdSDKQuit(string callObj, string callFunc, string data)
    {
    }


    /// <summary>
    /// 设置回调方法，，第三方SDK退出的时候被通知
    /// </summary>
    /// <param name="callObj"></param>
    /// <param name="callFunc"></param>
    /// <param name="data"></param>
    public static void setQuitCallback(string callObj, string callFunc, string data)
    {
    }


    /// <summary>
    /// 注销账号的时候，通知第三方
    /// </summary>
    /// <param name="callObj"></param>
    /// <param name="callFunc"></param>
    /// <param name="data"></param>
    public static void logout(string callObj, string callFunc, string data)
    {
    }


    /// <summary>
    /// 设置回调方法，第三方SDK注销账号的时候被通知
    /// </summary>
    /// <param name="callObj"></param>
    /// <param name="callFunc"></param>
    /// <param name="data"></param>
    public static void setLogoutCallback(string callObj, string callFunc, string data)
    {
    }


    /// <summary>
    /// 通用的方法，目的是减少接口变化
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static String commonMethod(String input)
    {
        return null;
    }


    /// <summary>
    /// 通用的第三方方法，目的是减少接口变化
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static String thirdCommonMethod(String input)
    {
        return null;
    }


    /// <summary>
    /// 第三方游戏中心（一般是浮动图标），是否可见
    /// </summary>
    /// <param name="value"></param>
    public static void setThirdCenterVisible(bool value)
    {
    }

    /// <summary>
    /// 弹出交叉推广
    /// </summary>
    /// <param name="value"></param>
    public static void popupExchangeService()
    {
    }

    /// <summary>
    /// 显示LogUtil信息
    /// </summary>
    /// <param name="value"></param>
    public static void showDebugInfo(string name, string unityInfo)
    {
    }

    public static void pay(string payType, string callObj, string callFunc, string data)
    {
    }

    /**
     * 取联网方式
     * 无联网 0
     * wifi 1
     * 2G 2 
     * 3G 3
     * 4G 4
     * @param value
     */
    public static int getNetWork()
    {
        return 1;
    }

    /**
     * 1表示测试包
     */
    public static string getIsTest()
    {
        return "1";
    }

    /**
     * 获取手机服务商信息 
     * 中国移动 1 
     * 中国联通 2
     * 中国电信 3
     * 无卡 0
     */
    public static int getProvidersName()
    {
        return 1;
    }

    public static void DownApk()
    {
    }

    public static bool IsThirdLogin()
    {
        return false;
    }

    public static string GetMacId()
    {
        return "1";
    }
}
#endif


public class HelperPlatform : PlatformHelper
{
}