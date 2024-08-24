using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Unity.VisualScripting;

public class DataManager : MonoBehaviour
{
    // save Data
    [SerializeField]
    private int _highScore;
    
    private UserData _saveData;
    
    // character data
    public class Character
    {
        public string name;
        public int spriteIndex;
        public int prefabIndex;
    }

    public class CharacterInfo
    {
        public Character character0;
        public Character character1;
        public Character character2;
        public Character character3;
        public Character character4;
    }

    public Sprite[] characterSprites;
    public GameObject[] characterPrefabs;

    private void Awake()
    {
        GameManager.Instance.dataManager = this;
        CharacterInfo characterInfo =
            JsonUtility.FromJson<CharacterInfo>(Resources.Load<TextAsset>("CharacterInfo").text);
        if (_highScore == null)
        {
            _highScore = 0;
        }
    }
    
    public int HighScore
    {
        get { return _highScore; }
        set { _highScore = value; }
    }

    public UserData SaveData
    {
        get { return _saveData; }
        set { _saveData = value; }
    }
}
