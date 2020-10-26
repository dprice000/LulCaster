using LulCaster.UI.WPF.Workers.EventArguments;
using LulCaster.Utility.ScreenCapture.Windows;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Workers
{
  internal class ScreenCaptureWorker : LulWorkerBase
  {
    private readonly IScreenCaptureService _screenCaptureService;
    private Task _workerTask;
    private IProgress<ScreenCaptureProgressArgs> _progressHandler;
    private Stopwatch _stopWatch = new Stopwatch();

    public event EventHandler<ScreenCaptureCompletedArgs> ScreenCaptureCompleted;
    public event EventHandler<ScreenCaptureProgressArgs> ProgressChanged;

    /// <summary>
    /// The lower limit in milliseconds on how fast a capture can run. Defaults to 60,000 ms.
    /// </summary>
    public int CaptureInterval { get; set; } = 60000;

    public ScreenCaptureWorker(IScreenCaptureService screenCaptureService, int captureInterval)
    {
      _screenCaptureService = screenCaptureService;
      ((GameCaptureService)_screenCaptureService).SetProcessPointer(HandleFinder.GetWindowsHandle("Valorant"));
      CaptureInterval = captureInterval;

      _progressHandler = new Progress<ScreenCaptureProgressArgs>(progressArgs => {
        ProgressChanged.Invoke(this, progressArgs);
      });
    }

    protected override void DoWork()
    {
      _workerTask = Task.Run(() =>
      {
        while (IsRunning)
        {
          _stopWatch.Start();
          byte[] byteImage = new byte[0];
          _screenCaptureService.CaptureScreenshot(ref byteImage);

          var captureArgs = new ScreenCaptureCompletedArgs
          {
            ScreenImageStream = byteImage,
            ScreenBounds = _screenCaptureService.ScreenOptions.GetBoundsAsRectangle()
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