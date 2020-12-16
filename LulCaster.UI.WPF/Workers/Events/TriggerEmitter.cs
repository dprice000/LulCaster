using LulCaster.UI.WPF.Workers.Events.Arguments;
using System;

namespace LulCaster.UI.WPF.Workers.Events
{
  public static class TriggerEmitter
  {
    public static event EventHandler<TriggerSoundArgs> TriggerActivated;

    //TODO: Ideally this would be an internal method that could only be called by the trigger logic.
    public static void OnTriggerActivated(TriggerSoundArgs sound)
    {
      TriggerActivated?.Invoke(null, sound);
    }
  }
}