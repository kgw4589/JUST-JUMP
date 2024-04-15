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
    
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _playerPosY;
    
    public void OnClickStartButton()
    {
        Debug.Log("Game Start");
        wave2DGameObject.SetActive(true);
        _gameManager.StartGame();
        startUICanvas.SetActive(false);
        inGameCanvas.SetActive(true);

        GameManager.Instance.gameState = GameManager.GameState.Play;
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.hana);
        SoundManager.Instance.PlayBgm(true);
    }

    public void OnClickPauseButton()
    {
        Debug.Log("Game Pause");
        _gameManager.PauseGame();
        pausePanel.SetActive(true);

        GameManager.Instance.gameState = GameManager.GameState.Pause;
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.dull);
        SoundManager.Instance.PlayBgm(false);
    }

    public void OnClickPauseClose()
    {
        pausePanel.SetActive(false);
        Debug.Log("Panel Close");
        _gameManager.StartGame();

        GameManager.Instance.gameState = GameManager.GameState.Play;
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.set);
        SoundManager.Instance.PlayBgm(true);
    }

    public void OnClickHomeButton()
    {
        Debug.Log("Go Home");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.net);
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
            _playerPosY.text = _gameManager.PlayerPosY;
        }
    }
}
