using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    
    [SerializeField]
    private int _highScore;
    
    [SerializeField]
    private float _playerPosY;
    public bool isPlay;
    
    private void Awake()
    {
        _highScore = PlayerPrefs.GetInt("highScore", 0);
        StartGame();
    }

    private void Update()
    {
        if (isPlay)
        {
            InGamePlay();
            return;
        }
    }
    
    private void StartGame()
    {
        isPlay = true;
    }

    private void InGamePlay()
    {
        _playerPosY = _player.gameObject.transform.position.y;

        /*if (!_player.die)
        {
            return;
        }
        
        if (_playerPosY > _highScore)
        {
            _highScore = (int)_playerPosY;
            PlayerPrefs.SetInt("highScore", _highScore);
        }
            
        isPlay = false;*/
    }

    public String PlayerPosY
    {
        get { return _playerPosY.ToString("F3") + " m";  }
    }
    
}
