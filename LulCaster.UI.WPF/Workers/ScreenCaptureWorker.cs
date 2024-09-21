using LulCaster.UI.WPF.Workers.Events.Arguments;
using LulCaster.Utility.ScreenCapture.Windows;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Workers
{
  internal class ScreenCaptureWorker : LulWorkerBase
  {
    private readonly IScreenCaptureService _screenCaptureService;
    private Stopwatch _stopWatch = new Stopwatch();

    /// <summary>
    /// The lower limit in milliseconds on how fast a capture can run. Defaults to 60,000 ms.
    /// </summary>
    public int CaptureInterval { get; } = 1000;
    public System.Windows.Size CanvasBounds { get; set; }

    public IProgress<ScreenCaptureProgressArgs> ProgressHandler { get; }

    public event EventHandler<ScreenCaptureProgressArgs> ProgressChanged;

    public event EventHandler<ScreenCaptureCompletedArgs> ScreenCaptureCompleted;

    public ConcurrentQueue<ScreenCapture> ScreenCaptureQueue { get; } = new ConcurrentQueue<ScreenCapture>();


    public ScreenCaptureWorker(IScreenCaptureService screenCaptureService, System.Windows.Size canvasBounds, int captureFps, int idleTimeout) : base(idleTimeout)
    {
      _screenCaptureService = screenCaptureService;
      CaptureInterval = CalculateHaltInterval(captureFps);
      CanvasBounds = canvasBounds;

      ProgressHandler = new Progress<ScreenCaptureProgressArgs>(progressArgs =>
      {
        ProgressChanged.Invoke(this, progressArgs);
      });
    }

    public void SetGameHandle(IntPtr gameHandle)
    {
      ((GameCaptureService)_screenCaptureService).SetProcessPointer(gameHandle);
    }

    protected async override void DoWork()
    {
      while (IsRunning)
      {
        if (!LegalCanvasSize(CanvasBounds))
        {
          continue;
        }

        _stopWatch.Start();

        var screenCapture = _screenCaptureService.CaptureScreenshot();

        // If game handle is not set we will get an empty array. Just do nothing.
        if (screenCapture == null)
        {
          await HaltUntilNextIntervalAsync();
          continue;
        }

        ScreenCaptureQueue.Enqueue(new ScreenCapture()
        {
          Image = screenCapture,
          ScreenBounds = _screenCaptureService.ScreenOptions.GetBoundsAsRectangle(),
          CanvasBounds = this.CanvasBounds
        });

        OnScreenCaptureCompleted(new ScreenCaptureCompletedArgs
        {
          Image = screenCapture,
          ScreenBounds = _screenCaptureService.ScreenOptions.GetBoundsAsRectangle(),
          CanvasBounds = this.CanvasBounds
        });

        screenCapture = null;

        if (!AutoReset)
        {
          IsRunning = false;
          break;
        }

        await HaltUntilNextIntervalAsync();
      }
    }

    private Image ResizeImage(byte[] bytes)
    {
      MemoryStream memoryStream = new MemoryStream(bytes);
      return Image.FromStream(memoryStream).GetThumbnailImage((int)CanvasBounds.Width, (int)CanvasBounds.Height, null, IntPtr.Zero);
    }

    private bool LegalCanvasSize(System.Windows.Size bounds)
    {
      return bounds.Width > 0 && bounds.Height > 0;
    }

    private async Task HaltUntilNextIntervalAsync()
    {
      //ProgressChanged?.Invoke(null, new ScreenCaptureProgressArgs
      //{
      //  Status = $"Screen capture interval completed in {_stopWatch.Elapsed.Milliseconds}ms."
      //});

      _stopWatch.Restart();

      var haltTime = CaptureInterval - (int)_stopWatch.Elapsed.TotalMilliseconds;

      if (haltTime > 0)
      {
        await Task.Delay(haltTime);
      }
    }

    private int CalculateHaltInterval(int captureFps)
    {
      const int SECOND_AS_MS = 1000;
      return SECOND_AS_MS / captureFps;
    }

    private void OnScreenCaptureCompleted(ScreenCaptureCompletedArgs captureArgs)
    {
      ScreenCaptureCompleted?.Invoke(this, captureArgs);
    }
  }
}