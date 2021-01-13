using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using LulCaster.UI.WPF.ViewModels;
using System;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs
{
  /// <summary>
  /// Interaction logic for RegionDialog.xaml
  /// </summary>
  public partial class RegionDialog : Window, INestedViewDialog<RegionViewModel>
  {
    public RegionDialog()
    {
      InitializeComponent();
    }

    public new DialogResults DialogResult { get; set; }

    public NestedDialogResults<RegionViewModel> Show(INestedViewModel<RegionViewModel> data)
    {
      DataContext = new RegionDialogViewModel((NestedViewModel<RegionViewModel>)data);

      ShowDialog();
      var viewModel = (RegionDialogViewModel)DataContext;

      return new NestedDialogResults<RegionViewModel>(viewModel.InnerItem, DialogResult);
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = DialogResults.Cancel;
      Hide();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = DialogResults.Ok;
      Hide();
    }

    private void ctrlRegionDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = true;
      Hide();
    }
  }
}