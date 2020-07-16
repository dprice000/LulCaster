using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Config
{
  public interface IPresetConfigService
  {
    PresetViewModel CreatePreset(string name);
    void DeletePreset(PresetViewModel preset);
    IEnumerable<PresetViewModel> GetAllPresets();
  }
}