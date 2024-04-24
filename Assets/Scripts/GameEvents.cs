using System;

public class GameEvents
{
    public event Action OnPlayEvent;
    public void Play()
    {
        OnPlayEvent?.Invoke();
    }

    public event Action OnPauseEvent;
    public void Pause()
    {
        OnPauseEvent?.Invoke();
    }

    public event Action OnRestartEvent;
    public void Restart()
    {
        OnRestartEvent?.Invoke();
    }

    public event Action OnLoseEvent;
    public void Lose()
    {
        OnLoseEvent?.Invoke();
    }

    public event Action OnFinishEvent;
    public void Finish()
    {
        OnFinishEvent?.Invoke();
    }
}
