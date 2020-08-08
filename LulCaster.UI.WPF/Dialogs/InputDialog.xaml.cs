using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs
{
  /// <summary>
  /// Interaction logic for InputDialog.xaml
  /// </summary>
  public partial class InputDialog : Window, ILulDialog
  {
    public new DialogResults DialogResult { get; set; }

    public InputDialog()
    {
      InitializeComponent();

      Loaded += InputDialog_Loaded;
      Closing += InputDialog_Closing;
    }

    private void InputDialog_Loaded(object sender, RoutedEventArgs e)
    {
      var currentApp = Application.Current;
      var mainWindow = currentApp.MainWindow;
      Left = mainWindow.Left + (mainWindow.Width - ActualWidth) / 2;
      Top = mainWindow.Top + (mainWindow.Height - ActualHeight) / 2;
    }

    private void InputDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = true;
      Visibility = Visibility.Hidden;
    }

    public LulDialogResult Show(string title, string message, DialogButtons dialogButtons)
    {
      DataContext = new InputDialogViewModel()
      {
        Title = title,
        Message = message,
        MessageBoxButtons = dialogButtons
      };

      ShowDialog();

      return new InputDialogResult
      {
        DialogResult = DialogResult,
        Input = (DialogResult == DialogResults.Ok) ? ((InputDialogViewModel)DataContext).Input : null
      };
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