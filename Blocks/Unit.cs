public class Unit
{
    public int Row { get; set; }
    public int Column { get; set; }

    public Unit(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public bool Collides(Unit unit)
    {
        return Row == unit.Row && Column == unit.Column;
    }
}