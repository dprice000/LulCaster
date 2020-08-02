using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Services;
using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public class PresetListController : IPresetListController
  {
    private readonly IPresetConfigService _presetConfigService;
    private readonly IDialogService<InputDialog, string> _inputDialog;
    private readonly IDialogService<MessageBoxDialog, DialogResults> _messageBoxService;

    public PresetListController(IPresetConfigService configService, IDialogService<InputDialog, string> inputDialog, IDialogService<MessageBoxDialog, DialogResults>  messageBoxService)
    {
      _presetConfigService = configService;
      _inputDialog = inputDialog;
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
      if (_inputDialog.Show("New Preset", "Enter New Preset Name: ", DialogButtons.OkCancel) is string presetName)
      {
        return _presetConfigService.CreatePreset(presetName);
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