using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AndroidCallBack : MonoBehaviour {

    public delegate void onPauseCallBack();
    public static onPauseCallBack s_onPauseCallBack = null;
    private bool isPause;
    private bool isFocus;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void OnPauseCallBack(string data)
    {
        ToastScript.createToast("回到后台:"+Thread.CurrentThread);

        if (s_onPauseCallBack != null)
        {
            s_onPauseCallBack();
        }
    }

    public void OnResumeCallBack(string data)
    {
        ToastScript.createToast("返回游戏:"+Thread.CurrentThread);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        isPause = pauseStatus;
        
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        isFocus = hasFocus;
    }

    private void Update()
    {
        if (isPause && !isFocus)
        {
            ToastScript.createToast("回到后台:"+Thread.CurrentThread);
        }
    }
}
