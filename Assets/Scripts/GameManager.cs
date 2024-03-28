using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    
    [SerializeField]
    private SoundManager _soundManager;
    
    [SerializeField]
    private UIManager _uiManager;
    
    [SerializeField]
    private int _highScore;
    
    private float _playerPosY;
    private bool _isPlay;
    
    private void Awake()
    {
        _highScore = PlayerPrefs.GetInt("highScore", 0);
    }

    private void Update()
    {
        if (_isPlay)
        {
            InGamePlay();
            return;
        }
    }

    public void StartGame()
    {
        _isPlay = true;
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
            
        _isPlay = false;*/
    }
    
}
