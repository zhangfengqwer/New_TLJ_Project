using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AndroidCallBack : MonoBehaviour {

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void OnPauseCallBack(string data)
    {
        ToastScript.createToast("回到后台:"+Thread.CurrentThread);

        LoginServiceSocket.s_instance.Stop();
        LogicEnginerScript.Instance.Stop();
        PlayServiceSocket.s_instance.Stop();
    }

    public void OnResumeCallBack(string data)
    {
        ToastScript.createToast("返回游戏:"+Thread.CurrentThread);
    }
}
