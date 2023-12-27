using System.Collections.Generic;

public class GridUnitsBuilder
{
    private int MinRow = 0;
    private int MaxRow = 0;
    private int MinColumn = 0;
    private int MaxColumn = 0;
    private List<Unit> Units;

    public GridUnitsBuilder()
    {
        Units = new List<Unit>();
    }

    private void ComparePositions(int row, int column)
    {
        if (row > MaxRow)
        {
            MaxRow = row;
        }

        if (row < MinRow)
        {
            MinRow = row;
        }

        if (column > MaxColumn)
        {
            MaxColumn = column;
        }

        if (column < MinColumn)
        {
            MinColumn = column;
        }
    }

    public GridUnitsBuilder UnitAt(int row, int column)
    {
        if (Units.Count == 0)
        {
            MinRow = row;
            MaxRow = row;
            MinColumn = column;
            MaxColumn = column;
        }
        else
        {
            ComparePositions(row, column);
        }

        Units.Add(new Unit(row, column));

        return this;
    }

    public List<Unit> BuildList()
    {
        return Units;
    }

    public UnitsWrapper BuildWrapper()
    {
        return new UnitsWrapper(
            MaxRow - MinRow + 1,
            MaxColumn - MinColumn + 1,
            Units
        );
    }
}