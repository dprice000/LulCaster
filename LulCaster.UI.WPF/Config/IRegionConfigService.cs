using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Config;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Config
{
  public interface IRegionConfigService
  {
    RegionViewModel CreateRegion(Guid presetId, string regionName);
    void WriteAllRegions(string filePath, IEnumerable<RegionViewModel> regions);
    void DeleteRegion(Guid presetId, Guid regionId);
    IEnumerable<RegionViewModel> GetAllRegions(string importFilePath);
    IEnumerable<RegionConfig> GetAllRegions(Guid presetId);
    IEnumerable<RegionViewModel> GetAllRegionsAsViewModels(Guid presetId);
    RegionViewModel GetRegion(Guid presetId, Guid id);
    void UpdateRegion(Guid presetId, RegionViewModel regionViewModel);
    Task UpdateRegionAsync(Guid presetId, RegionViewModel regionViewModel);
  }
}