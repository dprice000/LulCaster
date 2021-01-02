using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs
{
  /// <summary>
  /// Interaction logic for PresetInputDialog.xaml
  /// </summary>
  public partial class PresetInputDialog : Window, ILulDialog
  {
    public PresetInputDialog()
    {
      InitializeComponent();

      Loaded += PresetInputDialog_Loaded;
      Closing += PresetInputDialog_Closing;
    }

    public new DialogResults DialogResult { get; set; }

    public LulDialogResult Show(string title, string message, DialogButtons dialogButtons)
    {
      DataContext = new PresetDialogViewModel(title, message, dialogButtons);

      ShowDialog();

      return new InputDialogResult
      {
        DialogResult = DialogResult,
        Input = (DialogResult == DialogResults.Ok) ? ((InputDialogViewModel)DataContext).Input : null
      };
    }

    public LulDialogResult Show(string title, string message, PresetDialogViewModel viewModel, DialogButtons dialogButtons)
    {
      DataContext = viewModel;

      ShowDialog();

      viewModel = (PresetDialogViewModel)DataContext;

      return new PresetInputDialogResult
      {
        DialogResult = DialogResult,
        PresetName = viewModel.PresetName,
        ProcessName = viewModel.ProcessName
      };
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