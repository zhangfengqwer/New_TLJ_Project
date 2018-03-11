using UnityEngine;
using System.Collections.Generic;

public class AudioScript : MonoBehaviour {

    public static GameObject s_audioObj = null;
    public static AudioScript s_audioScript;

    //#背景音乐只会有一个;
    public AudioSource m_musicPlayer;
    //#音效会同时播放多个，所以用List;
    public List<AudioSource> m_soundPlayer;

    public float m_musicVolume = 1.0f;
    public float m_soundVolume = 1.0f;

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
        initSoundPlayer();
    }

    public void initMusicPlayer ()
    {
        m_musicVolume = PlayerPrefs.GetFloat("MusicVolume",1.0f);

        GameObject go = new GameObject("musicPlayer");
        go.transform.SetParent(transform, false);
        AudioSource player = go.AddComponent<AudioSource>();
        m_musicPlayer = player;

        player.loop = true;
        player.mute = false;
        player.volume = m_musicVolume;
        player.pitch = 1.0f;
        player.playOnAwake = false;
    }

    public void initSoundPlayer ()
    {
        m_soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1.0f);

        m_soundPlayer = new List<AudioSource>();

        GameObject go = new GameObject("soundPlayer");
        go.transform.SetParent(transform, false);
        AudioSource player = go.AddComponent<AudioSource>();
        m_soundPlayer.Add(player);

        player.loop = false;
        player.mute = false;
        player.volume = m_soundVolume;
        player.pitch = 1.0f;
        player.playOnAwake = false;
    }

    public void playMusic (string audioPath)
    {
        m_musicPlayer.clip = CommonUtil.getAudioClipByAssetBundle("audios.unity3d", audioPath);
        m_musicPlayer.Play();
        m_musicPlayer.volume = m_musicVolume;
    }

    public void playSound (string audioPath)
    {
        for(int i = 0; i < m_soundPlayer.Count; i++)
        {
            if(!m_soundPlayer[i].isPlaying)
            {
                m_soundPlayer[i].clip = CommonUtil.getAudioClipByAssetBundle("audios.unity3d", audioPath);
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

    public float getMusicVolume()
    {
        return m_musicVolume;
    }

    public void setMusicVolume(float volume)
    {
        m_musicVolume = volume;
        m_musicPlayer.volume = m_musicVolume;

       PlayerPrefs.SetFloat("MusicVolume", m_musicVolume);
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

        PlayerPrefs.SetFloat("SoundVolume", m_soundVolume);
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

    //----------------------------------------------------------------------------播放 start

    // 主界面背景音乐
    public void playMusic_MainBg()
    {
        playMusic("bg_main");
    }

    // 游戏内普通场背景音乐
    public void playMusic_GameBg_Relax()
    {
        playMusic("bg_game_relax");
    }

    // 游戏内PVP背景音乐
    public void playMusic_GameBg_PVP()
    {
        playMusic("bg_game_pvp");
    }

    // 点击按钮
    public void playSound_ButtonClick ()
    {
        playSound("yx_anniu");
    }

    // 界面弹出
    public void playSound_LayerShow()
    {
        playSound("yx_jiemiantanchu");
    }

    // 界面关闭
    public void playSound_LayerClose()
    {
        playSound("yx_jiemianguanbi");
    }

    // 显示奖励
    public void playSound_ShowReward()
    {
        playSound("yx_getReward");
    }

    // 出牌时选中牌
    public void playSound_XuanPai()
    {
        playSound("yx_xuanpai");
    }

    // 炒底
    public void playSound_ChaoDi()
    {
        playSound("yx_chaodi");
    }

    // 不炒底
    public void playSound_BuChaoDi()
    {
        playSound("yx_buchaodi");
    }

    // 胜利
    public void playSound_Win()
    {
        playSound("yx_win");
    }

    // 失败
    public void playSound_Fail()
    {
        playSound("yx_fail");
    }

    // 出牌
    public void playSound_ChuPai()
    {
        playSound("yx_chupai");
    }

    // 破
    public void playSound_Po()
    {
        playSound("yx_po");
    }

    // 拖拉机
    public void playSound_TuoLaJi()
    {
        playSound("yx_tuolaji");
    }

    //----------------------------------------------------------------------------播放 end
}
