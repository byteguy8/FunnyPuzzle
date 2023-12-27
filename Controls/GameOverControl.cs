using Godot;
using System;

public partial class GameOverControl : CenterContainer
{
	[Signal]
	public delegate void OnRetryEventHandler();

	[Signal]
	public delegate void OnExitEventHandler();

	[Signal]
	public delegate void OnMainMenuEventHandler();

	private void _OnRetryButtonPressed()
	{
		CallDeferred("emit_signal", "OnRetry");
	}


	private void _OnExitButtonPressed()
	{
		CallDeferred("emit_signal", "OnExit");
	}


	private void _OnMainMenuButtonPressed()
	{
		CallDeferred("emit_signal", "OnMainMenu");
	}
}
