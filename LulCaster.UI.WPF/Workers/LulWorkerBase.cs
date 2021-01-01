using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Workers
{
  public abstract class LulWorkerBase
  {
    protected readonly int IDLE_TIMEOUT = 10;

    protected object _autoResetLock = new object();
    protected object _runningFlagLock = new object();
    protected bool _isRunning, _autoReset = true;
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
      _workerTask = new Task(() => DoWork());
    }

    public void Start()
    {
      if (!IsRunning)
      {
        IsRunning = true;
        _workerTask.Start();
      }
    }

    public void Reset()
    {
      _workerTask = new Task(() => DoWork());
    }

    public void Stop()
    {
      IsRunning = false;
      AutoReset = false;
    }

    protected void Wait(int millisecond)
    {
      _workerTask.Wait(millisecond);
    }

    protected abstract void DoWork();
  }
}