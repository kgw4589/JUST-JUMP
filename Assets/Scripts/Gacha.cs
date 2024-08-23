using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Gacha : MonoBehaviour
{
    public enum Probability
    {
        Normal = 700,
        Epic = 250,
        Legend = 500
    }
    private int _totalProbability;
    private int _currentPivot;

    [SerializeField] private TextMeshProUGUI tempRatingText;
    [SerializeField] private TextMeshProUGUI tempGachaName;
    [SerializeField] private Image tempGachaImage;

    private CharacterInfo _characterInfo;

    private List<CharacterInfo> _normalCharacters = new List<CharacterInfo>();
    private List<CharacterInfo> _epicCharacters = new List<CharacterInfo>();
    private List<CharacterInfo> _legendCharacters = new List<CharacterInfo>();

    private void Awake()
    {
        _totalProbability = 0;
        _currentPivot = 0;
        foreach (Probability probability in Enum.GetValues(typeof(Probability)))
        {
            _totalProbability += (int)probability;
        }
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
                SelectedCharacter(probability);
                break;
            }
        }

        _currentPivot = 0;
    }

    private void SelectedCharacter(Probability probability)
    {
        
    }

    private void SetData()
    {
        // tempGachaImage.sprite = character.characterImage;
        // tempGachaName.text = character.characterName;
    }
}
