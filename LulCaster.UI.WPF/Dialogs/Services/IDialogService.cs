using System.Windows;

namespace LulCaster.UI.WPF.Dialogs.Services
{
  public interface IDialogService<IWindowType, TReturnType>  
  {
    TReturnType Show(string title, string message, DialogButtons messageBoxButtons);
  }
}