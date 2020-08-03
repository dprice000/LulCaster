using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Services;
using LulCaster.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public class RegionListController : IRegionListController
  {
    private readonly IRegionConfigService _regionConfigService;
    private readonly IDialogService<InputDialog, string> _inputDialog;
    private readonly IDialogService<MessageBoxDialog, DialogResults> _messageBoxService;

    public RegionListController(IRegionConfigService configService, IDialogService<InputDialog, string> inputDialog, IDialogService<MessageBoxDialog, DialogResults> messageBoxService)
    {
      _regionConfigService = configService;
      _inputDialog = inputDialog;
      _messageBoxService = messageBoxService;
    }

    public RegionViewModel CreateRegion(Guid presetId, string regionName)
    {
      return _regionConfigService.CreateRegion(presetId, regionName);
    }

    public void DeleteRegion(Guid presetId, Guid regionId)
    {
      _regionConfigService.DeleteRegion(presetId, regionId);
    }

    /// <summary>
    /// Shows new preset dialog.
    /// </summary>
    /// <returns>Returns PresetViewModel for new Preset.</returns>
    public string ShowNewRegionDialog()
    {
      if (_inputDialog.Show("New Region", "Enter New Region Name: ", DialogButtons.OkCancel) is string regionName)
      {
        return regionName;
      }

      return null;
    }

    public DialogResults ShowMessageBox(string title, string message, DialogButtons dialogButtons)
    {
      return _messageBoxService.Show(title, message, dialogButtons);
    }

    public IEnumerable<RegionViewModel> GetRegions(Guid presetId)
    {
      return _regionConfigService.GetAllRegionsAsViewModels(presetId);
    }
  }
}