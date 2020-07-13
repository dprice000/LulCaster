using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public class PresetListController : ControllerBase, IPresetListController
  {
    private readonly IConfigService _configService;
    private readonly ISimpleDialogService<string> _newPresetDialog;

    public PresetListController(IConfigService configService, ISimpleDialogService<string> newPresetDialog)
    {
      _configService = configService;
      _newPresetDialog = newPresetDialog;
    }

    public IEnumerable<PresetViewModel> GetAllPresets()
    {
      return _configService.GetAllPresets();
    }

    /// <summary>
    /// Shows new preset dialog.
    /// </summary>
    /// <returns>Returns PresetViewModel for new Preset.</returns>
    public PresetViewModel ShowNewPresetDialog()
    {
      if (_newPresetDialog.ShowDialog() == true)
      {
        var preset = _newPresetDialog.ReturnValue;
        return _configService.CreatePreset(preset);
      }

      return null;
    }

    public void DeletePreset(Guid id)
    {
      _configService.DeletePreset(id);
    }
  }
}