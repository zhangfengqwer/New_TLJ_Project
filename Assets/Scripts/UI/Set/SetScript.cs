using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetScript : MonoBehaviour {

    public Slider m_sliderMusic;
    public Slider m_sliderSound;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/SettingPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        m_sliderMusic.value = AudioScript.getAudioScript().getMusicVolume();
        m_sliderSound.value = AudioScript.getAudioScript().getSoundVolume();
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

        OtherData.s_isFromSetToLogin = true;
        SceneManager.LoadScene("LoginScene");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
