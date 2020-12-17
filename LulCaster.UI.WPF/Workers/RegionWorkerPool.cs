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

    private const int IDLE_HALT_TIMEOUT = 50;
    private readonly int _captureFps;
    private readonly List<RegionWorker> _regionWorkers = new List<RegionWorker>();
    private Queue<ScreenCapture> _oldScreenCaptures = new Queue<ScreenCapture>();

    public ConcurrentQueue<ScreenCapture> ScreenCaptureQueue { get; } = new ConcurrentQueue<ScreenCapture>();

    public bool IsFull
    {
      get
      {
        return _regionWorkers.Count >= MaxPoolSize;
      }
    }
    
    public int MaxPoolSize { get; }

    public RegionWorkerPool(int maxPoolSize, int captureFps)
    {
      MaxPoolSize = maxPoolSize;
      _captureFps = captureFps;
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

        _oldScreenCaptures.Enqueue(screenCapture);
      }
    }

    private void WaitForFreeWorker()
    {
      while (IsFull)
      {
        try
        {
          Wait(IDLE_HALT_TIMEOUT);
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
      var worker = new RegionWorker(screenCapture, region);
      worker.Start();
      _regionWorkers.Add(worker);
    }
  }
}