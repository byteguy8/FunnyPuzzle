using System;
using Godot;

public partial class FunnyPuzzleNode : Node
{
	private Label PointsLabel;
	private Label SpeedLabel;
	private Label PlayingTime;
	private Container ScoresContainer;
	private Container GameOverContainer;
	private Container MainMenuContainer;
	private Container PauseMenuContainer;
	private Container InformationContainer;
	private PackedScene TableScene = null;
	private TableNode TableNode = null;
	private FunnyPuzzleState FunnyPuzzleState = null;
	private AudioStreamPlayer GameAudioPlayer = null;
	private AudioStreamPlayer GameOverPlayer = null;
	private float GameAudioPosition = 0f;

	public override void _Ready()
	{
		PointsLabel = FindChild("PointsLabel") as Label;
		SpeedLabel = FindChild("SpeedLabel") as Label;
		PlayingTime = FindChild("PlayingTimeLabel") as Label;
		InformationContainer = FindChild("InformationContainer") as Container;
		MainMenuContainer = FindChild("MainMenuContainer") as Container;
		PauseMenuContainer = FindChild("PauseMenuContainer") as Container;
		GameOverContainer = FindChild("GameOverContainer") as Container;
		ScoresContainer = FindChild("ScoresContainer") as Container;

		GameAudioPlayer = FindChild("GameAudioPlayer") as AudioStreamPlayer;
		GameOverPlayer = FindChild("GameOverPlayer") as AudioStreamPlayer;

		FunnyPuzzleState = GetNode<FunnyPuzzleState>("/root/FunnyPuzzleState");
		FunnyPuzzleState.TopMargin = InformationContainer.Size.Y;

		TableScene = ResourceLoader.Load<PackedScene>("res://TableScene.tscn");

		// Containers
		FunnyPuzzleState.EnvMachineState.AddStateListener(state =>
		{
			ScoresContainer.Visible = false;
			GameOverContainer.Visible = false;
			MainMenuContainer.Visible = false;
			PauseMenuContainer.Visible = false;

			if (state == EnvFiniteStateMachine.State.MAIN_MENU)
			{
				MainMenuContainer.Visible = true;
			}

			if (state == EnvFiniteStateMachine.State.SCORES)
			{
				ScoresContainer.Visible = true;
			}

			if (state == EnvFiniteStateMachine.State.PAUSED)
			{
				PauseMenuContainer.Visible = true;
			}

			if (state == EnvFiniteStateMachine.State.GAME_OVER)
			{
				GameOverContainer.Visible = true;
			}
		}, true);

		// Labels
		FunnyPuzzleState.EnvMachineState.AddStateListener(state =>
		{
			if (state == EnvFiniteStateMachine.State.GAMING)
			{
				PointsLabel.Visible = true;
				SpeedLabel.Visible = true;
				PlayingTime.Visible = true;
			}
			else
			{
				PointsLabel.Visible = false;
				SpeedLabel.Visible = false;
				PlayingTime.Visible = false;
			}
		}, true);

		// Audio
		FunnyPuzzleState.EnvMachineState.AddStateListener(state =>
		{
			if (state == EnvFiniteStateMachine.State.GAMING)
			{
				GameAudioPlayer.Play();
				GameAudioPlayer.Seek(GameAudioPosition);
			}
			else if (state == EnvFiniteStateMachine.State.PAUSED)
			{
				GameAudioPosition = GameAudioPlayer.GetPlaybackPosition();
				GameAudioPlayer.Stop();
			}
			else
			{
				GameAudioPlayer.Stop();
			}

			if (state == EnvFiniteStateMachine.State.GAME_OVER)
			{
				GameOverPlayer.Play();
			}
			else
			{
				GameOverPlayer.Stop();
			}
		}, true);

		UpdateSpeed();

		FunnyPuzzleState.EnvMachineState.OnMainMenu();
	}

	public override void _Process(double delta)
	{
		UpdatePlayingTime();
	}

	public override void _Input(InputEvent @event)
	{
		var canProcess = FunnyPuzzleState.EnvMachineState.GetState() == EnvFiniteStateMachine.State.GAMING ||
		FunnyPuzzleState.EnvMachineState.GetState() == EnvFiniteStateMachine.State.PAUSED;

		if (@event.IsActionPressed("Pause") && canProcess)
		{
			if (FunnyPuzzleState.EnvMachineState.GetState() == EnvFiniteStateMachine.State.PAUSED)
			{
				FunnyPuzzleState.EnvMachineState.OnResume();
			}
			else
			{
				FunnyPuzzleState.EnvMachineState.OnPause();
			}
		}
	}

	public void _OnRowsDeleted(int rowsCount)
	{
		if (rowsCount == 1)
		{
			UpdatePoints(100);
		}
		else if (rowsCount == 2)
		{
			UpdatePoints(300);
		}
		else if (rowsCount == 3)
		{
			UpdatePoints(600);
		}
		else if (rowsCount == 4)
		{
			UpdatePoints(1000);
		}
		else if (rowsCount >= 5)
		{
			UpdatePoints(1500);
		}

		UpdateSpeed();
	}

	public void _OnGameOver()
	{
		// Updating machine state
		FunnyPuzzleState.EnvMachineState.OnGameOver();

		FunnyPuzzleState.SaveScore(FunnyPuzzleState.Points);
	}

	public void _OnNewGame()
	{
		GameAudioPlayer.Stop();
		GameAudioPosition = 0f;

		TableNode = TableScene.Instantiate<TableNode>();

		TableNode.Connect("OnRowsDeleted", new Callable(this, "_OnRowsDeleted"));
		TableNode.Connect("OnGameOver", new Callable(this, "_OnGameOver"));

		GetTree().Root.AddChild(TableNode);
		GetTree().CurrentScene = TableNode;

		// Updating machine state
		FunnyPuzzleState.EnvMachineState.OnNewGame();
	}

	public void _OnRetry()
	{
		RestartGame();

		// Updating machine state
		FunnyPuzzleState.EnvMachineState.OnNewGame();
	}

	public void _OnContinue()
	{
		// Updating machine state
		FunnyPuzzleState.EnvMachineState.OnResume();
	}

	public void _OnRestartGame()
	{
		GameAudioPlayer.Stop();
		GameAudioPosition = 0f;

		RestartGame();

		// Updating machine state
		FunnyPuzzleState.EnvMachineState.OnNewGame();
	}

	public void _OnMainMenu()
	{
		FreeTable();

		// Updating machine state
		FunnyPuzzleState.EnvMachineState.OnMainMenu();
	}

	public void _OnExit()
	{
		// Updating machine state
		FunnyPuzzleState.EnvMachineState.OnExit();

		GetTree().Quit();
	}

	public void _OnScores()
	{
		// Updating machine state
		FunnyPuzzleState.EnvMachineState.OnScores();
	}

	private void UpdatePoints(int poins)
	{
		FunnyPuzzleState.Points += poins;
		PointsLabel.Text = $"Points: {FunnyPuzzleState.Points}";
	}

	private void UpdatePlayingTime()
	{
		PlayingTime.Text = $"Playing Time: {Math.Round(FunnyPuzzleState.PlayingTime, 0)}";
	}

	private void UpdateSpeed()
	{
		SpeedLabel.Text = $"Speed: {FunnyPuzzleState.GetSpeed()}";
	}

	private void RestartPoints()
	{
		FunnyPuzzleState.Points = 0;
		PointsLabel.Text = $"Points: {FunnyPuzzleState.Points}";
	}

	private void RestartPlayingTime()
	{
		FunnyPuzzleState.PlayingTime = 0;
	}

	private void FreeTable()
	{
		if (TableNode != null)
		{
			TableNode.QueueFree();
			TableNode = null;

			GetTree().CurrentScene = this;
		}
	}

	private void RestartGame()
	{
		RestartPoints();
		UpdateSpeed();
		RestartPlayingTime();
		TableNode.RestartTable();
	}
}
