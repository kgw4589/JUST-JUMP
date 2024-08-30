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
    public Image ViewCharacterImage;   // 화면에 보일 이미지
    public TextMeshProUGUI NowCharacterText;

    private GameObject _selectCharacter; //  고른 캐릭터
    private int _characterId = 0;      //현재 화면에 보일 캐릭터 ID
    
    private GameObject _player;           // 적용될 플레이어
    
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
        if (_characterId == 0)
        {
            RigthButton.SetActive(false);
        }
        else
        {
            RigthButton.SetActive(true);
        }
        if (_characterId == GameManager.Instance.datamanager.SaveData.unlockCharacters.Count -1)
        {
            LeftButton.SetActive(false);
        }
        else
        {
            LeftButton.SetActive(true);
        }
        NowCharacterText.text = GameManager.Instance.datamanager.characterInfos[GameManager.Instance.datamanager.SaveData.unlockCharacters[_characterId]].characterName;
        ViewCharacterImage.sprite = GameManager.Instance.datamanager.characterIsoScriptableObject.characterIso
            [GameManager.Instance.datamanager.characterInfos[GameManager.Instance.datamanager.SaveData.unlockCharacters[_characterId]].characterIndex].characterImage;
        
    }
    

    public void Apply()
    {
        GameObject DeletObject = GameObject.FindWithTag("Skin");
        if (DeletObject != null)
        {
            Destroy(DeletObject);
        }
        _selectCharacter = GameManager.Instance.datamanager.characterIsoScriptableObject.characterIso[GameManager.Instance.datamanager.characterInfos
            [GameManager.Instance.datamanager.SaveData.unlockCharacters[_characterId]].characterIndex].characterPrefab;
        _skin = Instantiate(_selectCharacter);
        _skin.transform.SetParent(_player.transform);

        // if (_characterId == 0)
        // {
        //     _skin.gameObject.transform.localPosition = new Vector3(0, -0.4f, 0);
        //     _skin.gameObject.transform.localScale = new Vector3(5, 7, 0);
        // }
        
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
        
        if (_characterId != GameManager.Instance.datamanager.SaveData.unlockCharacters.Count -1)
        {
            _characterId ++;
        }
    }
    //
    // public void AddList(GameObject gameObject,Sprite sprite)
    // {//가챠로 뽑았을때
    //     CharacterList.Add(gameObject);
    //     ImagesList.Add(sprite);
    // }
}