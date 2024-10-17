using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    public enum Sfx
    {
        jump,
        menuTouch,
        menuClose,
        coin,
        cannon,
        elevator,
        fallingBlock,
        tickDamage,
        gameOver,
        buyCharacter
    }
    
    [Header("#Volume SET")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("#BGM SET")]
    public AudioClip bgmClip;
    public AudioSource bgmPlayer;
    
    [Header("#SFX SET")]
    public AudioClip[] sfxClips;
    public int channels;
    private AudioSource[] sfxPlayers;
    private int channelIndex;

    protected override void Init()
    {
        bgmSlider = bgmSlider.GetComponent<Slider>();
        sfxSlider = sfxSlider.GetComponent<Slider>();
        
        bgmSlider.onValueChanged.AddListener(ChangeBgmSound);
        sfxSlider.onValueChanged.AddListener(ChangeSfxSound);
        
        #region bgmPlayer Initalize
        
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmSlider.value;
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
            sfxPlayers[i].volume = sfxSlider.value;
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

    public void PauseBGM()
    {
        if (bgmPlayer.isPlaying)
        {
            bgmPlayer.Pause();
        }
    }

    public void UnPauseBGM()
    {
        if (!bgmPlayer.isPlaying)
        {
            bgmPlayer.UnPause();
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
    
    void ChangeBgmSound(float value)
    {
        bgmPlayer.volume = value;
    }
    
    void ChangeSfxSound(float value)
    {
        foreach (AudioSource sfxs in sfxPlayers)
        {
            sfxs.volume = value;
        }
    }
    
}
