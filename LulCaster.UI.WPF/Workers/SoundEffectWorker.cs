using LulCaster.UI.WPF.Workers.Events.Arguments;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Media;

namespace LulCaster.UI.WPF.Workers
{
  internal class SoundEffectWorker : LulWorkerBase
  {
    private Stopwatch _stopwatch = new Stopwatch();
    private ConcurrentQueue<TriggerSoundArgs> _soundQueue = new ConcurrentQueue<TriggerSoundArgs>();
    private SoundPlayer player = new SoundPlayer();

    public SoundEffectWorker(int idleTimeout) : base (idleTimeout)
    {
    }

    public void EnqueueSound(TriggerSoundArgs sound)
    {
      _soundQueue.Enqueue(sound);

      if (!IsRunning)
      {
        Reset();
        Start();
      }
    }

    protected override void DoWork()
    {
      while (!_soundQueue.IsEmpty)
      {
        if (!_soundQueue.TryDequeue(out TriggerSoundArgs sound)) 
        {
          Wait(IDLE_TIMEOUT);
          continue;
        }

        _stopwatch.Start();
        player.SoundLocation = sound.FilePath;
        player.Load();
        player.Play();
        _stopwatch.Reset();
      }

      IsRunning = false;
    }
  }
}
