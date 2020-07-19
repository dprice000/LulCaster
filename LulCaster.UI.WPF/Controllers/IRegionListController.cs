using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public interface IRegionListController
  {
    IEnumerable<RegionViewModel> GetRegions(string presetFilePath);
  }
}