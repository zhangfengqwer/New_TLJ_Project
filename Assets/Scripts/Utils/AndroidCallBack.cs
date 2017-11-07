using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AndroidCallBack : MonoBehaviour {

    public delegate void onPauseCallBack();
    public static onPauseCallBack s_onPauseCallBack = null;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void OnPauseCallBack(string data)
    {
        OtherData.s_ifOnPause = true;

        if (s_onPauseCallBack != null)
        {
            s_onPauseCallBack();
        }
    }

    public void OnResumeCallBack(string data)
    {
    }
}
