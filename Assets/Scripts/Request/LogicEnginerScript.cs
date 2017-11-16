using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using TLJCommon;
using UnityEngine.SceneManagement;

public class LogicEnginerScript : MonoBehaviour
{
    public static LogicEnginerScript Instance;

    public SocketUtil m_socketUtil;
   
    private Dictionary<string, Request> requestDic = new Dictionary<string, Request>();
    private List<Request> requestList = new List<Request>();

    //请求
    private GetSignRecordRequest _getSignRecordRequest;
    private GetUserInfoRequest _getUserInfoRequest;
    private GetEmailRequest _getEmailRequest;
    private GetNoticeRequest _getNoticeRequest;
    private MainRequest _mainRequest;
    private GetRankRequest _getRankRequest;
    private GetTaskRequest _getTaskRequest;
    [HideInInspector] public GetUserBagRequest _getUserBagRequest;

    //判断loading中是否返回所有需要的信息
    public static List<bool> IsSuccessList = new List<bool>();

    bool m_isConnecion = false;
    bool m_isCloseSocket = false;
    int m_connectState = 2;             // 0:连接失败  1:连接成功   2:无状态

    public delegate void OnLogicService_Close();                        // 与服务器断开
    OnLogicService_Close m_onLogicService_Close = null;

    public delegate void OnLogicService_Connect(bool result);           // 连接服务器结果
    OnLogicService_Connect m_onLogicService_Connect = null;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/Logic/LogicEnginer") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab);

        return obj;
    }

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
        // 断开连接
        if (m_isCloseSocket)
        {
            m_isCloseSocket = false;

            if (m_onLogicService_Close != null)
            {
                m_onLogicService_Close();
            }
        }

        // 连接失败
        if (m_connectState == 0)
        {
            m_connectState = 2;

            if (m_onLogicService_Connect != null)
            {
                m_onLogicService_Connect(false);
            }
        }
        // 连接成功
        else if (m_connectState == 1)
        {
            m_connectState = 2;

            if (m_onLogicService_Connect != null)
            {
                m_onLogicService_Connect(true);
            }
        }
    }

    public bool isConnecion()
    {
        return m_isConnecion;
    }

    private void Start()
    {
        AddRequest();
        InitRequest();
        InitSocket();
    }

    public void Stop()
    {
        m_socketUtil.stop();
    }

    public void clear()
    {
        Instance = null;
        Destroy(gameObject);
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
        _getRankRequest = GetComponent<GetRankRequest>();
        _mainRequest = GetComponent<MainRequest>();
        _getTaskRequest = GetComponent<GetTaskRequest>();
        LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().CallBack = onReceive_GetUserBag;
        LogicEnginerScript.Instance.GetComponent<GetRankRequest>().CallBack = onReceive_GetGoldRank;
    }


    /// <summary>
    /// 设置Socket事件
    /// </summary>
    public void InitSocket()
    {
        m_socketUtil = new SocketUtil();
        m_socketUtil.setOnSocketEvent_Connect(onSocketConnect);
        m_socketUtil.setOnSocketEvent_Receive(onSocketReceive);
        m_socketUtil.setOnSocketEvent_Close(onSocketClose);
        m_socketUtil.setOnSocketEvent_Stop(onSocketStop);

        m_socketUtil.init(NetConfig.s_logicService_ip, NetConfig.s_logicService_port);
        m_socketUtil.start();
    }

    public void setOnLogicService_Connect(OnLogicService_Connect onLogicService_Connect)
    {
        m_onLogicService_Connect = onLogicService_Connect;
    }

    public void setOnLogicService_Close(OnLogicService_Close onLogicService_Close)
    {
        m_onLogicService_Close = onLogicService_Close;
    }

    private void onSocketConnect(bool result)
    {
        if (result)
        {
            LogUtil.Log("Logic:连接服务器成功");
            m_connectState = 1;
            m_isConnecion = true;
        }
        else
        {
            LogUtil.Log("Logic:连接服务器失败，尝试重新连接");
            m_connectState = 0;
            m_isConnecion = false;
        }
    }

    private void onSocketStop()
    {
        LogUtil.Log("logic:主动与服务器断开连接");
        m_isConnecion = false;
    }

    private void onSocketClose()
    {
        LogUtil.Log("logic:被动与服务器断开连接,尝试重新连接");
        m_isCloseSocket = true;
        m_isConnecion = false;
    }

    private void onSocketReceive(string data)
    {
        LogUtil.Log("收到服务器消息:" + data);
        try
        {
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
                _mainRequest.OnResponse(data);
            }
        }
        catch (Exception e)
        {
            LogUtil.Log(e.Message);
        }
    }

    /// <summary>
    /// loading过程中需要添加的请求
    /// </summary>
    //private void SendRequest()
    //{
    //    _getUserInfoRequest.OnRequest();
    //    _getSignRecordRequest.OnRequest();
    //    //排行榜
    //    _getRankRequest.OnRequest();

    //    _getEmailRequest.OnRequest();
    //    _getUserBagRequest.OnRequest();

    //    _getNoticeRequest.OnRequest();

    //    _getTaskRequest.OnRequest();
    //}

    //收到金币排行榜回调
    private void onReceive_GetGoldRank(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int) jd["code"];

        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            RankData.goldRankDataList = JsonMapper.ToObject<List<GoldRankItemData>>(jd["gold_list"].ToString());
            RankData.medalRankDataList = JsonMapper.ToObject<List<MedalRankItemData>>(jd["medal_list"].ToString());

            RankListJifenScript.Instance.InitData();
            RankListJifenScript.Instance.InitUI();

            RankListCaifuScript.Instance.InitData();
            RankListCaifuScript.Instance.InitUI();
        }
        else
        {
            ToastScript.createToast("金币排行榜数据错误");
        }
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

    public void SendMyMessage(string sendData)
    {
        m_socketUtil.sendMessage(sendData);
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
        m_socketUtil.stop();
    }

    public void startConnect()
    {
        m_socketUtil.start();
    }
}