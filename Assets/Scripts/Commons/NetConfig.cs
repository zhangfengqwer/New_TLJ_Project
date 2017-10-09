using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class NetConfig
{
    enum NetType
    {
        NetType_Dev,
        NetType_Test,
        NetType_Formal,
    }

    static NetType m_netType = NetType.NetType_Test;

    // 登录服务器
    public static string s_loginService_ip;
    public static int s_loginService_port;

    // 普通逻辑服务器
    public static string s_logicService_ip;
    public static int s_logicService_port;

    // 打牌服务器
    public static string s_playService_ip;
    public static int s_playService_port;

    // 数据库服务器
    public static string s_mySqlService_ip;
    public static int s_mySqlService_port;

    public static void reqNetConfig()
    {
        switch (m_netType)
        {
            case NetType.NetType_Dev:
                {
                    UnityWebReqUtil.Instance.Get("http://oru510uv8.bkt.clouddn.com/NetConfig_dev.json", httpCallBack);
                }
                break;

            case NetType.NetType_Test:
                {
                    UnityWebReqUtil.Instance.Get("http://oru510uv8.bkt.clouddn.com/NetConfig_test.json", httpCallBack);
                }
                break;

            case NetType.NetType_Formal:
                {
                    //UnityWebReqUtil.Instance.Get("http://oru510uv8.bkt.clouddn.com/NetConfig.json", httpCallBack);
                }
                break;
        }
        
    }

    static void httpCallBack(string tag,string data)
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
                s_loginService_port = (int)jd["LoginService"]["port"];

                // 普通逻辑服务器
                s_logicService_ip = jd["LogicService"]["ip"].ToString();
                s_logicService_port = (int)jd["LogicService"]["port"];

                // 打牌服务器
                s_playService_ip = jd["PlayService"]["ip"].ToString();
                s_playService_port = (int)jd["PlayService"]["port"];

                // 数据库服务器
                s_mySqlService_ip = jd["MySqlService"]["ip"].ToString();
                s_mySqlService_port = (int)jd["MySqlService"]["port"];

                GameObject.Find("Login").GetComponent<LoginScript>().init();
//                GameObject.Find("LogicEnginer").GetComponent<LogicEnginerScript>().init();
            }
        }
        catch (Exception ex)
        {
            Debug.Log("读取网络配置文件出错：" + ex.Message);
        }
    }
}
