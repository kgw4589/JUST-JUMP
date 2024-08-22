using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;


public class GameManager : Singleton<GameManager>
{
    public Player player;
    
    [SerializeField]
    private int _highScore;
    
    public float _playerPosY;

    private JsonData _saveData;

    public Action initAction;
    public Action startAction;

    public enum GameState
    {
        Ready,
        Pause,
        Play,
        End
    }

    public GameState gameState = GameState.Ready;

    protected override void Init()
    {
        if (_saveData == null)
        {
            try {
                FireBaseManager.Instance.LoadSaveData();
            } catch (Exception e) {
                _highScore = 0;
                Debug.LogError(e);
            }
        }
        Time.timeScale = 0; // game stop
        _highScore = _saveData.highScore;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        // Debug.Log(_saveData.highScore);
    }
    
    public void InitObjects()
    {
        gameState = GameState.Ready;

        initAction();
    }
    
    public void StartGame(bool restart)
    {
        while (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        Time.timeScale = 1; // game start
        gameState = GameState.Play;
        Application.targetFrameRate = 120;
        if (!restart)
        {
            startAction();
        }
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0; // game pause (stop)
        gameState = GameState.Pause;
        Application.targetFrameRate = 30;
        SoundManager.Instance.PauseBGM();
    }
    
    private void GameOver()
    {
        if (_playerPosY > _highScore)
        {
            _highScore = (int)_playerPosY;
            _saveData.highScore = _highScore;
            try {
                FireBaseManager.Instance.GetSaveInDB(JsonUtility.ToJson(_saveData));
            } catch (Exception e) {
                Debug.LogError(e);
            }
        }
        
        Time.timeScale = 0;
        gameState = GameState.End;
        Application.targetFrameRate = 30;
        SoundManager.Instance.PlayBgm(false);
    }

    private void Update()
    {
        if (gameState != GameState.Play || Time.timeScale == 0 || !player)
        {
            return;
        }
        
        _playerPosY = player.gameObject.transform.position.y;

        if (player.isDie)
        {
            GameOver();
        }

    }
    
    public float PlayerPosY
    {
        get { return _playerPosY; }
    }

    public int HighScore
    {
        get { return _highScore; }
    }

    public JsonData SaveData
    {
        get { return _saveData; }
        set { _saveData = value; }
    }
}
