using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Unity.VisualScripting;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    private int _highScore;
    
    private UserData _saveData;
    
    [System.Serializable]
    public class CharacterInfo
    {
        public int characterNumber;
        public string characterName;
        public Sprite characterImage;
        public GameObject characterObject;
    }
    
    public List<CharacterInfo> characterInfos;
    

    private void Awake()
    {
        GameManager.Instance.dataManager = this;
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
