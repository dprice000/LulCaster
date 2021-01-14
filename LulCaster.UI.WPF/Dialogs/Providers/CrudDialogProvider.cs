using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using LulCaster.UI.WPF.ViewModels;
using System;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Dialogs.Providers
{
  public class CrudDialogProvider
  {
    private static Dictionary<Type, object> _dialogLookup = new Dictionary<Type, object>();

    public static void AddDialog<TViewModel>(object dialogService)
    {
      if (!_dialogLookup.ContainsKey(typeof(TViewModel)))
        _dialogLookup.Add(typeof(TViewModel), dialogService);
    }

    public static NestedDialogResults<TViewModel> Show<TViewModel>(INestedViewModel<TViewModel> viewModel)
        where TViewModel : ViewModelBase
    {
      var dialog = (INestedViewDialog<TViewModel>)_dialogLookup[typeof(TViewModel)];
      return dialog.Show(viewModel);
    }
  }
}