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
    private Dictionary<string, int> _testDic = new Dictionary<string, int>();

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
        int cnt = 0;
        
        for (int i = 0; i < 1000; i++)
        {
            ++cnt;
            int a = Random.Range(1, _totalProbability + 1);
            Debug.Log(a);

            foreach (int probability in Enum.GetValues(typeof(Probability)))
            {
                Debug.Log("확률" + probability);
                _currentPivot += probability;
            
                if (a <= _currentPivot)
                {
                    Debug.Log((Probability)probability);
                    if (!_testDic.TryAdd(probability.ToString(), 1))
                    {
                        ++_testDic[probability.ToString()];
                        break;
                    }
                }
            }

            _currentPivot = 0;
        }

        foreach (var data in _testDic)
        {
            Debug.Log("g" + data.Key);
            Debug.Log("fad" + data.Value);
        }
        Debug.Log(cnt);
    }
}
