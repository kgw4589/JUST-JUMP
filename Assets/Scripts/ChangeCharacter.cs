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
    private int _characterIndex = 0;      //현재 화면에 보일 캐릭터 인덱스임
    
    private GameObject _player;           // 적용될 플레이어
    
    private GameObject _skin; // 

    public GameObject RigthButton;
    public GameObject LeftButton;

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _characterIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_characterIndex == 0)
        {
            RigthButton.SetActive(false);
        }
        else
        {
            RigthButton.SetActive(true);
        }
        if (_characterIndex == GameManager.Instance.dataManager.characterIso.Count -1)
        {
            LeftButton.SetActive(false);
        }
        else
        {
            LeftButton.SetActive(true);
        }
        NowCharacterText.text = GameManager.Instance.dataManager.characterIso[_characterIndex].characterPrefab.gameObject.name;
        ViewCharacterImage.sprite = GameManager.Instance.dataManager.characterIso[_characterIndex].characterImage;
        
    }
    

    public void Apply()
    {
        GameObject DeletObject = GameObject.FindWithTag("Skin");
        if (DeletObject != null)
        {
            Destroy(DeletObject);
        }
        _selectCharacter = GameManager.Instance.dataManager.characterIso[_characterIndex].characterPrefab;
        _skin = Instantiate(_selectCharacter);
        _skin.transform.SetParent(_player.transform);

        if (_characterIndex == 0)
        {
            _skin.gameObject.transform.localPosition = new Vector3(0, -0.4f, 0);
            _skin.gameObject.transform.localScale = new Vector3(5, 7, 0);
        }
        else
        {
            _skin.gameObject.transform.localPosition = new Vector3(0, -1.6f, 0);
            _skin.gameObject.transform.localScale = new Vector3(2.5f, 4, 1);
        }
    }

    public void RigthButtonPush()
    {
        
        if (_characterIndex != 0)
        {
            _characterIndex--;
        }
    }
    public void LeftButtonPush()
    {
        
        if (_characterIndex != GameManager.Instance.dataManager.characterIso.Count -1)
        {
            _characterIndex ++;
        }
    }
    //
    // public void AddList(GameObject gameObject,Sprite sprite)
    // {//가챠로 뽑았을때
    //     CharacterList.Add(gameObject);
    //     ImagesList.Add(sprite);
    // }
}