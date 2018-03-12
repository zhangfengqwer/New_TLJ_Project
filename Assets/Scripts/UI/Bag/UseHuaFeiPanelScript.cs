using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseHuaFeiPanelScript : MonoBehaviour {

    public InputField m_inputField_phone;

    public PropInfo m_propInfo = null;
    public Text m_text_time;
    public Button m_btn_queren;

    public int m_shengyuTime = 0;
    public int m_useNum = 1;

    public static GameObject create(PropInfo propInfo,int useNum)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/UseHuaFeiPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<UseHuaFeiPanelScript>().m_propInfo = propInfo;
        obj.GetComponent<UseHuaFeiPanelScript>().m_useNum = useNum;

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("UseHuaFeiPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.UseHuaFeiPanelScript_hotfix", "Start", null, null);
            return;
        }

        // 保存当前充值时间
        {
            string beforeTime = PlayerPrefs.GetString("beforeChongZhiHuaFeiTime", "2018-2-27 0:0:0");
            string nowtime = CommonUtil.getCurYear() + "-" + CommonUtil.getCurMonth() + "-" + CommonUtil.getCurDay() + " " +
                          CommonUtil.getCurHour() + ":" + CommonUtil.getCurMinute() + ":" + CommonUtil.getCurSecond();

            
            int seconds = CommonUtil.miaoshucha(beforeTime, nowtime);
            Debug.Log(seconds);
            if (seconds < 60)
            {
                m_btn_queren.interactable = false;

                m_shengyuTime = (60 - seconds);
                m_text_time.text = m_shengyuTime.ToString();
                m_text_time.transform.localScale = new Vector3(1,1,1);
                InvokeRepeating("onInvokeTime", 1,1);
            }
        }
    }

    public void onInvokeTime()
    {
        --m_shengyuTime;

        if (m_shengyuTime >= 0)
        {
            m_text_time.text = m_shengyuTime.ToString();
        }
        else
        {
            m_btn_queren.interactable = true;
            m_text_time.transform.localScale = new Vector3(0, 0, 0);
            CancelInvoke("onInvokeTime");
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickQueRen()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("UseHuaFeiPanelScript_hotfix", "onClickQueRen"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.UseHuaFeiPanelScript_hotfix", "onClickQueRen", null, null);
            return;
        }

        if (!VerifyRuleUtil.CheckPhone(m_inputField_phone.text))
        {
            ToastScript.createToast("请输入正确的手机号");

            return;
        }

        NetLoading.getInstance().Show();

        LogicEnginerScript.Instance.GetComponent<UseHuaFeiRequest>().SetData(m_propInfo.m_id,m_useNum, m_inputField_phone.text);
        LogicEnginerScript.Instance.GetComponent<UseHuaFeiRequest>().CallBack = onReceive_UseHuaFei;
        LogicEnginerScript.Instance.GetComponent<UseHuaFeiRequest>().OnRequest();
    }

    public void onReceive_UseHuaFei(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("UseHuaFeiPanelScript_hotfix", "onReceive_UseHuaFei"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.UseHuaFeiPanelScript_hotfix", "onReceive_UseHuaFei", null, data);
            return;
        }

        NetLoading.getInstance().Close();

        // 保存当前充值时间
        {
            string time = CommonUtil.getCurYear() + "-" + CommonUtil.getCurMonth() + "-" + CommonUtil.getCurDay() + " " +
                          CommonUtil.getCurHour() + ":" + CommonUtil.getCurMinute() + ":" + CommonUtil.getCurSecond();

            PlayerPrefs.SetString("beforeChongZhiHuaFeiTime", time);
        }

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];
        if (code == (int) TLJCommon.Consts.Code.Code_OK)
        {
            ToastScript.createToast("使用成功，请等待充值到账");

            GameUtil.changeData(m_propInfo.m_id, -m_useNum);

            if (BagPanelScript.Instance != null)
            {
                BagPanelScript.Instance.UpdateUI();
            }

            // 保存当前充值时间
            {
                string time = CommonUtil.getCurYear() + "-" + CommonUtil.getCurMonth() + "-" + CommonUtil.getCurDay() + " " +
                              CommonUtil.getCurHour() + ":" + CommonUtil.getCurMinute() + ":" + CommonUtil.getCurSecond();

                PlayerPrefs.SetString("beforeChongZhiHuaFeiTime",time);
            }

            Destroy(gameObject);
            Destroy(GameObject.Find("PropDetailPanel(Clone)"));
        }
        else
        {
            ToastScript.createToast((string)jd["msg"]);
        }
    }
}
