using System.Collections.Generic;

public class UnitsWrapper
{
    public readonly int RowsCount;
    public readonly int ColumnsCount;
    public readonly List<Unit> Units;

    public UnitsWrapper(
        int rowsCount,
        int columnsCount,
        List<Unit> units
    )
    {
        RowsCount = rowsCount;
        ColumnsCount = columnsCount;
        Units = units;
    }
}