﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetScript : MonoBehaviour {

    public Slider m_sliderMusic;
    public Slider m_sliderSound;

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
}