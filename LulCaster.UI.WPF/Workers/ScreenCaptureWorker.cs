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
    public int CaptureInterval { get; set; }

    public ScreenCaptureWorker(IScreenCaptureService screenCaptureService, int captureInterval)
    {
      _screenCaptureService = screenCaptureService;

      CaptureInterval = captureInterval;

      _progressHandler = new Progress<ScreenCaptureProgressArgs>(progressArgs =>
      {
        ProgressChanged.Invoke(this, progressArgs);
      });
    }

    public void SetGameHandle(IntPtr gameHandle)
    {
      ((GameCaptureService)_screenCaptureService).SetProcessPointer(gameHandle);
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

          // If game handle is not set we will get an empty array. Just do nothing.
          if (byteImage == null)
          {
            HaltUntilNextInterval();
          }

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