using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class NetListen
{
    virtual public void onNetListen(string tag , string data) {}
    virtual public void onNetListen(string data) { }

    virtual public void onNetListenError(string tag) { }
}

public class ReqParameter :Object
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

public class NetUtil
{
    public static NetUtil s_netUtil = null;

    //IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
    public IPAddress ipAddress = IPAddress.Parse("10.224.5.62");
    public int ipPort = 10001;
    public IPEndPoint iPEndPoint;

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NetUtil_hotfix", "reqNet"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NetUtil_hotfix", "reqNet", null, netListen, tag, reqData);
            return;
        }

        Thread t1 = new Thread(new ParameterizedThreadStart(ReqServer));
        t1.Start(new ReqParameter(netListen , tag , reqData));
    }

    public void ReqServer(object reqData)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NetUtil_hotfix", "ReqServer"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NetUtil_hotfix", "ReqServer", null, reqData);
            return;
        }

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

    public void sendmessage(Socket socket, string sendData)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NetUtil_hotfix", "sendmessage"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NetUtil_hotfix", "sendmessage", null, socket, sendData);
            return;
        }

        byte[] bytes = new byte[1024];
        bytes = Encoding.ASCII.GetBytes(sendData);
        socket.Send(bytes);
    }

    public string receive(Socket socket)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("NetUtil_hotfix", "receive"))
        {
            return (string)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.NetUtil_hotfix", "receive", null, socket);
        }

        byte[] rece = new byte[1024];
        int recelong = socket.Receive(rece, rece.Length, 0);
        string reces = Encoding.ASCII.GetString(rece, 0, recelong);
            
        return reces;
    }
}
