using UnityEngine;
using System.Collections.Generic;

public class AudioScript : MonoBehaviour {

    static public AudioScript getAudioScript ()
    {
        if(!s_audioObj)
        {
            s_audioObj = new GameObject();
            s_audioObj.transform.name = "Audio";
            MonoBehaviour.DontDestroyOnLoad(s_audioObj);
            s_audioScript = s_audioObj.AddComponent<AudioScript>();
            s_audioScript.init();
        }

        return s_audioScript;
    }

    public void init ()
    {
        initMusicPlayer();
        initSfxPlayer();
    }

    void initMusicPlayer ()
    {
        GameObject go = new GameObject("musicPlayer");
        go.transform.SetParent(transform, false);
        AudioSource player = go.AddComponent<AudioSource>();
        m_musicPlayer = player;

        player.loop = true;
        player.mute = false;
        player.volume = 0.2f;
        player.pitch = 1.0f;
        player.playOnAwake = false;

        m_musicEnable = getMusicEnable();
    }

    void initSfxPlayer ()
    {
        m_soundPlayer = new List<AudioSource>();

        GameObject go = new GameObject("soundPlayer");
        go.transform.SetParent(transform, false);
        AudioSource player = go.AddComponent<AudioSource>();
        m_soundPlayer.Add(player);

        player.loop = false;
        player.mute = false;
        player.volume = 1.0f;
        player.pitch = 1.0f;
        player.playOnAwake = false;

        m_soundEnable = getSoundEnable();
    }

    void playMusic (string audioPath)
    {
        if(m_musicEnable)
        {
            m_musicPlayer.clip = (AudioClip)Resources.Load(audioPath, typeof(AudioClip));
            m_musicPlayer.Play();
            m_musicPlayer.volume = m_musicVolume;
        }
    }

    void playSound (string audioPath)
    {
        if(m_soundEnable)
        {
            for(int i = 0; i < m_soundPlayer.Count; i++)
            {
                if(!m_soundPlayer[i].isPlaying)
                {
                    m_soundPlayer[i].clip = (AudioClip)Resources.Load(audioPath, typeof(AudioClip));
                    m_soundPlayer[i].Play();

                    return;
                }
            }

            // 如果执行到这里，说明暂时没有空余的音效组件使用，需要再新建一个
            {
                GameObject go = new GameObject("soundPlayer");
                go.transform.SetParent(transform, false);
                AudioSource player = go.AddComponent<AudioSource>();
                m_soundPlayer.Add(player);

                player.loop = false;
                player.mute = false;
                player.volume = m_soundVolume;
                player.pitch = 1.0f;
                player.playOnAwake = false;

                player.clip = (AudioClip)Resources.Load(audioPath, typeof(AudioClip));
                player.Play();
            }
        }
    }

    public float getMusicVolume()
    {
        return m_musicVolume;
    }

    public void setMusicVolume(float volume)
    {
        m_musicVolume = volume;
        m_musicPlayer.volume = m_musicVolume;
    }

    public float getSoundVolume()
    {
        return m_soundVolume;
    }

    public void setSoundVolume(float volume)
    {
        m_soundVolume = volume;
        for (int i = 0; i < m_soundPlayer.Count; i++)
        {
            m_soundPlayer[i].volume = m_soundVolume;
        }
    }

    public void stopMusic ()
    {
        if(m_musicPlayer.isPlaying)
        {
            m_musicPlayer.Stop();
        }
    }

    public void stopSound ()
    {
        for(int i = 0; i < m_soundPlayer.Count; i++)
        {
            if(m_soundPlayer[i].isPlaying)
            {
                m_soundPlayer[i].Stop();
            }
        }
    }

    public void setMusicEnable (bool enable)
    {
        PlayerPrefs.SetInt("music enable", enable ? 1 : 0);
        m_musicEnable = enable;
    }

    public void setSoundEnable (bool enable)
    {
        PlayerPrefs.SetInt("sound enable", enable ? 1 : 0);
        m_soundEnable = enable;
    }

    public bool getMusicEnable ()
    {
        return PlayerPrefs.GetInt("music enable", 1) == 1 ? true : false;
    }

    public bool getSoundEnable ()
    {
        return PlayerPrefs.GetInt("sound enable", 1) == 1 ? true : false;
    }

    //----------------------------------------------------------------------------播放 start

    // 背景音乐
    public void playMusic_GameBg()
    {
        playMusic("Audios/bg_music");
    }

    // 点击按钮
    public void playSound_ButtonClick ()
    {
        playSound("Audios/button_click");
    }

    //----------------------------------------------------------------------------播放 end

    //---------------------------------------------------
    static GameObject s_audioObj = null;
    static AudioScript s_audioScript;


    //#背景音乐只会有一个;
    AudioSource m_musicPlayer;
    //#音效会同时播放多个，所以用List;
    List<AudioSource> m_soundPlayer;

    private bool m_musicEnable = true;
    private bool m_soundEnable = true;

    float m_musicVolume = 1.0f;
    float m_soundVolume = 1.0f;
}
