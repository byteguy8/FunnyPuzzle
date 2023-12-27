using Godot;
using System;

public partial class ScoresControl : CenterContainer
{
	private Label TitleLabel;
	private FunnyPuzzleState FunnyPuzzleState;
	private ScrollContainer ScrollContainer;
	private VBoxContainer ScoresItemsContainer;

	[Signal]
	public delegate void OnMainMenuEventHandler();

	public override void _Ready()
	{
		FunnyPuzzleState = GetNode<FunnyPuzzleState>("/root/FunnyPuzzleState");

		TitleLabel = GetNode<Label>("ControlsContainer/TitleLabel");
		ScrollContainer = GetNode<ScrollContainer>("ControlsContainer/ScoresScrollContainer");
		ScoresItemsContainer = GetNode<VBoxContainer>("ControlsContainer/ScoresScrollContainer/ScoresItemsContainer");

		ScrollContainer.Set("custom_minimum_size", new Vector2(TitleLabel.Size.X + 300, 250));

		Connect("visibility_changed", Callable.From(_OnVisibilityChange));
	}

	private void _OnVisibilityChange()
	{
		if (!IsVisibleInTree())
		{
			return;
		}

		for (int i = 0; i < ScoresItemsContainer.GetChildCount(); i++)
		{
			ScoresItemsContainer.GetChild(i).QueueFree();
		}

		var scores = FunnyPuzzleState.GetScores();

		scores.Iterate((index, score) =>
		{
			var container = new HBoxContainer
			{
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			};

			var positionLabel = new Label
			{
				Text = $"{index + 1}"
			};

			var playerLabel = new Label
			{
				Text = score.Player,
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			};

			var scoreLabel = new Label
			{
				Text = Convert.ToString(score.Value)
			};

			ScoresItemsContainer.AddChild(container);

			container.AddChild(positionLabel);
			container.AddChild(playerLabel);
			container.AddChild(scoreLabel);
		});
	}

	private void _OnMainMenuButtonPressed()
	{
		CallDeferred("emit_signal", "OnMainMenu");
	}
}
