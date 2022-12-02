using LulCaster.UI.WPF.Utility;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers.Events.Arguments;
using LulCaster.Utility.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LulCaster.UI.WPF.Workers
{
  public class RegionWorkerPool : LulWorkerBase
  {
    private const string CAPTURE_FPS_ERROR = "Capture FPS must be greater than 0.";
    private const string WORKER_MAX_ERROR = "Worker pool size must be greater than 0.";

    public event EventHandler<ScreenCaptureProgressArgs> ProgressChanged;
    public event EventHandler<ScreenCapture> ProcessingCompleted;

    private readonly int _captureFps = 0;
    private readonly List<RegionWorker> _regionWorkers = new List<RegionWorker>();
    
    private WireFrameViewModel ViewModel { get; }

    public ThreadSafeQueue<ScreenCapture> ScreenCaptureQueue { get; } = new ThreadSafeQueue<ScreenCapture>();

    public bool IsFull
    {
      get
      {
        return _regionWorkers.Count >= MaxPoolSize;
      }
    }

    public int MaxPoolSize { get; }

    public RegionWorkerPool(int maxPoolSize, int captureFps, int idleTimeout, WireFrameViewModel viewModel) : base(idleTimeout)
    {
      if (maxPoolSize < 1)
      {
        throw new ArgumentOutOfRangeException(WORKER_MAX_ERROR);
      }
      else if (captureFps < 1)
      {
        throw new ArgumentOutOfRangeException(CAPTURE_FPS_ERROR);
      }

      MaxPoolSize = maxPoolSize;
      _captureFps = captureFps;
      ViewModel = viewModel;
    }

    protected override void DoWork()
    {
      while (IsRunning)
      {
        if (ScreenCaptureQueue.IsEmpty)
        {
          Wait(IDLE_TIMEOUT);
          continue;
        }

        var screenCapture = ScreenCaptureQueue.Dequeue();

        if (screenCapture == null)
        {
          continue;
        }

        foreach (var region in screenCapture.RegionViewModels)
        {
          WaitForFreeWorker();
          CreateWorker(screenCapture, region);
        }
      }
    }

    private void WaitForFreeWorker()
    {
      while (IsFull)
      {
        Wait(IDLE_TIMEOUT);
        PruneFinishedWorkers();
      }
    }

    private void PruneFinishedWorkers()
    {
      var workers = _regionWorkers.Where(worker => !worker.IsRunning).ToList();

      foreach (var worker in workers)
      {
        worker.ProcessingComplete -= null;
        _regionWorkers.Remove(worker);
      }
    }

    private void CreateWorker(ScreenCapture screenCapture, RegionViewModel region)
    {
      var worker = new RegionWorker(screenCapture, region, IDLE_TIMEOUT);
      worker.ProcessingComplete += Worker_ProcessingComplete;

      worker.Start();
      _regionWorkers.Add(worker);
    }

    private void Worker_ProcessingComplete(object sender, ScreenCapture e)
    {
        OnProcessingComplete(e);
    }

    #region "Event Handlers"

    internal void screenCaptureWorker_ScreenCaptureCompleted(object sender, ScreenCaptureCompletedArgs captureArgs)
    {
      if (captureArgs.ScreenBounds.Height == 0 || captureArgs.ScreenBounds.Width == 0)
        return;

      ScreenCaptureQueue.Enqueue(BitmapHelper.ConvertArgsToScreenCap(captureArgs, ViewModel.RegionControl.Regions, DateTime.Now));

      captureArgs.HasBeenProcessed = true;

      if (captureArgs.HasBeenProcessed && captureArgs.HasBeenDrawn)
      {
        captureArgs.Dispose();
      }
    }

    private void OnProcessingComplete(ScreenCapture screenCapture)
    {
      ProcessingCompleted?.Invoke(this, screenCapture);
    }

    #endregion "Event Handlers"
  }
}