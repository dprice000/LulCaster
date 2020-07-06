using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Config;
using System;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Config
{
  public interface IConfigService
  {
    void CreateRegionConfig(string preset, RegionConfig regionConfig);
    void DeleteRegion(string preset, Guid regionId);
    IEnumerable<RegionConfig> GetAllRegions(string preset);
    IEnumerable<RegionViewModel> GetAllRegionsAsViewModels(string preset);
    RegionViewModel GetRegion(string preset, Guid id);
    void UpdateRegion(string preset, RegionViewModel regionViewModel);
    void CreatePreset(string preset);
    void DeletePreset(string preset);
  }
}