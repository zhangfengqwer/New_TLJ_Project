using System;
using System.Text;
using UnityEngine;
using HPSocketCS;

public class HpSocketEngine : MonoBehaviour {
    private TcpPackClient client;
    private String ip = "10.224.5.82";
    private ushort port = 60005;
    public static HpSocketEngine Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }
    // Use this for initialization
    void Start()
    {
        InitClient();
        Connet();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Send();
        }
    }

    private void InitClient()
    {
        client = new TcpPackClient();
        // 设置client事件
        client.OnPrepareConnect += new TcpClientEvent.OnPrepareConnectEventHandler(OnPrepareConnect);
        client.OnConnect += new TcpClientEvent.OnConnectEventHandler(OnConnect);
        client.OnSend += new TcpClientEvent.OnSendEventHandler(OnSend);
        client.OnReceive += new TcpClientEvent.OnReceiveEventHandler(OnReceive);
        client.OnClose += new TcpClientEvent.OnCloseEventHandler(OnClose);
        // 设置包头标识,与对端设置保证一致性
        client.PackHeaderFlag = 0xff;
        // 设置最大封包大小
        client.MaxPackSize = 0x1000;
    }
    private void Send()
    {
        IntPtr connId = client.ConnectionId;
        string str = "{\"tag\":\"Login\",\"connId\" :1,\"account\":\"123\",\"password\":\"123\"}";
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        if (client.Send(bytes, bytes.Length))
        {
            print("发送数据成功：" + str);
        }
        else
        {
            print("发送数据失败：" );
        }
    }

    private void Connet()
    {
        if (client.Connect(ip,port,false))
        {
            print("连接上服务器：" + ip + " " + port);
        }else
            print("连接服务器失败");
    }

    HandleResult OnPrepareConnect(TcpClient sender, IntPtr socket)
    {
        return HandleResult.Ok;
    }

    HandleResult OnConnect(TcpClient sender)
    {
        // 已连接 到达一次
        // 如果是异步联接,更新界面状态
        return HandleResult.Ok;
    }

    HandleResult OnSend(TcpClient sender, byte[] bytes)
    {
        // 客户端发数据了

        return HandleResult.Ok;
    }

    HandleResult OnReceive(TcpClient sender, byte[] bytes)
    {
        // 数据到达了
        var s1 = Convert.ToString(bytes);
        var s2 = Encoding.UTF8.GetString(bytes);
        var s = bytes.ToString();
        print("服务端回调："+ s2 + "\n"+ "服务端回调：" + s1);


        return HandleResult.Ok;
    }

    HandleResult OnClose(TcpClient sender, SocketOperation enOperation, int errorCode)
    {
        if (errorCode == 0)
        {
            // 连接关闭了
        }

        else
        {
            // 出错了
        }

        // 通知界面,只处理了连接错误,也没进行是不是连接错误的判断,所以有错误就会设置界面
        // 生产环境请自己控制

        return HandleResult.Ok;
    }
}
