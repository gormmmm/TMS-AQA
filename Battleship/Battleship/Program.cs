class Program
{
    public static void Main()
    {
        var shipPosition = new Position(2, 1);

        var ship = new Ship(shipPosition, 2); //x123234

        var board = new Board(5, 5, ship);

        var game = new Game();

        game.Play(board);



        int a = 2;
        int b = a; // взяли 2 из а и скопировали в b
        a = 5;
        //b = 2
        
        Position p = new  Position(1, 1);
        Position p2 = p;

        p.X = 2;
        
        //p2.X == 2
    }
}


class Position
{
    public int X { get; set; }
    public int Y { get; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

class Ship
{
    // Координаты самой левой верней палубы
    public Position Position { get; } //null
    public int Length { get; } //12

    public Ship(Position position, int length)
    {
        Position = position;
        Length = length;
    }
}

class Board
{
    public int Rows { get; }
    public int Columns { get; }

    public Ship Ship { get; }

    public Board(int rows, int columns, Ship ship)
    {
        Rows = rows;
        Columns = columns;
        Ship = ship;
    }

    public bool IsInside(Position position)
    {
        return position.X >= 0 && position.X < Rows && position.Y >= 0 && position.Y < Columns;
    }

    public bool HasShip(Position position)
    {
        return position.Y == Ship.Position.Y && position.X >= Ship.Position.X &&
               position.X < Ship.Position.X + Ship.Length;
    }
}

class Game
{
    private readonly Random _random = new Random();
    
    public int PlayerHits { get; private set; }
    public int ComputerHits { get; private set; }
    public void Play(Board board)
    {
        var opponentBoard = GenerateOpponentBoard(board.Rows, board.Columns);
        var roundCount = 0;
        
        //счетчики
        PlayerHits = 0;
        ComputerHits = 0;
        
        while (true)
        {
            roundCount++;
            // Ходит игрок
            var shotPosition = GetValidPlayerShot(opponentBoard, roundCount);

            if (opponentBoard.HasShip(shotPosition))
            {
                Console.WriteLine("Hit!");
                PlayerHits++;
            }
            else
            {
                Console.WriteLine("Miss!");
            }

            // Ходит компьютер
            MakeComputerMove(board);

            // Выводим счет
            Console.WriteLine($"\n--- End of Round {roundCount} ---");
            Console.WriteLine($"Score -> Player: {PlayerHits} | Computer: {ComputerHits}\n");
        }
    }
    
    private Position GetValidPlayerShot(Board targetBoard, int roundCount)
    {
        while (true)
        {
            if (!TryReadFromConsole("X", roundCount, out var xPosition))
                continue;

            Console.WriteLine();

            if (!TryReadFromConsole("Y", roundCount, out var yPosition))
                continue;

            var shotPosition = new Position(xPosition, yPosition);

            if (!targetBoard.IsInside(shotPosition))
            {
                Console.WriteLine("Invalid shot position! Coordinates are out of board limits. Try again.\n");
                continue; 
            }

            return shotPosition; // Возвращаем позицию только если она полностью корректна
        }
    }
    
    private void MakeComputerMove(Board playerBoard)
    {
        int compX = _random.Next(0, playerBoard.Columns);
        int compY = _random.Next(0, playerBoard.Rows);
        var compShotPosition = new Position(compX, compY);

        Console.WriteLine($"Computer shoots at X: {compX}, Y: {compY}");

        if (playerBoard.HasShip(compShotPosition))
        {
            Console.WriteLine("Computer Hit!");
            ComputerHits++;
        }
        else
        {
            Console.WriteLine("Computer Miss!");
        }
    }
    private Board GenerateOpponentBoard(int rows, int columns)
    {
        int length = _random.Next(1, columns + 1); 
        int y = _random.Next(0, rows);
        int x = _random.Next(0, columns - length + 1);
        
        var shipPosition = new Position(x, y);
        var ship = new Ship(shipPosition, length);
        
        return new Board(rows, columns, ship);
    }

    private bool TryReadFromConsole(string coordinateName, int roundCount, out int coordinate)
    {
        Console.WriteLine($"Input your {coordinateName} coordinate for round {roundCount}:");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out coordinate))
        {
            Console.WriteLine("Invalid input");
            return false;
        }
        
        return true;
    }
}

// Ship
// Board 
// Position
// Game

// Rows = 5

// 0 1 2 3 4 
// ------------X
// X X X X X 
// X X S S X   
// X X X X X 
// X X X X X 
// X X X X X 
// Y