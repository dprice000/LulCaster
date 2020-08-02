
using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Services;
using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public class RegionListController : IRegionListController
  {
    private readonly IRegionConfigService _regionConfigService;
    private readonly IDialogService<InputDialog, string> _inputDialog;

    public RegionListController(IRegionConfigService configService, IDialogService<InputDialog, string> inputDialog)
    {
      _regionConfigService = configService;
      _inputDialog = inputDialog;
    }

    public IEnumerable<RegionViewModel> GetRegions(string presetFilePath)
    {
      return _regionConfigService.GetAllRegionsAsViewModels(presetFilePath);
    }
  }
}
