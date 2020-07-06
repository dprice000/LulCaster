using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Dialogs;
using System.Collections.Generic;
using System.Linq;

namespace LulCaster.UI.WPF.Controllers
{
  public class PresetListController : ControllerBase, IPresetListController
  {
    private readonly IConfigService _configService;
    private readonly ISimpleDialogService<string> _newPresetDialog;

    public List<string> PresetNames { get; private set; } = new List<string>();

    public PresetListController(IConfigService configService, ISimpleDialogService<string> newPresetDialog)
    {
      _configService = configService;
      _newPresetDialog = newPresetDialog;

      PresetNames = _configService.GetAllPresets().ToList();
      OnPropertyChanged(nameof(PresetNames));
    }

    /// <summary>
    /// Shows new preset dialog
    /// </summary>
    /// <returns>Returns name of new preset</returns>
    public void ShowNewPresetDialog()
    {
      if (_newPresetDialog.ShowDialog() == true) 
      {
        var preset = _newPresetDialog.ReturnValue;
        _configService.CreatePreset(preset);
        PresetNames.Add(preset);
        OnPropertyChanged(nameof(PresetNames));
      }
    }

    public void DeletePreset(string preset)
    {
      _configService.DeletePreset(preset);
      PresetNames.Remove(preset);
      OnPropertyChanged(nameof(PresetNames));
    }
  }
}