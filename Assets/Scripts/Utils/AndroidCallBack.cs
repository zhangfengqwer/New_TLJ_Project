using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using LitJson;
using TLJCommon;
using UnityEngine;

public class AndroidCallBack : MonoBehaviour {

    public delegate void onPauseCallBack();
    public static onPauseCallBack s_onPauseCallBack = null;

    public delegate void onResumeCallBack();
    public static onResumeCallBack s_onResumeCallBack = null;
    private static AndroidCallBack Instance;

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

    public void SetLogIsShow(string isShow)
    {
        //显示log
        if ("0".Equals(isShow))
        {
            LogUtil.s_isShowLog = false;
        }
        else
        {
            LogUtil.s_isShowLog = true;
           
        }
       
    }

    public void SetIsTest(string isTest)
    {
        //正式包
        if ("0".Equals(isTest))
        {
            OtherData.s_isTest = false;
        }
        else
        {
            OtherData.s_isTest = true;
        }
    }

    public void OnPauseCallBack(string data)
    {
        if (s_onPauseCallBack != null)
        {
            s_onPauseCallBack();
        }
    }

    public void OnResumeCallBack(string data)
    {
        if (OtherData.s_isFirstOpenGame)
        {
            OtherData.s_isFirstOpenGame = false;
            return;
        }

        if (s_onResumeCallBack != null)
        {
            s_onResumeCallBack();
        }
    }

    public void OnWxShareFriends(string data)
    {
        
    }

    public void GetLoginResult(string data)
    {
        LogUtil.Log("Unity收到:" + data);
        try
        {
            JsonData jsonData = JsonMapper.ToObject(data);
            var openId = (string)jsonData["code"];
            var nickname = (string)jsonData["nickname"];
            var platform = (string)jsonData["platform"];

            JsonData jd = new JsonData();
            jd["tag"] = Consts.Tag_Third_Login;
            jd["nickname"] = nickname;
            jd["third_id"] = openId;
            jd["platform"] = platform;

            LoginServiceSocket.s_instance.sendMessage(jd.ToJson());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
