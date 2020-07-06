using LulCaster.Utility.Common.Logic;
using System;

namespace LulCaster.Utility.Common.Config
{
  public class RegionConfig
  {
    public Guid Id { get; set; }
    public LogicSets LogicSet { get; set; }
    public string Label { get; set; }
    public string TriggerValue { get; set; }
    public string BoundingBoxDimensions { get; set; }
    public string SoundFilePath { get; set; }
  }
}
