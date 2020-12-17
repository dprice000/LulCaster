using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Config.UserSettings
{
  public interface IPresetConfigService
  {
    PresetViewModel CreatePreset(string name, string processName);
    void DeletePreset(PresetViewModel preset);
    IEnumerable<PresetViewModel> GetAllPresets();
  }
}