using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _playerPosY;
    [SerializeField] private GameObject startUICanvas;
    [SerializeField] private GameObject inGameCanvas;

    [SerializeField] private GameObject wave2DGameObject;
    
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseClose;
    
    public void OnClickStartButton()
    {
        Debug.Log("Game Start");
        wave2DGameObject.SetActive(true);
        _gameManager.StartGame();
        startUICanvas.SetActive(false);
        inGameCanvas.SetActive(true);
    }

    public void OnClickPauseButton()
    {
        Debug.Log("Game Pause");
        _gameManager.PauseGame();
        pausePanel.SetActive(true);
    }

    public void OnClickPauseClose()
    {
        pausePanel.SetActive(false);
        Debug.Log("Panel Close");
        _gameManager.StartGame();
    }

    private void Awake()
    {
        wave2DGameObject.SetActive(false);
        inGameCanvas.SetActive(false);
    }

    private void Update()
    {
        if (_gameManager.isPlay)
        {
            _playerPosY.text = _gameManager.PlayerPosY;
        }
    }
}
