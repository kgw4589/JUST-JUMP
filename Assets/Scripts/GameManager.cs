using System;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;


public class GameManager : Singleton<GameManager>
{
    public MapManager mapManager;
    
    public Player player;
    
    [SerializeField]
    private int _highScore;
    
    [SerializeField]
    private float _playerPosY;

    private JsonData _saveData;

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
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "SaveData.json")))
        {
            CreateJson(new JsonData(0));
        }
        
        Time.timeScale = 0; // game stop
        _saveData = LoadJson<JsonData>();
        _highScore = _saveData.highScore;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }
    
    public void StartGame()
    {
        mapManager = FindObjectOfType<MapManager>();
        while (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        Time.timeScale = 1; // game start
        gameState = GameState.Play;
        Application.targetFrameRate = 120;
        mapManager.InitMap();
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // game pause (stop)
        gameState = GameState.Pause;
        Application.targetFrameRate = 30;
    }

    private void GameOver()
    {
        if (_playerPosY > _highScore)
        {
            _highScore = (int)_playerPosY;
            _saveData.highScore = _highScore;
            ChangeJson(_saveData);
        }
        
        Time.timeScale = 0;
        gameState = GameState.End;
        Application.targetFrameRate = 30;
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

    private void CreateJson(JsonData jsonData)
    {
        string saveData = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
        FileStream fileStream =
            new FileStream(string.Format("{0}/{1}.json", Application.dataPath, "SaveData"), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(saveData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    private void ChangeJson(JsonData jsonData)
    {
        string saveData = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", Application.dataPath, "SaveData"),
            FileMode.Open, FileAccess.Write);
        byte[] data = Encoding.UTF8.GetBytes(saveData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
    
    private T LoadJson<T>()
    {
        FileStream fileStream =
            new FileStream(string.Format("{0}/{1}.json", Application.dataPath, "SaveData"), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    public float PlayerPosY
    {
        get { return _playerPosY; }
    }

    public int HighScore
    {
        get { return _highScore; }
    }
}
