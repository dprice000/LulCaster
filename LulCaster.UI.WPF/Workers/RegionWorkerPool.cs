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
    public event EventHandler<ScreenCaptureProgressArgs> ProgressChanged;

    private const int IDLE_HALT_TIMEOUT = 100;
    private readonly List<RegionWorker> _regionWorkers = new List<RegionWorker>();
    private List<ScreenCapture> _oldScreenCaptures = new List<ScreenCapture>();

    public ConcurrentQueue<ScreenCapture> ScreenCaptureQueue { get; } = new ConcurrentQueue<ScreenCapture>();

    public bool IsFull
    {
      get
      {
        return _regionWorkers.Count >= MaxPoolSize;
      }
    }
    
    public int MaxPoolSize { get; }

    public RegionWorkerPool(int maxPoolSize)
    {
      MaxPoolSize = maxPoolSize;
    }

    protected override void DoWork()
    {
      while (IsRunning)
      {
        if (ScreenCaptureQueue.IsEmpty 
            || !ScreenCaptureQueue.TryDequeue(out ScreenCapture screenCapture))
        {
          Wait(IDLE_HALT_TIMEOUT);
          continue;
        }

          foreach (var region in screenCapture.RegionViewModels)
          {
            WaitForFreeWorker();
            CreateWorker(screenCapture, region);
          }

        _oldScreenCaptures.Add(screenCapture);
      }
    }

    private void WaitForFreeWorker()
    {
      while (IsFull)
      {
        Wait(IDLE_HALT_TIMEOUT);
        PruneFinishedWorkers();
        DisposeOldScreenCaptures();
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
      foreach (var screenCapture in _oldScreenCaptures)
      {
        screenCapture.Dispose();
      }
    }

    private void CreateWorker(ScreenCapture screenCapture, RegionViewModel region)
    {
      var worker = new RegionWorker(screenCapture, region);
      worker.Start();
      _regionWorkers.Add(worker);
    }
  }
}