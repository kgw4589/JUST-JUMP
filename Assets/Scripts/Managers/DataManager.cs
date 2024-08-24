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

    public class CharacterDictionary
    {
        public Dictionary<string, Character> characters = new Dictionary<string, Character>();
    }
    
    public Sprite[] characterSprites;
    public GameObject[] characterPrefabs;

    public Dictionary<string, Character> characterInfo = new Dictionary<string, Character>();
    
    private void Awake()
    {
        GameManager.Instance.dataManager = this;
        TextAsset characterInfoJson = Resources.Load<TextAsset>("CharacterInfo");
        
        if (characterInfoJson != null)
        {
            var characterDictionary =
                JsonUtility.FromJson<CharacterDictionary>(characterInfoJson.text);
            foreach (var i in characterDictionary.characters)
            {
                characterInfo[i.Key] = i.Value;
            }
        }
        else
        {
            Debug.LogError("Find CharacterInfo failed");
        }
        
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
