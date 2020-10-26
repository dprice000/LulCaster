using LulCaster.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace LulCaster.UI.WPF.Workers
{
  public class ScreenCapture : IDisposable
  {
    public MemoryStream ScreenMemoryStream { get; set; }
    public IList<RegionViewModel> RegionViewModels { get; set; }
    public Rectangle ScreenBounds { get; set; }
    public System.Windows.Size CanvasBounds { get; set; }
    public DateTime CreationTime { get; set; }

    public void Dispose()
    {
      ScreenMemoryStream.Dispose();
    }
  }
}