using System.Collections.Generic;

public class UserData
{
    // private string userName;
    
    private int _highScore;

    private int _coin;

    private bool _isFirstGame;
    
    private List<int> _unlockCharacters;

    public UserData()
    {
        _highScore = 0;
        _coin = 0;
        _unlockCharacters = null;
        _isFirstGame = true;
    }

    public UserData(int highScore, int coin, List<int> unlockCharacters,bool isFirstGame)
    {
        _highScore = highScore;
        _coin = coin;
        _unlockCharacters = unlockCharacters;
        _isFirstGame = isFirstGame;
    }

    public int HighScore
    {
        get { return _highScore; }
        set { _highScore = value;  }
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
