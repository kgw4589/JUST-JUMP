using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float soundSet;
    
    private void Awake()
    {
        soundSet = PlayerPrefs.GetFloat("soundSet", 0);
    }

    public void SoundChange(float sound)
    {
        soundSet = sound;
        PlayerPrefs.SetFloat("soundSet", soundSet);
    }
}
