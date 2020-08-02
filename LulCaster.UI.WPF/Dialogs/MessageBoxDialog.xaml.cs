using LulCaster.UI.WPF.Dialogs.ViewModels;
using System.Windows;

namespace LulCaster.UI.WPF.Dialogs
{
  /// <summary>
  /// Interaction logic for MessageBoxDialog.xaml
  /// </summary>
  public partial class MessageBoxDialog : Window
  {
    public new DialogResults DialogResult { get; set; }

    public MessageBoxDialog()
    {
      InitializeComponent();
    }

    public static DialogResults Show(string title, string message, DialogButtons messageBoxButtons)
    {
      var dialog = new MessageBoxDialog
      {
        DataContext = new MessageBoxViewModel()
        {
          Title = title,
          Message = message,
          MessageBoxButtons = messageBoxButtons
        }
      };

      dialog.ShowDialog();

      return dialog.DialogResult;
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