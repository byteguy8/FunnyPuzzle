using Godot;

public partial class BlockNode : Node2D
{
	public enum BlockState
	{
		DESCENDING,
		SITTING,
		IN_PLACE
	}

	public int BlockUnitsId = 0;
	private bool Active = true;
	private double LastTime = 0.0;
	public BlockUnits Block { get; }
	private TableReference TableReference;
	public FunnyPuzzleState FunnyPuzzleState = null;
	private BlockState state = BlockState.DESCENDING;
	private EnvFiniteStateMachine.GeneralMachineStateListener GeneralStateListener;

	[Signal]
	public delegate void StateChangeEventHandler();

	public BlockNode(
		TableReference tableReference,
		int id,
		int row,
		int column,
		int type
	)
	{
		BlockUnitsId = id;
		Block = BlockUnitsByNumberType(type, id, row, column);
		TableReference = tableReference;
	}

	public override void _Ready()
	{
		FunnyPuzzleState = GetNode<FunnyPuzzleState>("/root/FunnyPuzzleState");
		GeneralStateListener = FunnyPuzzleState.EnvMachineState.AddStateListener(_OnGameStateChange);

		LastTime = FunnyPuzzleState.GameTime;
	}

	public override void _Process(double delta)
	{
		var isDecending = state == BlockState.DESCENDING;
		var isRunning = FunnyPuzzleState.EnvMachineState.GetState() == EnvFiniteStateMachine.State.GAMING;

		if (!isRunning)
		{
			QueueRedraw();
		}

		if (isRunning && isDecending)
		{
			ProcessBlockNode(delta);
		}
	}

	public override void _Draw()
	{
		if (FunnyPuzzleState.EnvMachineState.GetState() == EnvFiniteStateMachine.State.GAMING)
		{
			for (int i = 0; i < Block.Units.Count; i++)
			{
				var unit = Block.Units[i];

				if (unit.Row < 0)
				{
					continue;
				}

				var rect = new Rect2(
					TableReference.CellPosition(unit.Row, unit.Column),
					TableReference.VectorCellSize()
				);

				DrawRect(rect, new Color(Block.Color), true);
			}
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (FunnyPuzzleState.EnvMachineState.GetState() == EnvFiniteStateMachine.State.PAUSED)
		{
			return;
		}

		if (@event.IsActionPressed("Rotate"))
		{
			Rotate();
		}
		else if (@event.IsActionPressed("MoveLeft"))
		{
			MoveLeft();
		}
		else if (@event.IsActionPressed("MoveRight"))
		{
			MoveRight();
		}
		else if (@event.IsActionPressed("MoveDown"))
		{
			MoveDownJump();
		}
		else if (@event.IsActionPressed("ToEnd"))
		{
			ToEnd();
		}
	}

	public override void _ExitTree()
	{
		FunnyPuzzleState.EnvMachineState.RemoveListener(GeneralStateListener);
	}

	public void _OnGameStateChange(EnvFiniteStateMachine.State state)
	{
		if (state == EnvFiniteStateMachine.State.GAMING)
		{
			QueueRedraw();
		}
	}

	private void Rotate()
	{
		if (state == BlockState.IN_PLACE)
		{
			return;
		}

		if (Block.CanRotate(TableReference))
		{
			Block.Rotate();
			QueueRedraw();
		}
	}

	private void MoveLeft()
	{
		if (state == BlockState.IN_PLACE)
		{
			return;
		}

		if (Block.CanMove(TableReference, -1, 0))
		{
			Block.MoveLeft();
			QueueRedraw();
		}
	}

	private void MoveRight()
	{
		if (state == BlockState.IN_PLACE)
		{
			return;
		}

		if (Block.CanMove(TableReference, 1, 0))
		{
			Block.MoveRight();
			QueueRedraw();
		}
	}

	private void MoveDown()
	{
		if (state == BlockState.IN_PLACE)
		{
			return;
		}

		if (Block.CanMove(TableReference, 0, 1))
		{
			Block.MoveDown();
			QueueRedraw();
			return;
		}
	}

	private void ToEnd()
	{
		if (state == BlockState.IN_PLACE)
		{
			return;
		}

		var steps = Block.CountDownMoves(TableReference);

		if (steps > 0)
		{
			Block.MoveDown(steps);
			QueueRedraw();
		}
	}

	private void MoveDownJump()
	{
		if (state == BlockState.IN_PLACE)
		{
			return;
		}

		var limit = 3;
		var steps = Block.CountDownMoves(TableReference);

		if (steps == 0)
		{
			return;
		}

		if (steps < limit)
		{
			Block.MoveDown(steps);
		}
		else
		{
			Block.MoveDown(limit);
		}

		QueueRedraw();
	}

	private void ProcessBlockNode(double delta)
	{
		var canMove = Block.CanMove(TableReference, 0, 1);

		if (!canMove)
		{
			state = BlockState.SITTING;

			var timer = new Timer();

			timer.Connect("timeout", Callable.From(() =>
			{
				timer.QueueFree();

				if (Block.CanMove(TableReference, 0, 1))
				{
					state = BlockState.DESCENDING;
					return;
				}

				state = BlockState.IN_PLACE;
				CallDeferred("emit_signal", "StateChange");
			}));

			timer.OneShot = true;
			timer.WaitTime = 0.35;

			AddChild(timer);

			timer.Start();

			return;
		}

		var diff = FunnyPuzzleState.GameTime - LastTime;

		if (diff >= FunnyPuzzleState.GetSpeed())
		{
			LastTime += diff;
			MoveDown();
		}
	}

	private BlockUnits BlockUnitsByNumberType(int block, int id, int row, int column)
	{
		if (block == 1)
		{
			return new IBlockUnits(id, row, column);
		}
		else if (block == 2)
		{
			return new JBlockUnits(id, row, column);
		}
		else if (block == 3)
		{
			return new LBlockUnits(id, row, column);
		}
		else if (block == 4)
		{
			return new OBlockUnits(id, row, column);
		}
		else if (block == 5)
		{
			return new SBlockUnits(id, row, column);
		}
		else if (block == 6)
		{
			return new TBlockUnits(id, row, column);
		}
		else
		{
			return new ZBlockUnits(id, row, column);
		}
	}

	public bool HasUnitInRow(int row)
	{
		return Block.HasUnitInRow(row);
	}

	public int CountUnitsInRow(int row)
	{
		return Block.CountUnitsInRow(row);
	}

	public bool RemoveUnitsInRow(int row)
	{
		Block.RemoveUnitsInRow(row);

		if (Block.Units.Count > 0)
		{
			QueueRedraw();
			return false;
		}
		else
		{
			QueueFree();
			return true;
		}
	}

	public void MoveDownUnitsAbove(int row, int count)
	{
		Block.MoveDownUnitsAbove(row, count);
	}
}
