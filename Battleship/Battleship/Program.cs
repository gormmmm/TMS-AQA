using System.Linq;

class Program
{
    public static void Main()
    {
        try
        {
            var shipPosition = new Position(2, 1);
            var ship = new Ship(shipPosition, 2); 
            var board = new Board(5, 5, ship);
            var game = new Game();
            game.Play(board);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка запуска игры: {ex.Message}");
        }
    }
}

class Position
{
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y)
    {
        if (x < 0 || y < 0)
            throw new ArgumentException("Координаты не могут быть отрицательными");

        X = x;
        Y = y;
    }
}

class Ship
{
    public Position Position { get; } 
    public int Length { get; } 

    public Ship(Position position, int length)
    {
        if (length <= 0)
            throw new ArgumentException("Длина корабля должна быть больше нуля.");

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
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Размеры доски должны быть больше нуля.");

        if (ship.Position.X < 0 || ship.Position.Y < 0 || 
            ship.Position.X + ship.Length > columns || 
            ship.Position.Y >= rows)
        {
            throw new ArgumentException("Корабль выходит за пределы игрового поля.");
        }

        Rows = rows;
        Columns = columns;
        Ship = ship;
    }

    public bool IsInside(Position position)
    {
        return position.X >= 0 && position.X < Columns && position.Y >= 0 && position.Y < Rows;
    }

    public Ship? FindShip(Position position)
    {
        if (position.Y == Ship.Position.Y && position.X >= Ship.Position.X &&
            position.X < Ship.Position.X + Ship.Length)
        {
            return Ship;
        }
        return null; // промах
    }

    public bool HasShip(Position position)
    {
        return FindShip(position) != null;
    }
}

class Game
{
    private readonly Random _random = new Random();
    
    // коллекция для хранения истории шотс
    public List<Shot> Shots { get; } = new List<Shot>();

    public int PlayerHits { get; private set; }
    public int ComputerHits { get; private set; }
    
    public void Play(Board board)
    {
        var opponentBoard = GenerateOpponentBoard(board.Rows, board.Columns);
        var roundCount = 0;
        
        PlayerHits = 0;
        ComputerHits = 0;
        
        while (true)
        {
            roundCount++;
            
            // ходит игрок
            bool playerShotSuccess = false;
            while (!playerShotSuccess)
            {
                var shotPosition = GetValidPlayerShot(opponentBoard, roundCount);

                try
                {
                    // стреляем
                    var shot = ExecuteShot(opponentBoard, shotPosition);
                    playerShotSuccess = true;

                    if (shot.HitShip != null)
                    {
                        Console.WriteLine("Попадание!");
                        PlayerHits++;
                    }
                    else
                    {
                        Console.WriteLine("Промах!");
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message} Попробуйте другие координаты.\n");
                }
            }

            // Ходит компьютер
            MakeComputerMove(board);

            // Выводим счет
            Console.WriteLine($"\n--- Конец раунда {roundCount} ---");
            Console.WriteLine($"Счет -> Игрок: {PlayerHits} | Компьютер: {ComputerHits}");
            
            // Выводим стату
            PrintBoardStats("Доска Компьютера (твои выстрелы)", opponentBoard);
            PrintBoardStats("Твоя доска (выстрелы Компьютера)", board);
            Console.WriteLine("------------------------\n");
        }
    }
    
    private Shot ExecuteShot(Board targetBoard, Position position)
    {
        // проверяем выстрел по истории
        foreach (var existingShot in Shots)
        {
            if (existingShot.TargetBoard == targetBoard && 
                existingShot.Position.X == position.X && 
                existingShot.Position.Y == position.Y)
            {
                throw new InvalidOperationException("Выстрел по этим координатам уже был сделан.");
            }
        }
        
        var hitShip = targetBoard.FindShip(position);
        var shot = new Shot(targetBoard, position, hitShip);
        Shots.Add(shot);

        return shot;
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
                Console.WriteLine("Неверные координаты - за пределами доски. Попробуй еще раз.\n");
                continue; 
            }

            return shotPosition; 
        }
    }
    
    private void MakeComputerMove(Board playerBoard)
    {
        bool compShotSuccess = false;
        while (!compShotSuccess)
        {
            int compX = _random.Next(0, playerBoard.Columns);
            int compY = _random.Next(0, playerBoard.Rows);
            var compShotPosition = new Position(compX, compY);

            try
            {
                // Компьютер тоже использует ExecuteShot
                var shot = ExecuteShot(playerBoard, compShotPosition);
                compShotSuccess = true; // Выстрел успешен

                Console.WriteLine($"Computer shoots at X: {compX}, Y: {compY}");

                if (shot.HitShip != null)
                {
                    Console.WriteLine("Computer Hit!");
                    ComputerHits++;
                }
                else
                {
                    Console.WriteLine("Computer Miss!");
                }
            }
            catch (InvalidOperationException)
            {
                // Молча ловим ошибку дубликата. 
                // Цикл while (!compShotSuccess) заставит генератор выдать новые координаты.
            }
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
        Console.WriteLine($"Введи координату {coordinateName} для раунда {roundCount}:");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out coordinate))
        {
            Console.WriteLine("Неверный ввод");
            return false;
        }
        
        return true;
    }
    
    private void PrintBoardStats(string title, Board board)
    {
        var boardShots = Shots.Where(s => s.TargetBoard == board).ToList();
        
        int totalShots = boardShots.Count;
        int hits = boardShots.Count(s => s.HitShip != null);
        int misses = boardShots.Count(s => s.HitShip == null);
        bool hasMisses = boardShots.Any(s => s.HitShip == null);
        var firstHit = boardShots.FirstOrDefault(s => s.HitShip != null);
        string firstHitStr = firstHit != null ? $"({firstHit.Position.X}, {firstHit.Position.Y})" : "None";
        
        var hitCoords = boardShots
            .Where(s => s.HitShip != null)
            .Select(s => $"({s.Position.X}, {s.Position.Y})")
            .ToList();
        string hitCoordsStr = hitCoords.Any() ? string.Join(", ", hitCoords) : "None";

        // вывод в консоль - англ
        Console.WriteLine($"\n> {title}:");
        Console.WriteLine($"  - Total shots: {totalShots}");
        Console.WriteLine($"  - Hits: {hits}");
        Console.WriteLine($"  - Misses: {misses}");
        Console.WriteLine($"  - Had at least one miss: {hasMisses}");
        Console.WriteLine($"  - First hit: {firstHitStr}");
        Console.WriteLine($"  - Hit coordinates: {hitCoordsStr}");
    }
}

record Shot(Board TargetBoard, Position Position, Ship HitShip);

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