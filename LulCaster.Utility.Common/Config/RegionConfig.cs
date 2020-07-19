using LulCaster.Utility.Common.Logic;
using System;
using System.Drawing;

namespace LulCaster.Utility.Common.Config
{
  public class RegionConfig
  {
    public Guid Id { get; set; }
    public LogicSets LogicSet { get; set; }
    public string Label { get; set; }
    public string TriggerValue { get; set; }
    public int Height { get; set; }
    public int Widht { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public string SoundFilePath { get; set; }

    public Rectangle BoundingBox 
    { 
      get
      {
        return new Rectangle(X, Y, Widht, Height);
      }
    }
  }
}
