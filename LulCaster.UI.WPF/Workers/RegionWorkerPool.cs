using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers.Events.Arguments;
using LulCaster.Utility.Common.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace LulCaster.UI.WPF.Workers
{
  public class RegionWorkerPool : LulWorkerBase
  {
    public event EventHandler<ScreenCaptureProgressArgs> ProgressChanged;

    private readonly int _captureFps;
    private readonly List<RegionWorker> _regionWorkers = new List<RegionWorker>();
    private Queue<ScreenCapture> _oldScreenCaptures = new Queue<ScreenCapture>();
    
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

        foreach (var region in screenCapture.RegionViewModels)
        {
          WaitForFreeWorker();
          CreateWorker(screenCapture, region);
        }

        _oldScreenCaptures.Enqueue(screenCapture);
      }
    }

    private void WaitForFreeWorker()
    {
      while (IsFull)
      {
        try
        {
          Wait(IDLE_TIMEOUT);
          PruneFinishedWorkers();
        }
        finally
        {
          DisposeOldScreenCaptures();
        }
      }
    }

    private void PruneFinishedWorkers()
    {
      for (int i = _regionWorkers.Count - 1; i > -1; i--)
      {
        if (!_regionWorkers[i].IsRunning)
        {
          _regionWorkers.RemoveAt(i);
        }
      }
    }

    public void DisposeOldScreenCaptures()
    {
      var extraFramesCount = _oldScreenCaptures.Count - _captureFps;

      if (extraFramesCount > 0)
      {
        for (int i = 0; i < extraFramesCount; i++)
        {
          _oldScreenCaptures.Dequeue().Dispose();
        }
      }
    }

    private void CreateWorker(ScreenCapture screenCapture, RegionViewModel region)
    {
      var worker = new RegionWorker(screenCapture, region, IDLE_TIMEOUT);
      worker.Start();
      _regionWorkers.Add(worker);
    }

    #region "Event Handlers"

    internal void screenCaptureWorker_ScreenCaptureCompleted(object sender, ScreenCaptureCompletedArgs captureArgs)
    {
      if (captureArgs.CanvasBounds.Height == 0 || captureArgs.CanvasBounds.Width == 0)
        return;

      var imageStream = new MemoryStream(captureArgs.ScreenImageStream);

      ScreenCaptureQueue.Enqueue(new ScreenCapture()
      {
        ScreenMemoryStream = imageStream,
        RegionViewModels = ViewModel.Regions,
        ScreenBounds = captureArgs.ScreenBounds,
        CanvasBounds = captureArgs.CanvasBounds,
        CreationTime = DateTime.Now
      });
    }

    #endregion "Event Handlers"
  }
}