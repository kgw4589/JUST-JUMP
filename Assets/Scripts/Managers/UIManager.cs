using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject wave2DGameObject; // PixelWave
    
    private float _currentFloor;
    [SerializeField]
    private bool rightSort = false;
    
    private Vector3 rightSortAllBtnPOS = new Vector3(600, 65, 0);
    private Vector3 midSortLBtnPOS = new Vector3(80, 65, 0);
    private Vector3 midSortRBtnPOS = new Vector3(1350, 65, 0);
    
    [Header("#UI Canvases")]
    [SerializeField] private GameObject startUICanvas; // Start Set UI Panel
    [SerializeField] private GameObject inGameCanvas; // In Game UI Panel
    [SerializeField] private GameObject tutorialImg;
    
    [Header("#UI Buttons")]
    [SerializeField] private GameObject homeButton; // HomeButton
    [SerializeField] private GameObject tutorialButton; // TutorialButton
    [SerializeField] private GameObject gachaButton; // GachaGoButton
    [SerializeField] private GameObject charaButton; // CharaChangeButton
    [SerializeField] private GameObject modeChangeButton; // ModeSelectButton
    [SerializeField] private GameObject inGameRightButton;
    [SerializeField] private GameObject inGameLeftButton;
    [SerializeField] private GameObject inGameSortButton;
    [SerializeField] private GameObject probabilityButton;
    
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
    [SerializeField] private GameObject paybackText;
    
    [Header("#Error Internet")]
    [SerializeField] private GameObject errorInternetPanel;
    [SerializeField] private Animator errorInternetAnimator;
    [SerializeField] private Gacha gacha;

    private void InitObject()
    {
        startUICanvas.SetActive(true);
        inGameCanvas.SetActive(false);
        tutorialImg.SetActive(false);
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
        DataManager.Instance.CheckInternet();
    }

    public void OnClickStartButton()
    {
        Debug.Log("Game Start");
        startUICanvas.SetActive(false);
        if (DataManager.Instance.SaveData.isFirstGame)
        {
            tutorialImg.SetActive(true);
            DataManager.Instance.SaveData.isFirstGame = false;
        }
        else
        {
            OnClickTutorial();
        }
    }

    public void OnClickTutorial()
    {
        tutorialImg.SetActive(false);
        wave2DGameObject.SetActive(true);
        startUICanvas.SetActive(false);
        inGameCanvas.SetActive(true);
        diePanel.SetActive(false);
        modeChangePanel.SetActive(false);
        tutorialPanel.SetActive(false);

        GameManager.Instance.StartGame(false);
        
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
        SoundManager.Instance.PlayBgm(true);
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

    public void ProbabilityButton()
    {
        Application.OpenURL("https://root-xylocarp-b3c.notion.site/2024-09-20-1079dfc52f1780568a3add1abaf17703");
    }

    public void OnMiddleSort()
    {
        rightSort = false;
        inGameLeftButton.transform.position = midSortLBtnPOS;
        inGameRightButton.transform.position = midSortRBtnPOS;
    }

    public void OnRightSort()
    {
        rightSort = true;
        inGameLeftButton.transform.position = rightSortAllBtnPOS;
    }

    public void SetCoinUI(int haveCoin)
    {
        coin.text = haveCoin.ToString();
    }

    public void OnClickPauseButton()
    {
        Debug.Log("Game Pause");
        if (inGameCanvas.activeSelf)
        {
            GameManager.Instance.PauseGame();
        }
        pausePanel.SetActive(true);

        GameManager.Instance.gameState = GameManager.GameState.Pause;
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickPauseClose()
    {
        pausePanel.SetActive(false);
        Debug.Log("Panel Close");

        if (inGameCanvas.activeSelf)
        {
            GameManager.Instance.StartGame(true);
        }
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
        SoundManager.Instance.UnPauseBGM();
    }

    public void OnClickHomeButton()
    {
        Debug.Log("Go Home");

        if (inGameCanvas.activeSelf)
        {
            GameManager.Instance.InitObjects();
        }
        
        pausePanel.SetActive(false);
        
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickTutorialButton()
    {
        tutorialPanel.SetActive(true);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickTutorialClose()
    {
        tutorialPanel.SetActive(false);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
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
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickModeClose()
    {
        modeChangePanel.SetActive(false);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickCharaChangeButton()
    {
        charaPanel.SetActive(true);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickCharaChangeClose()
    {
        charaPanel.SetActive(false);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickGachaButton()
    {
        gachaPanel.SetActive(true);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickGachaClose()
    {
        gachaPanel.SetActive(false);
        gacha.SetGachaPanelOrigin();
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
    }
    
    // public void OnClickRanking()
    // {
    //     if (FireBaseManager.Instance != null)
    //     {
    //         FireBaseManager.Instance.ShowLeaderBoard();
    //     }
    //     else
    //     {
    //         Debug.Log("Show failed : FirebaseManager is null");
    //     }
    // }

    public void ShowPayback()
    {
        paybackText.SetActive(true);
    }

    public void HidePayback()
    {
        paybackText.SetActive(false);
    }

    protected override void Init()
    {
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
            _currentFloor = GameManager.Instance.playerPosY;
        }

        if (GameManager.Instance.gameState == GameManager.GameState.End)
        {
            highScore.text = DataManager.Instance.HighScore.ToString("F2") + "m";
            modeText.text = MapManager.Instance.selectedMapScriptable.modeText;
            modeText.color = MapManager.Instance.selectedMapScriptable.modeColor;
            currentScore.text = _currentFloor.ToString("F2") + "m";
            diePanel.SetActive(true);
        }
    }
}
