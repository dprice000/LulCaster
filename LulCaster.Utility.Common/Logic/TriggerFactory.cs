using System;
using LulCaster.Utility.Common.Logic.Triggers;

namespace LulCaster.Utility.Common.Logic
{
  public class TriggerFactory
  {
    public ITrigger GetTrigger(TriggerTypes triggerType)
    {
      switch (triggerType)
      {
        case TriggerTypes.Text:
          return new TextTrigger();
        case TriggerTypes.Color:
          throw new NotImplementedException();
        case TriggerTypes.IncrementingNumber:
          throw new NotImplementedException();
        case TriggerTypes.DecrementingNumber:
          throw new NotImplementedException();
        default:
          throw new ArgumentException($"Type does not exist for passed in Trigger Type {triggerType}");
      }
    }
  }
}
