public class JsonData
{
    // private string userName;
    private int _highScore;
    // private float soundSet;
    
    public JsonData() { }

    public JsonData(int highScore)
    {
        _highScore = highScore;
    }

    public int highScore
    {
        get { return _highScore; }
        set { _highScore = value;  }
    }
}
