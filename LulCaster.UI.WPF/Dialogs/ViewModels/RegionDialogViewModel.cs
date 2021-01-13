using LulCaster.UI.WPF.ViewModels;
using LulCaster.Utility.Common.Enums;
using System;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Dialogs.ViewModels
{
  public class RegionDialogViewModel : NestedViewModel<RegionViewModel>, INestedViewModel<RegionViewModel>
  {
    public IEnumerable<string> AvailableTypes { get; set; }

    public RegionDialogViewModel(INestedViewModel<RegionViewModel> viewModel) : base(viewModel.Title, viewModel.Message, viewModel.InnerItem, viewModel.MessageBoxButtons)
    {
      AvailableTypes = Enum.GetNames(typeof(RegionTypes));
    }

    public RegionDialogViewModel(string title, string message, IEnumerable<string> availableTypes, RegionViewModel innerItem, DialogButtons dialogButtons) : base(title, message, innerItem, dialogButtons)
    {
      AvailableTypes = availableTypes;
    }
  }
}