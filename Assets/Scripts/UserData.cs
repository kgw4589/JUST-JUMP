using System.Collections.Generic;

public class UserData
{
    // private string userName;
    
    public float easyHighScore;

    public float normalHighScore;

    public float hardHighScore;
    
    public int coin;

    public bool isFirstGame;
    
    public List<int> unlockCharacters;
    
    public UserData()
    {
        easyHighScore = 0;
        normalHighScore = 0;
        hardHighScore = 0;
        coin = 1000;
        unlockCharacters = new List<int>();
        isFirstGame = true;
    }

    public UserData(float easyHighScore, float normalHighScore, float hardHighScore, int coin, List<int> unlockCharacters,bool isFirstGame, float bgmVolume, float sfxVolume)
    {
        this.easyHighScore = easyHighScore;
        this.normalHighScore = normalHighScore;
        this.hardHighScore = hardHighScore;
        this.coin = coin;
        this.unlockCharacters = unlockCharacters;
        this.isFirstGame = isFirstGame;
    }
}
