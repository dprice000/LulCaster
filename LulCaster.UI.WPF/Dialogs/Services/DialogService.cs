using System.Windows;

namespace LulCaster.UI.WPF.Dialogs.Services
{
  public class DialogService<TWindowType, TReturnType> : IDialogService<TWindowType, TReturnType> where TWindowType : Window
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