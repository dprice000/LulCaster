using LulCaster.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LulCaster.UI.WPF.Controllers
{
  public interface IRegionListController
  {
    RegionViewModel Create(Guid presetId, string regionName);
    void WriteAll(string filePath, IEnumerable<RegionViewModel> regions);
    void Delete(Guid presetId, Guid regionId);
    IEnumerable<RegionViewModel> GetAll(string importFilePath);
    IEnumerable<RegionViewModel> GetRegions(Guid presetId);
    void Update(Guid presetId, RegionViewModel region);
    Task UpdatAsync(Guid presetId, RegionViewModel region);
  }
}