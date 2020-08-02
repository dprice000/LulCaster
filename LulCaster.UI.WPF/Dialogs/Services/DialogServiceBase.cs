using LulCaster.UI.WPF.Dialogs.ViewModels;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs.Services
{
  public abstract class DialogServiceBase<TWindowType, TViewModelType> : IDialogService<TWindowType, TViewModelType> 
                                  where TWindowType : Window 
                                  where TViewModelType : DialogViewModelBase
  {
    private TWindowType _window;

    public DialogServiceBase(TWindowType window, TViewModelType viewModel)
    {
      _window = window;
      _window.DataContext = viewModel;
    }

    public object Show(string title, string message, DialogButtons messageBoxButtons)
    {
      var viewModel = (TViewModelType)_window.DataContext;
      viewModel.Title = title;
      viewModel.Message = message;
      viewModel.MessageBoxButtons = messageBoxButtons;

      _window.ShowDialog();

      return _window.DialogResult;
    }
  }
}