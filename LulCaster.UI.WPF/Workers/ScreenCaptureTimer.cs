using LulCaster.UI.WPF.Workers.EventArguments;
using LulCaster.Utility.ScreenCapture.Windows;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace LulCaster.UI.WPF.Workers
{
  internal class ScreenCaptureTimer
  {
    private object _lockAutoReset = new object();

    private readonly IScreenCaptureService _screenCaptureService;
    private double _timerInterval;
    private readonly Timer _screenShotTimer;
    private Task _workerTask;
    private IProgress<ScreenCaptureProgressArgs> _progressHandler;
    private bool _autoReset = true;

    public event EventHandler<ScreenCaptureCompletedArgs> ScreenCaptureCompleted;
    public event EventHandler<ScreenCaptureProgressArgs> ProgressChanged;

    public TimeSpan TickInterval { get; }
    public bool AutoReset 
    {
      get
      {
        lock (_lockAutoReset)
        {
          return _autoReset;
        }
      }
      set
      {
        lock (_lockAutoReset)
        {
          _autoReset = value;
        }
      }
    }

    public ScreenCaptureTimer(IScreenCaptureService screenCapService, double timerInterval)
    {
      _screenCaptureService = screenCapService;
      _timerInterval = timerInterval;
      _screenShotTimer = new Timer(_timerInterval);
      InitializeTimer();

      _progressHandler = new Progress<ScreenCaptureProgressArgs>(progressArgs => {
        ProgressChanged.Invoke(this, progressArgs);
      });
    }

    private void InitializeTimer()
    {
      _screenShotTimer.Elapsed += NextScreenShotTimer_Elapsed;
      _screenShotTimer.AutoReset = false;
    }

    public void Start()
    {
      _screenShotTimer.Start();
    }

    private void NextScreenShotTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      DoWork();
    }

    private void DoWork()
    {
      _workerTask = Task.Run(() =>
      {
        var captureArgs = new ScreenCaptureCompletedArgs
        {
          ScreenImageStream = _screenCaptureService.CaptureScreenshot()
        };

        OnScreenCaptureCompleted(captureArgs);
      });
    }

    private void OnScreenCaptureCompleted(ScreenCaptureCompletedArgs captureArgs)
    {
      ScreenCaptureCompleted?.Invoke(this, captureArgs);

      if (AutoReset)
        _screenShotTimer.Start();
    }
  }
}