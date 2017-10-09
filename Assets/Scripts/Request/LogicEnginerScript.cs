using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using TLJCommon;
using UnityEngine.SceneManagement;

public class LogicEnginerScript : MonoBehaviour
{
    bool m_isConnServerSuccess = false;
    public static bool IsLogicConnect = false;
    public static LogicEnginerScript Instance;
    private Dictionary<string, Request> requestDic = new Dictionary<string, Request>();
    private GetSignRecordRequest _getSignRecordRequest;
    //判断loading中是否返回所有需要的信息
    public static List<bool> IsSuccessList = new List<bool>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        if (IsSuccessList.Count == 1)
        {
            SceneManager.LoadScene("MainScene");
            IsSuccessList.Clear();
        }
    }

    private void Start()
    {
        init();
        _getSignRecordRequest = GetComponent<GetSignRecordRequest>();
      
       
    }


    /// <summary>
    /// 设置Socket事件
    /// </summary>
    public void init()
    {
        SocketUtil.getInstance().setOnSocketEvent_Connect(onSocketConnect);
        SocketUtil.getInstance().setOnSocketEvent_Receive(onSocketReceive);
        SocketUtil.getInstance().setOnSocketEvent_Close(onSocketClose);
        SocketUtil.getInstance().setOnSocketEvent_Stop(onSocketStop);
        SocketUtil.getInstance().init(NetConfig.s_logicService_ip, NetConfig.s_logicService_port);
        SocketUtil.getInstance().start();
    }
  
    private void onSocketConnect(bool result)
    {
        if (result)
        {
            Debug.Log("连接服务器成功");
            IsLogicConnect = true;
            m_isConnServerSuccess = true;
            //发送 请求签到数据的请求
            _getSignRecordRequest.OnRequest();
        }
        else
        {
            Debug.Log("连接服务器失败，尝试重新连接");
            SocketUtil.getInstance().start();
        }
    }

    private void onSocketReceive(string data)
    {
        Debug.Log("收到服务器消息:" + data);
        JsonData jd = JsonMapper.ToObject(data);
        var tag = jd["tag"].ToString();
        Request request = null;
        bool getValue = requestDic.TryGetValue(tag, out request);
        if (getValue)
        {
            request.OnResponse(data);
        }
        else
        {
            Debug.Log("没有找到tag对应的请求对象");
        }
    }

    private void onSocketStop()
    {
        Debug.Log("主动与服务器断开连接");
    }

    private void onSocketClose()
    {
        Debug.Log("被动与服务器断开连接,尝试重新连接");
        SocketUtil.getInstance().start();
    }

    public void SendMessage(string sendData)
    {
        SocketUtil.getInstance().sendMessage(sendData);
    }

    public void AddRequest(Request request)
    {
        requestDic.Add(request.Tag, request);
    }
    public void ReMoveRequest(Request request)
    {
        requestDic.Remove(request.Tag);
    }

    private void OnDestroy()
    {
        SocketUtil.getInstance().stop();
    }
}