using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    private string _sheetData;
    private const string _sheetURL ="https://docs.google.com/spreadsheets/d/1fXMD0-E3BzRYGxw1NP9vNgQME82UK3_nQsQUexYfYzo/export?format=tsv&range=A2:D2";
    
    // save Data
    [SerializeField]
    private int _highScore;
    
    private UserData _saveData;

    public Sprite[] characterSprites;
    public GameObject[] characterPrefabs;

    public List<CharacterInfo> characterInfos = new List<CharacterInfo>();

    private IEnumerator Start()
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
            string[] columns = row[0].Split("\t");

            CharacterInfo characterInfo = new CharacterInfo()
            {
                characterId = int.Parse(columns[0]),
                characterName = columns[1],
                characterRating = (Gacha.Probability)Enum.Parse(typeof(Gacha.Probability), columns[2]),
                characterIndex = int.Parse(columns[3])
            };
            
            this.characterInfos.Add(characterInfo);
        }
    }

    private void Awake()
    {
        GameManager.Instance.dataManager = this;
        
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
