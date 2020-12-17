namespace LulCaster.UI.WPF.Dialogs.ViewModels
{
  public class InputDialogViewModel : DialogViewModelBase
  {
    public string Input { get; set; }

    public InputDialogViewModel(string title, string message, DialogButtons dialogButtons) : base(title, message, dialogButtons)
    {
    }
  }
}