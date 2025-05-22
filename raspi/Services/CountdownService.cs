namespace raspi.Services;

public class CountdownService
{
    private Timer? _timer;
    private readonly object _lock = new();
    public int RemainingSeconds { get; private set; }
    public event Action? OnCountdownUpdated;
    public void Start(int secondsToAdd)
    {
        lock (_lock)
        {
            if (_timer == null)
            {
                RemainingSeconds = secondsToAdd;
                _timer = new Timer(_ =>
                {
                    lock (_lock)
                    {
                        if (RemainingSeconds > 0)
                        {
                            RemainingSeconds--;
                            OnCountdownUpdated?.Invoke();
                        }
                        else
                        {
                            _timer?.Dispose();
                            _timer = null;
                        }
                    }
                }, null, 1000, 1000);
            }
            else
            {
                // Add time if timer is already running
                RemainingSeconds += secondsToAdd;
                OnCountdownUpdated?.Invoke();
            }
        }
    }


    public void Reset()
    {
        lock (_lock)
        {
            _timer?.Dispose();
            _timer = null;
            RemainingSeconds = 0;
            OnCountdownUpdated?.Invoke();
        }
    }

    public bool IsRunning()
    {
        return _timer != null;
    }

    public string GetFormattedTime()
    {
        var minutes = RemainingSeconds / 60;
        var seconds = RemainingSeconds % 60;
        return $"{minutes}:{seconds:D2}";
    }
}