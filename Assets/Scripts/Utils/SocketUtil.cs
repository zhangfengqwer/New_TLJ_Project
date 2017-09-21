﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

class SocketUtil
{
    static SocketUtil s_instance = null;

    public delegate void OnSocketEvent_Connect(bool result);    // 连接服务器结果
    public delegate void OnSocketEvent_Receive(string data);    // 收到服务器消息
    public delegate void OnSocketEvent_Close();                 // 与服务器非正常断开连接
    public delegate void OnSocketEvent_Stop();                  // 与服务器正常断开连接

    OnSocketEvent_Connect m_onSocketEvent_Connect = null;
    OnSocketEvent_Receive m_onSocketEvent_Receive = null;
    OnSocketEvent_Close m_onSocketEvent_Close = null;
    OnSocketEvent_Stop m_onSocketEvent_Stop = null;

    Socket m_socket = null;
    IPAddress m_ipAddress = null;
    int m_ipPort = 0;
    
    bool m_isStart = false;
    bool m_isNormalStop = false;

    // 数据包尾部标识
    char m_packEndFlag = (char)1;
    string m_endStr = "";

    public static SocketUtil getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new SocketUtil();
        }

        return s_instance;
    }

    public void setOnSocketEvent_Connect(OnSocketEvent_Connect onSocketEvent_Connect)
    {
        m_onSocketEvent_Connect = onSocketEvent_Connect;
    }

    public void setOnSocketEvent_Receive(OnSocketEvent_Receive onSocketEvent_Receive)
    {
        m_onSocketEvent_Receive = onSocketEvent_Receive;
    }

    public void setOnSocketEvent_Close(OnSocketEvent_Close onSocketEvent_Close)
    {
        m_onSocketEvent_Close = onSocketEvent_Close;
    }

    public void setOnSocketEvent_Stop(OnSocketEvent_Stop onSocketEvent_Stop)
    {
        m_onSocketEvent_Stop = onSocketEvent_Stop;
    }

    public void init(string ip,int port)
    {
        m_ipAddress = IPAddress.Parse(ip);
        m_ipPort = port;
    }

    public void start()
    {
        if (!checkSocketIsInit())
        {
            return;
        }

        if (!m_isStart)
        {
            Thread t1 = new Thread(CreateConnectionInThread);
            t1.Start();
        }
        else
        {
            Debug.Log("SocketUtil----连接服务器失败，因为当前已经连接");
        }
    }

    public void stop()
    {
        if (!checkSocketIsInit())
        {
            return;
        }

        if (m_isStart)
        {
            m_isStart = false;
            m_isNormalStop = true;

            if (m_socket != null)
            {
                m_socket.Close();

                if(m_onSocketEvent_Stop != null)
                {
                    m_onSocketEvent_Stop();
                }
            }
        }
        else
        {
            Debug.Log("SocketUtil----断开服务器连接失败，因为当前已经断开");
            m_isNormalStop = true;
        }
    }

    void CreateConnectionInThread()
    {
        try
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPort = new IPEndPoint(m_ipAddress, m_ipPort);
            m_socket.Connect(ipEndPort);
            
            if (m_onSocketEvent_Connect != null)
            {
                m_onSocketEvent_Connect(true);
            }

            m_isStart = true;
            m_isNormalStop = false;

            receive();
        }
        catch (SocketException ex)
        {
            Debug.Log("SocketUtil----连接服务器失败：" + ex.Message);

            if (!m_isNormalStop)
            {
                m_onSocketEvent_Connect(false);
            }
        }
    }

    public void sendMessage(string sendData)
    {
        if (!checkSocketIsInit())
        {
            return;
        }

        if (m_isStart)
        {
            sendData = sendData.Replace("\r\n", "");
            Debug.Log("SocketUtil----发送给服务端消息：" + sendData);

            try
            {
                byte[] bytes = new byte[1024];
                bytes = Encoding.UTF8.GetBytes(sendData);
                m_socket.Send(bytes);
            }
            catch (SocketException ex)
            {
                if (!m_isNormalStop)
                {
                    Debug.Log("SocketUtil----与服务端连接断开：" + ex.Message);

                    m_isStart = false;

                    if (m_onSocketEvent_Close != null)
                    {
                        m_onSocketEvent_Close();
                    }
                }
            }
        }
        else
        {
            Debug.Log("SocketUtil----发送消息失败：已经与服务端断开");
        }
    }

    public void receive()
    {
        while (m_isStart)
        {
            try
            {
                byte[] rece = new byte[2048];
                int recelong = m_socket.Receive(rece, rece.Length, 0);
                string reces = Encoding.UTF8.GetString(rece, 0, recelong);

                reces = m_endStr + reces;

                reces = reces.Replace("\r\n", "");

                //Debug.Log("SocketUtil----收到服务端消息：" + reces);

                if (reces.CompareTo("") != 0)
                {
                    List<string> list = new List<string>();
                    bool b = CommonUtil.splitStrIsPerfect(reces, list , m_packEndFlag);

                    if (b)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (m_onSocketEvent_Receive != null)
                            {
                                m_onSocketEvent_Receive(list[i]);
                            }
                        }

                        reces = "";
                    }
                    else
                    {
                        for (int i = 0; i < list.Count - 1; i++)
                        {
                            if (m_onSocketEvent_Receive != null)
                            {
                                m_onSocketEvent_Receive(list[i]);
                            }
                        }

                        m_endStr = list[list.Count - 1];
                    }
                }
                else
                {
                    if (!m_isNormalStop)
                    {
                        Debug.Log("SocketUtil----被动与服务端连接断开");

                        m_isStart = false;

                        if (m_onSocketEvent_Close != null)
                        {
                            m_onSocketEvent_Close();
                        }
                    }

                    return;
                }
            }
            catch (SocketException ex)
            {
                if (!m_isNormalStop)
                {
                    Debug.Log("SocketUtil----被动与服务端连接断开：" + ex.Message);

                    m_isStart = false;

                    if (m_onSocketEvent_Close != null)
                    {
                        m_onSocketEvent_Close();
                    }
                }

                return;
            }
        }
    }

    bool checkSocketIsInit()
    {
        if (m_ipAddress == null || m_ipPort == 0)
        {
            Debug.Log("SocketUtil----没有设置IP和端口");
            return false;
        }

        return true;
    }
}