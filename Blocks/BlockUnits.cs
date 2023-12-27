using System.Collections.Generic;
using System.Linq;

public abstract class BlockUnits
{
	public int Id { get; }
	public string Color { get; }
	public Unit Leader { get; }
	public int Rotation { get; set; }
	public List<Unit> Units { get; set; }

	public BlockUnits(int id, int row, int column, string color)
	{
		Id = id;
		Color = color;
		Rotation = 0;
		Leader = new Unit(row, column);
		Units = BuildBlockUnits(row, column, Rotation);
	}

	private void UpdateBlock()
	{
		Units = BuildBlockUnits(Leader.Row, Leader.Column, Rotation);
	}

	private bool CollidesWithUnits(BlockUnits block, List<Unit> units)
	{
		if (block.Id == Id)
		{
			return false;
		}

		for (int bIndex = 0; bIndex < block.Units.Count; bIndex++)
		{
			var bUnit = block.Units[bIndex];

			for (int aIndex = 0; aIndex < units.Count; aIndex++)
			{
				var aUnit = units[aIndex];

				if (aUnit.Collides(bUnit))
				{
					return true;
				}
			}
		}

		return false;
	}

	private int RotateValue(int rotation, int limit)
	{
		if (rotation + 1 > limit)
		{
			return 0;
		}
		else
		{
			return rotation + 1;
		}
	}

	public abstract List<Unit> BuildBlockUnits(int row, int column, int rotation);

	public void MoveDown()
	{
		Leader.Row += 1;
		UpdateBlock();
	}

	public void MoveDown(int rows)
	{
		Leader.Row += rows;
		UpdateBlock();
	}

	public void MoveLeft()
	{
		Leader.Column -= 1;
		UpdateBlock();
	}

	public void MoveRight()
	{
		Leader.Column += 1;
		UpdateBlock();
	}

	public void Rotate()
	{
		Rotation = RotateValue(Rotation, 3);
		UpdateBlock();
	}

	public bool CanMove(TableReference tableReference, int x, int y, int rotation)
	{
		int row = Leader.Row + y;
		int column = Leader.Column + x;

		var units = BuildBlockUnits(row, column, rotation);

		foreach (var unit in units)
		{
			var cantX = unit.Row > tableReference.RowsCount - 1;
			var cantY = unit.Column < 0 || unit.Column > tableReference.ColumnsCount - 1;

			if (cantX || cantY)
			{
				return false;
			}
		}

		foreach (var block in tableReference.BlockNodes)
		{
			if (CollidesWithUnits(block.Block, units))
			{
				return false;
			}
		}

		return true;
	}

	public bool CanMove(TableReference tableReference, int x, int y)
	{
		return CanMove(tableReference, x, y, Rotation);
	}

	public bool CanRotate(TableReference tableReference)
	{
		var rotation = RotateValue(Rotation, 3);
		return CanMove(tableReference, 0, 0, rotation);
	}

	public int CountDownMoves(TableReference tableReference)
	{
		var counter = 0;

		while (true)
		{
			counter++;

			if (!CanMove(tableReference, 0, counter))
			{
				break;
			}
		}

		return counter - 1;
	}

	public List<Unit> ProjectBlock(TableReference tableReference)
	{
		var steps = CountDownMoves(tableReference);
		return BuildBlockUnits(Leader.Row + steps, Leader.Column, Rotation);
	}

	public bool HasUnitInRow(int row)
	{
		for (int i = 0; i < Units.Count; i++)
		{
			var unit = Units[i];

			if (unit.Row == row)
			{
				return true;
			}
		}

		return false;
	}

	public int CountUnitsInRow(int row)
	{
		int counter = 0;

		for (int i = 0; i < Units.Count; i++)
		{
			var unit = Units[i];

			if (unit.Row == row)
			{
				counter++;
			}
		}

		return counter;
	}

	public void RemoveUnitsInRow(int row)
	{
		for (int i = Units.Count - 1; i >= 0; i--)
		{
			var unit = Units[i];

			if (unit.Row == row)
			{
				Units.RemoveAt(i);
			}
		}
	}

	public void MoveDownUnitsAbove(int row, int count)
	{
		for (int i = 0; i < Units.Count; i++)
		{
			var unit = Units[i];

			if (unit.Row < row)
			{
				unit.Row += count;
			}
		}
	}

	public int MinColumn()
	{
		var min = Units[0].Column;

		for (int i = 1; i < Units.Count; i++)
		{
			var unit = Units[i];

			if (unit.Column < min)
			{
				min = unit.Column;
			}
		}

		return min;
	}

	public int MaxColumn()
	{
		var max = Units[0].Column;

		for (int i = 1; i < Units.Count; i++)
		{
			var unit = Units[i];

			if (unit.Column > max)
			{
				max = unit.Column;
			}
		}

		return max;
	}

	public int MinRow()
	{
		var min = Units[0].Row;

		for (int i = 1; i < Units.Count; i++)
		{
			var unit = Units[i];

			if (unit.Row < min)
			{
				min = unit.Row;
			}
		}

		return min;
	}

	public int MaxRow()
	{
		var max = Units[0].Row;

		for (int i = 1; i < Units.Count; i++)
		{
			var unit = Units[i];

			if (unit.Row > max)
			{
				max = unit.Row;
			}
		}

		return max;
	}

	public bool HasUnitIn(int row, int column)
	{
		for (int i = 0; i < Units.Count; i++)
		{
			var unit = Units[i];

			if (unit.Row == row && unit.Column == column)
			{
				return true;
			}
		}

		return false;
	}

	public List<Unit> DivineRayProjection(TableReference tableReference)
	{
		var units = new List<Unit>();

		var minRow = MinRow();
		var minColumn = MinColumn();
		var maxRow = MaxRow();
		var maxColumn = MaxColumn();

		for (int column = minColumn; column <= maxColumn; column++)
		{
			for (int row = minRow; row <= maxRow; row++)
			{
				if (!HasUnitIn(row, column) && !HasUnitIn(row + 1, column) && !HasUnitIn(row + 2, column))
				{
					units.Add(new Unit(row, column));
				}
			}
		}

		var blockProjection = ProjectBlock(tableReference);

		var minBlockRow = blockProjection.Min(b => b.Row) - 1;
		var maxBlockRow = blockProjection.Max(b => b.Row);

		for (int column = minColumn; column <= maxColumn; column++)
		{
			for (int row = maxRow + 1; row < tableReference.RowsCount; row++)
			{
				if (!tableReference.HasUnitIn(Id, row, column))
				{
					units.Add(new Unit(row, column));
				}
			}
		}

		return units;
	}
}
