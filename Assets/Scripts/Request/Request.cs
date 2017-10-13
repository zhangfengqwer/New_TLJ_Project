using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Request : MonoBehaviour
{
    [HideInInspector]
    public string Tag;
    public abstract void OnRequest();
    public abstract void OnResponse(string data);

    private void Start()
    {
        LogicEnginerScript.Instance.AddRequest(this);
    }

    private void OnDestroy()
    {
        LogicEnginerScript.Instance.ReMoveRequest(this);
    }

    public static explicit operator Request(Dictionary<string, Request>.ValueCollection v)
    {
        throw new NotImplementedException();
    }
}