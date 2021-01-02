using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs
{
  /// <summary>
  /// Interaction logic for RegionDialog.xaml
  /// </summary>
  public partial class RegionDialog : Window, ILulDialog
  {
    public RegionDialog()
    {
      InitializeComponent();
    }

    public new DialogResults DialogResult { get; set; }

    public LulDialogResult Show(string title, string message, DialogButtons dialogButtons)
    {
      DataContext = new RegionDialogViewModel(title, message, dialogButtons);

      ShowDialog();

      return new InputDialogResult
      {
        DialogResult = DialogResult,
        Input = (DialogResult == DialogResults.Ok) ? ((InputDialogViewModel)DataContext).Input : null
      };
    }

    public LulDialogResult Show(RegionDialogViewModel viewModel, DialogButtons dialogButtons)
    {
      DataContext = viewModel;

      ShowDialog();
      viewModel = (RegionDialogViewModel)DataContext;

      return new RegionDialogResults
      {
        DialogResult = DialogResult,
        Name = viewModel.Name,
        Type = viewModel.SelectedType
      };
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = DialogResults.Cancel;
      Close();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = DialogResults.Ok;
      Close();
    }
  }
}