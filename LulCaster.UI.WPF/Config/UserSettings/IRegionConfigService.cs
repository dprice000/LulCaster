using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Config;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Config.UserSettings
{
  public interface IRegionConfigService
  {
    RegionViewModel Create(Guid presetId, string regionName);
    void WriteAll(string filePath, IEnumerable<RegionViewModel> regions);
    void Delete(Guid presetId, Guid regionId);
    IEnumerable<RegionViewModel> GetAllRegions(string importFilePath);
    IEnumerable<RegionConfig> GetAllRegions(Guid presetId);
    IEnumerable<RegionViewModel> GetAllRegionsAsViewModels(Guid presetId);
    RegionViewModel Get(Guid presetId, Guid id);
    void Update(Guid presetId, RegionViewModel regionViewModel);
    Task UpdateAsync(Guid presetId, RegionViewModel regionViewModel);
  }
}