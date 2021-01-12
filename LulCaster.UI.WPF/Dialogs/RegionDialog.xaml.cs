using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using LulCaster.UI.WPF.ViewModels;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs
{
  /// <summary>
  /// Interaction logic for RegionDialog.xaml
  /// </summary>
  public partial class RegionDialog : Window
  {
    public RegionDialog()
    {
      InitializeComponent();
    }

    public new DialogResults DialogResult { get; set; }

    public NestedDialogResults<RegionViewModel> Show(NestedDialogViewModel<RegionViewModel> data)
    {
      DataContext = new RegionDialogViewModel(data);

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