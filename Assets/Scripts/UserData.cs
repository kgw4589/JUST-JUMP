using System.Collections.Generic;

public class UserData
{
    // private string userName;
    
    private float _easyHighScore;

    private float _normalHighScore;

    private float _hardHighScore;
    
    private int _coin;

    private bool _isFirstGame;
    
    private List<int> _unlockCharacters;
    
    public UserData()
    {
        _easyHighScore = 0;
        _normalHighScore = 0;
        _hardHighScore = 0;
        _coin = 0;
        _unlockCharacters = null;
        _isFirstGame = true;
    }

    public UserData(float easyHighScore, float normalHighScore, float hardHighScore, int coin, List<int> unlockCharacters,bool isFirstGame, float bgmVolume, float sfxVolume)
    {
        _easyHighScore = easyHighScore;
        _normalHighScore = normalHighScore;
        _hardHighScore = hardHighScore;
        _coin = coin;
        _unlockCharacters = unlockCharacters;
        _isFirstGame = isFirstGame;
    }

    public float EasyHighScore
    {
        get { return _easyHighScore; }
        set { _easyHighScore = value;  }
    }
    
    public float NormalHighScore
    {
        get { return _normalHighScore; }
        set { _normalHighScore = value;  }
    }

    public float HardHighScore
    {
        get { return _hardHighScore; }
        set { _hardHighScore = value;  }
    }

    public int Coin
    {
        get { return _coin; }
        set { _coin = value; }
    }

    public List<int> UnlockCharacters
    {
        get { return _unlockCharacters; }
        set { _unlockCharacters = value; }
    }

    public bool IsFirstGame
    {
        get { return _isFirstGame; }
        set { _isFirstGame = value; }
    }
}
