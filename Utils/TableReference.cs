using System.Collections.Generic;
using Godot;

public class TableReference
{
    public enum Position
    {
        LEFT, CENTER, RIGHT
    }

    public int RowsCount { get; }
    public int ColumnsCount { get; }
    public int CellSize { get; }
    public float XMargin { get; }
    public float YMargin { get; }
    public List<BlockNode> BlockNodes { get; }

    public TableReference(
        int rowsCount,
        int columnsCount,
        int cellSize,
        float xMargin,
        float yMargin
    )
    {
        RowsCount = rowsCount;
        ColumnsCount = columnsCount;
        CellSize = cellSize;
        XMargin = xMargin;
        YMargin = yMargin;
        BlockNodes = new List<BlockNode>();
    }

    public Rect2 Rect()
    {
        return new Rect2(
            CellPosition(0, 0),
            new Vector2(CellSize * ColumnsCount, CellSize * RowsCount)
        );
    }

    public Vector2 CellPosition(int row, int column)
    {
        return new Vector2(column * CellSize + XMargin, row * CellSize + YMargin);
    }

    public Vector2 CellPosition(Unit unit)
    {
        return CellPosition(unit.Row, unit.Column);
    }

    public Vector2 VectorCellSize()
    {
        return new Vector2(CellSize, CellSize);
    }

    public bool IsGameOver()
    {
        for (int i = 0; i < BlockNodes.Count; i++)
        {
            var block = BlockNodes[i];

            if (block.HasUnitInRow(-1))
            {
                return true;
            }
        }

        return false;
    }

    public int FindTopRow()
    {
        for (int row = RowsCount - 1; row >= 0; row--)
        {
            for (int nodeIndex = 0; nodeIndex < BlockNodes.Count; nodeIndex++)
            {
                var node = BlockNodes[nodeIndex];

                if (!node.Block.HasUnitInRow(row))
                {
                    return row - 1;
                }
            }
        }

        return -1;
    }

    public bool HasUnitIn(int fromId, int row, int column)
    {
        for (int nodeIndex = 0; nodeIndex < BlockNodes.Count; nodeIndex++)
        {
            var node = BlockNodes[nodeIndex];

            if (fromId == node.BlockUnitsId)
            {
                continue;
            }

            var block = node.Block;

            if (block.HasUnitIn(row, column))
            {
                return true;
            }
        }

        return false;
    }

    public int CountUnitsInRow(int row)
    {
        int count = 0;

        for (int i = 0; i < BlockNodes.Count; i++)
        {
            var block = BlockNodes[i];

            count += block.CountUnitsInRow(row);
        }

        return count;
    }

    public void RemoveUnitsInRow(int row)
    {
        for (int i = BlockNodes.Count - 1; i >= 0; i--)
        {
            var block = BlockNodes[i];

            if (block.RemoveUnitsInRow(row))
            {
                BlockNodes.RemoveAt(i);
            }
        }
    }

    public void MoveDownUnitsAboveRow(int row)
    {
        for (int i = 0; i < BlockNodes.Count; i++)
        {
            var block = BlockNodes[i];
            block.MoveDownUnitsAbove(row, 1);
        }
    }

    public int CheckAndDeleteRows()
    {
        var deletedRowsCount = 0;
        var row = RowsCount - 1;

        while (true)
        {
            var count = CountUnitsInRow(row);

            if (count == 0)
            {
                break;
            }
            else
            {
                if (count == ColumnsCount)
                {
                    RemoveUnitsInRow(row);
                    MoveDownUnitsAboveRow(row);

                    deletedRowsCount++;

                    row = RowsCount - 1;
                }
                else
                {
                    row--;
                }
            }
        }

        return deletedRowsCount;
    }

    public void Restart()
    {
        for (int i = 0; i < BlockNodes.Count; i++)
        {
            var node = BlockNodes[i];
            node.QueueFree();
        }

        BlockNodes.Clear();
    }

    public static TableReference Create(
        Rect2 viewport,
        Vector2 dimension,
        int columnsCount,
        Position position = Position.LEFT
    )
    {
        int cellSize = (int)(dimension.X / columnsCount);
        int rowsCount = (int)(dimension.Y / cellSize);

        float xMargin;
        float yMargin;

        if (position == Position.LEFT)
        {
            xMargin = viewport.Position.X;
            yMargin = viewport.Position.Y + ((viewport.Size.Y - (rowsCount * cellSize)) / 2f);
        }
        else if (position == Position.CENTER)
        {
            xMargin = viewport.Position.X + ((viewport.Size.X - (columnsCount * cellSize)) / 2f);
            yMargin = viewport.Position.Y + ((viewport.Size.Y - (rowsCount * cellSize)) / 2f);
        }
        else
        {
            xMargin = viewport.Position.X + (viewport.Size.X - (columnsCount * cellSize));
            yMargin = viewport.Position.Y + ((viewport.Size.Y - (rowsCount * cellSize)) / 2f);
        }

        return new TableReference(
            rowsCount,
            columnsCount,
            cellSize,
            xMargin,
            yMargin
        );
    }

    public static TableReference Create(
        Rect2 viewport,
        Vector2 dimension,
        int rowsCount,
        int columnsCount,
        Position position = Position.LEFT
    )
    {
        int rowCellSize = (int)(dimension.Y / rowsCount);
        int columnCellSize = (int)(dimension.X / columnsCount);

        int cellSize;

        if (rowCellSize > columnCellSize)
        {
            cellSize = columnCellSize;
        }
        else
        {
            cellSize = rowCellSize;
        }

        float xMargin;
        float yMargin;

        if (position == Position.LEFT)
        {
            xMargin = viewport.Position.X;
            yMargin = viewport.Position.Y + ((viewport.Size.Y - (rowsCount * cellSize)) / 2f);
        }
        else if (position == Position.CENTER)
        {
            xMargin = viewport.Position.X + ((viewport.Size.X - (columnsCount * cellSize)) / 2f);
            yMargin = viewport.Position.Y + ((viewport.Size.Y - (rowsCount * cellSize)) / 2f);
        }
        else
        {
            xMargin = viewport.Position.X + (viewport.Size.X - (columnsCount * cellSize));
            yMargin = viewport.Position.Y + ((viewport.Size.Y - (rowsCount * cellSize)) / 2f);
        }

        return new TableReference(
            rowsCount,
            columnsCount,
            cellSize,
            xMargin,
            yMargin
        );
    }
}