using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public interface IPresetListController
  {
    PresetViewModel CreatePreset(string name, string processName);

    IEnumerable<PresetViewModel> GetAllPresets();

    void DeletePreset(PresetViewModel preset);
  }
}