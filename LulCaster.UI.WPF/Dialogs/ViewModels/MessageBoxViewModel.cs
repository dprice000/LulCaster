namespace LulCaster.UI.WPF.Dialogs.ViewModels
{
  public class MessageBoxViewModel
  {
    public string Title { get; set; }
    public string Message { get; set; }
    public MessageBoxButtons MessageBoxButtons { get; set; }
    public bool ShowOk => MessageBoxButtons == MessageBoxButtons.Ok || MessageBoxButtons == MessageBoxButtons.OkCancel;
    public bool ShowCancel => MessageBoxButtons == MessageBoxButtons.OkCancel;
    public bool ShowYesNo => MessageBoxButtons == MessageBoxButtons.YesNo;
  }
}
