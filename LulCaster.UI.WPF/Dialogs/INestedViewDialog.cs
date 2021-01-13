using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using LulCaster.UI.WPF.ViewModels;

namespace LulCaster.UI.WPF.Dialogs
{
  public interface INestedViewDialog<TViewModel> 
    where TViewModel : ViewModelBase
  {
    NestedDialogResults<TViewModel> Show(INestedViewModel<TViewModel> viewModel);
  }
}