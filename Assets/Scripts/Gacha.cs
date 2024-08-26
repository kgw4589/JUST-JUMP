using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Gacha : MonoBehaviour
{
    public enum Probability
    {
        Normal = 700,
        Epic = 600,
        Legend = 500
    }
    private int _totalProbability;
    private int _currentPivot;

    [SerializeField] private TextMeshProUGUI tempRatingText;
    [SerializeField] private TextMeshProUGUI tempGachaName;
    [SerializeField] private Image tempGachaImage;
    
    private Dictionary<Probability, List<CharacterInfo>> _characterInfos = new Dictionary<Probability, List<CharacterInfo>>();

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance.dataManager.characterInfos.Count > 0);
        Debug.Log(GameManager.Instance.dataManager.characterInfos.Count);
        foreach (var characterInfo in GameManager.Instance.dataManager.characterInfos)
        {
            if (!_characterInfos.ContainsKey(characterInfo.Value.characterRating))
            {
                _characterInfos.Add(characterInfo.Value.characterRating, new List<CharacterInfo>(){characterInfo.Value});
            }
            else
            {
                _characterInfos[characterInfo.Value.characterRating].Add(characterInfo.Value);
            }
        }

        _totalProbability = 0;
        foreach (int probability in Enum.GetValues(typeof(Probability)))
        {
            _totalProbability += probability;
        }
    }

    public void PlayGacha()
    {
        _currentPivot = 0;
        
        if (_characterInfos.Keys.Count <= 0)
        {
            Debug.Log("모든 캐릭터를 뽑았음");
            return;
        }

        int randomValue = Random.Range(1, _totalProbability + 1);
        Debug.Log("가챠 랜덤 값 : " + randomValue);
        
        foreach (Probability probability in _characterInfos.Keys)
        {
            _currentPivot += (int)probability;

            if (randomValue <= _currentPivot)
            {
                Debug.Log(probability);
                SelectedCharacter(probability);
                break;
            }
        }
    }

    private void SelectedCharacter(Probability probability)
    {
        CharacterInfo selectedCharacter = _characterInfos[probability][Random.Range(0, _characterInfos[probability].Count)];
        _characterInfos[probability].Remove(selectedCharacter);

        Debug.Log(selectedCharacter.characterName);
        // GameManager.Instance.dataManager.SaveData.UnlockCharacters.Add(selectedCharacter.characterId);
        SetUIData(selectedCharacter);
        
        if (_characterInfos[probability].Count <= 0)
        {
            _characterInfos.Remove(probability);
            Debug.Log(probability + " 등급의 모든 캐릭터를 뽑음");
            foreach (var characterInfo in _characterInfos)
            {
                _totalProbability = 0;
                _totalProbability += (int)characterInfo.Key;
            }
        }
    }

    private void SetUIData(CharacterInfo character)
    {
        tempGachaName.text = character.characterName;
        tempRatingText.text = character.characterRating.ToString();
        tempGachaImage.sprite = GameManager.Instance.dataManager.characterIso[character.characterIndex].characterImage;
    }
}
