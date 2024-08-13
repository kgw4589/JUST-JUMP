using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class Gacha : MonoBehaviour
{
    private const string SHEET_URL =
        "https://docs.google.com/spreadsheets/d/1fXMD0-E3BzRYGxw1NP9vNgQME82UK3_nQsQUexYfYzo/export?format=tsv&range=A2:C7";

    private string[] _sheetDataRow;

    private enum Probability
    {
        Normal = 700, Epic = 250, Legend = 50
    }
    private int _totalProbability;
    private int _currentPivot;

    private Dictionary<string, int> _ratingDictionary = new Dictionary<string, int>();

    private IEnumerator Start()
    {
        _totalProbability = 0;
        _currentPivot = 0;
        foreach (int probability in Enum.GetValues(typeof(Probability)))
        {
            _totalProbability += probability;
        }

        // UnityWebRequest 인스턴스 리소스 해제를 위해 Using 사용함.
        using (UnityWebRequest www = UnityWebRequest.Get(SHEET_URL))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                SetData(www.downloadHandler.text);
            }
        }
    }

    private void SetData(string sheetData)
    {
        _sheetDataRow = sheetData.Split("\n");

        foreach (var data in _sheetDataRow)
        {
            if (!_ratingDictionary.TryAdd(data[0].ToString(), 0))
            {
                ++_ratingDictionary[data[0].ToString()];
            }
        }

        PlayGacha();
    }

    public void PlayGacha()
    {
        int randomValue = Random.Range(1, _totalProbability + 1);

        foreach (int probability in Enum.GetValues(typeof(Probability)))
        {
            Debug.Log("확률 : " + probability);
            _currentPivot += probability;

            if (randomValue <= _currentPivot)
            {
                Debug.Log((Probability)probability);
                break;
            }
        }
    }
}
