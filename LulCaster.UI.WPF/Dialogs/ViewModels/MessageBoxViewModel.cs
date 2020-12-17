namespace LulCaster.UI.WPF.Dialogs.ViewModels
{
  public class MessageBoxViewModel : DialogViewModelBase
  {
    public bool ShowOk => MessageBoxButtons == DialogButtons.Ok || MessageBoxButtons == DialogButtons.OkCancel;
    public bool ShowCancel => MessageBoxButtons == DialogButtons.OkCancel;
    public bool ShowYesNo => MessageBoxButtons == DialogButtons.YesNo;

    public MessageBoxViewModel(string title, string message, DialogButtons dialogButtons) : base(title, message, dialogButtons)
    {
    }
  }
}
