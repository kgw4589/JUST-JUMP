using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject wave2DGameObject; // PixelWave
    
    private int _currentFloor;
    [SerializeField]
    private bool rightSort = false;
    
    [Header("#UI Canvases")]
    public GameObject startUICanvas; // Start Set UI Panel
    public GameObject inGameCanvas; // In Game UI Panel
    
    [Header("#UI Buttons")]
    [SerializeField] private GameObject homeButton; // HomeButton
    [SerializeField] private GameObject tutorialButton; // TutorialButton
    [SerializeField] private GameObject gachaButton; // GachaGoButton
    [SerializeField] private GameObject charaButton; // CharaChangeButton
    [SerializeField] private GameObject modeChangeButton; // ModeSelectButton
    [SerializeField] private GameObject inGameRightButton;
    [SerializeField] private GameObject inGameLeftButton;
    [SerializeField] private GameObject inGameSortButton;
    
    [Header("#UI Panels")]
    [SerializeField] private GameObject pausePanel; // PausePanel
    [SerializeField] private GameObject diePanel; // DiePanel
    [SerializeField] private GameObject tutorialPanel; // TutorialPanel
    [SerializeField] private GameObject gachaPanel; // GachaPanel
    [SerializeField] private GameObject charaPanel; // CharaChangePanel
    [SerializeField] private GameObject modeChangePanel; // ModePanel
    
    [Header("#UI Texts")]
    [SerializeField] private TextMeshProUGUI playerPosY; // PlayerPosY
    [SerializeField] private TextMeshProUGUI highScore; // Best Score
    [SerializeField] private TextMeshProUGUI currentScore; // Score
    [SerializeField] private TextMeshProUGUI coin; // Coin
    [SerializeField] private TextMeshProUGUI modeText; // Mode Name

    [Header("#Error Internet")]
    [SerializeField] private GameObject errorInternetPanel;
    [SerializeField] private Animator errorInternetAnimator;
    [SerializeField] private Gacha gacha;

    public void InitObject()
    {
        startUICanvas.SetActive(true);
        inGameCanvas.SetActive(false);
        wave2DGameObject.SetActive(false);
        pausePanel.SetActive(false);
        diePanel.SetActive(false);
        tutorialPanel.SetActive(false);
        gachaPanel.SetActive(false);
    }

    public void OnErrorInternet()
    {
        if (!errorInternetPanel.activeSelf)
        {
            errorInternetPanel.SetActive(true);
        }
        else
        {
            errorInternetAnimator.SetTrigger("PopUp");
        }
    }

    public void OffErrorInternet()
    {
        errorInternetPanel.SetActive(false);
    }

    public void OnClickCheckInternet()
    {
        GameManager.Instance.dataManager.CheckInternet();
    }

    public void OnClickStartButton()
    {
        Debug.Log("Game Start");
        wave2DGameObject.SetActive(true);
        GameManager.Instance.StartGame(false);
        startUICanvas.SetActive(false);
        inGameCanvas.SetActive(true);
        diePanel.SetActive(false);
        modeChangePanel.SetActive(false);
        tutorialPanel.SetActive(false);

        GameManager.Instance.gameState = GameManager.GameState.Play;
        GameManager.Instance.soundManager.PlaySfx(SoundManager.Sfx.menuClose);
        GameManager.Instance.soundManager.PlayBgm(true);
    }

    public void OnSortButton()
    {
        if (!rightSort)
        {
            OnRightSort();
        }
        else
        {
            OnMiddleSort();
        }
    }

    public void OnMiddleSort()
    {
        rightSort = false;
        inGameRightButton.transform.position = new Vector3(1350, 65, 0);
        inGameLeftButton.transform.position = new Vector3(80, 65, 0);
    }

    public void OnRightSort()
    {
        rightSort = true;
        inGameLeftButton.transform.position = new Vector3(600, 65, 0);
    }

    public void SetCoinUI(int haveCoin)
    {
        coin.text = haveCoin.ToString();
    }

    public void OnClickPauseButton()
    {
        Debug.Log("Game Pause");
        GameManager.Instance.PauseGame();
        pausePanel.SetActive(true);

        GameManager.Instance.gameState = GameManager.GameState.Pause;
        GameManager.Instance.soundManager.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickPauseClose()
    {
        pausePanel.SetActive(false);
        Debug.Log("Panel Close");
        GameManager.Instance.StartGame(true);

        GameManager.Instance.gameState = GameManager.GameState.Play;
        GameManager.Instance.soundManager.PlaySfx(SoundManager.Sfx.menuClose);
        GameManager.Instance.soundManager.UnPauseBGM();
    }

    public void OnClickHomeButton()
    {
        Debug.Log("Go Home");
        
        GameManager.Instance.InitObjects();
        
        GameManager.Instance.soundManager.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickTutorialButton()
    {
        tutorialPanel.SetActive(true);
        GameManager.Instance.soundManager.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickTutorialClose()
    {
        tutorialPanel.SetActive(false);
        GameManager.Instance.soundManager.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickDieClose()
    {
        Debug.Log("Die Close");
        diePanel.SetActive(false);
        inGameCanvas.SetActive(false);
        startUICanvas.SetActive(true);
        
        GameManager.Instance.InitObjects();
        GameManager.Instance.gameState = GameManager.GameState.Ready;
    }

    public void OnClickModeChangeButton()
    {
        modeChangePanel.SetActive(true);
        GameManager.Instance.soundManager.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickModeClose()
    {
        modeChangePanel.SetActive(false);
        GameManager.Instance.soundManager.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickCharaChangeButton()
    {
        charaPanel.SetActive(true);
        GameManager.Instance.soundManager.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickCharaChangeClose()
    {
        charaPanel.SetActive(false);
        GameManager.Instance.soundManager.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickGachaButton()
    {
        gachaPanel.SetActive(true);
        GameManager.Instance.soundManager.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickGachaClose()
    {
        gachaPanel.SetActive(false);
        gacha.SetGachaPanelOrigin();
        GameManager.Instance.soundManager.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickRanking()
    {
        if (GameManager.Instance.firebaseManager != null)
        {
            GameManager.Instance.firebaseManager.ShowLeaderBoard();
        }
        else
        {
            Debug.Log("Show failed : FirebaseManager is null");
        }
    }

    private void Awake()
    {
        GameManager.Instance.uiManager = this;
        
        wave2DGameObject.SetActive(false);
        inGameCanvas.SetActive(false);

        GameManager.Instance.initAction += InitObject;
    }

    private void Update()
    {
        if (!GameManager.Instance)
        {
            return;
        }
        if (GameManager.Instance.gameState == GameManager.GameState.Play)
        {
            playerPosY.text = GameManager.Instance.playerPosY.ToString("F2") + "m";
            _currentFloor = Mathf.FloorToInt(GameManager.Instance.playerPosY);
        }

        if (GameManager.Instance.gameState == GameManager.GameState.End)
        {
            // highScore.text = GameManager.Instance.dataManager.HighScore.ToString("F2") + "m";
            highScore.text = GameManager.Instance.mapManager.selectedMapScriptable.bestScore.ToString("F2") + "m";
            modeText.text = GameManager.Instance.mapManager.selectedMapScriptable.modeText;
            modeText.color = GameManager.Instance.mapManager.selectedMapScriptable.modeColor;
            currentScore.text = _currentFloor.ToString("F2") + "m";
            diePanel.SetActive(true);
        }
    }
}
