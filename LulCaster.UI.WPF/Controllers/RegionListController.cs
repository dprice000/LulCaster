using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public class RegionListController : IRegionListController
  {
    private readonly IRegionConfigService _regionConfigService;

    public RegionListController(IRegionConfigService configService)
    {
      _regionConfigService = configService;
    }

    public RegionViewModel CreateRegion(Guid presetId, string regionName)
    {
      return _regionConfigService.CreateRegion(presetId, regionName);
    }

    public void DeleteRegion(Guid presetId, Guid regionId)
    {
      _regionConfigService.DeleteRegion(presetId, regionId);
    }

    public IEnumerable<RegionViewModel> GetRegions(Guid presetId)
    {
      return _regionConfigService.GetAllRegionsAsViewModels(presetId);
    }
  }
}