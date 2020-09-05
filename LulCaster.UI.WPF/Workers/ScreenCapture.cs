using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;
using System.IO;

namespace LulCaster.UI.WPF.Workers
{
  public class ScreenCapture
  {
    public MemoryStream ScreenMemoryStream { get; set; }
    public IList<RegionViewModel> RegionViewModels { get; set; }
  }
}