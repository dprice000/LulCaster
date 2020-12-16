using LulCaster.UI.WPF.Config.UserSettings;
using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public class PresetListController : IPresetListController
  {
    private readonly IPresetConfigService _presetConfigService;

    public PresetListController(IPresetConfigService configService)
    {
      _presetConfigService = configService;
    }

    public PresetViewModel CreatePreset(string name)
    {
      return _presetConfigService.CreatePreset(name);
    }

    public IEnumerable<PresetViewModel> GetAllPresets()
    {
      return _presetConfigService.GetAllPresets();
    }

    public void DeletePreset(PresetViewModel preset)
    {
      _presetConfigService.DeletePreset(preset);
    }
  }
}