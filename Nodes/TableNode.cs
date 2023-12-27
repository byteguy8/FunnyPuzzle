using Godot;
using System.Collections.Generic;

public partial class TableNode : Node2D
{
	private int BlockNodeId = 0;
	private BlockUnits LastBlock = null;
	private TableReference TableReference;
	public FunnyPuzzleState FunnyPuzzleState = null;
	private Queue<int> BlockIndexes = new Queue<int>();
	private RandomNumberGenerator random = new RandomNumberGenerator();
	private List<Unit> LastBlockProjectionUnits = new List<Unit>();
	private Rect2 RightRect;
	private Font MessagesFont;
	private bool ShowDivineRay = false;

	[Signal]
	public delegate void OnRowsDeletedEventHandler(int rowsCount);
	[Signal]
	public delegate void OnGameOverEventHandler();

	public override void _Ready()
	{
		MessagesFont = (Font)GD.Load("res://Hack_Regular.ttf");

		FunnyPuzzleState = GetNode<FunnyPuzzleState>("/root/FunnyPuzzleState");

		var viewportRect = GetViewportRect();

		var playableRect = new Rect2(
			new Vector2(viewportRect.Position.X, viewportRect.Position.Y + FunnyPuzzleState.TopMargin),
			new Vector2(viewportRect.Size.X, viewportRect.Size.Y - FunnyPuzzleState.TopMargin)
		);

		var dimension = new Vector2(playableRect.Size.X, playableRect.Size.Y);

		TableReference = TableReference.Create(
			playableRect,
			dimension,
			20,
			11,
			TableReference.Position.CENTER
		);

		var tableRect = TableReference.Rect();

		var position = new Vector2(
			tableRect.Position.X + tableRect.Size.X,
			tableRect.Position.Y
		);

		var size = new Vector2(
			playableRect.Size.X - position.X,
			playableRect.Size.Y
		);

		RightRect = new Rect2(
			position,
			size
		);

		random.Randomize();

		BlockIndexes.Enqueue(random.RandiRange(1, 7));
		BlockIndexes.Enqueue(random.RandiRange(1, 7));
		BlockIndexes.Enqueue(random.RandiRange(1, 7));

		AddBlockNode();
	}

	public override void _Process(double delta)
	{
		if (FunnyPuzzleState.EnvMachineState.GetState() == EnvFiniteStateMachine.State.PAUSED)
		{
			QueueRedraw();
			return;
		}

		if (LastBlock != null)
		{
			LastBlockProjectionUnits = LastBlock.ProjectBlock(TableReference);
			QueueRedraw();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (FunnyPuzzleState.EnvMachineState.GetState() == EnvFiniteStateMachine.State.GAMING)
		{
			if (@event.IsActionPressed("DivineRay"))
			{
				ShowDivineRay = !ShowDivineRay;
			}
		}
	}

	public override void _Draw()
	{
		if (FunnyPuzzleState.EnvMachineState.GetState() == EnvFiniteStateMachine.State.GAMING)
		{
			DrawTable();
			DrawQueueBlocks();

			if (!ShowDivineRay)
			{
				DrawTableProjection();
			}
			else
			{
				DrawDivineRay();
			}
		}
	}

	private void DrawQueueBlocks()
	{
		var values = BlockIndexes.ToArray();

		var queueRect = Utils.CreateRect(RightRect, 50, 50);
		var cellSize = (int)queueRect.Size.Y / 16;

		var fromY = cellSize;
		var totalHeight = cellSize;
		var blocksPackages = new List<dynamic>();

		for (int i = 0; i < values.Length; i++)
		{
			var value = values[i];
			var type = UnitsUtil.UnitsTypeByNumberType(value);
			var wrapper = UnitsUtil.GenerateGenericUnits(type);
			var vectors = UnitsUtil.UnitsToVectors(cellSize, 0, 0, wrapper.Units);

			var width = wrapper.ColumnsCount * cellSize;
			var height = wrapper.RowsCount * cellSize;

			var xMargin = queueRect.Position.X + (queueRect.Size.X / 2f) - (width / 2f);

			var positions = new List<Vector2>();

			for (int o = 0; o < vectors.Count; o++)
			{
				var vector = vectors[o];
				positions.Add(new Vector2(vector.X + xMargin, vector.Y + fromY));
			}

			blocksPackages.Add(new
			{
				Positions = positions,
				Color = new Color(UnitsUtil.GetBlockColor(type))
			});

			fromY += height + cellSize;
			totalHeight += height + cellSize;
		}

		DrawRect(queueRect, new Color("#AACCFF20"), true);

		DrawString(
			MessagesFont,
			queueRect.Position,
			"Next Block",
			alignment: HorizontalAlignment.Center,
			fontSize: 32,
			width: queueRect.Size.X
		);

		var yMargin = queueRect.Position.Y + (queueRect.Size.Y / 2f) - (totalHeight / 2f);

		for (int i = 0; i < blocksPackages.Count; i++)
		{
			var blockPackage = blocksPackages[i];
			var positions = blockPackage.Positions;

			for (int o = 0; o < positions.Count; o++)
			{
				var position = positions[o];

				var rect = new Rect2(
					new Vector2(position.X, position.Y + yMargin),
					new Vector2(cellSize, cellSize)
				);

				DrawRect(rect, blockPackage.Color, true);
			}
		}
	}

	private void DrawTable()
	{
		DrawRect(TableReference.Rect(), new Color("#AACCFF20"), true);

		for (int column = 0; column < TableReference.ColumnsCount; column++)
		{
			for (int row = 0; row < TableReference.RowsCount; row++)
			{
				var rect = new Rect2(
					TableReference.CellPosition(row, column),
					TableReference.VectorCellSize()
				);

				DrawRect(rect, new Color("#ffffff20"), false);
			}
		}
	}

	private void DrawDivineRay()
	{
		var projection = LastBlock.DivineRayProjection(TableReference);

		for (int i = 0; i < projection.Count; i++)
		{
			var unit = projection[i];

			var rect = new Rect2(
				TableReference.CellPosition(unit.Row, unit.Column),
				TableReference.VectorCellSize()
			);

			DrawRect(rect, new Color("#cce1eb50"), true);
		}
	}

	private void DrawTableProjection()
	{
		for (int i = 0; i < LastBlockProjectionUnits.Count; i++)
		{
			var unit = LastBlockProjectionUnits[i];

			var rect = new Rect2(
					TableReference.CellPosition(unit.Row, unit.Column),
					TableReference.VectorCellSize()
				);

			DrawRect(rect, new Color("#cce1eb50"), true);
		}
	}

	private int GetBlockIndex()
	{
		var index = BlockIndexes.Dequeue();

		BlockIndexes.Enqueue(random.RandiRange(1, 7));

		return index;
	}

	private void AddBlockNode()
	{
		BlockNodeId++;

		var blockNode = new BlockNode(
			TableReference,
			BlockNodeId,
			0,
			TableReference.ColumnsCount / 2,
			GetBlockIndex()
		);

		LastBlock = blockNode.Block;
		TableReference.BlockNodes.Add(blockNode);

		blockNode.Connect("StateChange", Callable.From(_OnStateChange));
		CallDeferred("add_child", blockNode);
	}

	public void RestartTable()
	{
		LastBlock = null;
		LastBlockProjectionUnits = new List<Unit>();
		TableReference.Restart();
		random.Randomize();
		AddBlockNode();
	}

	public void _OnStateChange()
	{
		var deletedRowsCount = TableReference.CheckAndDeleteRows();
		CallDeferred("emit_signal", "OnRowsDeleted", deletedRowsCount);

		if (TableReference.IsGameOver())
		{
			CallDeferred("emit_signal", "OnGameOver");
		}
		else
		{
			AddBlockNode();
		}
	}
}
