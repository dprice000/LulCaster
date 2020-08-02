namespace LulCaster.UI.WPF.Dialogs
{
  public class MessageBoxDialogService : IMessageBoxDialogService
  {
    public DialogResults Show(string title, string message, MessageBoxButtons messageBoxButtons)
    {
      return MessageBoxDialog.Show(title, message, messageBoxButtons);
    }
  }
}