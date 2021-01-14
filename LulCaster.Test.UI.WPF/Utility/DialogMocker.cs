using LulCaster.UI.WPF.Dialogs;
using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.Providers;
using LulCaster.UI.WPF.Dialogs.Services;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using LulCaster.UI.WPF.ViewModels;
using NSubstitute;

namespace LulCaster.Test.UI.WPF.Utility
{
  public static class DialogMocker
  {
    public static void InitializeOkCancelDialog(PresetViewModel dialogViewModel, DialogResults buttonResult)
    {
      var dialogResult = new NestedDialogResults<PresetViewModel>(dialogViewModel, buttonResult);
      var dialogService = Substitute.For<INestedViewDialog<PresetViewModel>>();
      dialogService.Show(Arg.Any<NestedViewModel<PresetViewModel>>()).Returns(dialogResult);

      CrudDialogProvider.AddDialog<PresetViewModel>(dialogService);
    }

    public static void InitializeOkCancelDialog(RegionViewModel dialogViewModel, DialogResults buttonResult)
    {
      var dialogResult = new NestedDialogResults<RegionViewModel>(dialogViewModel, buttonResult);
      var dialogService = Substitute.For<INestedViewDialog<RegionViewModel>>();
      dialogService.Show(Arg.Any<NestedViewModel<RegionViewModel>>()).Returns(dialogResult);

      CrudDialogProvider.AddDialog<RegionViewModel>(dialogService);
    }

    public static void InitializeYesNoDialog(DialogResults dialogResult)
    {
      var result = new LulDialogResult() { DialogResult = dialogResult };
      var dialogService = Substitute.For<IDialogService<MessageBoxDialog, LulDialogResult>>();
      dialogService.Show(Arg.Any<string>(), Arg.Any<string>(), DialogButtons.YesNo).Returns(result);

      var messanger = new MessageBoxProvider(dialogService);
    }
  }
}
