namespace LulCaster.UI.WPF.Dialogs
{
  public interface IMessageBoxDialogService
  {
    DialogResults Show(string title, string message, MessageBoxButtons messageBoxButtons);
  }
}