using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetScript : MonoBehaviour {

    public Button m_button_qiehuanzhanghao;
    public Button m_button_tuichu;
    public Button m_button_guanyu;

    public Slider m_sliderMusic;
    public Slider m_sliderSound;

    public Text m_text_VersionCode;

    public bool m_isFromGameLayer = true;

    public static GameObject create(bool isFromGameLayer)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/SettingPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<SetScript>().m_isFromGameLayer = isFromGameLayer;

        return obj;
    }
    public static SetScript Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Start ()
    {
        OtherData.s_setScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SetScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SetScript_hotfix", "Start", null, null);
            return;
        }

        m_sliderMusic.value = AudioScript.getAudioScript().getMusicVolume();
        m_sliderSound.value = AudioScript.getAudioScript().getSoundVolume();

        if (m_isFromGameLayer)
        {
            m_button_qiehuanzhanghao.transform.localScale = new Vector3(0, 0, 0);
            m_button_tuichu.transform.localScale = new Vector3(0, 0, 0);
            m_button_guanyu.transform.localScale = new Vector3(0, 0, 0);
        }

        if (OtherData.s_channelName.CompareTo("ios") == 0)
        {
            m_button_qiehuanzhanghao.transform.localPosition = new Vector3(-180, -154.36f, 0);
            m_button_tuichu.transform.localScale = new Vector3(0, 0, 0);
            m_button_guanyu.transform.localPosition = new Vector3(180, -154.36f, 0);
        }

        m_text_VersionCode.text = OtherData.s_apkVersion;

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void onSliderChange_Music()
    {
        AudioScript.getAudioScript().setMusicVolume(m_sliderMusic.value);
    }

    public void onSliderChange_Sound()
    {
        AudioScript.getAudioScript().setSoundVolume(m_sliderSound.value);
    }

    public void OnClickChangeAccount()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SetScript_hotfix", "OnClickChangeAccount"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SetScript_hotfix", "OnClickChangeAccount", null, null);
            return;
        }

        //LogicEnginerScript.Instance.Stop();
        //LogicEnginerScript.Instance.clear();

        PlayerPrefs.SetInt("DefaultLoginType", (int)OtherData.DefaultLoginType.DefaultLoginType_Default);

        OtherData.s_isFromSetToLogin = true;
        SceneManager.LoadScene("LoginScene");

        Destroy(LogicEnginerScript.Instance.gameObject);
        Destroy(PlayServiceSocket.s_instance.gameObject);
    }

    public void OnClickExit()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SetScript_hotfix", "OnClickExit"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SetScript_hotfix", "OnClickExit", null, null);
            return;
        }

        Application.Quit();
    }

    public void OnClickAbout()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SetScript_hotfix", "OnClickAbout"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SetScript_hotfix", "OnClickAbout", null, null);
            return;
        }

        GameObject prefab = Resources.Load("Prefabs/UI/Panel/AboutPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
    }
}
