namespace LulCaster.UI.WPF.Dialogs.ViewModels
{
  public class PresetDialogViewModel : DialogViewModelBase
  {
    public string PresetName { get; set; }
    public string ProcessName { get; set; }

    public PresetDialogViewModel(string title, string message, DialogButtons dialogButtons) : base(title, message, dialogButtons)
    {
    }
  }
}