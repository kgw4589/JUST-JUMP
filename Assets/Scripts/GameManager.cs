using System;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;


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

    private JsonData _saveData;

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

        if (!File.Exists(Path.Combine(Application.dataPath, "SaveData.json")))
        {
            CreateJson(new JsonData(0));
        }
        
        Time.timeScale = 0; // game stop
        _saveData = LoadJson<JsonData>();
        _highScore = _saveData.highScore;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }
    
    public void StartGame()
    {
        player = FindObjectOfType<Player>();
        Time.timeScale = 1; // game start
        gameState = GameState.Play;
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // game pause (stop)
        gameState = GameState.Pause;
    }

    private void Update()
    {
        if (gameState != GameState.Play)
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
            JsonData.highScore = _highScore;
            ChangeJson(JsonData);
        }

        gameState = GameState.Ready;*/
    }

    private void CreateJson(JsonData jsonData)
    {
        string saveData = JsonConvert.SerializeObject(jsonData);
        FileStream fileStream =
            new FileStream(string.Format("{0}/{1}.json", Application.dataPath, "SaveData"), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(saveData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    private void ChangeJson(JsonData jsonData)
    {
        string saveData = JsonConvert.SerializeObject(jsonData);
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

    public String PlayerPosY
    {
        get { return _playerPosY.ToString("F3") + " m";  }
    }
    
}
