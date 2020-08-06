using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Services;
using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public class PresetListController : IPresetListController
  {
    private readonly IPresetConfigService _presetConfigService;
    private readonly IDialogService<InputDialog, InputDialogResult> _inputDialog;
    private readonly IDialogService<MessageBoxDialog, LulDialogResult> _messageBoxService;

    public PresetListController(IPresetConfigService configService, IDialogService<InputDialog, InputDialogResult> inputDialog, IDialogService<MessageBoxDialog, LulDialogResult> messageBoxService)
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
    public InputDialogResult ShowNewPresetDialog()
    {
      return _inputDialog.Show("New Preset", "Enter New Preset Name: ", DialogButtons.OkCancel);
    }

    public LulDialogResult ShowMessageBox(string title, string message, DialogButtons dialogButtons)
    {
      return _messageBoxService.Show(title, message, dialogButtons);
    }

    public void DeletePreset(PresetViewModel preset)
    {
      _presetConfigService.DeletePreset(preset);
    }
  }
}