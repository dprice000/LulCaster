using System.Collections.Generic;

namespace LulCaster.UI.WPF.Controllers
{
  public interface IPresetListController
  {
    List<string> PresetNames { get; }

    void ShowNewPresetDialog();

    void DeletePreset(string preset);
  }
}