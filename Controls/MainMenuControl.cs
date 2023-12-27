using Godot;
using System;

public partial class MainMenuControl : CenterContainer
{
	[Signal]
	public delegate void OnNewGameEventHandler();
	[Signal]
	public delegate void OnExitEventHandler();
	[Signal]
	public delegate void OnScoresEventHandler();

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _OnNewGameButtonPressed()
	{
		CallDeferred("emit_signal", "OnNewGame");
	}

	private void _OnExitButtonPressed()
	{
		CallDeferred("emit_signal", "OnExit");
	}

	private void _OnScoresButtonPressed()
	{
		CallDeferred("emit_signal", "OnScores");
	}
}
