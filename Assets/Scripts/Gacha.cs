using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Gacha : MonoBehaviour
{
    private enum Probability
    {
        Normal = 700,
        Epic = 250,
        Legend = 500
    }
    private int _totalProbability;
    private int _currentPivot;
    
    private Dictionary<Probability, string> _sheetURL = new Dictionary<Probability, string>()
    {
        { Probability.Normal , "https://docs.google.com/spreadsheets/d/1fXMD0-E3BzRYGxw1NP9vNgQME82UK3_nQsQUexYfYzo/export?format=tsv&range=A2:C4"},
        { Probability.Epic , "https://docs.google.com/spreadsheets/d/1RzikpWotuwisLCb3Tk2-IKyTTldfbQMozatybseHVzI/export?format=tsv&range=A2:C3"},
        { Probability.Legend , "https://docs.google.com/spreadsheets/d/1z_Ep63oibFqS7L808ab9Bu1lOCxEMSxXrEagNNGU2Oo/export?format=tsv&range=A2:C2"}
    };

    private Dictionary<Probability, string> _sheetData = new Dictionary<Probability, string>();

    private string[] _sheetDataRow;

    [SerializeField] private TextMeshProUGUI tempRatingText;
    [SerializeField] private TextMeshProUGUI tempGachaName;
    [SerializeField] private Image tempGachaImage;

    private void Start()
    {
        _totalProbability = 0;
        _currentPivot = 0;

        StartCoroutine(SetSheetData());
    }

    IEnumerator SetSheetData()
    {
        foreach (Probability probability in Enum.GetValues(typeof(Probability)))
        {
            _totalProbability += (int)probability;
            
            // UnityWebRequest 인스턴스 리소스 해제를 위해 Using 사용함.
            using (UnityWebRequest www = UnityWebRequest.Get(_sheetURL[probability]))
            {
                yield return www.SendWebRequest();

                if (www.isDone)
                {
                    _sheetData.Add(probability, www.downloadHandler.text);
                }
            }
        }
    }

    private void SetData(string sheetData)
    {
        _sheetDataRow = sheetData.Split("\n");

        int value = Random.Range(0, _sheetDataRow.Length);

        var a = _sheetDataRow[value].Split("\t");

        tempRatingText.text = a[0];
        tempGachaName.text = a[1];
    }

    public void PlayGacha()
    {
        int randomValue = Random.Range(1, _totalProbability + 1);

        foreach (Probability probability in Enum.GetValues(typeof(Probability)))
        {
            _currentPivot += (int)probability;

            if (randomValue <= _currentPivot)
            {
                Debug.Log(probability);
                SetData(_sheetData[probability]);
                break;
            }
        }

        _currentPivot = 0;
    }
}
