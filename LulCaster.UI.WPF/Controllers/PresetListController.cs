using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public class PresetListController : IPresetListController
  {
    private readonly IPresetConfigService _presetConfigService;
    private readonly IDialogService<string> _newPresetDialog;
    private readonly IMessageBoxDialogService _messageBoxService;

    public PresetListController(IPresetConfigService configService, ISimpleDialogService<string> newPresetDialog, IMessageBoxDialogService messageBoxService)
    {
      _presetConfigService = configService;
      _newPresetDialog = newPresetDialog;
      _messageBoxService = messageBoxService;
    }

    public PresetViewModel CreatePreset(string name)
    {
      return _presetConfigService.CreatePreset(name);
    }

    public IEnumerable<PresetViewModel> GetAllPresets()
    {
      return _presetConfigService.GetAllPresets();
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
        return _presetConfigService.CreatePreset(preset);
      }

      return null;
    }

    public bool DeletePreset(PresetViewModel preset)
    {
      if (_messageBoxService.Show("Delete Preset", $"Are you sure you want to delete {preset.Name}?", DialogButtons.YesNo) == DialogResults.Yes)
      {
        _presetConfigService.DeletePreset(preset);
        return true;
      }

      return false;
    }
  }
}