using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class MainRequest : Request
{
    bool flag;
    string result;

    public delegate void MainCallBack(string result);
    public MainCallBack CallBack;

    private void Awake()
    {
    }

    private void FixedUpdate()
    {
        if (flag)
        {
            flag = false;
            CallBack(result);
        }
    }

    public override void OnRequest()
    {
    }

    public override void OnResponse(string data)
    {
        result = data;
        flag = true;
    }
}
