
using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public class RegionListController : IRegionListController
  {
    private readonly IRegionConfigService _regionConfigService;
    private readonly ISimpleDialogService<string> _newPresetDialog;

    public RegionListController(IRegionConfigService configService, ISimpleDialogService<string> newPresetDialog)
    {
      _regionConfigService = configService;
      _newPresetDialog = newPresetDialog;
    }

    public IEnumerable<RegionViewModel> GetRegions(string presetFilePath)
    {
      return _regionConfigService.GetAllRegionsAsViewModels(presetFilePath);
    }
  }
}
