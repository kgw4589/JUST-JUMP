using System;
using System.IO;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;


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
        gameState = GameState.Play;
    }

    public void PauseGame()
    {
        Time.timeScale = 0; // game pause (stop)
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
            PlayerPrefs.SetInt("highScore", _highScore);
        }

        gameState = GameState.Ready;*/
    }

    private void CreateJson(string saveData)
    {
        FileStream fileStream =
            new FileStream(string.Format("{0}/{1}.json", Application.dataPath, "SaveData"), FileMode.Create);
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
