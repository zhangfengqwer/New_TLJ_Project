using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SocketUtil
{
    //static SocketUtil s_instance = null;

    public delegate void OnSocketEvent_Connect(bool result); // 连接服务器结果

    public delegate void OnSocketEvent_Receive(string data); // 收到服务器消息

    public delegate void OnSocketEvent_Close(); // 与服务器非正常断开连接

    public delegate void OnSocketEvent_Stop(); // 与服务器正常断开连接

    public OnSocketEvent_Connect m_onSocketEvent_Connect = null;
    public OnSocketEvent_Receive m_onSocketEvent_Receive = null;
    public OnSocketEvent_Close m_onSocketEvent_Close = null;
    public OnSocketEvent_Stop m_onSocketEvent_Stop = null;

    public Socket m_socket = null;
    public IPAddress m_ipAddress = null;
    public int m_ipPort = 0;

    public bool m_isStart = false;
    public bool m_isNormalStop = false;

    // 数据包尾部标识
    public char m_packStartFlag = (char) 1;

    public string m_endStr = "";

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

    public void init(string ip, int port)
    {
        try
        {
            m_ipAddress = IPAddress.Parse(ip);
            m_ipPort = port;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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
            LogUtil.Log("SocketUtil----连接服务器失败，因为当前已经连接  " + m_ipAddress.ToString() + "  " + m_ipPort);
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

                if (m_onSocketEvent_Stop != null)
                {
                    m_onSocketEvent_Stop();
                }
            }
        }
        else
        {
            LogUtil.Log("SocketUtil----断开服务器连接失败，因为当前已经断开  " + m_ipAddress.ToString() + "  " + m_ipPort);
            m_isNormalStop = true;
        }
    }

    public void CreateConnectionInThread()
    {
        try
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipEndPort = new IPEndPoint(m_ipAddress, m_ipPort);
            m_socket.Connect(ipEndPort);
            m_isStart = true;
            m_isNormalStop = false;
            if (m_onSocketEvent_Connect != null)
            {
                m_onSocketEvent_Connect(true);
            }
            StartReceive();
        }
        catch (SocketException ex)
        {
            LogUtil.Log("SocketUtil----连接服务器失败：" + ex.Message + "  " + m_ipAddress.ToString() + "  " + m_ipPort);

            //if (!m_isNormalStop)
            //{
            //    m_onSocketEvent_Connect(false);
            //}

            m_onSocketEvent_Connect(false);
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
            //LogUtil.Log("SocketUtil----发送给服务端消息：" + sendData);

            try
            {
                byte[] bytes = new byte[1024 * 4];

                // 增加数据包尾部标识
                bytes = Encoding.UTF8.GetBytes(sendData + m_packStartFlag);

                LogUtil.Log("SocketUtil----发送给服务端消息：" + Encoding.UTF8.GetString(bytes, 0, bytes.Length));

                m_socket.Send(bytes);
            }
            catch (SocketException ex)
            {
                if (!m_isNormalStop)
                {
                    LogUtil.Log(
                        "SocketUtil----与服务端连接断开：" + ex.Message + "  " + m_ipAddress.ToString() + "  " + m_ipPort);

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
            LogUtil.Log("SocketUtil----发送消息失败：已经与服务端断开  " + m_ipAddress.ToString() + "  " + m_ipPort);
        }
    }

    public void StartReceive()
    {
        string result = null;
        bool isEnd = true;
        ushort size = 0;
        //当前读取的长度
        int alreadyReaderCount = 0;
        //当前读取的字节流
        byte[] alreadyReaderBody = new byte[] { };
        while (m_isStart)
        {
            try
            {
                if (!isEnd)
                {
                    int left = size - alreadyReaderCount;
                    var bytes = new byte[left];
                    int currentReadCount = m_socket.Receive(bytes, bytes.Length, SocketFlags.None);
                    if (currentReadCount == 0) continue;
                    if (currentReadCount < left)
                    {
                        alreadyReaderCount += currentReadCount;
                        byte[] body = new byte[currentReadCount];
                        Buffer.BlockCopy(bytes, 0, body, 0, body.Length);
                        alreadyReaderBody = CombineBytes(alreadyReaderBody, body);
                        continue;
                    }

                    byte[] combineBytes = CombineBytes(alreadyReaderBody, bytes);
                    result = Encoding.UTF8.GetString(combineBytes);
                    isEnd = true;
                    m_onSocketEvent_Receive(result);
                }
                else
                {
                    var len = new byte[2];
                    if (m_socket.Receive(len, len.Length, SocketFlags.None) != 0)
                    {
                        char c = BitConverter.ToChar(len, 0);
                        if (c != m_packStartFlag)
                        {
//                            LogUtil.Log("第一个字节不是1");
                            continue;
                        }
                    }
                    else
                    {
                        if (!m_isNormalStop)
                        {
                            LogUtil.Log("SocketUtil----被动与服务端连接断开  " + m_ipAddress.ToString() + "  " + m_ipPort);

                            m_isStart = false;

                            if (m_onSocketEvent_Close != null)
                            {
                                m_onSocketEvent_Close();
                            }
                        }

                        return;
                    }

                    int read = m_socket.Receive(len, len.Length, SocketFlags.None);

                    if (read != 0)
                    {
                        size = (ushort) BitConverter.ToInt16(len, 0);
                        alreadyReaderBody = new byte[size];
                        alreadyReaderCount = m_socket.Receive(alreadyReaderBody, alreadyReaderBody.Length, SocketFlags.None);
                        if (alreadyReaderCount < size)
                        {
                            var body = new byte[alreadyReaderCount];
                            Buffer.BlockCopy(alreadyReaderBody, 0, body, 0, body.Length);
                            alreadyReaderBody = body;
                            isEnd = false;
                        }
                        else
                        {
                            result = Encoding.UTF8.GetString(alreadyReaderBody, 0, alreadyReaderBody.Length);
                            isEnd = true;
                            m_onSocketEvent_Receive(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!m_isNormalStop)
                {
                    LogUtil.Log("SocketUtil----被动与服务端连接断开：" + ex.Message + "  " + m_ipAddress.ToString() + "  " +
                                m_ipPort);

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

    public byte[] CombineBytes(byte[] data1, byte[] data2)
    {
        byte[] data = new byte[data1.Length + data2.Length];
        Buffer.BlockCopy(data1, 0, data, 0, data1.Length); //这种方法仅适用于字节数组
        Buffer.BlockCopy(data2, 0, data, data1.Length, data2.Length);
        return data;
    }

    public bool checkSocketIsInit()
    {
        if (m_ipAddress == null || m_ipPort == 0)
        {
            LogUtil.Log("SocketUtil----没有设置IP和端口");
            return false;
        }

        return true;
    }
}