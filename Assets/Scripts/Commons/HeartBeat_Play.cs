using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat_Play : MonoBehaviour
{
    public static HeartBeat_Play s_instance = null;

    public float m_durTime = 5.0f;
    public float m_waitTime = 10.0f;

    public static HeartBeat_Play getInstance()
    {
        if (s_instance == null)
        {
            if (PlayServiceSocket.s_instance != null)
            {
                s_instance = PlayServiceSocket.s_instance.gameObject.GetComponent<HeartBeat_Play>();
            }
        }

        return s_instance;
    }

    public void startHeartBeat()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HeartBeat_Play_hotfix", "startHeartBeat"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HeartBeat_Play_hotfix", "startHeartBeat", null, null);
            return;
        }

        Invoke("reqHeartBeat", m_durTime);        
    }

    public void stopHeartBeat()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HeartBeat_Play_hotfix", "stopHeartBeat"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HeartBeat_Play_hotfix", "stopHeartBeat", null, null);
            return;
        }

        CancelInvoke("reqHeartBeat");
        CancelInvoke("onInvoke_timeout");

        s_instance = null;
    }

    public void onInvoke_timeout()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HeartBeat_Play_hotfix", "onInvoke_timeout"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HeartBeat_Play_hotfix", "onInvoke_timeout", null, null);
            return;
        }

        if (OtherData.s_gameScript != null)
        {
            Destroy(PlayServiceSocket.s_instance.gameObject);

            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(OtherData.s_gameScript.exitRoom);
            NetErrorPanelScript.getInstance().setContentText("与服务器通讯异常，点击确认回到主界面");
        }
    }

    public void reqHeartBeat()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HeartBeat_Play_hotfix", "reqHeartBeat"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HeartBeat_Play_hotfix", "reqHeartBeat", null, null);
            return;
        }

        if (PlayServiceSocket.s_instance.isConnecion())
        {
            Invoke("onInvoke_timeout", m_waitTime);

            JsonData data = new JsonData();
            data["tag"] = TLJCommon.Consts.Tag_HeartBeat_Play;
            data["uid"] = UserData.uid;

            PlayServiceSocket.s_instance.sendMessage(data.ToJson());
        }
        else
        {
            //ToastScript.createToast("心跳：网络断开");

            startHeartBeat();
        }
    }

    public void onRespond()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("HeartBeat_Play_hotfix", "onRespond"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.HeartBeat_Play_hotfix", "onRespond", null, null);
            return;
        }

        CancelInvoke("onInvoke_timeout");

        startHeartBeat();
    }
}
