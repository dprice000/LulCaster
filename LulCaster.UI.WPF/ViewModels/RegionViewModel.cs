using System;
using System.Windows.Shapes;

namespace LulCaster.UI.WPF.ViewModels
{
  public class RegionViewModel
  {
    public Guid Id { get; set; }
    public string Label { get; set; }
    public string TriggerText { get; set; }
    public string SoundFilePath { get; set; }
    public Rectangle BoundingBox { get; set; }
  }
}