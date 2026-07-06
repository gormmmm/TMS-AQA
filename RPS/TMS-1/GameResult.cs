namespace TMS_1;

public class GameResult
{
    public Move PlayerMove { get; }
    public Move ComputerMove { get; }
    public string ResultText { get; }
    
    public GameResult(Move playerMove, Move computerMove, string resultText)
    {
        PlayerMove = playerMove;
        ComputerMove = computerMove;
        ResultText = resultText;
    }
    
    public void Print()
    {
        Console.WriteLine($"You chose {PlayerMove.Name}");
        Console.WriteLine($"Computer chose {ComputerMove.Name}");
        Console.WriteLine($"--- {ResultText} ---");
    }
}