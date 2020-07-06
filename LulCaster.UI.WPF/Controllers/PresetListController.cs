using LulCaster.UI.WPF.Config;
using LulCaster.UI.WPF.Dialogs;

namespace LulCaster.UI.WPF.Controllers
{
  public class PresetListController : IPresetListController
  {
    private readonly IConfigService _configService;
    private readonly ISimpleDialogService<string> _newPresetDialog;

    public PresetListController(IConfigService configService, ISimpleDialogService<string> newPresetDialog)
    {
      _configService = configService;
      _newPresetDialog = newPresetDialog;
    }

    /// <summary>
    /// Shows new preset dialog
    /// </summary>
    /// <returns>Returns name of new preset</returns>
    public string ShowNewPresetDialog()
    {
      if (_newPresetDialog.ShowDialog() == true) 
      {
        var preset = _newPresetDialog.ReturnValue;
        _configService.CreatePreset(preset);
        return preset;
      }

      return null;
    }

    public void DeletePreset(string preset)
    {
      _configService.DeletePreset(preset);
    }
  }
}