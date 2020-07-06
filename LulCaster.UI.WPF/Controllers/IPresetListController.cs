namespace LulCaster.UI.WPF.Controllers
{
  public interface IPresetListController
  {
    string ShowNewPresetDialog();
    void DeletePreset(string preset)
  }
}