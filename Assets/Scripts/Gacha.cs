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

    [SerializeField] private TextMeshProUGUI tempRatingText;
    [SerializeField] private TextMeshProUGUI tempGachaName;
    [SerializeField] private Image tempGachaImage;

    private CharacterScriptable _characterScriptable;
    private List<CharacterScriptable.CharacterInfo> _haveNotCharacters = new List<CharacterScriptable.CharacterInfo>();

    private void Awake()
    {
        _totalProbability = 0;
        _currentPivot = 0;
        foreach (Probability probability in Enum.GetValues(typeof(Probability)))
        {
            _totalProbability += (int)probability;
        }
        
        _characterScriptable = Resources.Load<CharacterScriptable>("AllCharacterList");
        
        // foreach (var characterInfo in _characterScriptable.normalCharacters)
        // {
        //     _haveNotCharacters.Add(characterInfo);
        // }
        // foreach (var characterInfo in _characterScriptable.epicCharacters)
        // {
        //     _haveNotCharacters.Add(characterInfo);
        // }
        // foreach (var characterInfo in _characterScriptable.legendCharacters)
        // {
        //     _haveNotCharacters.Add(characterInfo);
        // }
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
        CharacterScriptable.CharacterInfo selectedCharacter = default;
        List<CharacterScriptable.CharacterInfo> selectedInfos;
        
        switch (probability)
        {
            // case Probability.Normal :
            //     selectedInfos = _characterScriptable.normalCharacters;
            //     selectedCharacter = selectedInfos[Random.Range(0, selectedInfos.Count)];
            //     selectedCharacter.ratingText = _characterScriptable.normalText;
            //     break;
            // case Probability.Epic :
            //     selectedInfos = _characterScriptable.epicCharacters;
            //     selectedCharacter = selectedInfos[Random.Range(0, selectedInfos.Count)];
            //     selectedCharacter.ratingText = _characterScriptable.epicText;
            //     break;
            // case Probability.Legend :
            //     selectedInfos = _characterScriptable.legendCharacters;
            //     selectedCharacter = selectedInfos[Random.Range(0, selectedInfos.Count)];
            //     selectedCharacter.ratingText = _characterScriptable.legendText;
            //     break;
        }

        _haveNotCharacters.Remove(selectedCharacter);
        SetData(selectedCharacter);
    }

    private void SetData(CharacterScriptable.CharacterInfo character)
    {
        tempRatingText.text = character.ratingText;
        tempGachaImage.sprite = character.characterImage;
        tempGachaName.text = character.characterName;
    }
}
