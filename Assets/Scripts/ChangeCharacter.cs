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
        { Gacha.Probability.Normal , 50},
        { Gacha.Probability.Epic , 100 },
        { Gacha.Probability.Legend , 200 }
    };

    private Animator _animator;
    private GameObject _selectCharacter; //  고른 캐릭터
    private GameObject _nowSelectCharacter; //  고른 캐릭터
    private int _characterId = 0; //현재 화면에 보일 캐릭터 ID

    private GameObject _player; // 적용될 플레이어

    private GameObject _skin; // 
    

    [SerializeField] private GameObject RigthButton;
    [SerializeField] private GameObject LeftButton;
    
    public int price = 5;

    void Start()
    {
        _animator = transform.GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player");
        _characterId = 0;
        ViewCharacterImage.sprite = GameManager.Instance.datamanager.characterIsoScriptableObject
            .characterIso[_characterId].characterImage;
        _nowSelectCharacter = GameManager.Instance.datamanager.characterIsoScriptableObject.characterIso[_characterId]
            .characterPrefab;
        isUnLockCharacter = GameManager.Instance.datamanager.SaveData.unlockCharacters.
            Contains(_characterId);
        price = _priceDictionary[GameManager.Instance.datamanager.characterInfos[_characterId].characterRating];
        characterRating.text = GameManager.Instance.datamanager.characterInfos[_characterId].characterRating.ToString();
        TextChageColor();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Application.persistentDataPath);
        if (_characterId == 0)
        {
            RigthButton.SetActive(false);
        }
        else
        {
            RigthButton.SetActive(true);
        }

        if (_characterId == GameManager.Instance.datamanager.characterIsoScriptableObject.characterIso.Count - 1)
        {
            LeftButton.SetActive(false);
        }
        else
        {
            LeftButton.SetActive(true);
        }
        // NowCharacterText.text = GameManager.Instance.datamanager.characterInfos[GameManager.Instance.datamanager.SaveData.unlockCharacters[_characterId]].characterName;
        NowCharacterText.text = GameManager.Instance.datamanager.characterInfos[_characterId].characterName;
        // ViewCharacterImage.sprite = GameManager.Instance.datamanager.characterIsoScriptableObject.characterIso
        //     [GameManager.Instance.datamanager.characterInfos
        //         [GameManager.Instance.datamanager.SaveData.unlockCharacters[_characterId]].characterIndex].characterImage;
       
        
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

    public void Buy()
    {
        if (GameManager.Instance.datamanager.SaveData.coin >= price)
        {
            GameManager.Instance.datamanager.SaveData.coin -= price;
            Debug.Log($"캐릭터 구입했습ㄴ다.{GameManager.Instance.datamanager.SaveData.coin}");
            UIManager.Instance.SetCoinUI(GameManager.Instance.datamanager.SaveData.coin);
            GameManager.Instance.datamanager.SaveData.unlockCharacters.Add(_characterId);
            SaveManager.Instance.GetSaveUserData(GameManager.Instance.datamanager.SaveData);
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
        isUnLockCharacter = GameManager.Instance.datamanager.SaveData.unlockCharacters.
            Contains(_characterId);
        price = _priceDictionary[GameManager.Instance.datamanager.characterInfos[_characterId].characterRating];
        characterRating.text = GameManager.Instance.datamanager.characterInfos[_characterId].characterRating.ToString();
    }

    public void Apply()
    {
        GameObject DeletObject = GameObject.FindWithTag("Skin");
        if (DeletObject != null)
        {
            Destroy(DeletObject);
        }

        // _selectCharacter = GameManager.Instance.datamanager.characterIsoScriptableObject.characterIso[GameManager.Instance.datamanager.characterInfos
        //     [GameManager.Instance.datamanager.SaveData.unlockCharacters[_characterId]].characterIndex].characterPrefab;
        _selectCharacter = _nowSelectCharacter;
        _skin = Instantiate(_selectCharacter);
        _skin.transform.SetParent(_player.transform);

        _skin.gameObject.transform.localPosition = new Vector3(0, -2f, 0);
        _skin.gameObject.transform.localScale = new Vector3(2.5f, 4, 1);

        UIManager.Instance.OnClickCharaChangeClose();
    }

    public void LeftButtonPush()
    {
        if (_characterId != 0)
        {
            _characterId--;
            ViewCharacterImage.sprite = GameManager.Instance.datamanager.characterIsoScriptableObject
                .characterIso[_characterId].characterImage;
            _nowSelectCharacter = GameManager.Instance.datamanager.characterIsoScriptableObject.characterIso[_characterId]
                .characterPrefab;
            isUnLockCharacter = GameManager.Instance.datamanager.SaveData.unlockCharacters.
                Contains(_characterId);
            price = _priceDictionary[GameManager.Instance.datamanager.characterInfos[_characterId].characterRating];
            characterRating.text = GameManager.Instance.datamanager.characterInfos[_characterId].characterRating.ToString();
            TextChageColor();
        }
    }

    void TextChageColor()
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

    public void RightButtonPush()
    {
        if (_characterId != GameManager.Instance.datamanager.characterIsoScriptableObject.characterIso.Count - 1)
        {
            _characterId++;
            ViewCharacterImage.sprite = GameManager.Instance.datamanager.characterIsoScriptableObject
                .characterIso[_characterId].characterImage;
            _nowSelectCharacter = GameManager.Instance.datamanager.characterIsoScriptableObject.characterIso[_characterId]
                .characterPrefab;
            isUnLockCharacter = GameManager.Instance.datamanager.SaveData.unlockCharacters.
                Contains(_characterId);
            price = _priceDictionary[GameManager.Instance.datamanager.characterInfos[_characterId].characterRating];
            characterRating.text = GameManager.Instance.datamanager.characterInfos[_characterId].characterRating.ToString();
            TextChageColor();
        }
    }
}