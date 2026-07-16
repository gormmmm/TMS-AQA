namespace Battleship;

class Game
{
    public void Play(PlayerType playerType, Board board)
    {
        var roundCount = 0;

        IPlayer player = playerType == PlayerType.Computer ? new ComputerPlayer() : new HumanPlayer();
        
        while (true)
        {
            roundCount++;

            var result = player.Shoot(board);

            switch (result)
            {
                case ShootResult.Hit:
                    Console.WriteLine("Hit!");
                    break;
                case ShootResult.Miss:
                    Console.WriteLine("Miss!");
                    break;
                case ShootResult.InvalidShot:
                    Console.WriteLine("Invalid shot!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}