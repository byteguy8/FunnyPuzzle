using System.Collections.Generic;
using Godot;

public class UnitsUtil
{
    private static List<Unit> GenerateIUnits(int row, int column, int rotation)
    {
        var builder = new UnitsBuilder(row, column);

        if (rotation == 0)
        {
            builder.AddTop();
            builder.AddTop();
            builder.ToLeader();
            builder.AddBottom();
        }
        else if (rotation == 1)
        {
            builder.AddRight();
            builder.AddRight();
            builder.ToLeader();
            builder.AddLeft();
        }
        else if (rotation == 2)
        {
            builder.AddBottom();
            builder.AddBottom();
            builder.ToLeader();
            builder.AddTop();
        }
        else
        {
            builder.AddLeft();
            builder.AddLeft();
            builder.ToLeader();
            builder.AddRight();
        }

        return builder.Build();
    }

    private static List<Unit> GenerateJUnits(int row, int column, int rotation)
    {
        var builder = new UnitsBuilder(row, column);

        if (rotation == 0)
        {
            builder.AddTop();
            builder.ToLeader();
            builder.AddBottom();
            builder.AddLeft();
        }
        else if (rotation == 1)
        {
            builder.AddRight();
            builder.ToLeader();
            builder.AddLeft();
            builder.AddTop();
        }
        else if (rotation == 2)
        {
            builder.AddBottom();
            builder.ToLeader();
            builder.AddTop();
            builder.AddRight();
        }
        else
        {
            builder.AddLeft();
            builder.ToLeader();
            builder.AddRight();
            builder.AddBottom();
        }

        return builder.Build();
    }

    private static List<Unit> GenerateLUnits(int row, int column, int rotation)
    {
        var builder = new UnitsBuilder(row, column);

        if (rotation == 0)
        {
            builder.AddTop();
            builder.ToLeader();
            builder.AddBottom();
            builder.AddRight();
        }
        else if (rotation == 1)
        {
            builder.AddRight();
            builder.ToLeader();
            builder.AddLeft();
            builder.AddBottom();
        }
        else if (rotation == 2)
        {
            builder.AddBottom();
            builder.ToLeader();
            builder.AddTop();
            builder.AddLeft();
        }
        else
        {
            builder.AddLeft();
            builder.ToLeader();
            builder.AddRight();
            builder.AddTop();
        }

        return builder.Build();
    }

    private static List<Unit> GenerateOUnits(int row, int column)
    {
        var builder = new UnitsBuilder(row, column);

        builder.AddRight();
        builder.AddBottom();
        builder.AddLeft();

        return builder.Build();
    }

    private static List<Unit> GenerateSUnits(int row, int column, int rotation)
    {
        var builder = new UnitsBuilder(row, column);

        if (rotation == 0)
        {
            builder.AddTop();
            builder.AddRight();
            builder.ToLeader();
            builder.AddLeft();
        }
        else if (rotation == 1)
        {
            builder.AddRight();
            builder.AddBottom();
            builder.ToLeader();
            builder.AddTop();
        }
        else if (rotation == 2)
        {
            builder.AddBottom();
            builder.AddLeft();
            builder.ToLeader();
            builder.AddRight();
        }
        else
        {
            builder.AddLeft();
            builder.AddTop();
            builder.ToLeader();
            builder.AddBottom();
        }

        return builder.Build();
    }

    private static List<Unit> GenerateTUnits(int row, int column, int rotation)
    {
        var builder = new UnitsBuilder(row, column);

        if (rotation == 0)
        {
            builder.AddTop();
            builder.ToLeader();
            builder.AddRight();
            builder.ToLeader();
            builder.AddLeft();
        }
        else if (rotation == 1)
        {
            builder.AddRight();
            builder.ToLeader();
            builder.AddTop();
            builder.ToLeader();
            builder.AddBottom();
        }
        else if (rotation == 2)
        {
            builder.AddBottom();
            builder.ToLeader();
            builder.AddLeft();
            builder.ToLeader();
            builder.AddRight();
        }
        else
        {
            builder.AddLeft();
            builder.ToLeader();
            builder.AddTop();
            builder.ToLeader();
            builder.AddBottom();
        }

        return builder.Build();
    }

    private static List<Unit> GenerateZUnits(int row, int column, int rotation)
    {
        var builder = new UnitsBuilder(row, column);

        if (rotation == 0)
        {
            builder.AddTop();
            builder.AddLeft();
            builder.ToLeader();
            builder.AddRight();
        }
        else if (rotation == 1)
        {
            builder.AddRight();
            builder.AddTop();
            builder.ToLeader();
            builder.AddBottom();
        }
        else if (rotation == 2)
        {
            builder.AddBottom();
            builder.AddRight();
            builder.ToLeader();
            builder.AddLeft();
        }
        else
        {
            builder.AddLeft();
            builder.AddBottom();
            builder.ToLeader();
            builder.AddTop();
        }

        return builder.Build();
    }

    public static List<Vector2> UnitsToVectors(
        int cellSize,
        float fromX,
        float fromY,
        List<Unit> units
    )
    {
        List<Vector2> vectors = new List<Vector2>();

        for (int i = 0; i < units.Count; i++)
        {
            var unit = units[i];

            var vector = new Vector2(
                unit.Column * cellSize + fromX,
                unit.Row * cellSize + fromY
            );

            vectors.Add(vector);
        }

        return vectors;
    }

    public static string GetBlockColor(UnitsType type)
    {
        if (type == UnitsType.IBLOCK)
        {
            return "#00fa9a";
        }
        else if (type == UnitsType.LBLOCK)
        {
            return "#9370db";
        }
        else if (type == UnitsType.JBLOCK)
        {
            return "#ff6347";
        }
        else if (type == UnitsType.TBLOCK)
        {
            return "#008080";
        }
        else if (type == UnitsType.OBLOCK)
        {
            return "#c71585";
        }
        else if (type == UnitsType.SBLOCK)
        {
            return "#ffd700";
        }
        else
        {
            return "#48d1cc";
        }
    }

    public static UnitsWrapper GenerateGenericUnits(UnitsType type)
    {
        var builder = new GridUnitsBuilder();

        if (type == UnitsType.IBLOCK)
        {
            builder.UnitAt(0, 0);
            builder.UnitAt(1, 0);
            builder.UnitAt(2, 0);
            builder.UnitAt(3, 0);
        }
        else if (type == UnitsType.LBLOCK)
        {
            builder.UnitAt(0, 0);
            builder.UnitAt(1, 0);
            builder.UnitAt(2, 0);
            builder.UnitAt(2, 1);
        }
        else if (type == UnitsType.JBLOCK)
        {
            builder.UnitAt(0, 1);
            builder.UnitAt(1, 1);
            builder.UnitAt(2, 1);
            builder.UnitAt(2, 0);
        }
        else if (type == UnitsType.TBLOCK)
        {
            builder.UnitAt(0, 1);
            builder.UnitAt(1, 0);
            builder.UnitAt(1, 1);
            builder.UnitAt(1, 2);
        }
        else if (type == UnitsType.OBLOCK)
        {
            builder.UnitAt(0, 0);
            builder.UnitAt(0, 1);
            builder.UnitAt(1, 0);
            builder.UnitAt(1, 1);
        }
        else if (type == UnitsType.SBLOCK)
        {
            builder.UnitAt(1, 0);
            builder.UnitAt(1, 1);
            builder.UnitAt(0, 1);
            builder.UnitAt(0, 2);
        }
        else if (type == UnitsType.ZBLOCK)
        {
            builder.UnitAt(0, 0);
            builder.UnitAt(0, 1);
            builder.UnitAt(1, 1);
            builder.UnitAt(1, 2);
        }

        return builder.BuildWrapper();
    }

    public static List<Unit> GenerateUnits(
        UnitsType type,
        int row,
        int column,
        int rotation
    )
    {
        if (type == UnitsType.IBLOCK)
        {
            return GenerateIUnits(row, column, rotation);
        }
        else if (type == UnitsType.JBLOCK)
        {
            return GenerateJUnits(row, column, rotation);
        }
        else if (type == UnitsType.LBLOCK)
        {
            return GenerateLUnits(row, column, rotation);
        }
        else if (type == UnitsType.OBLOCK)
        {
            return GenerateOUnits(row, column);
        }
        else if (type == UnitsType.SBLOCK)
        {
            return GenerateSUnits(row, column, rotation);
        }
        else if (type == UnitsType.TBLOCK)
        {
            return GenerateTUnits(row, column, rotation);
        }
        else
        {
            return GenerateZUnits(row, column, rotation);
        }
    }

    public static UnitsType UnitsTypeByNumberType(int block)
    {
        if (block == 1)
        {
            return UnitsType.IBLOCK;
        }
        else if (block == 2)
        {
            return UnitsType.JBLOCK;
        }
        else if (block == 3)
        {
            return UnitsType.LBLOCK;
        }
        else if (block == 4)
        {
            return UnitsType.OBLOCK;
        }
        else if (block == 5)
        {
            return UnitsType.SBLOCK;
        }
        else if (block == 6)
        {
            return UnitsType.TBLOCK;
        }
        else
        {
            return UnitsType.ZBLOCK;
        }
    }
}