using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine.Android;
using UnityEngine.Networking;

public class DataManager : Singleton<DataManager>
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

    public GameObject internetErrorPanel;

    public Dictionary<int, CharacterInfo> characterInfos = new Dictionary<int, CharacterInfo>();
    public CharacterIsoScriptable characterIsoScriptableObject;

    protected override void Init()
    {
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
        CheckInternet();

        characterIsoScriptableObject = Resources.Load<CharacterIsoScriptable>("Character Iso");
    }
    
    public void CheckInternet()
    {
        if (!Internet.IsOkInternet())
        {
            Time.timeScale = 0;
            internetErrorPanel.SetActive(true);
            LoadingSceneController.isInternetOk = false;
        }
        else
        {
            Time.timeScale = 1;
            internetErrorPanel.SetActive(false);
            StartCoroutine(StartLogic());
        }
    }

    private IEnumerator StartLogic()
    {
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
            
            if (_saveData.unlockCharacters.Count <= 0 && characterInfo.characterId == 0)
            {
                Debug.Log(characterInfo);
                _saveData.unlockCharacters = new List<int> { characterInfo.characterId };
            }
            
            characterInfos.Add(characterInfo.characterId, characterInfo);
        }
        
        LoadingSceneController.isInternetOk = true;
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
