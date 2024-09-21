using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Workers
{
  public abstract class LulWorkerBase
  {
    protected readonly int IDLE_TIMEOUT = 10;

    private readonly object _autoResetLock = new object();
    private readonly object _runningFlagLock = new object();
    private bool _isRunning, _autoReset = true;
    private Task _workerTask;

    public bool AutoReset
    {
      get
      {
        lock (_autoResetLock)
        {
          return _autoReset;
        }
      }
      set
      {
        lock (_autoResetLock)
        {
          _autoReset = value;
        }
      }
    }

    public bool IsRunning
    {
      get
      {
        lock (_runningFlagLock)
        {
          return _isRunning;
        }
      }
      set
      {
        lock (_runningFlagLock)
        {
          _isRunning = value;
        }
      }
    }

    protected LulWorkerBase(int idleTimeout)
    {
      IDLE_TIMEOUT = idleTimeout;
    }

    public void Start()
    {
      if (!IsRunning)
      {
        IsRunning = true;
        _workerTask = Task.Run(() => DoWork());
      }
    }

    public void Reset()
    {
      _workerTask = Task.Run(() => DoWork());
    }

    public void Stop()
    {
      IsRunning = false;
      AutoReset = false;
    }

    protected async Task WaitAsync(int millisecond)
    {
      await Task.Delay(millisecond);
    }

    protected abstract void DoWork();
  }
}