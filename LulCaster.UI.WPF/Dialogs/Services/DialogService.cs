using LulCaster.UI.WPF.Dialogs.Models;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs.Services
{
  public class DialogService<TWindowType, TReturnType> : IDialogService<TWindowType, TReturnType> 
    where TReturnType : LulDialogResult
    where TWindowType : Window
  {
    private readonly TWindowType _window;

    public DialogService(TWindowType window)
    {
      _window = window;
    }

    public TReturnType Show(string title, string message, DialogButtons dialogButtons)
    {
      var dialog = (ILulDialog)_window;
      return (TReturnType)dialog.Show(title, message, dialogButtons);
    }
  }
}