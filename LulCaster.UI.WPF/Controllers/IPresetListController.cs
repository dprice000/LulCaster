using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.ViewModels;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public interface IPresetListController
  {
    PresetViewModel CreatePreset(string name);

    IEnumerable<PresetViewModel> GetAllPresets();

    string ShowNewPresetDialog();
    DialogResults ShowMessageBox(string title, string message, DialogButtons dialogButtons);

    bool DeletePreset(PresetViewModel preset);
  }
}