using LulCaster.UI.WPF.Dialogs.ViewModels;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs.Services
{
  public interface IDialogService<TWindowType, TViewModelType> where TWindowType : Window where TViewModelType : DialogViewModelBase
  {
    object Show(string title, string message, DialogButtons messageBoxButtons);
  }
}