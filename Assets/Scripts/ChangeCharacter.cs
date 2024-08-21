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
    public List<GameObject> CharacterList; // 캐릭터들 담길 리스트
    public Image ViewCharacterImage;   // 화면에 보일 이미지
    public TextMeshProUGUI nowCharacterText;
    [SerializeField]
    private GameObject SelectCharacter; //  고른 캐릭터
    public int CharacterIndex = 0;      //현재 화면에 보일 캐릭터
    
    private GameObject _player;           // 적용될 플레이어
    
    private GameObject _skin;

    public GameObject RigthButton;
    public GameObject LeftButton;

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        CharacterIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (CharacterIndex == 0)
        {
            RigthButton.SetActive(false);
        }
        else
        {
            RigthButton.SetActive(true);
        }
        if (CharacterIndex == CharacterList.Count -1)
        {
            LeftButton.SetActive(false);
        }
        else
        {
            LeftButton.SetActive(true);
        }
        nowCharacterText.text ="Now Character: "+ CharacterList[CharacterIndex].gameObject.name;
        SpriteRenderer spriteRenderer = CharacterList[CharacterIndex].GetComponent<SpriteRenderer>();
        ViewCharacterImage.sprite = spriteRenderer.sprite;
        Debug.Log(CharacterList[CharacterIndex].gameObject.name);
        
    }

    public void Apply()
    {
        GameObject DeletObject = GameObject.FindWithTag("Skin");
        if (DeletObject != null)
        {
            Destroy(DeletObject);
        }
        SelectCharacter = CharacterList[CharacterIndex];
        _skin = Instantiate(SelectCharacter);
        _skin.transform.SetParent(_player.transform);
        
        
        _skin.gameObject.transform.localPosition = new Vector3(0, -1.6f, 0);
        _skin.gameObject.transform.localScale = new Vector3(2, 4, 1);
    }

    public void RigthButtonPush()
    {
        if (CharacterIndex != 0)
        {
            Debug.Log("오른쪽");
            CharacterIndex--;
        }
    }
    public void LeftButtonPush()
    {
        if (CharacterIndex != CharacterList.Count -1)
        {
            Debug.Log("왼쪽");
            CharacterIndex ++;
        }
        else
        {
            Debug.Log(CharacterList.Count - 1);
        }
    }

    void AddList(GameObject gameObject)
    {//가챠로 뽑았을때
        CharacterList.Append(gameObject);
    }
}