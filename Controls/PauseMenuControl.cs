using Godot;
using System;

public partial class PauseMenuControl : CenterContainer
{
	[Signal]
	public delegate void OnContinueEventHandler();
	[Signal]
	public delegate void OnExitEventHandler();
	[Signal]
	public delegate void OnRestartEventHandler();
	[Signal]
	public delegate void OnMainMenuEventHandler();

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	private void _OnContinueButtonPressed()
	{
		CallDeferred("emit_signal", "OnContinue");
	}

	private void _OnRestartButtonPressed()
	{
		CallDeferred("emit_signal", "OnRestart");
	}

	private void _OnMainMenuButtonPressed()
	{
		CallDeferred("emit_signal", "OnMainMenu");
	}

	private void _OnExitButtonPressed()
	{
		CallDeferred("emit_signal", "OnExit");
	}
}
