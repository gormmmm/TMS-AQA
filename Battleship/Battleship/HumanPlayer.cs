namespace Battleship;

class HumanPlayer : IPlayer
{
    public string Name { get; set; }
    
    public ShootResult Shoot(Board targetBoard)
    {
        if (!TryReadFromConsole("X", 0, out var xPosition))
            return ShootResult.InvalidShot;

        Console.WriteLine();

        if (!TryReadFromConsole("Y", 0, out var yPosition))
            return ShootResult.InvalidShot;

        if (xPosition == null || yPosition == null)
            return ShootResult.InvalidShot;

        var shotPosition = new Position(xPosition.Value, yPosition.Value);
        
        if (!targetBoard.IsInside(shotPosition))
            throw new ShotPositionOutOfRangeException("Invalid shot position!");
        
        return targetBoard.HasShip(shotPosition) ? ShootResult.Hit : ShootResult.Miss;
    }
    
    private bool TryReadFromConsole(string coordinateName, int roundCount, out int? coordinate)
    {
        Console.WriteLine($"Input your {coordinateName} coordinate for round {roundCount}:");
        var input = Console.ReadLine();
        coordinate = null;

        //start business operation

        try
        {
            coordinate = int.Parse(input); // ArgumentNullException
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine("Invalid input!");
        }

        return true;
        // if (!int.TryParse(input, out coordinate))
        // {
        //     Console.WriteLine("Invalid input");
        //     return false;
        // }
    }

    public void WriteName()
    {
        Console.WriteLine($"Player: {Name}");
    }
}