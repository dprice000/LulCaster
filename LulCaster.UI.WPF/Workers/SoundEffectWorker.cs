using LulCaster.UI.WPF.Workers.Events.Arguments;
using LulCaster.Utility.Common.Collections;
using System.Diagnostics;
using System.Media;

namespace LulCaster.UI.WPF.Workers
{
  internal class SoundEffectWorker : LulWorkerBase
  {
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private readonly ThreadSafeQueue<TriggerSoundArgs> _soundQueue = new ThreadSafeQueue<TriggerSoundArgs>();
    private readonly SoundPlayer player = new SoundPlayer();

    public SoundEffectWorker(int idleTimeout) : base(idleTimeout)
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
        var sound = _soundQueue.Dequeue();

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