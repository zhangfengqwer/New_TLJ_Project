using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class NetConfig
{
    // 登录服务器
    public static string s_loginService_ip;
    public static string s_loginService_yuming;     // ios用
    public static int s_loginService_port;

    // 普通逻辑服务器
    public static string s_logicService_ip;
    public static string s_logicService_yuming;     // ios用
    public static int s_logicService_port;

    // 打牌服务器
    public static string s_playService_ip;
    public static string s_playService_yuming;     // ios用
    public static int s_playService_port;

    // 数据库服务器
    public static string s_mySqlService_ip;
    public static string s_mySqlService_yuming;     // ios用
    public static int s_mySqlService_port;

    public static void reqNetConfig()
    {

        UnityWebReqUtil.Instance.Get(OtherData.getWebUrl() + "NetConfig.json", httpCallBack);

        //// 使用本地配置文件
        //string jsonData = Resources.Load("Entity/NetConfig").ToString();
        //httpCallBack("", jsonData);
    }

    public static void httpCallBack(string tag, string data)
    {
        try
        {
            // 读取配置文件
            {
                string str = "";

                if (true)
                {
                    str = data;
                }
                // 使用本地的
                else
                {
                    StreamReader sr = new StreamReader("NetConfig.json");
                    str = sr.ReadToEnd().ToString();
                    sr.Close();
                }
                
                JsonData jd = JsonMapper.ToObject(str);

                // 登录服务器
                s_loginService_ip = jd["LoginService"]["ip"].ToString();
                s_loginService_yuming = jd["LoginService"]["yuming"].ToString();
                s_loginService_port = (int) jd["LoginService"]["port"];

                // 普通逻辑服务器
                s_logicService_ip = jd["LogicService"]["ip"].ToString();
                s_logicService_yuming = jd["LogicService"]["yuming"].ToString();
                s_logicService_port = (int) jd["LogicService"]["port"];

                // 打牌服务器
                s_playService_ip = jd["PlayService"]["ip"].ToString();
                s_playService_yuming = jd["PlayService"]["yuming"].ToString();
                s_playService_port = (int) jd["PlayService"]["port"];

                // 数据库服务器
                s_mySqlService_ip = jd["MySqlService"]["ip"].ToString();
                s_mySqlService_yuming = jd["MySqlService"]["yuming"].ToString();
                s_mySqlService_port = (int) jd["MySqlService"]["port"];
            }

            OtherData.s_getNetEntityFile.GetFileSuccess("NetConfig.json");
        }
        catch (Exception ex)
        {
            LogUtil.Log("获取网络配置文件出错：" + ex.Message);
            OtherData.s_getNetEntityFile.GetFileFail("NetConfig.json");

            //throw ex;
        }
    }
}