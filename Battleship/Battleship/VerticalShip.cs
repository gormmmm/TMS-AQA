namespace Battleship;

class VerticalShip : Ship
{
    public VerticalShip(Position position, int length) : base(position, length)
    {
        Console.WriteLine("Vertical ship created!");
    }
    
    public override bool IsOnPosition(Position shotPosition)
    {
        return shotPosition.X == Position.X && shotPosition.Y >= Position.Y && shotPosition.Y < Position.Y + Length;
    }
}