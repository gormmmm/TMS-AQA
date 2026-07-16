namespace Battleship;

interface IShooter
{
    public ShootResult Shoot(Board targetBoard);
}