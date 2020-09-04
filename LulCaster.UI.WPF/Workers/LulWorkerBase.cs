using LulCaster.UI.WPF.Workers.EventArguments;
using System;

namespace LulCaster.UI.WPF.Workers
{
  public abstract class LulWorkerBase
  {
    protected object _autoResetLock = new object();
    protected object _runningFlagLock = new object();
    protected bool _isRunning, _autoReset = true;

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

    protected bool IsRunning
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

    public void Start()
    {
      if (!IsRunning)
      {
        IsRunning = true;
        DoWork();

        ProgressChanged?.Invoke(null, new ScreenCaptureProgressArgs
        {
          Status = "Screen capture is now running."
        });
      }
      else
      {
        ProgressChanged?.Invoke(null, new ScreenCaptureProgressArgs
        {
          Status = "Screen capture is already running. An attempt was made to start a new instance."
        });
      }
    }

    public void Stop()
    {
      ProgressChanged?.Invoke(null, new ScreenCaptureProgressArgs
      {
        Status = "Screen capture is halting."
      });

      IsRunning = false;
      AutoReset = false;
    }

    protected abstract void DoWork();
  }
}