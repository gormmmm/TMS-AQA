namespace Battleship;

class Program
{
    public static void Main()
    {
        try
        {
            
            // 0 1 2 3 4 
            // ------------X
            // X X X X X 
            // X H H X X   
            // X X V X X 
            // X X V X X 
            // X X X X X 
            // Y
            
            Console.WriteLine("Enter your player type (H for Human, C for Computer): ");
            var playerTypeInput = Console.ReadLine();

            PlayerType player;
            switch (playerTypeInput)
            {
                case "H":
                    player = PlayerType.Human;
                    break;
                case "C":
                    player = PlayerType.Computer;
                    break;
                default:
                    throw new ArgumentException("Invalid player type!");
            }
            
            var settings = new GameSettings(5, 5, player);
            
            var board = new Board(settings.BoardRows, settings.BoardColumns, new Ship[] { new HorizontalShip(new Position(1, 1), 2), 
                new VerticalShip(new Position(2, 2), 2) });
        
            Game game = new Game();

            game.Play(settings.PlayerType, board);
        }
        catch (ShotPositionOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
           Console.WriteLine("Press any key to exit...");
           Console.ReadKey();
        }
    }
}

record GameSettings(int BoardRows, int BoardColumns, PlayerType PlayerType)
{
    public GameSettings() : this(5, 5, PlayerType.Human)
    {
            
    }
    
    public void DoSomething()
    {
        Console.WriteLine("Game settings created!");
    }
}


enum PlayerType { Human, Computer } 