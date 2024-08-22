using System.Collections.Generic;

public class UserData
{
    // private string userName;
    
    private int _highScore;

    private int _coin;

    private List<int> _unlockCharacters;
    
    public UserData() { }

    public UserData(int highScore, int coin, List<int> unlockCharacters)
    {
        _highScore = highScore;
        _coin = coin;
        _unlockCharacters = unlockCharacters;
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
}
