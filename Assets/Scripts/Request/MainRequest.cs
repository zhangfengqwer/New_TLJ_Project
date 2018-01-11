using System.Collections;
using System.Collections.Generic;
using LitJson;
using TLJCommon;
using UnityEngine;

public class MainRequest : Request
{
    public delegate void MainCallBack(string result);
    public MainCallBack CallBack = null;

    public List<string> m_dataList = new List<string>();

    private void Awake()
    {
    }

    private void Update()
    {
        if (CallBack != null)
        {
            if (m_dataList.Count > 0)
            {
                CallBack(m_dataList[0]);
                m_dataList.RemoveAt(0);
            }
        }
    }

    public override void OnRequest()
    {
    }

    public override void OnResponse(string data)
    {
        m_dataList.Add(data);
    }
}
