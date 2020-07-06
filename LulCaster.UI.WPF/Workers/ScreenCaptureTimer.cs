using LulCaster.Utility.ScreenCapture.Windows;
using System;
using System.Timers;

namespace LulCaster.UI.WPF.Workers
{
  internal class ScreenCaptureTimer
  {
    private readonly IScreenCaptureService _screenCaptureService;
    private readonly Timer _nextScreenShotTimer;

    public event EventHandler<ScreenCaptureCompletedArgs> ScreenCaptureCompleted;

    public ScreenCaptureTimer(IScreenCaptureService screenCapService, double timerInterval)
    {
      _screenCaptureService = screenCapService;

      _nextScreenShotTimer = new Timer(timerInterval);
      _nextScreenShotTimer.Elapsed += _nextScreenShotTimer_Elapsed;
      _nextScreenShotTimer.AutoReset = true;
    }

    private void _nextScreenShotTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      var captureArgs = new ScreenCaptureCompletedArgs
      {
        ScreenImageStream = _screenCaptureService.CaptureScreenshot()
      };

      OnScreenCaptureCompleted(captureArgs);
    }

    private void OnScreenCaptureCompleted(ScreenCaptureCompletedArgs captureArgs)
    {
      ScreenCaptureCompleted?.Invoke(this, captureArgs);
    }

    public void Start()
    {
      _nextScreenShotTimer.Start();
    }
  }
}