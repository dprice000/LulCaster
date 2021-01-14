using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using LulCaster.UI.WPF.ViewModels;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs
{
  /// <summary>
  /// Interaction logic for PresetInputDialog.xaml
  /// </summary>
  public partial class PresetInputDialog : Window, INestedViewDialog<PresetViewModel>
  {
    public PresetInputDialog()
    {
      InitializeComponent();

      Loaded += PresetInputDialog_Loaded;
      Closing += PresetInputDialog_Closing;
    }

    public new DialogResults DialogResult { get; set; }

    public NestedDialogResults<PresetViewModel> Show(INestedViewModel<PresetViewModel> data)
    {
      DataContext = data.InnerItem;
      ShowDialog();

      var viewModel = (NestedViewModel<PresetViewModel>)DataContext;
      return new NestedDialogResults<PresetViewModel>(viewModel.InnerItem, DialogResult);
    }

    private void PresetInputDialog_Loaded(object sender, RoutedEventArgs e)
    {
      var currentApp = Application.Current;
      var mainWindow = currentApp.MainWindow;
      Left = mainWindow.Left + (mainWindow.Width - ActualWidth) / 2;
      Top = mainWindow.Top + (mainWindow.Height - ActualHeight) / 2;
    }

    private void PresetInputDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = true;
      Visibility = Visibility.Hidden;
    }

    private void Button_btnOk(object sender, RoutedEventArgs e)
    {
      DialogResult = DialogResults.Ok;
      Close();
    }

    private void Button_btnCancel(object sender, RoutedEventArgs e)
    {
      DialogResult = DialogResults.Cancel;
      Close();
    }
  }
}