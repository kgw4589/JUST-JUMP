using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;


public class GameManager : Singleton<GameManager>
{
    // GameObjects
    public Player player;
    public MapManager mapManager;
    public UIManager uiManager;
    public DataManager dataManager;
    public SoundManager soundManager;
    public FireBaseManager firebaseManager;
    
    public float playerPosY;

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
        Time.timeScale = 0; // game stop
        // _highScore = _saveData.highScore;
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
        soundManager.PauseBGM();
    }
    
    private void GameOver()
    {
        if (playerPosY > dataManager.HighScore)
        {
            dataManager.HighScore = (int)playerPosY;
            // _saveData.highScore = _highScore;
            try {
                firebaseManager.GetSaveInDB(JsonUtility.ToJson(dataManager.SaveData));
            } catch (Exception e) {
                Debug.Log(e);
                gameState = GameState.End;
            }
        }
        
        Time.timeScale = 0;
        gameState = GameState.End;
        Application.targetFrameRate = 30;
        soundManager.PlayBgm(false);
    }

    private void Update()
    {
        if (gameState != GameState.Play || Time.timeScale == 0 || !player)
        {
            return;
        }
        
        playerPosY = player.gameObject.transform.position.y;

        if (player.isDie)
        {
            GameOver();
        }

    }
    

    
}
