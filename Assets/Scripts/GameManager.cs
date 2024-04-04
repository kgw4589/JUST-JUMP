using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get { return instance; }
    }
    
    public Player player;
    
    [SerializeField]
    private int _highScore;
    
    [SerializeField]
    private float _playerPosY;
    public bool isPlay;

    public enum GameState
    {
        Ready,
        Pause,
        Play,
        End
    }

    public GameState gameState = GameState.Ready;

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        
        Time.timeScale = 0; // game stop
        _highScore = PlayerPrefs.GetInt("highScore", 0);
    }
    
    public void StartGame()
    {
        player = FindObjectOfType<Player>();
        Time.timeScale = 1; // game start
        isPlay = true;
    }

    public void PauseGame()
    {
        player.useJump = false;
        Time.timeScale = 0; // game pause (stop)
    }

    private void Update()
    {
        if (!isPlay)
        {
            return;
        }
        
        _playerPosY = player.gameObject.transform.position.y;

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
