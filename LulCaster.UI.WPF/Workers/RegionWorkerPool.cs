using LulCaster.UI.WPF.ViewModels;
using LulCaster.UI.WPF.Workers.EventArguments;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace LulCaster.UI.WPF.Workers
{
  public class RegionWorkerPool : LulWorkerBase
  {
    public event EventHandler<ScreenCaptureProgressArgs> ProgressChanged;

    public event EventHandler<TriggerViewModel> TriggerActivated;

    private const int IDLE_HALT_TIMEOUT = 100;

    private readonly ConcurrentQueue<ScreenCapture> _screenCaptureQueue = new ConcurrentQueue<ScreenCapture>();
    private readonly List<RegionWorker> _regionWorkers = new List<RegionWorker>();

    public bool IsFull
    {
      get
      {
        return _regionWorkers.Count <= MaxPoolSize;
      }
    }
    
    public int MaxPoolSize { get; }

    public RegionWorkerPool(int maxPoolSize)
    {
      MaxPoolSize = maxPoolSize;
    }

    public void EnqueueScreenCapture(ScreenCapture screenCapture)
    {
      _screenCaptureQueue.Enqueue(screenCapture);
    }

    private void OnTriggerActivated(TriggerViewModel trigger)
    {
      TriggerActivated?.Invoke(this, trigger);
    }

    protected override void DoWork()
    {
      while (IsRunning)
      {
        if (_screenCaptureQueue.IsEmpty 
            || IsFull
            || !_screenCaptureQueue.TryDequeue(out ScreenCapture screenCapture))
        {
          Wait(IDLE_HALT_TIMEOUT);
          continue;
        }

        using (screenCapture)
        {
          foreach (var region in screenCapture.RegionViewModels)
          {
            WaitForFreeWorker();
            CreateWorker(screenCapture, region);
          }
        }
      }
    }

    private void WaitForFreeWorker()
    {
      while (IsFull)
      {
        Wait(IDLE_HALT_TIMEOUT);
        PruneFinishedWorkers();
      }
    }

    private void PruneFinishedWorkers()
    {
      foreach (var worker in _regionWorkers.Where(x => !x.IsRunning))
      {
        _regionWorkers.Remove(worker);
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