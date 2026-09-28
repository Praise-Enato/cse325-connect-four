namespace ConnectFour;

/// <summary>
/// Tracks the outcome of every completed game in a series.
///
/// This is the custom feature added on top of the Microsoft Learn tutorial.
/// It is deliberately kept separate from <see cref="GameState"/>: GameState owns a
/// single game, this owns the running tally across games. Registered as a singleton
/// so the score survives a board reset, which is what makes a "series" meaningful.
/// </summary>
public class SeriesScore
{
	/// <summary>Games won by player 1 so far.</summary>
	public int Player1Wins { get; private set; }

	/// <summary>Games won by player 2 so far.</summary>
	public int Player2Wins { get; private set; }

	/// <summary>Games that ended with a full board and no winner.</summary>
	public int Ties { get; private set; }

	/// <summary>Total completed games in the current series.</summary>
	public int GamesPlayed => Player1Wins + Player2Wins + Ties;

	/// <summary>
	/// The result of the most recent completed game, used by the UI to
	/// highlight whichever row just scored.
	/// </summary>
	public GameState.WinState LastResult { get; private set; } = GameState.WinState.No_Winner;

	/// <summary>
	/// Who is ahead in the series: 1, 2, or 0 when it is level.
	/// </summary>
	public int Leader
	{
		get
		{
			if (Player1Wins > Player2Wins) return 1;
			if (Player2Wins > Player1Wins) return 2;
			return 0;
		}
	}

	/// <summary>
	/// A short human-readable summary of who is ahead.
	/// </summary>
	public string StandingsMessage => GamesPlayed switch
	{
		0 => "No games played yet.",
		_ => Leader switch
		{
			1 => $"Player 1 leads the series {Player1Wins}–{Player2Wins}.",
			2 => $"Player 2 leads the series {Player2Wins}–{Player1Wins}.",
			_ => $"Series tied {Player1Wins}–{Player2Wins}."
		}
	};

	/// <summary>
	/// Records a finished game. Anything other than a real result is ignored,
	/// so calling this with <see cref="GameState.WinState.No_Winner"/> is a no-op.
	/// </summary>
	public void Record(GameState.WinState result)
	{
		switch (result)
		{
			case GameState.WinState.Player1_Wins:
				Player1Wins++;
				break;
			case GameState.WinState.Player2_Wins:
				Player2Wins++;
				break;
			case GameState.WinState.Tie:
				Ties++;
				break;
			default:
				return;
		}

		LastResult = result;
	}

	/// <summary>Wipes the series back to zero.</summary>
	public void Clear()
	{
		Player1Wins = 0;
		Player2Wins = 0;
		Ties = 0;
		LastResult = GameState.WinState.No_Winner;
	}
}
