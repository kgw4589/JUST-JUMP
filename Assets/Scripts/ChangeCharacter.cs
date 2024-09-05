using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ChangeCharacter : MonoBehaviour
{
    [SerializeField] private bool isUnLockCharacter;
    public GameObject lockCharacterText;


    public Image ViewCharacterImage; // 화면에 보일 이미지
    public TextMeshProUGUI NowCharacterText;

    private GameObject _selectCharacter; //  고른 캐릭터
    private GameObject _nowSelectCharacter; //  고른 캐릭터
    private int _characterId = 0; //현재 화면에 보일 캐릭터 ID

    private GameObject _player; // 적용될 플레이어

    private GameObject _skin; // 

    public GameObject RigthButton;
    public GameObject LeftButton;

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _characterId = 0;
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
        ViewCharacterImage.sprite = GameManager.Instance.datamanager.characterIsoScriptableObject
            .characterIso[_characterId].characterImage;
        _nowSelectCharacter = GameManager.Instance.datamanager.characterIsoScriptableObject.characterIso[_characterId]
            .characterPrefab;
        isUnLockCharacter = GameManager.Instance.datamanager.SaveData.unlockCharacters.
            Contains(_characterId);
        lockCharacterText.SetActive(!isUnLockCharacter);
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
        }
    }

    public void RightButtonPush()
    {
        if (_characterId != GameManager.Instance.datamanager.characterIsoScriptableObject.characterIso.Count - 1)
        {
            _characterId++;
        }
    }
}