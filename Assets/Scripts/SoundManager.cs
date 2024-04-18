using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    public enum Sfx
    {
        jump,
        menuTouch,
        menuClose,
    }

    [Header("#Sliders")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("#BGM SET")]
    public AudioClip bgmClip;
    public float bgmVolume;
    private AudioSource bgmPlayer;
    
    [Header("#SFX SET")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    private AudioSource[] sfxPlayers;
    private int channelIndex;

    void Start()
    {
        bgmSlider = bgmSlider.GetComponent<Slider>();
        sfxSlider = bgmSlider.GetComponent<Slider>();
        
        bgmSlider.onValueChanged.AddListener(ChangeBgmSound);
        sfxSlider.onValueChanged.AddListener(ChangeSfxSound);
    }

    void ChangeBgmSound(float volume)
    {
        bgmPlayer.volume = volume;
    }
    
    void ChangeSfxSound(float volume)
    {
        foreach (AudioSource sfxs in sfxPlayers)
        {
            sfxs.volume = volume;
        }
    }
    
    
    protected override void Init()
    {
        #region bgmPlayer Initalize
        
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        #endregion
        
        #region sfxPlayer Initalize
        
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }

        #endregion
        
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying) // resting player check
            {
                continue;
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break; 
        }
        
        
    }
    
}
