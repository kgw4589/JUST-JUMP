using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

public class ChangeCharacter : MonoBehaviour
{
    [SerializeField] private bool isUnLockCharacter;
    [SerializeField] private GameObject lockCharacterDimmed;
    [SerializeField] private GameObject lockCharacterImage;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject applyButton;
    [SerializeField] private TextMeshProUGUI characterPrice;
    [SerializeField] private TextMeshProUGUI characterRating;
    [SerializeField] private Animator errorText;
    
    

    [SerializeField] private Image ViewCharacterImage; // 화면에 보일 이미지
    [SerializeField] private TextMeshProUGUI NowCharacterText;

    private Dictionary<Gacha.Probability, int> _priceDictionary = new Dictionary<Gacha.Probability, int>()
    {
        { Gacha.Probability.Normal , 350},
        { Gacha.Probability.Epic , 750 },
        { Gacha.Probability.Legend , 1500 }
    };

    private Animator _animator;
    private GameObject _selectCharacter; //  고른 캐릭터
    private GameObject _nowSelectCharacter; //  고른 캐릭터
    private int _characterId = 0; //현재 화면에 보일 캐릭터 ID

    private GameObject _player; // 적용될 플레이어

    private bool _isPassable = true;
    private bool _isBuy;
    private GameObject _skin; // 
    

    [SerializeField] private GameObject RigthButton;
    [SerializeField] private GameObject LeftButton;
    
    public int price = 5;

    void Start()
    {
        _isPassable = true;
        _isBuy = false;
        _animator = transform.GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player");
        _characterId = 0;
        ReRoadingReSoucse();
        CheckRLButton();
        
    }
    
    void UnLockCharacter()
    {
        if (isUnLockCharacter)
        {
            applyButton.SetActive(true);
            lockCharacterImage.SetActive(false);
            lockCharacterDimmed.SetActive(false);
            buyButton.SetActive(false);
            characterPrice.text = "";
        }
        else
        {
            characterPrice.text = $"가격: {price}";
            applyButton.SetActive(false);
            lockCharacterImage.SetActive(true);
            lockCharacterDimmed.SetActive(true);
            buyButton.SetActive(true);
        }
    }

    void CheckRLButton()
    {
        if (_characterId == 0)
        {
            RigthButton.SetActive(false);
        }
        else
        {
            RigthButton.SetActive(true);
        }
        if (_characterId == DataManager.Instance.characterIsoScriptableObject.characterIso.Count - 1)
        {
            LeftButton.SetActive(false);
        }
        else
        {
            LeftButton.SetActive(true);
        } 
    }

    void ReRoadingReSoucse()
    {
        ViewCharacterImage.sprite = DataManager.Instance.characterIsoScriptableObject
            .characterIso[_characterId].characterImage;
        _nowSelectCharacter = DataManager.Instance.characterIsoScriptableObject.characterIso[_characterId]
            .characterPrefab;
        isUnLockCharacter = DataManager.Instance.SaveData.unlockCharacters.
            Contains(_characterId);
        price = _priceDictionary[DataManager.Instance.characterInfos[_characterId].characterRating];
        characterRating.text = DataManager.Instance.characterInfos[_characterId].characterRating.ToString();
        NowCharacterText.text = DataManager.Instance.characterInfos[_characterId].characterName;
        TextChangeColor();
        UnLockCharacter();
    }

    public void CharacterBuy()
    {
        if (DataManager.Instance.SaveData.coin >= price && !_isBuy)
        {
            
            _isPassable = false;
            _isBuy = true;
            DataManager.Instance.SaveData.coin -= price;
            Debug.Log($"캐릭터 구입했습ㄴ다.{DataManager.Instance.SaveData.coin}");
            UIManager.Instance.SetCoinUI(DataManager.Instance.SaveData.coin);
            DataManager.Instance.SaveData.unlockCharacters.Add(_characterId);
            SaveManager.Instance.GetSaveUserData(DataManager.Instance.SaveData);
            UnLockCharacter();
            StartCoroutine(BuyAnimation());
        }
        else
        {
            errorText.SetTrigger("PopUp");
        }
    }

    IEnumerator BuyAnimation()
    {
        _animator.SetTrigger("Buy");
        yield return new WaitForSeconds(1f);
        Debug.Log("ㅠㅠㅠㅠㅠㅠㅠㅠㅠㅠ 매그네릭~");
        isUnLockCharacter = DataManager.Instance.SaveData.unlockCharacters.
            Contains(_characterId);
        price = _priceDictionary[DataManager.Instance.characterInfos[_characterId].characterRating];
        characterRating.text = DataManager.Instance.characterInfos[_characterId].characterRating.ToString();
        _isBuy = false;
        ReRoadingReSoucse();
        _isPassable = true;
    }

    public void CharacterApply()
    {
        _isPassable = false;
        GameObject DeletObject = GameObject.FindWithTag("Skin");
        if (DeletObject != null)
        {
            Destroy(DeletObject);
        }

        // _selectCharacter = DataManager.Instance.characterIsoScriptableObject.characterIso[DataManager.Instance.characterInfos
        //     [DataManager.Instance.SaveData.unlockCharacters[_characterId]].characterIndex].characterPrefab;
        _selectCharacter = _nowSelectCharacter;
        _skin = Instantiate(_selectCharacter);
        _skin.transform.SetParent(_player.transform);

        _skin.gameObject.transform.localPosition = new Vector3(0, -2f, 0);
        _skin.gameObject.transform.localScale = new Vector3(2.5f, 4, 1);
        UnLockCharacter();
        _isPassable = true;
        UIManager.Instance.OnClickCharaChangeClose();
    }

    public void LeftButtonPush()
    {
        Debug.Log(Application.persistentDataPath);
        if (_characterId != 0 && _isPassable)
        {
            _characterId--;
            CheckRLButton();
            ReRoadingReSoucse();
        }
    }
    public void RightButtonPush()
    {
        
        if (_characterId != DataManager.Instance.characterIsoScriptableObject.characterIso.Count - 1 && _isPassable)
        {
            _characterId++;
            CheckRLButton();
            ReRoadingReSoucse();   
        }
    }

    void TextChangeColor()
    {
        
        if (characterRating.text == Gacha.Probability.Normal.ToString())
        {
            characterRating.color = Color.gray;
        }
        else if (characterRating.text == Gacha.Probability.Epic.ToString())
        {
            characterRating.color = Color.magenta;
        }
        else
        {
            characterRating.color = Color.yellow;
        }
    }

    private void OnEnable()
    {
        
        _isPassable = true;
        _isBuy = false;
    }

    public void PlaySound()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.buyCharacter);
    }
    
}