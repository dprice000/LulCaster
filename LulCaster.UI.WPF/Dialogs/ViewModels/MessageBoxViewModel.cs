namespace LulCaster.UI.WPF.Dialogs.ViewModels
{
  public class MessageBoxViewModel : DialogViewModelBase
  {
    public bool ShowOk => MessageBoxButtons == DialogButtons.Ok || MessageBoxButtons == DialogButtons.OkCancel;
    public bool ShowCancel => MessageBoxButtons == DialogButtons.OkCancel;
    public bool ShowYesNo => MessageBoxButtons == DialogButtons.YesNo;
  }
}
