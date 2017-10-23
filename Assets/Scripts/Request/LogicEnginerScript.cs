using System;
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

    private List<Request> requestList = new List<Request>();

    //请求
    private GetSignRecordRequest _getSignRecordRequest;

    private GetUserInfoRequest _getUserInfoRequest;
    private GetEmailRequest _getEmailRequest;
    private GetNoticeRequest _getNoticeRequest;
    private MainRequest _mainRequest;
    [HideInInspector] public GetUserBagRequest _getUserBagRequest;


    //判断loading中是否返回所有需要的信息
    public static List<bool> IsSuccessList = new List<bool>();

    private void Awake()
    {
        Instance = this;
//        if (Instance == null)
//        {
//            
//            DontDestroyOnLoad(this.gameObject);
//        }
//        else if (Instance != this)
//        {
//            Destroy(this.gameObject);
//        }
    }

    private void Update()
    {
    }

    private void Start()
    {
        AddRequest();
        InitRequest();
        InitSocket();
    }

    private void AddRequest()
    {
    }

    private void InitRequest()
    {
        _getSignRecordRequest = GetComponent<GetSignRecordRequest>();
        _getUserInfoRequest = GetComponent<GetUserInfoRequest>();
        _getEmailRequest = GetComponent<GetEmailRequest>();
        _getUserBagRequest = GetComponent<GetUserBagRequest>();
        _getNoticeRequest = GetComponent<GetNoticeRequest>();
        _mainRequest = GetComponent<MainRequest>();
        LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().CallBack = onReceive_GetUserBag;
    }


    /// <summary>
    /// 设置Socket事件
    /// </summary>
    public void InitSocket()
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
            //发送 一些数据的请求
            SendRequest();
        }
        else
        {
            Debug.Log("连接服务器失败，尝试重新连接");
            SocketUtil.getInstance().start();
        }
    }

    /// <summary>
    /// loading过程中需要添加的请求
    /// </summary>
    private void SendRequest()
    {
        _getUserInfoRequest.OnRequest();
        _getSignRecordRequest.OnRequest();
        _getEmailRequest.OnRequest();
        _getUserBagRequest.OnRequest();
       

        _getNoticeRequest.OnRequest();
    }

    private void onReceive_GetUserBag(string result)
    {
        JsonData jsonData = JsonMapper.ToObject(result);
        var code = (int) jsonData["code"];
        if (code == (int) Consts.Code.Code_OK)
        {
            UserData.propData = JsonMapper.ToObject<List<UserPropData>>(jsonData["prop_list"].ToString());
        }
        else
        {
            ToastScript.createToast("用户背包数据错误");
            return;
        }
    }

    private void onSocketReceive(string data)
    {
        Debug.Log("收到服务器消息:" + data);
        try
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            JsonData jd = JsonMapper.ToObject(System.Text.Encoding.UTF8.GetString(bytes));
            var tag = jd["tag"].ToString();
            print("tag:" + tag);
            Request request = null;
            bool getValue = requestDic.TryGetValue(tag, out request);
            if (getValue)
            {
                request.OnResponse(data);
            }
            else
            {
                _mainRequest.OnResponse(data);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
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

    public void SendMyMessage(string sendData)
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