using LulCaster.UI.WPF.Workers.EventArguments;
using LulCaster.Utility.ScreenCapture.Windows;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Workers
{
  internal class ScreenCaptureTimer
  {
    private object _autoResetLock = new object();
    private object _runningFlagLock = new object();

    private readonly IScreenCaptureService _screenCaptureService;
    private Task _workerTask;
    private IProgress<ScreenCaptureProgressArgs> _progressHandler;
    private bool _isRunning, _autoReset = true;
    private Stopwatch _stopWatch = new Stopwatch();

    public event EventHandler<ScreenCaptureCompletedArgs> ScreenCaptureCompleted;
    public event EventHandler<ScreenCaptureProgressArgs> ProgressChanged;

    private bool IsRunning
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

    /// <summary>
    /// The lower limit in milliseconds on how fast a capture can run. Defaults to 60,000 ms.
    /// </summary>
    public int CaptureInterval { get; set; } = 60000;
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

    public ScreenCaptureTimer(IScreenCaptureService screenCaptureService, int captureInterval)
    {
      _screenCaptureService = screenCaptureService;
      CaptureInterval = captureInterval;

      _progressHandler = new Progress<ScreenCaptureProgressArgs>(progressArgs => {
        ProgressChanged.Invoke(this, progressArgs);
      });
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

    private void DoWork()
    {
      _workerTask = Task.Run(() =>
      {
        while (IsRunning)
        {
          _stopWatch.Start();

          var captureArgs = new ScreenCaptureCompletedArgs
          {
            ScreenImageStream = _screenCaptureService.CaptureScreenshot()
          };

          OnScreenCaptureCompleted(captureArgs);

          if (!AutoReset)
          {
            IsRunning = false;
            break;
          }
          
          HaltUntilNextInterval();
        }
      });
    }

    private void HaltUntilNextInterval()
    {
      ProgressChanged?.Invoke(null, new ScreenCaptureProgressArgs
      {
        Status = $"Screen capture interval completed in {_stopWatch.Elapsed.Milliseconds}ms."
      });

      var haltTime = CaptureInterval - _stopWatch.Elapsed.Milliseconds;
      _stopWatch.Restart();

      if (haltTime > 0)
      {
        Thread.Sleep(haltTime);
      }
    }

    private void OnScreenCaptureCompleted(ScreenCaptureCompletedArgs captureArgs)
    {
      ScreenCaptureCompleted?.Invoke(this, captureArgs);
    }
  }
}