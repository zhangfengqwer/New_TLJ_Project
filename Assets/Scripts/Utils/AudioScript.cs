using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

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

    // 抢地主
    public void playSound_QiangDiZhu(int score)
    {
        playSound("jiaofen_" +score.ToString());
    }

    // 斗地主胜利
    public void playSound_DouDiZhu_win()
    {
        playSound("win");
    }

    // 斗地主失败
    public void playSound_DouDiZhu_lose()
    {
        playSound("lose");
    }

    // 斗地主出牌音效
    public void playSound_DouDiZhu_ChuPai(List<TLJCommon.PokerInfo> list,string uid)
    {
        CrazyLandlords.Helper.LandlordsCardsHelper.SetWeight(list);
        CrazyLandlords.Helper.CardsType cardsType;

        CrazyLandlords.Helper.LandlordsCardsHelper.GetCardsType(list.ToArray(), out cardsType);

        if (list.Count == 0)
        {
            playSound("guo");
            return;
        }

        switch (cardsType)
        {
            case CrazyLandlords.Helper.CardsType.JokerBoom:
                {
                    playSound("huojian");

                    {
                        GameObject obj = CreateUGUI.createImageObj(GameObject.Find("Canvas_Middle").gameObject, CommonUtil.getImageSpriteByAssetBundle("animations.unity3d", "huojian1"));
                        PlayAnimation playAnimation = obj.AddComponent<PlayAnimation>();
                        playAnimation.start("animations.unity3d", "huojian", true, 0.07f);

                        obj.transform.localPosition = new Vector3(0, -200, 0);
                        obj.transform.DOMoveY(10, 3).OnComplete(() =>
                        {
                            GameObject.Destroy(obj);
                        });
                    }
                }
                break;
            case CrazyLandlords.Helper.CardsType.Boom:
                {
                    playSound("bomb");
                    
                    {
                        GameObject obj = CreateUGUI.createImageObj(GameObject.Find("Canvas_Middle").gameObject,CommonUtil.getImageSpriteByAssetBundle("animations.unity3d", "zhadan1"));
                        PlayAnimation playAnimation = obj.AddComponent<PlayAnimation>();
                        playAnimation.start("animations.unity3d", "zhadan",false, 0.07f);
                    }
                }
                break;

            case CrazyLandlords.Helper.CardsType.BoomAndOne:                //四带一
                {
                    //playSound("bomb");
                }
                break;
            case CrazyLandlords.Helper.CardsType.BoomAndTwo:                //四带二
                {
                    //playSound("bomb");
                }
                break;
            case CrazyLandlords.Helper.CardsType.OnlyThree:
                {
                    playSound("three");
                }
                break;
            case CrazyLandlords.Helper.CardsType.ThreeAndOne:
                {
                    playSound("three_one");
                }
                break;
            case CrazyLandlords.Helper.CardsType.ThreeAndTwo:
                {
                    playSound("three_two");
                }
                break;
            case CrazyLandlords.Helper.CardsType.Straight:
                {
                    playSound("shunzi");

                    {
                        GameObject obj = CreateUGUI.createImageObj(GameObject.Find("Canvas_Middle").gameObject, CommonUtil.getImageSpriteByAssetBundle("animations.unity3d", "shunzi1"));
                        PlayAnimation playAnimation = obj.AddComponent<PlayAnimation>();
                        playAnimation.start("animations.unity3d", "shunzi", false, 0.07f);

                        int middle = DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_outPokerObjList.Count / 2;
                        float y = DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_outPokerObjList[middle].transform.position.y + 80;
                        obj.transform.localPosition = new Vector3(0 , y , 0);
                    }
                }
                break;
            case CrazyLandlords.Helper.CardsType.DoubleStraight:
                {
                    playSound("liandui");

                    {
                        GameObject obj = CreateUGUI.createImageObj(GameObject.Find("Canvas_Middle").gameObject, CommonUtil.getImageSpriteByAssetBundle("animations.unity3d", "liandui1"));
                        PlayAnimation playAnimation = obj.AddComponent<PlayAnimation>();
                        playAnimation.start("animations.unity3d", "liandui", false, 0.07f);

                        int middle = DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_outPokerObjList.Count / 2;
                        float x = DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_outPokerObjList[middle].transform.position.x;
                        float y = DDZ_GameData.getInstance().getPlayerDataByUid(uid).m_outPokerObjList[middle].transform.position.y;
                        obj.transform.position = new Vector3(x, y, 0);
                    }
                }
                break;
            case CrazyLandlords.Helper.CardsType.TripleStraight:
            case CrazyLandlords.Helper.CardsType.TripleStraightAndOne:
            case CrazyLandlords.Helper.CardsType.TripleStraightAndTwo:
                {
                    playSound("feiji");

                    {
                        GameObject obj = CreateUGUI.createImageObj(GameObject.Find("Canvas_Middle").gameObject, CommonUtil.getImageSpriteByAssetBundle("animations.unity3d", "feiji1"));
                        PlayAnimation playAnimation = obj.AddComponent<PlayAnimation>();
                        playAnimation.start("animations.unity3d", "feiji", true, 0.07f);

                        obj.transform.localPosition = new Vector3(-500, 0, 0);
                        obj.transform.DOMoveX(10, 3).OnComplete(() =>
                        {
                            GameObject.Destroy(obj);
                        });
                    }
                }
                break;
            case CrazyLandlords.Helper.CardsType.Double:
                {
                    int num = list[0].m_num;

                    if ((num >= 2) && (num <= 10))
                    {
                        playSound("double_" + num.ToString());
                    }
                    else if (num == 11)
                    {
                        playSound("double_J");
                    }
                    else if (num == 12)
                    {
                        playSound("double_Q");
                    }
                    else if (num == 13)
                    {
                        playSound("double_K");
                    }
                    else if (num == 14)
                    {
                        playSound("double_A");
                    }
                }
                break;
            case CrazyLandlords.Helper.CardsType.Single:
                {
                    int num = list[0].m_num;

                    if ((num >= 2) && (num <= 10))
                    {
                        playSound(num.ToString());
                    }
                    else if (num == 11)
                    {
                        playSound("J");
                    }
                    else if (num == 12)
                    {
                        playSound("Q");
                    }
                    else if (num == 13)
                    {
                        playSound("K");
                    }
                    else if (num == 14)
                    {
                        playSound("A");
                    }
                    else if (num == 15)
                    {
                        playSound("xiaowang");
                    }
                    else if (num == 16)
                    {
                        playSound("dawang");
                    }
                }
                break;
        }
    }

    //----------------------------------------------------------------------------播放 end
}
