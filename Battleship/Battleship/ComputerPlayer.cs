namespace Battleship;

class ComputerPlayer : IPlayer
{
    public ShootResult Shoot(Board targetBoard)
    {
        var random = new Random();

        var shotPosition = targetBoard.GeneratePosition(random);

        return targetBoard.HasShip(shotPosition) ? ShootResult.Hit : ShootResult.Miss;
    }

    public void WriteName()
    {
        Console.WriteLine("Robot");
    }
}