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
          break;
        case TriggerTypes.Color:
          throw new NotImplementedException();
          break;
        case TriggerTypes.IncrementingNumber:
          throw new NotImplementedException();
          break;
        case TriggerTypes.DecrementingNumber:
          throw new NotImplementedException();
          break;
        default:
          throw new ArgumentException($"Type does not exist for passed in Trigger Type {triggerType}");
      }
    }
  }
}
