using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using LulCaster.UI.WPF.ViewModels;

namespace LulCaster.UI.WPF.Dialogs.Providers
{
  public class CrudDialogProvider
  {
    private static PresetInputDialog _presetInputDialog;
    private static RegionDialog _regionDialog;

    public CrudDialogProvider(PresetInputDialog presetInputDialog, RegionDialog regionDialog)
    {
      _presetInputDialog = presetInputDialog;
      _regionDialog = regionDialog;
    }

    public static NestedDialogResults<PresetViewModel> PresetModal(NestedDialogViewModel<PresetViewModel> viewModel)
    {
      return _presetInputDialog.Show(viewModel);
    }

    public static NestedDialogResults<RegionViewModel> RegionModal(NestedDialogViewModel<RegionViewModel> viewModel)
    {
      return _regionDialog.Show(viewModel);
    }
  }
}