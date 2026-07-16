namespace Battleship;

abstract class Ship
{
    // Координаты самой левой верней палубы
    public Position Position { get; }
    public int Length { get; }

    public Ship(Position position, int length)
    {
        Position = position;
        Length = length;
    }

    public abstract bool IsOnPosition(Position position);
}