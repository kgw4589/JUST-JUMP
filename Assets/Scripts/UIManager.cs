using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _playerPosY;
    [SerializeField] private GameObject startUICanvas;
    
    public void OnClickStartButton()
    {
        Debug.Log("Game Start");
        _gameManager.StartGame();
        startUICanvas.SetActive(false);
    }

    public void OnClickPauseButton()
    {
        Debug.Log("Game Pause");
        _gameManager.PauseGame();
    }
    
    void Update()
    {
        if (_gameManager.isPlay)
        {
            _playerPosY.text = _gameManager.PlayerPosY;
        }
    }
}
