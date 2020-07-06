using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Config;
using System;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Config
{
  public interface IConfigService
  {
    void CreateRegionConfig(RegionConfig regionConfig);
    void DeleteRegion(Guid regionId);
    IEnumerable<RegionConfig> GetAllRegions();
    IEnumerable<RegionViewModel> GetAllRegionsAsViewModels();
    RegionViewModel GetRegion(Guid id);
    void UpdateRegion(RegionViewModel regionViewModel);
  }
}