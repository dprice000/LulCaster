using LulCaster.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public interface IPresetListController
  {
    IEnumerable <PresetViewModel> GetAllPresets();
    PresetViewModel ShowNewPresetDialog();

    void DeletePreset(PresetViewModel preset);
  }
}