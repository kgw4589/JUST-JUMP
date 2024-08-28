using System;
using System.Collections;
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
        Epic = 600,
        Legend = 500
    }
    private int _totalProbability;
    private int _currentPivot;

    private Dictionary<Probability, List<CharacterInfo>> _characterInfos = new Dictionary<Probability, List<CharacterInfo>>();

    private const int _COIN_PRICE = 10;  
    
    [Header("#UI Gacha")]
    [SerializeField] private Sprite originGachaImage;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private TextMeshProUGUI gachaName;
    [SerializeField] private Image gachaImage;
    [SerializeField] private TextMeshProUGUI gachaErrorText;
    [SerializeField] private Animator gachaErrorAnimator;
    
    private const string _GACHA_ORIGIN_NAME_MESSAGE = "캐릭터 뽑기";
    private const string _GACHA_ORIGIN_RATING_MESSAGE = "10 코인";
    private const string _HAVE_NO_COIN_MESSAGE = "코인이 부족합니다";
    private const string _HAVE_ALL_CHARACTER_MESSAGE = "모든 캐릭터를 보유하고 있습니다";

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
        
        SetGachaPanelOrigin();
    }
    
    public void PlayGacha()
    {
        if (_characterInfos.Keys.Count <= 0)
        {
            GachaError(_HAVE_ALL_CHARACTER_MESSAGE);
        }
        else if (GameManager.Instance.dataManager.Coin < _COIN_PRICE)
        {
            GachaError(_HAVE_NO_COIN_MESSAGE);
        }
        else
        {
            SelectRating();
            GameManager.Instance.dataManager.Coin -= _COIN_PRICE;
        }
    }

    private void SelectRating()
    {
        _currentPivot = 0;

        int randomValue = Random.Range(1, _totalProbability + 1);
        Debug.Log("가챠 랜덤 값 : " + randomValue);
        
        foreach (Probability probability in _characterInfos.Keys)
        {
            _currentPivot += (int)probability;
            
            if (randomValue <= _currentPivot)
            {
                SelectedCharacter(probability);
                break;
            }
        }
    }

    private void SetGachaPanelOrigin()
    {
        gachaName.text = _GACHA_ORIGIN_NAME_MESSAGE;
        ratingText.text = _GACHA_ORIGIN_RATING_MESSAGE;
        gachaImage.sprite = originGachaImage;
    }

    private void GachaError(string errorMessage)
    {
        Debug.Log("가챠 에러 " + errorMessage);
        gachaErrorText.text = errorMessage;
        gachaErrorAnimator.SetTrigger("PopUp");
    }

    private void SetGachaPanel(CharacterInfo character)
    {
        gachaName.text = character.characterName;
        ratingText.text = character.characterRating.ToString();
        gachaImage.sprite = GameManager.Instance.dataManager.characterIso[character.characterIndex].characterImage;
    }

    private void SelectedCharacter(Probability probability)
    {
        CharacterInfo selectedCharacter = _characterInfos[probability][Random.Range(0, _characterInfos[probability].Count)];
        _characterInfos[probability].Remove(selectedCharacter);

        Debug.Log(selectedCharacter.characterName);
        // GameManager.Instance.dataManager.SaveData.UnlockCharacters.Add(selectedCharacter.characterId);
        GameManager.Instance.dataManager.haveCharacters.Add(selectedCharacter);
        SetGachaPanel(selectedCharacter);
        
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
}
