using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitMatchPanelScript : MonoBehaviour {

    public Text m_text_time;
    public Button m_button_TuiSai;

    public delegate void OnTimerEvent_TimeEnd(bool isContinueGame);
    public OnTimerEvent_TimeEnd m_onTimerEvent_TimeEnd = null;

    public bool m_isStart = false;
    public bool m_isContinueGame = false;
    public float m_time;

    public static GameObject create(string gameRoomType)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/WaitMatchPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<WaitMatchPanelScript>().checkHideTuiSai(gameRoomType);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("WaitMatchPanelScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.WaitMatchPanelScript", "Start", null, null);
            return;
        }
    }

    public void onClickTuiSai()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("WaitMatchPanelScript", "onClickTuiSai"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.WaitMatchPanelScript", "onClickTuiSai", null, null);
            return;
        }

        reqExitRoom();
    }

    public void checkHideTuiSai(string gameRoomType)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("WaitMatchPanelScript", "checkHideTuiSai"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.WaitMatchPanelScript", "checkHideTuiSai", null, gameRoomType);
            return;
        }

        // 休闲场
        List<string> list = new List<string>();
        CommonUtil.splitStr(GameData.getInstance().getGameRoomType(), list, '_');
        if (list[0].CompareTo("XiuXian") == 0)
        {
            m_button_TuiSai.transform.localScale = new Vector3(0, 0, 0);
        }
    }

	// Update is called once per frame
	void Update ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("WaitMatchPanelScript", "Update"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.WaitMatchPanelScript", "Update", null, null);
            return;
        }

        if (m_isStart)
        {
            m_time -= Time.deltaTime;

            m_text_time.text = ((int)m_time).ToString();

            // 时间到
            if (m_time <= 1)
            {
                m_isStart = false;

                gameObject.transform.localScale = new Vector3(0, 0, 0);
                
                if (m_onTimerEvent_TimeEnd != null)
                {
                    m_onTimerEvent_TimeEnd(m_isContinueGame);
                }

                Destroy(gameObject);
            }
        }
    }

    public void setOnTimerEvent_TimeEnd(OnTimerEvent_TimeEnd onTimerEvent_TimeEnd)
    {
        m_onTimerEvent_TimeEnd = onTimerEvent_TimeEnd;
    }

    public void start(float seconds, bool isContinueGame)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("WaitMatchPanelScript", "start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.WaitMatchPanelScript", "start", null, seconds, isContinueGame);
            return;
        }

        m_time = seconds + 1;
        m_isStart = true;
        m_isContinueGame = isContinueGame;

        m_text_time.text = ((int)m_time).ToString();
    }

    public void stop()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("WaitMatchPanelScript", "stop"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.WaitMatchPanelScript", "stop", null, null);
            return;
        }

        gameObject.transform.localScale = new Vector3(0, 0, 0);

        m_isStart = false;

        m_text_time.text = "0";
    }

    //-------------------------------------------------------------
    // 请求退出房间
    public void reqExitRoom()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("WaitMatchPanelScript", "reqExitRoom"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.WaitMatchPanelScript", "reqExitRoom", null, null);
            return;
        }

        JsonData data = new JsonData();

        data["tag"] = TLJCommon.Consts.Tag_JingJiChang;
        data["uid"] = UserData.uid;
        data["playAction"] = (int)TLJCommon.Consts.PlayAction.PlayAction_ExitPVP;

        PlayServiceSocket.s_instance.sendMessage(data.ToJson());

        Destroy(gameObject);
    }
}
