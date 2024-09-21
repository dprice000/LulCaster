using LulCaster.UI.WPF.Utility;
using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers.Events.Arguments;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace LulCaster.UI.WPF.Workers
{
  public class RegionWorkerPool : LulWorkerBase
  {
    private const string CAPTURE_FPS_ERROR = "Capture FPS must be greater than 0.";
    private const string WORKER_MAX_ERROR = "Worker pool size must be greater than 0.";

    public event EventHandler<ScreenCaptureProgressArgs> ProgressChanged;

    private readonly int _captureFps = 0;
    private readonly List<RegionWorker> _regionWorkers = new List<RegionWorker>();
    
    private WireFrameViewModel ViewModel { get; }

    public ConcurrentQueue<ScreenCapture> ScreenCaptureQueue { get; set; }

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

    protected async override void DoWork()
    {
      while (IsRunning)
      {
        if (ScreenCaptureQueue.IsEmpty)
        {
          await WaitAsync(IDLE_TIMEOUT);
          continue;
        }

        if (ScreenCaptureQueue.TryDequeue(out var screenCapture) == false || screenCapture == null || screenCapture.RegionViewModels == null)
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

    private async void WaitForFreeWorker()
    {
      while (IsFull)
      {
        await WaitAsync(IDLE_TIMEOUT);
        PruneFinishedWorkers();
      }
    }

    private void PruneFinishedWorkers()
    {
      var workers = _regionWorkers.Where(worker => !worker.IsRunning).ToList();

      foreach (var worker in workers)
      {
        _regionWorkers.Remove(worker);
      }
    }

    private void CreateWorker(ScreenCapture screenCapture, RegionViewModel region)
    {
      var worker = new RegionWorker(screenCapture, region, IDLE_TIMEOUT);

      worker.Start();
      _regionWorkers.Add(worker);
    }
  }
}