using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private static float _soundSet;
    
    private void Awake()
    {
        _soundSet = PlayerPrefs.GetFloat("soundSet", 0);
    }

    public static void SoundSet(float sound)
    {
        _soundSet = sound;
        PlayerPrefs.SetFloat("soundSet", 0);
    }
}
