using LulCaster.Utility.Common.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace LulCaster.Utility.Common.Config
{
  public class RegionConfig
  {
    public Guid Id { get; set; }
    public TriggerTypes LogicSet { get; set; }
    public string Name { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public IList<TriggerConfig> Triggers { get; set; }
    public Rectangle BoundingBox 
    { 
      get
      {
        return new Rectangle(X, Y, Width, Height);
      }
    }
  }
}
