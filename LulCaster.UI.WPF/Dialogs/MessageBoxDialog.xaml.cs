using LulCaster.UI.WPF.Dialogs.Models;
using LulCaster.UI.WPF.Dialogs.ViewModels;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs
{
  /// <summary>
  /// Interaction logic for MessageBoxDialog.xaml
  /// </summary>
  public partial class MessageBoxDialog : Window, ILulDialog
  {
    public new DialogResults DialogResult { get; set; }

    public MessageBoxDialog()
    {
      InitializeComponent();

      Loaded += MessageBoxDialog_Loaded;
      Closing += MessageBoxDialog_Closing;
    }

    private void MessageBoxDialog_Loaded(object sender, RoutedEventArgs e)
    {
      var currentApp = Application.Current;
      var mainWindow = currentApp.MainWindow;
      Left = mainWindow.Left + (mainWindow.Width - ActualWidth) / 2;
      Top = mainWindow.Top + (mainWindow.Height - ActualHeight) / 2;
    }

    private void MessageBoxDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = true;
      Visibility = Visibility.Hidden;
    }

    public LulDialogResult Show(string title, string message, DialogButtons messageBoxButtons)
    {
      DataContext = new MessageBoxViewModel()
      {
        Title = title,
        Message = message,
        MessageBoxButtons = messageBoxButtons
      };

      ShowDialog();

      return new LulDialogResult
      {
        DialogResult = DialogResult
      };
    }

    private void Button_btnCancel(object sender, RoutedEventArgs e)
    {
      CloseDialog(DialogResults.Cancel);
    }

    private void Button_btnOk(object sender, RoutedEventArgs e)
    {
      CloseDialog(DialogResults.Ok);
    }

    private void Button_btnYes(object sender, RoutedEventArgs e)
    {
      CloseDialog(DialogResults.Yes);
    }

    private void Button_btnNo(object sender, RoutedEventArgs e)
    {
      CloseDialog(DialogResults.No);
    }

    private void CloseDialog(DialogResults dialogResults)
    {
      DialogResult = dialogResults;
      Close();
    }
  }
}