using System;

public class GameInfoEvent 
{

    public event Action<int> OnScoreChange;
    public void ScoreChange(int value)
    {
        OnScoreChange?.Invoke(value);
    }

    public event Action<int> OnMovesChange;
    public void MovesChange(int value)
    {
        OnMovesChange?.Invoke(value);
    }


}
