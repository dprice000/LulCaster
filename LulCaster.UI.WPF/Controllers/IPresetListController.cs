using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public interface IPresetListController
  {
    PresetViewModel CreatePreset(string name);

    IEnumerable<PresetViewModel> GetAllPresets();

    PresetViewModel ShowNewPresetDialog();

    bool DeletePreset(PresetViewModel preset);
  }
}