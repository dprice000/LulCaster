using LulCaster.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Controllers
{
  public interface IRegionListController
  {
    RegionViewModel CreateRegion(Guid presetId, string regionName);
    void WriteAllRegions(string filePath, IEnumerable<RegionViewModel> regions);
    void DeleteRegion(Guid presetId, Guid regionId);
    IEnumerable<LulCaster.Utility.Common.Config.RegionConfig> GetAllRegions(string importFilePath);
    IEnumerable<RegionViewModel> GetRegions(Guid presetId);
    void UpdateRegion(Guid presetId, RegionViewModel region);
    Task UpdateRegionAsync(Guid presetId, RegionViewModel region);
  }
}