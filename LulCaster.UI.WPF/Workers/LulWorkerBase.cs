using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Workers
{
  public abstract class LulWorkerBase
  {
    protected object _autoResetLock = new object();
    protected object _runningFlagLock = new object();
    protected bool _isRunning, _autoReset = true;
    protected readonly Task _workerTask;

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

    protected LulWorkerBase()
    {
      _workerTask = new Task(() => DoWork());
    }

    public void Start()
    {
      if (!IsRunning)
      {
        IsRunning = true;
        _workerTask.Start();

        //ProgressChanged?.Invoke(null, new ScreenCaptureProgressArgs
        //{
        //  Status = "Screen capture is now running."
        //});
      }
      else
      {
        //ProgressChanged?.Invoke(null, new ScreenCaptureProgressArgs
        //{
        //  Status = "Screen capture is already running. An attempt was made to start a new instance."
        //});
      }
    }

    public void Stop()
    {
      //ProgressChanged?.Invoke(null, new ScreenCaptureProgressArgs
      //{
      //  Status = "Screen capture is halting."
      //});

      IsRunning = false;
      AutoReset = false;
    }

    protected abstract void DoWork();
  }
}