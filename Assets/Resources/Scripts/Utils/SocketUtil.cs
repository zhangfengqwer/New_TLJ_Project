using System;
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

    public delegate void OnSocketEvent_Connect();
    public delegate void OnSocketEvent_Receive(string data);
    public delegate void OnSocketEvent_Close();

    OnSocketEvent_Connect m_onSocketEvent_Connect = null;
    OnSocketEvent_Receive m_onSocketEvent_Receive = null;
    OnSocketEvent_Close m_onSocketEvent_Close = null;
    OnSocketEvent_Close m_onSocketEvent_Stop = null;

    Socket m_socket = null;
    IPAddress m_ipAddress = IPAddress.Parse("10.224.5.110");
    int m_ipPort = 60001;
    
    bool m_isStart = false;

    // 数据包尾部标识
    string m_packEndFlag = "..";
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

    public void start()
    {
        Thread t1 = new Thread(CreateConnectionInThread);
        t1.Start();
    }

    void CreateConnectionInThread()
    {
        try
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPort = new IPEndPoint(m_ipAddress, m_ipPort);
            m_socket.Connect(ipEndPort);

            m_onSocketEvent_Connect();
            
            receive();
        }
        catch (SocketException ex)
        {
            Debug.Log("连接服务器失败");
            Debug.Log("错误日志：" + ex.Message);

            //m_netListen.onNetListenError("");
        }
    }

    public void stop()
    {
        if (m_socket != null)
        { 
            m_socket.Close();
            m_onSocketEvent_Stop();
        }
    }

    public void sendMessage(string sendData)
    {
        sendData = sendData.Replace("\r\n", "");
        Debug.Log("发送给服务端消息：" + sendData);

        //sendData += m_packEndFlag;

        try
        {
            byte[] bytes = new byte[1024];
            bytes = Encoding.UTF8.GetBytes(sendData);
            m_socket.Send(bytes);
        }
        catch (SocketException ex)
        {
            Debug.Log("与服务端连接断开");
            Debug.Log("错误日志：" + ex.Message);

            m_onSocketEvent_Close();
        }
    }

    public void receive()
    {
        while (true)
        {
            try
            {
                byte[] rece = new byte[2048];
                int recelong = m_socket.Receive(rece, rece.Length, 0);
                string reces = Encoding.UTF8.GetString(rece, 0, recelong);

                reces = m_endStr + reces;

                reces = reces.Replace("\r\n", "");

                Debug.Log("----收到服务端消息：" + reces);
                if (reces.CompareTo("") != 0)
                {
                    List<string> list = new List<string>();
                    bool b = CommonUtil.splitStrIsPerfect(reces, list, "..");

                    if (b)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            m_onSocketEvent_Receive(list[i]);
                        }

                        reces = "";
                    }
                    else
                    {
                        for (int i = 0; i < list.Count - 1; i++)
                        {
                            m_onSocketEvent_Receive(list[i]);
                        }

                        m_endStr = list[list.Count - 1];
                    }
                }
                else
                {
                    Debug.Log("--与服务端连接断开");
                    m_onSocketEvent_Close();

                    return;
                }
            }
            catch (SocketException ex)
            {
                Debug.Log("与服务端连接断开");
                Debug.Log("错误日志：" + ex.Message);
                m_onSocketEvent_Close();

                return;
            }
        }
    }
}
