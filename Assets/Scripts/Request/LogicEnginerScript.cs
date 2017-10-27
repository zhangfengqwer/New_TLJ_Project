﻿using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using TLJCommon;
using UnityEngine.SceneManagement;

public class LogicEnginerScript : MonoBehaviour
{
    public SocketUtil m_socketUtil;

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
    private GetRankRequest _getRankRequest;
    [HideInInspector] public GetUserBagRequest _getUserBagRequest;


    //判断loading中是否返回所有需要的信息
    public static List<bool> IsSuccessList = new List<bool>();

    private void Awake()
    {
//        Instance = this;
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
        _getRankRequest = GetComponent<GetRankRequest>();
        _mainRequest = GetComponent<MainRequest>();
        LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().CallBack = onReceive_GetUserBag;
        LogicEnginerScript.Instance.GetComponent<GetRankRequest>().CallBack = onReceive_GetGoldRank;
    }


    public delegate void OnLogicService_Receive(string data); // 收到服务器消息

    public OnLogicService_Receive logicService_Receive = null;


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
            m_socketUtil.start();
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

        //排行榜
        _getRankRequest.OnRequest();
    }

    //收到金币排行榜回调
    private void onReceive_GetGoldRank(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int) jd["code"];

        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            RankData.goldRankDataList = JsonMapper.ToObject<List<GoldRankItemData>>(jd["gold_list"].ToString());
            RankData.medalRankDataList = JsonMapper.ToObject<List<MedalRankItemData>>(jd["medal_list"].ToString());
            print(RankData.goldRankDataList.Count);
            print(RankData.medalRankDataList.Count);
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

    private void onSocketReceive(string data)
    {
        Debug.Log("收到服务器消息:" + data);
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
            Debug.Log(e);
        }
    }

    private void onSocketStop()
    {
        Debug.Log("logic:主动与服务器断开连接");
    }

    private void onSocketClose()
    {
        Debug.Log("logic:被动与服务器断开连接,尝试重新连接");
        m_socketUtil.start();
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

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/Logic/LogicEnginer") as GameObject;
        GameObject obj = MonoBehaviour.Instantiate(prefab);

        return obj;
    }

    public void startConnect()
    {
        m_socketUtil.start();
    }
}