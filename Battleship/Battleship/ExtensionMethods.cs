namespace Battleship;

static class ExtensionMethods
{
    public static Position GeneratePosition(this Board targetBoard, Random random)
    {
        return new Position(random.Next(0, targetBoard.Columns), random.Next(0, targetBoard.Rows));
    }
}