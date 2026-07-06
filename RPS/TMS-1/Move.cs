namespace TMS_1;

public class Move
{
    private readonly int _moveValue;
    
    public Move(int moveValue)
    {
        _moveValue = moveValue;
    }
    
    public int Number => _moveValue;
    
    public string Name
    {
        get
        {
            switch (_moveValue)
            {
                case 1: return "Rock";
                case 2: return "Paper";
                case 3: return "Scissors";
                case 4: return "Well";
                default: return "Unknown";
            }
        }
    }
    
    public bool IsValid()
    {
        return _moveValue >= 1 && _moveValue <= 3;
    }
    
    public static Move ReadFromConsole()
    {
        string userInput = Console.ReadLine();
        
        if (int.TryParse(userInput, out int choice))
        {
            return new Move(choice);
        }
        
        return new Move(-1);
    }
    
    public static Move GenerateRandom()
    {
        var random = new Random();
        int choice = random.Next(1, 4);
        
        return new Move(choice);
    }
}


