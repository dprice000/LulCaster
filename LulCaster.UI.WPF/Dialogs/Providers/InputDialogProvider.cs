using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Services;

namespace LulCaster.UI.WPF.Dialogs.Providers
{
  public class InputDialogProvider
  {
    private static IDialogService<InputDialog, InputDialogResult> _inputDialog;

    public InputDialogProvider(IDialogService<InputDialog, InputDialogResult> inputDialog)
    {
      _inputDialog = inputDialog;
    }
    
    public static InputDialogResult Show(string title, string message, DialogButtons messageBoxButtons)
    {
      return _inputDialog.Show(title, message, messageBoxButtons);
    }
  }
}
