using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject startUICanvas;
    public GameObject inGameCanvas;

    public GameObject wave2DGameObject;
    
    public GameObject pausePanel;
    public GameObject pauseClose;
    public GameObject homeButton;
    public GameObject diePanel;
    
    private int _currentFloor;
    
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private TextMeshProUGUI _playerPosY;
    [SerializeField]
    private TextMeshProUGUI _highScore;
    [SerializeField]
    private TextMeshProUGUI _currentScore;
    
    public void OnClickStartButton()
    {
        Debug.Log("Game Start");
        wave2DGameObject.SetActive(true);
        _gameManager.StartGame();
        startUICanvas.SetActive(false);
        inGameCanvas.SetActive(true);
        diePanel.SetActive(false);

        GameManager.Instance.gameState = GameManager.GameState.Play;
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
        SoundManager.Instance.PlayBgm(true);
    }

    public void OnClickPauseButton()
    {
        Debug.Log("Game Pause");
        _gameManager.PauseGame();
        pausePanel.SetActive(true);

        GameManager.Instance.gameState = GameManager.GameState.Pause;
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
        SoundManager.Instance.PlayBgm(false);
    }

    public void OnClickPauseClose()
    {
        pausePanel.SetActive(false);
        Debug.Log("Panel Close");
        _gameManager.StartGame();

        GameManager.Instance.gameState = GameManager.GameState.Play;
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
        SoundManager.Instance.PlayBgm(true);
    }

    public void OnClickHomeButton()
    {
        Debug.Log("Go Home");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.menuClose);
    }

    public void OnClickDieClose()
    {
        Debug.Log("Die Close");
        diePanel.SetActive(false);
        inGameCanvas.SetActive(false);
        startUICanvas.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.gameState = GameManager.GameState.Ready;
    }

    private void Awake()
    {
        wave2DGameObject.SetActive(false);
        inGameCanvas.SetActive(false);
    }

    private void Update()
    {
        if (_gameManager.gameState == GameManager.GameState.Play)
        {
            _playerPosY.text = _gameManager.PlayerPosY.ToString("F3") + "m";
            _currentFloor = Mathf.FloorToInt(_gameManager.PlayerPosY);
        }

        if (_gameManager.gameState == GameManager.GameState.End)
        {
            _highScore.text = _gameManager.HighScore.ToString("F3") + "m";
            _currentScore.text = _currentFloor.ToString("F3") + "m";
            diePanel.SetActive(true);
        }
    }
}
