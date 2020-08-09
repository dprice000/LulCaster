using LulCaster.Utility.Common.Logic;

namespace LulCaster.Utility.Common.Config
{
  public class TriggerConfig
  {
    public string Name { get; set; }
    public TriggerTypes TriggerType { get; set; }
    public string ThresholdValue { get; set; }
    public string SoundFilePath { get; set; }
  }
}
