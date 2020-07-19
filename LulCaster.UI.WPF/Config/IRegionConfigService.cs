using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Config;
using System;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Config
{
  public interface IRegionConfigService
  {
    void CreateRegionConfig(string presetFilePath, RegionConfig regionConfig);
    void DeleteRegion(string presetFilePath, Guid regionId);
    IEnumerable<RegionConfig> GetAllRegions(string presetFilePath);
    IEnumerable<RegionViewModel> GetAllRegionsAsViewModels(string presetFilePath);
    RegionViewModel GetRegion(string presetFilePath, Guid id);
    void UpdateRegion(string presetFilePath, RegionViewModel regionViewModel);
  }
}