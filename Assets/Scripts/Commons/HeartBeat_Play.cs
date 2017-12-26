using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat_Play : MonoBehaviour
{
    static HeartBeat_Play s_instance = null;

    float m_durTime = 5.0f;
    float m_waitTime = 3.0f;

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
        Invoke("reqHeartBeat", m_durTime);        
    }

    public void stopHeartBeat()
    {
        CancelInvoke("reqHeartBeat");
        CancelInvoke("onInvoke_timeout");
    }

    void onInvoke_timeout()
    {
        if (OtherData.s_gameScript != null)
        {
            Destroy(PlayServiceSocket.s_instance.gameObject);

            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(OtherData.s_gameScript.exitRoom);
            NetErrorPanelScript.getInstance().setContentText("与服务器通讯异常，点击确认回到主界面");
        }
    }

    void reqHeartBeat()
    {
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
            ToastScript.createToast("心跳：网络断开");

            startHeartBeat();
        }
    }

    public void onRespond()
    {
        CancelInvoke("onInvoke_timeout");

        startHeartBeat();
    }
}
