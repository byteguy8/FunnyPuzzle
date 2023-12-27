using System;
using Godot;

public partial class FunnyPuzzleState : Node
{
	public class Score : ILimitedOrderedItem<Score>
	{
		public string Player;
		public int Value;

		public int Compare(Score item)
		{
			if (Value < item.Value)
			{
				return -1;
			}
			else if (Value > item.Value)
			{
				return 1;
			}
			else
			{
				return 0;
			}
		}
	}

	public int Points = 0;
	public float TopMargin = 0;
	public double GameTime = 0.0;
	public double PlayingTime = 0.0;
	public readonly EnvFiniteStateMachine EnvMachineState = new EnvFiniteStateMachine();

	public override void _Process(double delta)
	{
		GameTime += delta;

		if (EnvMachineState.GetState() == EnvFiniteStateMachine.State.GAMING)
		{
			PlayingTime += delta;
		}
	}

	public double GetSpeed()
	{
		if (Points >= 0 && Points <= 5000)
		{
			return 0.5;
		}
		else if (Points >= 5001 && Points <= 10000)
		{
			return 0.25;
		}
		else if (Points >= 10001 && Points <= 20000)
		{
			return 0.125;
		}
		else
		{
			return 0.0625;
		}
	}

	public static LimitedOrderedList<Score> GetScores()
	{
		try
		{
			if (!FileAccess.FileExists("user://scores.cfg"))
			{
				return new LimitedOrderedList<Score>(10);
			}

			using var scoreFile = FileAccess.Open(
				"user://scores.cfg",
				FileAccess.ModeFlags.ReadWrite
			);

			var scores = new LimitedOrderedList<Score>(10);

			while (scoreFile.GetPosition() < scoreFile.GetLength())
			{
				var line = scoreFile.GetLine();
				var split = line.Split("=");

				if (split.Length != 2)
				{
					scoreFile.Close();
					DirAccess.RemoveAbsolute("user://scores.cfg");
					return new LimitedOrderedList<Score>(10);
				}

				if (!int.TryParse(split[1], out var score))
				{
					scoreFile.Close();
					DirAccess.RemoveAbsolute("user://scores.cfg");
					return new LimitedOrderedList<Score>(10);
				}

				scores.Add(new Score
				{
					Player = split[0],
					Value = score
				});
			}

			return scores;
		}
		catch (Exception)
		{
			return new LimitedOrderedList<Score>(10);
		}
	}

	public static void SaveScore(int score, string player = "player")
	{
		try
		{
			if (score <= 0)
			{
				return;
			}

			var scores = GetScores();

			using var scoreFile = FileAccess.Open(
				"user://scores.cfg",
				FileAccess.ModeFlags.Write
			);

			scores.Add(new Score
			{
				Player = player,
				Value = score
			});

			scores.Iterate((index, s) =>
			{
				scoreFile.StoreLine($"{s.Player}={s.Value}");
			});
		}
		catch (Exception) { }
	}
}
