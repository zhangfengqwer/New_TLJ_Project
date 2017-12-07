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

    public bool m_isFromGameLayer = true;

    public static GameObject create(bool isFromGameLayer)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/SettingPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<SetScript>().m_isFromGameLayer = isFromGameLayer;

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        m_sliderMusic.value = AudioScript.getAudioScript().getMusicVolume();
        m_sliderSound.value = AudioScript.getAudioScript().getSoundVolume();

        if (m_isFromGameLayer)
        {
            m_button_qiehuanzhanghao.transform.localScale = new Vector3(0, 0, 0);
            m_button_tuichu.transform.localScale = new Vector3(0, 0, 0);
            m_button_guanyu.transform.localScale = new Vector3(0, 0, 0);
        }
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
        Application.Quit();
    }

    public void OnClickAbout()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/AboutPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
    }
}
