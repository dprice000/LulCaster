using LulCaster.UI.WPF.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace LulCaster.UI.WPF.Workers
{
  internal class SoundEffectWorker : LulWorkerBase
  {
    private Stopwatch _stopwatch = new Stopwatch();
    private ConcurrentQueue<TriggerViewModel> _processingQueue = new ConcurrentQueue<TriggerViewModel>();
    
    public SoundEffectWorker()
    {
    }

    public SoundEffectWorker(TriggerViewModel trigger)
    {
      _processingQueue.Enqueue(trigger);
    }

    public void EnqueueSound(TriggerViewModel trigger)
    {
      _processingQueue.Enqueue(trigger);

      if (!IsRunning)
      {
        Start();
      }
    }

    protected override void DoWork()
    {
      while (!_processingQueue.IsEmpty)
      {
        if (!_processingQueue.TryDequeue(out TriggerViewModel trigger)) 
        {
          Wait(1000);
          continue;
        }

        _stopwatch.Start();
        trigger.SoundFile.Play();
        _stopwatch.Reset();
      }

      IsRunning = false;
    }

  }
}
