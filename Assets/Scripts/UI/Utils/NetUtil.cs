using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class NetListen
{
    virtual public void onNetListen(string tag , string data) {}
    virtual public void onNetListen(string data) { }

    virtual public void onNetListenError(string tag) { }
}

class ReqParameter:Object
{
    public NetListen m_netListen;
    public string m_tag;
    public string m_reqData;

    public ReqParameter(NetListen netListen , string tag , string reqData)
    {
        m_netListen = netListen;
        m_tag = tag;
        m_reqData = reqData;
    }
}

class NetUtil
{
    static NetUtil s_netUtil = null;

    //IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
    IPAddress ipAddress = IPAddress.Parse("10.224.5.62");
    int ipPort = 10001;
    IPEndPoint iPEndPoint;

    public static NetUtil getInstance()
    {
        if (s_netUtil == null)
        {
            s_netUtil = new NetUtil();
        }

        return s_netUtil;
    }

    public void reqNet(NetListen netListen, string tag, string reqData)
    {
        Thread t1 = new Thread(new ParameterizedThreadStart(ReqServer));
        t1.Start(new ReqParameter(netListen , tag , reqData));
    }

    void ReqServer(object reqData)
    {
        ReqParameter reqParameter = (ReqParameter)reqData;

        try
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            iPEndPoint = new IPEndPoint(ipAddress, ipPort);
            socket.Connect(iPEndPoint);

            Console.WriteLine("连接服务器成功");

            // 发送消息
            sendmessage(socket, reqParameter.m_reqData);
            Console.WriteLine("发送消息：" + reqParameter.m_reqData);

            // 接收消息
            string reces = receive(socket);
            //Console.WriteLine("收到服务端消息：" + reces);
            // 调用回调
            if (reqParameter.m_netListen != null)
            {
                reqParameter.m_netListen.onNetListen(reqParameter.m_tag , reces);
            }

            socket.Close();
        }
        catch (SocketException ex)
        {
            Console.WriteLine("异常：" + ex.Message);

            // 调用回调
            if (reqParameter.m_netListen != null)
            {
                reqParameter.m_netListen.onNetListenError(reqParameter.m_tag);
            }
        }
    }

    void sendmessage(Socket socket, string sendData)
    {
        byte[] bytes = new byte[1024];
        bytes = Encoding.ASCII.GetBytes(sendData);
        socket.Send(bytes);
    }

    string receive(Socket socket)
    {
        byte[] rece = new byte[1024];
        int recelong = socket.Receive(rece, rece.Length, 0);
        string reces = Encoding.ASCII.GetString(rece, 0, recelong);
            
        return reces;
    }
}
