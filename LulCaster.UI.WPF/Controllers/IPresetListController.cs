using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public interface IPresetListController
  {
    PresetViewModel CreatePreset(string name);

    IEnumerable<PresetViewModel> GetAllPresets();

    void DeletePreset(PresetViewModel preset);
  }
}