using LulCaster.Utility.Common.Enums;
using System.Collections.Generic;

namespace LulCaster.UI.WPF.Dialogs.ViewModels
{
  public class RegionDialogViewModel : DialogViewModelBase
  {
    public string Name { get; set; }
    public IEnumerable<RegionTypes> AvailableTypes { get; set; }
    public RegionTypes SelectedType { get; set; }

    public RegionDialogViewModel(string title, string message, DialogButtons dialogButtons) : base(title, message, dialogButtons)
    {
    }
  }
}