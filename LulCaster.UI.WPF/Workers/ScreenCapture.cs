using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace LulCaster.UI.WPF.Workers
{
  public class ScreenCapture
  {
    public MemoryStream ScreenMemoryStream { get; set; }
    public IList<RegionViewModel> RegionViewModels { get; set; }
    public Rectangle ScreenBounds { get; set; }
    public System.Windows.Size CanvasBounds { get; set; }
  }
}