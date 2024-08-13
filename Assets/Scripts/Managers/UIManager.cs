using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public GameObject startUICanvas;
    public GameObject inGameCanvas;

    public GameObject wave2DGameObject;
    
    public GameObject pausePanel;
    public GameObject pauseClose;
    public GameObject homeButton;
    public GameObject tutorialButton;
    public GameObject diePanel;
    public GameObject tutorialPanel;
    public GameObject gachaButton;
    public GameObject gachaPanel;
    
    private int _currentFloor;
    
    [SerializeField]
    private TextMeshProUGUI _playerPosY;
    [SerializeField]
    private TextMeshProUGUI _highScore;
    [SerializeField]
    private TextMeshProUGUI _currentScore;

    public void InitObject()
    {
        startUICanvas.SetActive(true);
        inGameCanvas.SetActive(false);
        wave2DGameObject.SetActive(false);
        pausePanel.SetActive(false);
        diePanel.SetActive(false);
        gachaPanel.SetActive(false);
    }

    public void OnClickStartButton()
    {
        Debug.Log("Game Start");
        wave2DGameObject.SetActive(true);
        GameManager.Instance.StartGame(false);
        startUICanvas.SetActive(false);
        inGameCanvas.SetActive(true);
        diePanel.SetActive(false);
        tutorialPanel.SetActive(false);

        GameManager.Instance.gameState = GameManager.GameState.Play;
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
        SoundManager.Instance.PlayBgm(true);
    }

    public void OnClickPauseButton()
    {
        Debug.Log("Game Pause");
        GameManager.Instance.PauseGame();
        pausePanel.SetActive(true);

        GameManager.Instance.gameState = GameManager.GameState.Pause;
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickPauseClose()
    {
        pausePanel.SetActive(false);
        Debug.Log("Panel Close");
        GameManager.Instance.StartGame(true);

        GameManager.Instance.gameState = GameManager.GameState.Play;
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
        SoundManager.Instance.UnPauseBGM();
    }

    public void OnClickHomeButton()
    {
        Debug.Log("Go Home");
        
        GameManager.Instance.InitObjects();
        
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

    public void OnClickGachaButton()
    {
        gachaPanel.SetActive(true);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickGachaClose()
    {
        gachaPanel.SetActive(false);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
    }

    private void Awake()
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
            _playerPosY.text = GameManager.Instance.PlayerPosY.ToString("F2") + "m";
            _currentFloor = Mathf.FloorToInt(GameManager.Instance.PlayerPosY);
        }

        if (GameManager.Instance.gameState == GameManager.GameState.End)
        {
            _highScore.text = GameManager.Instance.HighScore.ToString("F2") + "m";
            _currentScore.text = _currentFloor.ToString("F2") + "m";
            diePanel.SetActive(true);
        }
    }
}
