namespace TMS_1;

public class Player
{
    public string Name { get; }
    public int Score { get; private set; }
    
    public Player(string name)
    {
        Name = name;
        Score = 0;
    }
    
    public void AddPoint()
    {
        Score++;
    }
    
    public void ResetScore()
    {
        Score = 0;
    }
}