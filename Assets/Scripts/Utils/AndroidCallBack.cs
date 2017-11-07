using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AndroidCallBack : MonoBehaviour {

    public delegate void onPauseCallBack();
    public static onPauseCallBack s_onPauseCallBack = null;

    public delegate void onResumeCallBack();
    public static onResumeCallBack s_onResumeCallBack = null;

    private void Awake()
    {
        DontDestroyOnLoad(this);
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
}
