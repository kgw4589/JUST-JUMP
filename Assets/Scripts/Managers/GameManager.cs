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

    public float playerPosY;

    public Action initAction;
    public Action startAction;

    public enum GameMode
    {
        Easy,
        Normal,
        Hard
    }

    public enum GameState
    {
        Ready,
        Pause,
        Play,
        End
    }

    public GameState gameState = GameState.Ready;
    public GameMode gameMode = GameMode.Easy;
    
    protected override void Init()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
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
        Application.targetFrameRate = 60;
        SoundManager.Instance.PlayBgm(true);
        switch (gameMode)
        {
            case GameMode.Easy:
                DataManager.Instance.HighScore = DataManager.Instance.SaveData.easyHighScore;
                break;
            case GameMode.Normal:
                DataManager.Instance.HighScore = DataManager.Instance.SaveData.normalHighScore;
                break;
            case GameMode.Hard:
                DataManager.Instance.HighScore = DataManager.Instance.SaveData.hardHighScore;
                break;
        }
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
        if (playerPosY > DataManager.Instance.HighScore)
        {
            DataManager.Instance.HighScore = playerPosY;
            switch (gameMode)
            {
                case GameMode.Easy:
                    DataManager.Instance.SaveData.easyHighScore = playerPosY;
                    break;
                case GameMode.Normal:
                    DataManager.Instance.SaveData.normalHighScore = playerPosY;
                    break;
                case GameMode.Hard:
                    DataManager.Instance.SaveData.hardHighScore = playerPosY;
                    break;
            }
            // try {
            //     FireBaseManager.Instance.GetSaveInDB(JsonUtility.ToJson(DataManager.Instance.Instance.SaveData));
            //     FireBaseManager.Instance.WriteRanking();
            // } catch (Exception e) {
            //     Debug.Log(e);
            //     gameState = GameState.End;
            // }
            SaveManager.Instance.GetSaveUserData(DataManager.Instance.SaveData);
        }
        
        Time.timeScale = 0;
        gameState = GameState.End;
        Application.targetFrameRate = 30;
        SoundManager.Instance.PlayBgm(false);
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.gameOver);
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

    private void OnApplicationQuit()
    {
        SaveManager.Instance.GetSaveUserData(DataManager.Instance.SaveData);
    }
}
