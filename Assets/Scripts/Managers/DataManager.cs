using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine.Android;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    private string _sheetData;
    private const string _sheetURL ="https://docs.google.com/spreadsheets/d/1fXMD0-E3BzRYGxw1NP9vNgQME82UK3_nQsQUexYfYzo/export?format=tsv&range=A2:D";
    
    // sheet Data Index
    private const int _SHEET_CHARACTER_ID_INDEX = 0;
    private const int _SHEET_CHARACTER_NAME_INDEX = 1;
    private const int _SHEET_CHARACTER_RATING_INDEX = 2;
    private const int _SHEET_CHARACTER_INDEX_INDEX = 3;
    
    // save Data
    [SerializeField]
    private float _highScore;
    
    private UserData _saveData;

    public Dictionary<int, CharacterInfo> characterInfos = new Dictionary<int, CharacterInfo>();
    public List<CharacterInfo> haveCharacters = new List<CharacterInfo>();

    [Serializable]
    public class CharacterIso
    {
        public Sprite characterImage;
        public GameObject characterPrefab;
    }
    
    public List<CharacterIso> characterIso = new List<CharacterIso>();

    private void Awake()
    {
        GameManager.Instance.datamanager = this;

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
        SaveManager.Instance.LoadUserData();
        if (_saveData == null)
        {
            _saveData = new UserData();
        }
        UIManager.Instance.SetCoinUI(_saveData.coin);
        CheckInternet();
    }
    
    public void CheckInternet()
    {
        if (!Internet.IsOkInternet())
        {
            Time.timeScale = 0;
            UIManager.Instance.OnErrorInternet();
        }
        else
        {
            Time.timeScale = 1;
            UIManager.Instance.OffErrorInternet();
            StartCoroutine(StartLogic());
        }
    }

    private IEnumerator StartLogic()
    {
        UIManager.Instance.SetCoinUI(SaveData.coin);
        
        using (UnityWebRequest www = UnityWebRequest.Get(_sheetURL))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                _sheetData = www.downloadHandler.text;
                SetData();
            }
        }
    }

    private void SetData()
    {
        string[] row = _sheetData.Split("\n");
        
        foreach (var data in row)
        {
            string[] columns = data.Split("\t");

            CharacterInfo characterInfo = new CharacterInfo()
            {
                characterId = int.Parse(columns[_SHEET_CHARACTER_ID_INDEX]),
                characterName = columns[_SHEET_CHARACTER_NAME_INDEX],
                characterRating = (Gacha.Probability)Enum.Parse(typeof(Gacha.Probability), columns[_SHEET_CHARACTER_RATING_INDEX]),
                characterIndex = int.Parse(columns[_SHEET_CHARACTER_INDEX_INDEX])
            };
            
            if (characterInfo.characterId == 0)
            {
                Debug.Log(characterInfo);
                _saveData.unlockCharacters = new List<int> { characterInfo.characterId };
                continue;
            }
            
            characterInfos.Add(characterInfo.characterId, characterInfo);
        }
    }
    
    
    public float HighScore
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
